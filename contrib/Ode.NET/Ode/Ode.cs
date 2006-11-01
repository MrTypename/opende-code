using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Ode.NET
{
#if dDOUBLE
	using dReal = System.Double;
#else
	using dReal = System.Single;
#endif

	public static class d
	{
		public static dReal Infinity = dReal.MaxValue;

		#region Flags and Enumerations

		[Flags]
		public enum ContactFlags : int
		{
			Mu2 = 0x001,
			FDir1 = 0x002,
			Bounce = 0x004,
			SoftERP = 0x008,
			SoftCFM = 0x010,
			Motion1 = 0x020,
			Motion2 = 0x040,
			Slip1 = 0x080,
			Slip2 = 0x100,
			Approx0 = 0x0000,
			Approx1_1 = 0x1000,
			Approx1_2 = 0x2000,
			Approx1 = 0x3000
		}

		public enum GeomClassID : int
		{
			SphereClass,
			BoxClass,
			CapsuleClass,
			CylinderClass,
			PlaneClass,
			RayClass,
			ConvexClass,
			GeomTransformClass,
			TriMeshClass,
			HeightfieldClass,
			FirstSpaceClass,
			SimpleSpaceClass = FirstSpaceClass,
			HashSpaceClass,
			QuadTreeSpaceClass,
			LastSpaceClass = QuadTreeSpaceClass,
			FirstUserClass,
			LastUserClass = FirstUserClass + MaxUserClasses - 1,
			NumClasses,
			MaxUserClasses = 4
		}

		public enum JointType : int
		{
			None,
			Ball,
			Hinge,
			Slider,
			Contact,
			Universal,
			Hinge2,
			Fixed,
			Null,
			AMotor,
			LMotor,
			Plane2D
		}

		#endregion

		#region Callbacks

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int AABBTestFn(IntPtr o1, IntPtr o2, ref AABB aabb);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int ColliderFn(IntPtr o1, IntPtr o2, int flags, out ContactGeom contact, int skip);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void GetAABBFn(IntPtr geom, out AABB aabb);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate ColliderFn GetColliderFnFn(int num);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void GeomDtorFn(IntPtr o);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate dReal HeightfieldGetHeight(IntPtr p_user_data, int x, int z);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void NearCallback(IntPtr data, IntPtr geom1, IntPtr geom2);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int TriCallback(IntPtr trimesh, IntPtr refObject, int triangleIndex);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int TriArrayCallback(IntPtr trimesh, IntPtr refObject, int[] triangleIndex, int triCount);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int TriRayCallback(IntPtr trimesh, IntPtr ray, int triangleIndex, dReal u, dReal v);

		#endregion

		#region Type Definitions

		[StructLayout(LayoutKind.Sequential)]
		public struct AABB
		{
			public dReal MinX, MaxX;
			public dReal MinY, MaxY;
			public dReal MinZ, MaxZ;
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct Contact
		{
			public SurfaceParameters surface;
			public ContactGeom geom;
			public Vector3 fdir1;
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct ContactGeom
		{
			public static readonly int SizeOf = Marshal.SizeOf(typeof(ContactGeom));

			public Vector3 pos;
			public Vector3 normal;
			public dReal depth;
			public IntPtr g1;
			public IntPtr g2;
			public int side1;
			public int side2;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct GeomClass
		{
			public int bytes;
			public GetColliderFnFn collider;
			public GetAABBFn aabb;
			public AABBTestFn aabb_test;
			public GeomDtorFn dtor;
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct Mass
		{
			public dReal mass;
			public Vector4 c;
			public Matrix3 I;
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct Matrix3
		{
			public Matrix3(dReal m00, dReal m10, dReal m20, dReal m01, dReal m11, dReal m21, dReal m02, dReal m12, dReal m22)
			{
				M00 = m00;  M10 = m10;  M20 = m20;  _m30 = 0.0f;
				M01 = m01;  M11 = m11;  M21 = m21;  _m31 = 0.0f;
				M02 = m02;  M12 = m12;  M22 = m22;  _m32 = 0.0f;
			}
			public dReal M00, M10, M20;
			private dReal _m30;
			public dReal M01, M11, M21;
			private dReal _m31;
			public dReal M02, M12, M22;
			private dReal _m32;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Matrix4
		{
			public Matrix4(dReal m00, dReal m10, dReal m20, dReal m30,
				dReal m01, dReal m11, dReal m21, dReal m31,
				dReal m02, dReal m12, dReal m22, dReal m32,
				dReal m03, dReal m13, dReal m23, dReal m33)
			{
				M00 = m00; M10 = m10; M20 = m20; M30 = m30;
				M01 = m01; M11 = m11; M21 = m21; M31 = m31;
				M02 = m02; M12 = m12; M22 = m22; M32 = m32;
				M03 = m03; M13 = m13; M23 = m23; M33 = m33;
			}
			public dReal M00, M10, M20, M30;
			public dReal M01, M11, M21, M31;
			public dReal M02, M12, M22, M32;
			public dReal M03, M13, M23, M33;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Quaternion
		{
			public dReal W, X, Y, Z;
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct SurfaceParameters
		{
			public ContactFlags mode;
			public dReal mu;
			public dReal mu2;
			public dReal bounce;
			public dReal bounce_vel;
			public dReal soft_erp;
			public dReal soft_cfm;
			public dReal motion1;
			public dReal motion2;
			public dReal slip1;
			public dReal slip2;
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct Vector3
		{
			public Vector3(dReal x, dReal y, dReal z)
			{
				X = x;  Y = y;  Z = z;  _w = 0.0f;
			}
			public dReal X, Y, Z;
			private dReal _w;
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct Vector4
		{
			public Vector4(dReal x, dReal y, dReal z, dReal w)
			{
				X = x;  Y = y;  Z = z;  W = w;
			}
			public dReal X, Y, Z, W;
		}

		#endregion

		[DllImport("ode", EntryPoint = "dAreConnectedExcluding"), SuppressUnmanagedCodeSecurity]
		public static extern bool AreConnectedExcluding(IntPtr body0, IntPtr body1, JointType jointType);

		[DllImport("ode", EntryPoint = "dBodyCopyPosition"), SuppressUnmanagedCodeSecurity]
		public static extern void BodyCopyPosition(IntPtr body, out Vector3 pos);

		[DllImport("ode", EntryPoint = "dBodyCopyPosition"), SuppressUnmanagedCodeSecurity]
		public static extern void BodyCopyPosition(IntPtr body, out dReal X);

		[DllImport("ode", EntryPoint = "dBodyCopyQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void BodyCopyQuaternion(IntPtr body, out Quaternion quat);

		[DllImport("ode", EntryPoint = "dBodyCopyQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void BodyCopyQuaternion(IntPtr body, out dReal X);

		[DllImport("ode", EntryPoint = "dBodyCopyRotation"), SuppressUnmanagedCodeSecurity]
		public static extern void BodyCopyRotation(IntPtr body, out Matrix3 R);

		[DllImport("ode", EntryPoint = "dBodyCopyRotation"), SuppressUnmanagedCodeSecurity]
		public static extern void BodyCopyRotation(IntPtr body, out dReal M00);

		[DllImport("ode", EntryPoint = "dBodyCreate"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr BodyCreate(IntPtr world);

		[DllImport("ode", EntryPoint = "dBodyDestroy"), SuppressUnmanagedCodeSecurity]
		public static extern void BodyDestroy(IntPtr body);

		[DllImport("ode", EntryPoint = "dBodyGetAutoDisableAngularThreshold"), SuppressUnmanagedCodeSecurity]
		public static extern dReal BodyGetAutoDisableAngularThreshold(IntPtr body);

		[DllImport("ode", EntryPoint = "dBodyGetAutoDisableFlag"), SuppressUnmanagedCodeSecurity]
		public static extern bool BodyGetAutoDisableFlag(IntPtr body);

		[DllImport("ode", EntryPoint = "dBodyGetAutoDisableLinearThreshold"), SuppressUnmanagedCodeSecurity]
		public static extern dReal BodyGetAutoDisableLinearThreshold(IntPtr body);

		[DllImport("ode", EntryPoint = "dBodyGetAutoDisableSteps"), SuppressUnmanagedCodeSecurity]
		public static extern int BodyGetAutoDisableSteps(IntPtr body);

		[DllImport("ode", EntryPoint = "dBodyGetAutoDisableTime"), SuppressUnmanagedCodeSecurity]
		public static extern dReal BodyGetAutoDisableTime(IntPtr body);

		[DllImport("ode", EntryPoint = "dBodyGetData"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr BodyGetData(IntPtr body);

		[DllImport("ode", EntryPoint = "dBodySetAngularVel"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetAngularVel(IntPtr body, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dBodySetAutoDisableAngularThreshold"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetAutoDisableAngularThreshold(IntPtr body, dReal angular_threshold);

		[DllImport("ode", EntryPoint = "dBodySetAutoDisableDefaults"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetAutoDisableDefaults(IntPtr body);

		[DllImport("ode", EntryPoint = "dBodySetAutoDisableFlag"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetAutoDisableFlag(IntPtr body, bool do_auto_disable);

		[DllImport("ode", EntryPoint = "dBodySetAutoDisableLinearThreshold"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetAutoDisableLinearThreshold(IntPtr body, dReal linear_threshold);

		[DllImport("ode", EntryPoint = "dBodySetAutoDisableSteps"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetAutoDisableSteps(IntPtr body, int steps);

		[DllImport("ode", EntryPoint = "dBodySetAutoDisableTime"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetAutoDisableTime(IntPtr body, dReal time);

		[DllImport("ode", EntryPoint = "dBodySetData"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetData(IntPtr body, IntPtr data);

		[DllImport("ode", EntryPoint = "dBodySetLinearVel"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetLinearVel(IntPtr body, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dBodySetMass"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetMass(IntPtr body, ref Mass mass);

		[DllImport("ode", EntryPoint = "dBodySetPosition"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetPosition(IntPtr body, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dBodySetQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetQuaternion(IntPtr body, ref Quaternion q);

		[DllImport("ode", EntryPoint = "dBodySetQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetQuaternion(IntPtr body, ref dReal w);

		[DllImport("ode", EntryPoint = "dBodySetRotation"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetRotation(IntPtr body, ref Matrix3 R);

		[DllImport("ode", EntryPoint = "dBodySetRotation"), SuppressUnmanagedCodeSecurity]
		public static extern void BodySetRotation(IntPtr body, ref dReal M00);

		[DllImport("ode", EntryPoint = "dBoxBox"), SuppressUnmanagedCodeSecurity]
		public static extern void BoxBox(ref Vector3 p1, ref Matrix3 R1,
			ref Vector3 side1, ref Vector3 p2,
			ref Matrix3 R2, ref Vector3 side2,
			ref Vector3 normal, out dReal depth, out int return_code,
			int maxc, out ContactGeom contact, int skip);

		[DllImport("ode", EntryPoint = "dBoxTouchesBox"), SuppressUnmanagedCodeSecurity]
		public static extern void BoxTouchesBox(ref Vector3 _p1, ref Matrix3 R1,
			ref Vector3 side1, ref Vector3 _p2,
			ref Matrix3 R2, ref Vector3 side2);

		[DllImport("ode", EntryPoint = "dClosestLineSegmentPoints"), SuppressUnmanagedCodeSecurity]
		public static extern void ClosestLineSegmentPoints(ref Vector3 a1, ref Vector3 a2, 
			ref Vector3 b1, ref Vector3 b2, 
			ref Vector3 cp1, ref Vector3 cp2);

		[DllImport("ode", EntryPoint = "dCloseODE"), SuppressUnmanagedCodeSecurity]
		public static extern void CloseODE();

		[DllImport("ode", EntryPoint = "dCollide"), SuppressUnmanagedCodeSecurity]
		public static extern int Collide(IntPtr o1, IntPtr o2, int flags, [In, Out] ContactGeom[] contact, int skip);

		[DllImport("ode", EntryPoint = "dCreateBox"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateBox(IntPtr space, dReal lx, dReal ly, dReal lz);

		[DllImport("ode", EntryPoint = "dCreateCapsule"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateCapsule(IntPtr space, dReal radius, dReal length);

		[DllImport("ode", EntryPoint = "dCreateConvex"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateConvex(IntPtr space, dReal[] planes, int planeCount, dReal[] points, int pointCount, int[] polygons);

		[DllImport("ode", EntryPoint = "dCreateCylinder"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateCylinder(IntPtr space, dReal radius, dReal length);

		[DllImport("ode", EntryPoint = "dCreateHeightfield"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateHeightfield(IntPtr space, IntPtr data, int bPlaceable);

		[DllImport("ode", EntryPoint = "dCreateGeom"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateGeom(int classnum);

		[DllImport("ode", EntryPoint = "dCreateGeomClass"), SuppressUnmanagedCodeSecurity]
		public static extern int CreateGeomClass(ref GeomClass classptr);

		[DllImport("ode", EntryPoint = "dCreateGeomTransform"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateGeomTransform(IntPtr space);

		[DllImport("ode", EntryPoint = "dCreatePlane"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreatePlane(IntPtr space, dReal a, dReal b, dReal c, dReal d);

		[DllImport("ode", EntryPoint = "dCreateRay"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateRay(IntPtr space, dReal length);

		[DllImport("ode", EntryPoint = "dCreateSphere"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateSphere(IntPtr space, dReal radius);

		[DllImport("ode", EntryPoint = "dCreateTriMesh"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateTriMesh(IntPtr space, IntPtr data, 
			TriCallback callback, TriArrayCallback arrayCallback, TriRayCallback rayCallback);

		[DllImport("ode", EntryPoint = "dGeomBoxGetLengths"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomBoxGetLengths(IntPtr geom, out Vector3 len);

		[DllImport("ode", EntryPoint = "dGeomBoxGetLengths"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomBoxGetLengths(IntPtr geom, out dReal x);

		[DllImport("ode", EntryPoint = "dGeomBoxPointDepth"), SuppressUnmanagedCodeSecurity]
		public static extern dReal GeomBoxPointDepth(IntPtr geom, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dGeomBoxSetLengths"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomBoxSetLengths(IntPtr geom, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dGeomCapsuleGetParams"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomCapsuleGetParams(IntPtr geom, out dReal radius, out dReal length);

		[DllImport("ode", EntryPoint = "dGeomCapsulePointDepth"), SuppressUnmanagedCodeSecurity]
		public static extern dReal GeomCapsulePointDepth(IntPtr geom, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dGeomCapsuleSetParams"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomCapsuleSetParams(IntPtr geom, dReal radius, dReal length);

		[DllImport("ode", EntryPoint = "dGeomClearOffset"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomClearOffset(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomCopyOffsetPosition"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomCopyOffsetPosition(IntPtr geom, ref Vector3 pos);

		[DllImport("ode", EntryPoint = "dGeomCopyOffsetPosition"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomCopyOffsetPosition(IntPtr geom, ref dReal X);

		[DllImport("ode", EntryPoint = "dGeomGetOffsetQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomCopyOffsetQuaternion(IntPtr geom, ref Quaternion Q);

		[DllImport("ode", EntryPoint = "dGeomGetOffsetQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomCopyOffsetQuaternion(IntPtr geom, ref dReal X);

		[DllImport("ode", EntryPoint = "dGeomCopyOffsetRotation"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomCopyOffsetRotation(IntPtr geom, ref Matrix3 R);

		[DllImport("ode", EntryPoint = "dGeomCopyOffsetRotation"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomCopyOffsetRotation(IntPtr geom, ref dReal M00);

		[DllImport("ode", EntryPoint = "dGeomCopyPosition"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomCopyPosition(IntPtr geom, out Vector3 pos);

		[DllImport("ode", EntryPoint = "dGeomCopyPosition"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomCopyPosition(IntPtr geom, out dReal X);

		[DllImport("ode", EntryPoint = "dGeomGetQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomCopyQuaternion(IntPtr geom, out Quaternion q);

		[DllImport("ode", EntryPoint = "dGeomGetQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomCopyQuaternion(IntPtr geom, out dReal X);

		[DllImport("ode", EntryPoint = "dGeomCopyRotation"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomCopyRotation(IntPtr geom, out Matrix3 R);

		[DllImport("ode", EntryPoint = "dGeomCopyRotation"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomCopyRotation(IntPtr geom, out dReal M00);

		[DllImport("ode", EntryPoint = "dGeomCylinderGetParams"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomCylinderGetParams(IntPtr geom, out dReal radius, out dReal length);

		[DllImport("ode", EntryPoint = "dGeomCylinderSetParams"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomCylinderSetParams(IntPtr geom, dReal radius, dReal length);

		[DllImport("ode", EntryPoint = "dGeomDestroy"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomDestroy(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomDisable"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomDisable(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomEnable"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomEnable(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomGetAABB"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomGetAABB(IntPtr geom, out AABB aabb);

		[DllImport("ode", EntryPoint = "dGeomGetAABB"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomGetAABB(IntPtr geom, out dReal minX);

		[DllImport("ode", EntryPoint = "dGeomGetBody"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomGetBody(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomGetCategoryBits"), SuppressUnmanagedCodeSecurity]
		public static extern int GeomGetCategoryBits(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomGetClassData"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomGetClassData(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomGetCollideBits"), SuppressUnmanagedCodeSecurity]
		public static extern int GeomGetCollideBits(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomGetClass"), SuppressUnmanagedCodeSecurity]
		public static extern GeomClassID GeomGetClass(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomGetData"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomGetData(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomGetSpace"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomGetSpace(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataBuildByte"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldDataBuildByte(IntPtr d, byte[] pHeightData, int bCopyHeightData,
				dReal width, dReal depth, int widthSamples, int depthSamples,
				dReal scale, dReal offset, dReal thickness, int bWrap);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataBuildByte"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldDataBuildByte(IntPtr d, IntPtr pHeightData, int bCopyHeightData,
				dReal width, dReal depth, int widthSamples, int depthSamples,
				dReal scale, dReal offset, dReal thickness,	int bWrap);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataBuildCallback"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldDataBuildCallback(IntPtr d, IntPtr pUserData, HeightfieldGetHeight pCallback,
				dReal width, dReal depth, int widthSamples, int depthSamples,
				dReal scale, dReal offset, dReal thickness, int bWrap);

		[CLSCompliant(false)]
		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataBuildShort"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldDataBuildShort(IntPtr d, ushort[] pHeightData, int bCopyHeightData,
				dReal width, dReal depth, int widthSamples, int depthSamples,
				dReal scale, dReal offset, dReal thickness, int bWrap);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataBuildShort"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldDataBuildShort(IntPtr d, short[] pHeightData, int bCopyHeightData,
				dReal width, dReal depth, int widthSamples, int depthSamples,
				dReal scale, dReal offset, dReal thickness, int bWrap);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataBuildShort"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldDataBuildShort(IntPtr d, IntPtr pHeightData, int bCopyHeightData,
				dReal width, dReal depth, int widthSamples, int depthSamples,
				dReal scale, dReal offset, dReal thickness, int bWrap);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataBuildSingle"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldDataBuildSingle(IntPtr d, float[] pHeightData, int bCopyHeightData,
				dReal width, dReal depth, int widthSamples, int depthSamples,
				dReal scale, dReal offset, dReal thickness, int bWrap);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataBuildSingle"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldDataBuildSingle(IntPtr d, IntPtr pHeightData, int bCopyHeightData,
				dReal width, dReal depth, int widthSamples, int depthSamples,
				dReal scale, dReal offset, dReal thickness, int bWrap);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataBuildDouble"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldDataBuildDouble(IntPtr d, double[] pHeightData, int bCopyHeightData,
				dReal width, dReal depth, int widthSamples, int depthSamples,
				dReal scale, dReal offset, dReal thickness, int bWrap);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataBuildDouble"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldDataBuildDouble(IntPtr d, IntPtr pHeightData, int bCopyHeightData,
				dReal width, dReal depth, int widthSamples, int depthSamples,
				dReal scale, dReal offset, dReal thickness, int bWrap);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataCreate"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomHeightfieldDataCreate();

		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataDestroy"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldDataDestroy(IntPtr d);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldDataSetBounds"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldDataSetBounds(IntPtr d, dReal minHeight, dReal maxHeight);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldGetHeightfieldData"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomHeightfieldGetHeightfieldData(IntPtr g);

		[DllImport("ode", EntryPoint = "dGeomHeightfieldSetHeightfieldData"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomHeightfieldSetHeightfieldData(IntPtr g, IntPtr d);

		[DllImport("ode", EntryPoint = "dGeomIsEnabled"), SuppressUnmanagedCodeSecurity]
		public static extern bool GeomIsEnabled(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomIsOffset"), SuppressUnmanagedCodeSecurity]
		public static extern bool GeomIsOffset(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomIsSpace"), SuppressUnmanagedCodeSecurity]
		public static extern bool GeomIsSpace(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomPlaneGetParams"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomPlaneGetParams(IntPtr geom, ref Vector4 result);

		[DllImport("ode", EntryPoint = "dGeomPlaneGetParams"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomPlaneGetParams(IntPtr geom, ref dReal A);

		[DllImport("ode", EntryPoint = "dGeomPlanePointDepth"), SuppressUnmanagedCodeSecurity]
		public static extern dReal GeomPlanePointDepth(IntPtr geom, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dGeomPlaneSetParams"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomPlaneSetParams(IntPtr plane, dReal a, dReal b, dReal c, dReal d);

		[DllImport("ode", EntryPoint = "dGeomRayGet"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomRayGet(IntPtr ray, ref Vector3 start, ref Vector3 dir);

		[DllImport("ode", EntryPoint = "dGeomRayGet"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomRayGet(IntPtr ray, ref dReal startX, ref dReal dirX);

		[DllImport("ode", EntryPoint = "dGeomRayGetClosestHit"), SuppressUnmanagedCodeSecurity]
		public static extern int GeomRayGetClosestHit(IntPtr ray);

		[DllImport("ode", EntryPoint = "dGeomRayGetLength"), SuppressUnmanagedCodeSecurity]
		public static extern dReal GeomRayGetLength(IntPtr ray);

		[DllImport("ode", EntryPoint = "dGeomRayGetParams"), SuppressUnmanagedCodeSecurity]
		public static extern dReal GeomRayGetParams(IntPtr g, out int firstContact, out int backfaceCull);

		[DllImport("ode", EntryPoint = "dGeomRaySet"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomRaySet(IntPtr ray, dReal px, dReal py, dReal pz, dReal dx, dReal dy, dReal dz);

		[DllImport("ode", EntryPoint = "dGeomRaySetClosestHit"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomRaySetClosestHit(IntPtr ray, int closestHit);

		[DllImport("ode", EntryPoint = "dGeomRaySetLength"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomRaySetLength(IntPtr ray, dReal length);

		[DllImport("ode", EntryPoint = "dGeomRaySetParams"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomRaySetParams(IntPtr ray, int firstContact, int backfaceCull);

		[DllImport("ode", EntryPoint = "dGeomSetBody"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetBody(IntPtr geom, IntPtr body);

		[DllImport("ode", EntryPoint = "dGeomSetCategoryBits"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetCategoryBits(IntPtr geom, int bits);

		[DllImport("ode", EntryPoint = "dGeomSetCollideBits"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetCollideBits(IntPtr geom, int bits);

		[DllImport("ode", EntryPoint = "dGeomSetConvex"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomSetConvex(IntPtr geom, dReal[] planes, int planeCount, dReal[] points, int pointCount, int[] polygons);

		[DllImport("ode", EntryPoint = "dGeomSetData"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetData(IntPtr geom, IntPtr data);

		[DllImport("ode", EntryPoint = "dGeomSetOffsetPosition"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetOffsetPosition(IntPtr geom, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dGeomSetOffsetQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetOffsetQuaternion(IntPtr geom, ref Quaternion Q);

		[DllImport("ode", EntryPoint = "dGeomSetOffsetQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetOffsetQuaternion(IntPtr geom, ref dReal X);

		[DllImport("ode", EntryPoint = "dGeomSetOffsetRotation"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetOffsetRotation(IntPtr geom, ref Matrix3 R);

		[DllImport("ode", EntryPoint = "dGeomSetOffsetRotation"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetOffsetRotation(IntPtr geom, ref dReal M00);

		[DllImport("ode", EntryPoint = "dGeomSetOffsetWorldPosition"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetOffsetWorldPosition(IntPtr geom, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dGeomSetOffsetWorldQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetOffsetWorldQuaternion(IntPtr geom, ref Quaternion Q);

		[DllImport("ode", EntryPoint = "dGeomSetOffsetWorldQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetOffsetWorldQuaternion(IntPtr geom, ref dReal X);

		[DllImport("ode", EntryPoint = "dGeomSetOffsetWorldRotation"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetOffsetWorldRotation(IntPtr geom, ref Matrix3 R);

		[DllImport("ode", EntryPoint = "dGeomSetOffsetWorldRotation"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetOffsetWorldRotation(IntPtr geom, ref dReal M00);

		[DllImport("ode", EntryPoint = "dGeomSetPosition"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetPosition(IntPtr geom, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dGeomSetQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetQuaternion(IntPtr geom, ref Quaternion quat);

		[DllImport("ode", EntryPoint = "dGeomSetQuaternion"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetQuaternion(IntPtr geom, ref dReal w);

		[DllImport("ode", EntryPoint = "dGeomSetRotation"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetRotation(IntPtr geom, ref Matrix3 R);

		[DllImport("ode", EntryPoint = "dGeomSetRotation"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSetRotation(IntPtr geom, ref dReal M00);

		[DllImport("ode", EntryPoint = "dGeomSphereGetRadius"), SuppressUnmanagedCodeSecurity]
		public static extern dReal GeomSphereGetRadius(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomSpherePointDepth"), SuppressUnmanagedCodeSecurity]
		public static extern dReal GeomSpherePointDepth(IntPtr geom, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dGeomSphereSetRadius"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomSphereSetRadius(IntPtr geom, dReal radius);

		[DllImport("ode", EntryPoint = "dGeomTransformGetCleanup"), SuppressUnmanagedCodeSecurity]
		public static extern int GeomTransformGetCleanup(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomTransformGetGeom"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomTransformGetGeom(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomTransformGetInfo"), SuppressUnmanagedCodeSecurity]
		public static extern int GeomTransformGetInfo(IntPtr geom);

		[DllImport("ode", EntryPoint = "dGeomTransformSetCleanup"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTransformSetCleanup(IntPtr geom, int mode);

		[DllImport("ode", EntryPoint = "dGeomTransformSetGeom"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTransformSetGeom(IntPtr geom, IntPtr obj);

		[DllImport("ode", EntryPoint = "dGeomTransformSetInfo"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTransformSetInfo(IntPtr geom, int info);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataBuildDouble"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataBuildDouble(IntPtr d,
			double[] vertices, int vertexStride, int vertexCount,
			int[] indices, int indexCount, int triStride);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataBuildDouble"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataBuildDouble(IntPtr d,
			IntPtr vertices, int vertexStride, int vertexCount,
			IntPtr indices, int indexCount, int triStride);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataBuildDouble1"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataBuildDouble1(IntPtr d,
			double[] vertices, int vertexStride, int vertexCount,
			int[] indices, int indexCount, int triStride,
			double[] normals);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataBuildDouble1"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataBuildDouble(IntPtr d,
			IntPtr vertices, int vertexStride, int vertexCount,
			IntPtr indices, int indexCount, int triStride,
			IntPtr normals);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataBuildSimple"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataBuildSingle(IntPtr d,
			dReal[] vertices, int vertexStride, int vertexCount,
			int[] indices, int indexCount, int triStride);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataBuildSimple"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataBuildSingle(IntPtr d,
			IntPtr vertices, int vertexStride, int vertexCount,
			IntPtr indices, int indexCount, int triStride);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataBuildSimple1"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataBuildSingle1(IntPtr d,
			dReal[] vertices, int vertexStride, int vertexCount,
			int[] indices, int indexCount, int triStride,
			dReal[] normals);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataBuildSimple1"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataBuildSingle1(IntPtr d,
			IntPtr vertices, int vertexStride, int vertexCount,
			IntPtr indices, int indexCount, int triStride,
			IntPtr normals);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataBuildSingle"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataBuildSimple(IntPtr d, 
			float[] vertices, int vertexStride, int vertexCount,
			int[] indices, int indexCount, int triStride);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataBuildSingle"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataBuildSimple(IntPtr d,
			IntPtr vertices, int vertexStride, int vertexCount,
			IntPtr indices, int indexCount, int triStride);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataBuildSingle1"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataBuildSimple1(IntPtr d, 
			float[] vertices, int vertexStride, int vertexCount,
			int[] indices, int indexCount, int triStride,
			float[] normals);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataBuildSingle1"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataBuildSimple1(IntPtr d,
			IntPtr vertices, int vertexStride, int vertexCount,
			IntPtr indices, int indexCount, int triStride,
			IntPtr normals);

		[DllImport("ode", EntryPoint = "dGeomTriMeshClearTCCache"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshClearTCCache(IntPtr g);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataCreate"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomTriMeshDataCreate();

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataDestroy"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataDestroy(IntPtr d);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataGet"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomTriMeshDataGet(IntPtr d, int data_id);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataPreprocess"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataPreprocess(IntPtr d);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataSet"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataSet(IntPtr d, int data_id, IntPtr in_data);

		[DllImport("ode", EntryPoint = "dGeomTriMeshDataUpdate"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshDataUpdate(IntPtr d);

		[DllImport("ode", EntryPoint = "dGeomTriMeshEnableTC"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshEnableTC(IntPtr g, int geomClass, bool enable);

		[DllImport("ode", EntryPoint = "dGeomTriMeshGetArrayCallback"), SuppressUnmanagedCodeSecurity]
		public static extern TriArrayCallback GeomTriMeshGetArrayCallback(IntPtr g);

		[DllImport("ode", EntryPoint = "dGeomTriMeshGetCallback"), SuppressUnmanagedCodeSecurity]
		public static extern TriCallback GeomTriMeshGetCallback(IntPtr g);

		[DllImport("ode", EntryPoint = "dGeomTriMeshGetData"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomTriMeshGetData(IntPtr g);

		[DllImport("ode", EntryPoint = "dGeomTriMeshGetPoint"), SuppressUnmanagedCodeSecurity]
		public extern static void GeomTriMeshGetPoint(IntPtr g, int index, dReal u, dReal v, ref Vector3 outVec);

		[DllImport("ode", EntryPoint = "dGeomTriMeshGetRayCallback"), SuppressUnmanagedCodeSecurity]
		public static extern TriRayCallback GeomTriMeshGetRayCallback(IntPtr g);

		[DllImport("ode", EntryPoint = "dGeomTriMeshGetTriangle"), SuppressUnmanagedCodeSecurity]
		public extern static void GeomTriMeshGetTriangle(IntPtr g, int index, ref Vector3 v0, ref Vector3 v1, ref Vector3 v2);

		[DllImport("ode", EntryPoint = "dGeomTriMeshGetTriangleCount"), SuppressUnmanagedCodeSecurity]
		public extern static int GeomTriMeshGetTriangleCount(IntPtr g);

		[DllImport("ode", EntryPoint = "dGeomTriMeshGetTriMeshDataID"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GeomTriMeshGetTriMeshDataID(IntPtr g);

		[DllImport("ode", EntryPoint = "dGeomTriMeshIsTCEnabled"), SuppressUnmanagedCodeSecurity]
		public static extern bool GeomTriMeshIsTCEnabled(IntPtr g, int geomClass);

		[DllImport("ode", EntryPoint = "dGeomTriMeshSetArrayCallback"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshSetArrayCallback(IntPtr g, TriArrayCallback arrayCallback);

		[DllImport("ode", EntryPoint = "dGeomTriMeshSetCallback"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshSetCallback(IntPtr g, TriCallback callback);

		[DllImport("ode", EntryPoint = "dGeomTriMeshSetData"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshSetData(IntPtr g, IntPtr data);

		[DllImport("ode", EntryPoint = "dGeomTriMeshSetLastTransform"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshSetLastTransform(IntPtr g, ref Matrix4 last_trans);

		[DllImport("ode", EntryPoint = "dGeomTriMeshSetLastTransform"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshSetLastTransform(IntPtr g, ref dReal M00);

		[DllImport("ode", EntryPoint = "dGeomTriMeshSetRayCallback"), SuppressUnmanagedCodeSecurity]
		public static extern void GeomTriMeshSetRayCallback(IntPtr g, TriRayCallback callback);

		[DllImport("ode", EntryPoint = "dHashSpaceCreate"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr HashSpaceCreate(IntPtr space);

		[DllImport("ode", EntryPoint = "dHashSpaceGetLevels"), SuppressUnmanagedCodeSecurity]
		public static extern void HashSpaceGetLevels(IntPtr space, out int minlevel, out int maxlevel);

		[DllImport("ode", EntryPoint = "dHashSpaceSetLevels"), SuppressUnmanagedCodeSecurity]
		public static extern void HashSpaceSetLevels(IntPtr space, int minlevel, int maxlevel);

		[DllImport("ode", EntryPoint = "dInfiniteAABB"), SuppressUnmanagedCodeSecurity]
		public static extern void InfiniteAABB(IntPtr geom, out AABB aabb);

		[DllImport("ode", EntryPoint = "dInitODE"), SuppressUnmanagedCodeSecurity]
		public static extern void InitODE();

		[DllImport("ode", EntryPoint = "dJointAttach"), SuppressUnmanagedCodeSecurity]
		public static extern void JointAttach(IntPtr joint, IntPtr body1, IntPtr body2);

		[DllImport("ode", EntryPoint = "dJointCreateContact"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr JointCreateContact(IntPtr world, IntPtr group, ref Contact contact);

		[DllImport("ode", EntryPoint = "dJointGroupCreate"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr JointGroupCreate(int max_size);

		[DllImport("ode", EntryPoint = "dJointGroupDestroy"), SuppressUnmanagedCodeSecurity]
		public static extern void JointGroupDestroy(IntPtr group);

		[DllImport("ode", EntryPoint = "dJointGroupEmpty"), SuppressUnmanagedCodeSecurity]
		public static extern void JointGroupEmpty(IntPtr group);

		[DllImport("ode", EntryPoint = "dMassAdd"), SuppressUnmanagedCodeSecurity]
		public static extern void MassAdd(ref Mass a, ref Mass b);

		[DllImport("ode", EntryPoint = "dMassAdjust"), SuppressUnmanagedCodeSecurity]
		public static extern void MassAdjust(ref Mass m, dReal newmass);

		[DllImport("ode", EntryPoint = "dMassCheck"), SuppressUnmanagedCodeSecurity]
		public static extern bool MassCheck(ref Mass m);

		[DllImport("ode", EntryPoint = "dMassRotate"), SuppressUnmanagedCodeSecurity]
		public static extern void MassRotate(out Mass mass, ref Matrix3 R);

		[DllImport("ode", EntryPoint = "dMassRotate"), SuppressUnmanagedCodeSecurity]
		public static extern void MassRotate(out Mass mass, ref dReal M00);

		[DllImport("ode", EntryPoint = "dMassSetBox"), SuppressUnmanagedCodeSecurity]
		public static extern void MassSetBox(out Mass mass, dReal density, dReal lx, dReal ly, dReal lz);

		[DllImport("ode", EntryPoint = "dMassSetBoxTotal"), SuppressUnmanagedCodeSecurity]
		public static extern void MassSetBoxTotal(out Mass mass, dReal total_mass, dReal lx, dReal ly, dReal lz);

		[DllImport("ode", EntryPoint = "dMassSetCapsule"), SuppressUnmanagedCodeSecurity]
		public static extern void MassSetCapsule(out Mass mass, dReal density, int direction, dReal radius, dReal length);

		[DllImport("ode", EntryPoint = "dMassSetCapsuleTotal"), SuppressUnmanagedCodeSecurity]
		public static extern void MassSetCapsuleTotal(out Mass mass, dReal total_mass, int direction, dReal radius, dReal length);

		[DllImport("ode", EntryPoint = "dMassSetCylinder"), SuppressUnmanagedCodeSecurity]
		public static extern void MassSetCylinder(out Mass mass, dReal density, int direction, dReal radius, dReal length);

		[DllImport("ode", EntryPoint = "dMassSetCylinderTotal"), SuppressUnmanagedCodeSecurity]
		public static extern void MassSetCylinderTotal(out Mass mass, dReal total_mass, int direction, dReal radius, dReal length);

		[DllImport("ode", EntryPoint = "dMassSetParameters"), SuppressUnmanagedCodeSecurity]
		public static extern void MassSetParameters(out Mass mass, dReal themass,
			 dReal cgx, dReal cgy, dReal cgz,
			 dReal i11, dReal i22, dReal i33,
			 dReal i12, dReal i13, dReal i23);

		[DllImport("ode", EntryPoint = "dMassSetSphere"), SuppressUnmanagedCodeSecurity]
		public static extern void MassSetSphere(out Mass mass, dReal density, dReal radius);

		[DllImport("ode", EntryPoint = "dMassSetSphereTotal"), SuppressUnmanagedCodeSecurity]
		public static extern void dMassSetSphereTotal(out Mass mass, dReal total_mass, dReal radius);

		[DllImport("ode", EntryPoint = "dMassSetTrimesh"), SuppressUnmanagedCodeSecurity]
		public static extern void MassSetTrimesh(out Mass mass, dReal density, IntPtr g);

		[DllImport("ode", EntryPoint = "dMassSetZero"), SuppressUnmanagedCodeSecurity]
		public static extern void MassSetZero(out Mass mass);

		[DllImport("ode", EntryPoint = "dMassTranslate"), SuppressUnmanagedCodeSecurity]
		public static extern void MassTranslate(out Mass mass, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dQuadTreeSpaceCreate"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr QuadTreeSpaceCreate(IntPtr space, ref Vector3 center, ref Vector3 extents, int depth);

		[DllImport("ode", EntryPoint = "dQuadTreeSpaceCreate"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr QuadTreeSpaceCreate(IntPtr space, ref dReal centerX, ref dReal extentsX, int depth);

		[DllImport("ode", EntryPoint = "dRandReal"), SuppressUnmanagedCodeSecurity]
		public static extern dReal RandReal();

		[DllImport("ode", EntryPoint = "dRFromAxisAndAngle"), SuppressUnmanagedCodeSecurity]
		public static extern void RFromAxisAndAngle(out Matrix3 R, dReal x, dReal y, dReal z, dReal angle);

		[DllImport("ode", EntryPoint = "dSetValue"), SuppressUnmanagedCodeSecurity]
		public static extern void SetValue(out dReal a, int n);

		[DllImport("ode", EntryPoint = "dSetZero"), SuppressUnmanagedCodeSecurity]
		public static extern void SetZero(out dReal a, int n);

		[DllImport("ode", EntryPoint = "dSimpleSpaceCreate"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr SimpleSpaceCreate(IntPtr space);

		[DllImport("ode", EntryPoint = "dSpaceAdd"), SuppressUnmanagedCodeSecurity]
		public static extern void SpaceAdd(IntPtr space, IntPtr geom);

		[DllImport("ode", EntryPoint = "dSpaceClean"), SuppressUnmanagedCodeSecurity]
		public static extern void SpaceClean(IntPtr space);

		[DllImport("ode", EntryPoint = "dSpaceCollide"), SuppressUnmanagedCodeSecurity]
		public static extern void SpaceCollide(IntPtr space, IntPtr data, NearCallback callback);

		[DllImport("ode", EntryPoint = "dSpaceCollide2"), SuppressUnmanagedCodeSecurity]
		public static extern void SpaceCollide2(IntPtr space1, IntPtr space2, IntPtr data, NearCallback callback);

		[DllImport("ode", EntryPoint = "dSpaceDestroy"), SuppressUnmanagedCodeSecurity]
		public static extern void SpaceDestroy(IntPtr space);

		[DllImport("ode", EntryPoint = "dSpaceGetCleanup"), SuppressUnmanagedCodeSecurity]
		public static extern bool SpaceGetCleanup(IntPtr space);

		[DllImport("ode", EntryPoint = "dSpaceGetNumGeoms"), SuppressUnmanagedCodeSecurity]
		public static extern int SpaceGetNumGeoms(IntPtr space);

		[DllImport("ode", EntryPoint = "dSpaceGetGeom"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr SpaceGetGeom(IntPtr space, int i);

		[DllImport("ode", EntryPoint = "dSpaceQuery"), SuppressUnmanagedCodeSecurity]
		public static extern bool SpaceQuery(IntPtr space, IntPtr geom);

		[DllImport("ode", EntryPoint = "dSpaceRemove"), SuppressUnmanagedCodeSecurity]
		public static extern void SpaceRemove(IntPtr space, IntPtr geom);

		[DllImport("ode", EntryPoint = "dSpaceSetCleanup"), SuppressUnmanagedCodeSecurity]
		public static extern void SpaceSetCleanup(IntPtr space, bool mode);

		[DllImport("ode", EntryPoint = "dWorldCreate"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr WorldCreate();

		[DllImport("ode", EntryPoint = "dWorldDestroy"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldDestroy(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldGetAutoDisableAngularThreshold"), SuppressUnmanagedCodeSecurity]
		public static extern dReal WorldGetAutoDisableAngularThreshold(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldGetAutoDisableFlag"), SuppressUnmanagedCodeSecurity]
		public static extern bool WorldGetAutoDisableFlag(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldGetAutoDisableLinearThreshold"), SuppressUnmanagedCodeSecurity]
		public static extern dReal WorldGetAutoDisableLinearThreshold(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldGetAutoDisableSteps"), SuppressUnmanagedCodeSecurity]
		public static extern int WorldGetAutoDisableSteps(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldGetAutoDisableTime"), SuppressUnmanagedCodeSecurity]
		public static extern dReal WorldGetAutoDisableTime(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldGetAutoEnableDepthSF1"), SuppressUnmanagedCodeSecurity]
		public static extern int WorldGetAutoEnableDepthSF1(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldGetCFM"), SuppressUnmanagedCodeSecurity]
		public static extern dReal WorldGetCFM(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldGetERP"), SuppressUnmanagedCodeSecurity]
		public static extern dReal WorldGetERP(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldGetGravity"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldGetGravity(IntPtr world, out Vector3 gravity);

		[DllImport("ode", EntryPoint = "dWorldGetGravity"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldGetGravity(IntPtr world, out dReal X);

		[DllImport("ode", EntryPoint = "dWorldGetContactMaxCorrectingVel"), SuppressUnmanagedCodeSecurity]
		public static extern dReal WorldGetContactMaxCorrectingVel(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldGetContactSurfaceLayer"), SuppressUnmanagedCodeSecurity]
		public static extern dReal WorldGetContactSurfaceLayer(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldGetQuickStepNumIterations"), SuppressUnmanagedCodeSecurity]
		public static extern int WorldGetQuickStepNumIterations(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldGetQuickStepW"), SuppressUnmanagedCodeSecurity]
		public static extern dReal WorldGetQuickStepW(IntPtr world);

		[DllImport("ode", EntryPoint = "dWorldImpulseToForce"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldImpulseToForce(IntPtr world, dReal stepsize, dReal ix, dReal iy, dReal iz, out Vector3 force);

		[DllImport("ode", EntryPoint = "dWorldImpulseToForce"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldImpulseToForce(IntPtr world, dReal stepsize, dReal ix, dReal iy, dReal iz, out dReal forceX);

		[DllImport("ode", EntryPoint = "dWorldQuickStep"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldQuickStep(IntPtr world, dReal stepsize);

		[DllImport("ode", EntryPoint = "dWorldSetAutoDisableAngularThreshold"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetAutoDisableAngularThreshold(IntPtr world, dReal angular_threshold);

		[DllImport("ode", EntryPoint = "dWorldSetAutoDisableFlag"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetAutoDisableFlag(IntPtr world, bool do_auto_disable);

		[DllImport("ode", EntryPoint = "dWorldSetAutoDisableLinearThreshold"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetAutoDisableLinearThreshold(IntPtr world, dReal linear_threshold);

		[DllImport("ode", EntryPoint = "dWorldSetAutoDisableSteps"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetAutoDisableSteps(IntPtr world, int steps);

		[DllImport("ode", EntryPoint = "dWorldSetAutoDisableTime"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetAutoDisableTime(IntPtr world, dReal time);

		[DllImport("ode", EntryPoint = "dWorldSetAutoEnableDepthSF1"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetAutoEnableDepthSF1(IntPtr world, int autoEnableDepth);

		[DllImport("ode", EntryPoint = "dWorldSetCFM"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetCFM(IntPtr world, dReal cfm);

		[DllImport("ode", EntryPoint = "dWorldSetContactMaxCorrectingVel"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetContactMaxCorrectingVel(IntPtr world, dReal vel);

		[DllImport("ode", EntryPoint = "dWorldSetContactSurfaceLayer"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetContactSurfaceLayer(IntPtr world, dReal depth);

		[DllImport("ode", EntryPoint = "dWorldSetERP"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetERP(IntPtr world, dReal erp);

		[DllImport("ode", EntryPoint = "dWorldSetGravity"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetGravity(IntPtr world, dReal x, dReal y, dReal z);

		[DllImport("ode", EntryPoint = "dWorldSetQuickStepNumIterations"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetQuickStepNumIterations(IntPtr world, int num);

		[DllImport("ode", EntryPoint = "dWorldSetQuickStepW"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldSetQuickStepW(IntPtr world, dReal over_relaxation);

		[DllImport("ode", EntryPoint = "dWorldStep"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldStep(IntPtr world, dReal stepsize);

		[DllImport("ode", EntryPoint = "dWorldStepFast1"), SuppressUnmanagedCodeSecurity]
		public static extern void WorldStepFast1(IntPtr world, dReal stepsize, int maxiterations);
	}
}
