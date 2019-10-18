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

namespace Systems {
    [UpdateAfter (typeof (ChunkUpdateSystem))]
    public class ChunkMeshSystem : ComponentSystem {

        protected override void OnUpdate () {

            Entities.WithAllReadOnly<VoxelChunkTag, VoxelChunkChanged> ().ForEach ((Entity entity, RenderMesh renderMesh) => {
                var mesh = renderMesh.mesh;
                var vertices = EntityManager.GetBuffer<Vertex> (entity).Reinterpret<float3> ();
                var normals = EntityManager.GetBuffer<Normal> (entity).Reinterpret<Vector3> ().ToNativeArray (Allocator.Temp);
                var triangles = EntityManager.GetBuffer<Triangle> (entity).Reinterpret<int> ().ToNativeArray (Allocator.Temp);
                var float3Verts = vertices.ToNativeArray (Allocator.Temp);
                PostUpdateCommands.AddComponent (entity, new PhysicsCollider {
                    Value = MeshCollider.Create (vertices.ToNativeArray(Allocator.Temp), triangles)
                });
                float3Verts.Dispose();
                var vector3verts = vertices.Reinterpret<Vector3>().ToNativeArray(Allocator.Temp);
                mesh.vertices = vector3verts.ToArray ();
                mesh.normals = normals.ToArray ();
                mesh.triangles = triangles.ToArray ();
                vector3verts.Dispose ();
                normals.Dispose ();
                triangles.Dispose ();
                PostUpdateCommands.RemoveComponent<VoxelChunkChanged> (entity);
            });
        }
    }
}