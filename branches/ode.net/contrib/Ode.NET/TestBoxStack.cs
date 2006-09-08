using System;
using System.Collections.Generic;
using Drawstuff.NET;

namespace Ode.NET
{
	public class TestBoxStack
	{
		const int NUM = 100;
		const float DENSITY = 5.0f;
		const int GPB = 3;
		const int MAX_CONTACTS = 8;
		
		static IntPtr world;
		static IntPtr space;
		static IntPtr contactgroup;

		static Queue<IntPtr> obj = new Queue<IntPtr>();

		static d.Vector3 xyz = new d.Vector3(2.1640f, -1.3079f, 1.7600f);
		static d.Vector3 hpr = new d.Vector3(125.5000f, -17.0000f, 0.0000f);

		static d.NearCallback nearCallback = near;
		static d.ContactGeom[] contacts = new d.ContactGeom[MAX_CONTACTS];
		static d.Contact contact;


		static void near(IntPtr space, IntPtr g1, IntPtr g2)
		{
			IntPtr b1 = d.GeomGetBody(g1);
			IntPtr b2 = d.GeomGetBody(g2);
			if (b1 != IntPtr.Zero && b2 != IntPtr.Zero && d.AreConnectedExcluding(b1, b2, d.JointType.Contact))
				return;

			/*
			d.Contact[] contact = new d.Contact[MAX_CONTACTS];
			for (int i = 0; i < MAX_CONTACTS; ++i)
			{
				contact[i].surface.mode = d.ContactFlags.Bounce | d.ContactFlags.SoftCFM;
				contact[i].surface.mu = d.Infinity;
				contact[i].surface.mu2 = 0.0f;
				contact[i].surface.bounce = 0.1f;
				contact[i].surface.bounce_vel = 0.1f;
				contact[i].surface.soft_cfm = 0.01f;
			}
			*/

			int count = d.Collide(g1, g2, MAX_CONTACTS, contacts, d.ContactGeom.SizeOf);
			for (int i = 0; i < count; ++i)
			{
				contact.geom = contacts[i];
				IntPtr c = d.JointCreateContact(world, contactgroup, ref contact);
				d.JointAttach(c, b1, b2);
			}
		}


		static void start(int unused)
		{
			ds.SetViewpoint(ref xyz, ref hpr);
			Console.WriteLine("To drop another object, press:");
			Console.WriteLine("   b for box.");
			Console.WriteLine("   s for sphere.");
			Console.WriteLine("   c for capsule.");
			Console.WriteLine("   y for cylinder.");
			Console.WriteLine("   v for a convex object.");
			Console.WriteLine("   x for a composite object.");
			Console.WriteLine("To select an object, press space.");
			Console.WriteLine("To disable the selected object, press d.");
			Console.WriteLine("To enable the selected object, press e.");
			Console.WriteLine("To toggle showing the geom AABBs, press a.");
			Console.WriteLine("To toggle showing the contact points, press t.");
			Console.WriteLine("To toggle dropping from random position/orientation, press r.");
			Console.WriteLine("To save the current state to 'state.dif', press 1.");
		}


		static void addBody(IntPtr geom)
		{
			// Create a body for this object
			IntPtr body = d.BodyCreate(world);
			d.GeomSetBody(geom, body);
			obj.Enqueue(geom);

			// Set the position of the new object
			d.Matrix3 R;
			d.BodySetPosition(body, d.RandReal() * 2 - 1, d.RandReal() * 2 - 1, d.RandReal() + 2);
			d.RFromAxisAndAngle(out R, d.RandReal() * 2 - 1, d.RandReal() * 2 - 1, d.RandReal() * 2 - 1, d.RandReal() * 10 - 5);
			d.BodySetRotation(body, ref R);

			// Cap the total number of objects
			if (obj.Count > NUM)
			{
				geom = obj.Dequeue();
				body = d.GeomGetBody(geom);
				d.BodyDestroy(body);
				d.GeomDestroy(geom);
			}
		}


		static void command(int cmd)
		{
			IntPtr geom;
			d.Vector3 sides = new d.Vector3(d.RandReal() * 0.5f + 0.1f, d.RandReal() * 0.5f + 0.1f, d.RandReal() * 0.5f + 0.1f);

			Char ch = Char.ToLower((Char)cmd);
			switch ((Char)ch)
			{
			case 'b':
				geom = d.CreateBox(space, sides.X, sides.Y, sides.Z);
				addBody(geom);
				break;
			}
		}


		static void drawGeom(IntPtr geom)
		{
			IntPtr body = d.GeomGetBody(geom);

			d.Vector3 pos;
			d.BodyCopyPosition(body, out pos);

			// d.Matrix3 r = new d.Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);
			d.Matrix3 R;
			d.BodyCopyRotation(body, out R);

			// if (is box class)
			d.Vector3 sides;
			d.GeomBoxGetLengths(geom, out sides);
			ds.DrawBox(ref pos, ref R, ref sides);
		}


		static void step(int pause)
		{
			d.SpaceCollide(space, IntPtr.Zero, nearCallback);
			if (pause == 0)
				d.WorldQuickStep(world, 0.02f);
			d.JointGroupEmpty(contactgroup);

			ds.SetColor(1.0f, 1.0f, 0.0f);
			ds.SetTexture(ds.Texture.Wood);

			foreach (IntPtr geom in obj)
			{
				drawGeom(geom);
			}
		}


		static void Main(string[] args)
		{
			// Setup pointers to drawstuff callback functions
			ds.Functions fn;
			fn.version = ds.VERSION;
			fn.start = new ds.CallbackFunction(start);
			fn.step = new ds.CallbackFunction(step);
			fn.command = new ds.CallbackFunction(command);
			fn.stop = null;
			fn.path_to_textures = "../../drawstuff/textures";
			if (args.Length > 0)
			{
				fn.path_to_textures = args[0];
			}

			contact.surface.mode = d.ContactFlags.Bounce | d.ContactFlags.SoftCFM;
			contact.surface.mu = d.Infinity;
			contact.surface.mu2 = 0.0f;
			contact.surface.bounce = 0.1f;
			contact.surface.bounce_vel = 0.1f;
			contact.surface.soft_cfm = 0.01f;

			world = d.WorldCreate();
			space = d.HashSpaceCreate(IntPtr.Zero);
			contactgroup = d.JointGroupCreate(0);
			d.WorldSetGravity(world, 0.0f, 0.0f, -0.5f);
			d.WorldSetCFM(world, 1e-5f);
			d.WorldSetAutoDisableFlag(world, true);
			d.WorldSetContactMaxCorrectingVel(world, 0.1f);
			d.WorldSetContactSurfaceLayer(world, 0.001f);
			d.CreatePlane(space, 0, 0, 1, 0);

			ds.SimulationLoop(args.Length, args, 352, 288, ref fn);

			d.JointGroupDestroy(contactgroup);
			d.SpaceDestroy(space);
			d.WorldDestroy(world);
			d.CloseODE();
		}
	}
}
