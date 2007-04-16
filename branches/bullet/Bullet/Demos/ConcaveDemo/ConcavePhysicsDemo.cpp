/*
Bullet Continuous Collision Detection and Physics Library
Copyright (c) 2003-2006 Erwin Coumans  http://continuousphysics.com/Bullet/

This software is provided 'as-is', without any express or implied warranty.
In no event will the authors be held liable for any damages arising from the use of this software.
Permission is granted to anyone to use this software for any purpose, 
including commercial applications, and to alter it and redistribute it freely, 
subject to the following restrictions:

1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
3. This notice may not be removed or altered from any source distribution.
*/

#include "btBulletDynamicsCommon.h"
#include "LinearMath/btIDebugDraw.h"
#include "GLDebugDrawer.h"
#include "ConcaveDemo.h"
#include "GL_ShapeDrawer.h"
#include "GlutStuff.h"


GLDebugDrawer	debugDrawer;


btVector3*	gVertices=0;
int*	gIndices=0;
btBvhTriangleMeshShape* trimeshShape =0;
btRigidBody* staticBody = 0;
static float waveheight = 5.f;

const float TRIANGLE_SIZE=8.f;



///User can override this material combiner by implementing gContactAddedCallback and setting body0->m_collisionFlags |= btCollisionObject::customMaterialCallback;
inline btScalar	calculateCombinedFriction(float friction0,float friction1)
{
	btScalar friction = friction0 * friction1;

	const btScalar MAX_FRICTION  = 10.f;
	if (friction < -MAX_FRICTION)
		friction = -MAX_FRICTION;
	if (friction > MAX_FRICTION)
		friction = MAX_FRICTION;
	return friction;

}

inline btScalar	calculateCombinedRestitution(float restitution0,float restitution1)
{
	return restitution0 * restitution1;
}



bool CustomMaterialCombinerCallback(btManifoldPoint& cp,	const btCollisionObject* colObj0,int partId0,int index0,const btCollisionObject* colObj1,int partId1,int index1)
{

	float friction0 = colObj0->getFriction();
	float friction1 = colObj1->getFriction();
	float restitution0 = colObj0->getRestitution();
	float restitution1 = colObj1->getRestitution();

	if (colObj0->getCollisionFlags() & btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK)
	{
		friction0 = 1.0;//partId0,index0
		restitution0 = 0.f;
	}
	if (colObj1->getCollisionFlags() & btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK)
	{
		if (index1&1)
		{
			friction1 = 1.0f;//partId1,index1
		} else
		{
			friction1 = 0.f;
		}
		restitution1 = 0.f;
	}

	cp.m_combinedFriction = calculateCombinedFriction(friction0,friction1);
	cp.m_combinedRestitution = calculateCombinedRestitution(restitution0,restitution1);

	//this return value is currently ignored, but to be on the safe side: return false if you don't calculate friction
	return true;
}

extern ContactAddedCallback		gContactAddedCallback;



int main(int argc,char** argv)
{
	gContactAddedCallback = CustomMaterialCombinerCallback;

	ConcaveDemo* concaveDemo = new ConcaveDemo();
	concaveDemo->initPhysics();
	concaveDemo->setCameraDistance(30.f);

	return glutmain(argc, argv,640,480,"Static Concave Mesh Demo",concaveDemo);
}


	const int NUM_VERTS_X = 50;
	const int NUM_VERTS_Y = 50;
	const int totalVerts = NUM_VERTS_X*NUM_VERTS_Y;

