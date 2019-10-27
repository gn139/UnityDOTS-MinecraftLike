using Systems;
using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;
using Unity.Physics;
using MeshCollider = Unity.Physics.MeshCollider;
using System;

namespace Systems {
    [UpdateAfter (typeof (ChunkUpdateSystem))]
    public class ChunkMeshSystem : ComponentSystem {

        protected override void OnUpdate () {

            Entities.WithAllReadOnly<VoxelChunkTag, VoxelChunkChanged> ()
                .ForEach ((Entity entity, RenderMesh renderMesh) => {

                    var vertices = EntityManager.GetBuffer<Vertex> (entity).Reinterpret<float3> ();
                    var normals = EntityManager.GetBuffer<Normal> (entity).Reinterpret<Vector3> ().ToNativeArray (Allocator.Temp);
                    var triangles = EntityManager.GetBuffer<Triangle> (entity).Reinterpret<int> ().ToNativeArray (Allocator.Temp);
                    if (vertices.Length != normals.Length || triangles.Length / 3 != vertices.Length / 2)
                        return;

                    var mesh = new Mesh ();

                    var position = EntityManager.GetComponentData<VoxelChunkRelativePosition> (entity);
                    Debug.Log (position.Y);

                    var vector3verts = vertices.Reinterpret<Vector3> ().ToNativeArray (Allocator.Temp);
                    mesh.vertices = vector3verts.ToArray ();
                    mesh.normals = normals.ToArray ();
                    mesh.triangles = triangles.ToArray ();

                    // renderMesh.mesh = mesh;
                    PostUpdateCommands.SetSharedComponent(entity, new RenderMesh{
                        mesh = mesh,
                        material = renderMesh.material
                    });

                    var float3Verts = vertices.ToNativeArray (Allocator.Temp);
                    PostUpdateCommands.AddComponent (entity, new PhysicsCollider {
                        Value = MeshCollider.Create (float3Verts, triangles)
                    });

                    PostUpdateCommands.RemoveComponent<VoxelChunkChanged> (entity);

                    float3Verts.Dispose ();
                    vector3verts.Dispose ();
                    normals.Dispose ();
                    triangles.Dispose ();
                });
        }
    }
}