void	ConcaveDemo::setVertexPositions(float waveheight, float offset)
{
	int i;
	int j;

	for ( i=0;i<NUM_VERTS_X;i++)
	{
		for (j=0;j<NUM_VERTS_Y;j++)
		{
			gVertices[i+j*NUM_VERTS_X].setValue((i-NUM_VERTS_X*0.5f)*TRIANGLE_SIZE,
				//0.f,
				waveheight*sinf((float)i+offset)*cosf((float)j+offset),
				(j-NUM_VERTS_Y*0.5f)*TRIANGLE_SIZE);
		}
	}
}
void	ConcaveDemo::initPhysics()
{
	#define TRISIZE 10.f

	int vertStride = sizeof(btVector3);
	int indexStride = 3*sizeof(int);

	
	const int totalTriangles = 2*(NUM_VERTS_X-1)*(NUM_VERTS_Y-1);

	gVertices = new btVector3[totalVerts];
	gIndices = new int[totalTriangles*3];

	int i;


	setVertexPositions(waveheight,0.f);

	int index=0;
	for ( i=0;i<NUM_VERTS_X-1;i++)
	{
		for (int j=0;j<NUM_VERTS_Y-1;j++)
		{
			gIndices[index++] = j*NUM_VERTS_X+i;
			gIndices[index++] = j*NUM_VERTS_X+i+1;
			gIndices[index++] = (j+1)*NUM_VERTS_X+i+1;

			gIndices[index++] = j*NUM_VERTS_X+i;
			gIndices[index++] = (j+1)*NUM_VERTS_X+i+1;
			gIndices[index++] = (j+1)*NUM_VERTS_X+i;
		}
	}
	
	btTriangleIndexVertexArray* indexVertexArrays = new btTriangleIndexVertexArray(totalTriangles,
		gIndices,
		indexStride,
		totalVerts,(btScalar*) &gVertices[0].x(),vertStride);

	bool useQuantizedAabbCompression = true;
	trimeshShape  = new btBvhTriangleMeshShape(indexVertexArrays,useQuantizedAabbCompression);


//	btCollisionShape* groundShape = new btBoxShape(btVector3(50,3,50));
	btCollisionDispatcher* dispatcher = new btCollisionDispatcher();
	btVector3 worldMin(-1000,-1000,-1000);
	btVector3 worldMax(1000,1000,1000);
	btOverlappingPairCache* pairCache = new btAxisSweep3(worldMin,worldMax);
	btConstraintSolver* constraintSolver = new btSequentialImpulseConstraintSolver();
	m_dynamicsWorld = new btDiscreteDynamicsWorld(dispatcher,pairCache,constraintSolver);
	m_dynamicsWorld->setDebugDrawer(&debugDrawer);
	
	float mass = 0.f;
	btTransform	startTransform;
	startTransform.setIdentity();
	startTransform.setOrigin(btVector3(0,-2,0));

	staticBody = localCreateRigidBody(mass, startTransform,trimeshShape);

	staticBody->setCollisionFlags(staticBody->getCollisionFlags() | btCollisionObject::CF_STATIC_OBJECT);

	//enable custom material callback
	staticBody->setCollisionFlags(staticBody->getCollisionFlags()  | btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK);

	{
		for (int i=0;i<10;i++)
		{
			btCollisionShape* boxShape = new btBoxShape(btVector3(1,1,1));
			startTransform.setOrigin(btVector3(2*i,10,1));
			localCreateRigidBody(1, startTransform,boxShape);
		}
	}
	
}

void ConcaveDemo::clientMoveAndDisplay()
{
	 glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT); 

	float dt = m_clock.getTimeMicroseconds() * 0.000001f;
	m_clock.reset();

	
	

	static float offset=0.f;
	offset+=0.01f;

	setVertexPositions(waveheight,offset);

	trimeshShape->refitTree();

	//clear all contact points involving mesh proxy. Note: this is a slow/unoptimized operation.
	m_dynamicsWorld->getBroadphase()->cleanProxyFromPairs(staticBody->getBroadphaseHandle());

	m_dynamicsWorld->stepSimulation(dt);
	
	renderme();

    glFlush();
    glutSwapBuffers();

}




void ConcaveDemo::displayCallback(void) {

    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT); 

	renderme();

    glFlush();
    glutSwapBuffers();
}


