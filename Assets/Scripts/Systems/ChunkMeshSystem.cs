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

namespace Systems {
    [UpdateAfter (typeof (TerrainSpawnerSystem))]
    public class ChunkMeshSystem : ComponentSystem {

        protected override void OnUpdate () {

            Entities.WithAllReadOnly<VoxelChunkTag, VoxelChunkChanged> ().ForEach ((Entity entity, RenderMesh renderMesh) => {
                var mesh = renderMesh.mesh;
                var vertices = EntityManager.GetBuffer<Vertex> (entity).Reinterpret<Vector3> ().ToNativeArray (Allocator.Temp);
                var normals = EntityManager.GetBuffer<Normal> (entity).Reinterpret<Vector3> ().ToNativeArray (Allocator.Temp);
                var triangles = EntityManager.GetBuffer<Triangle> (entity).Reinterpret<int> ().ToNativeArray (Allocator.Temp);
                mesh.vertices = vertices.ToArray ();
                mesh.normals = normals.ToArray ();
                mesh.triangles = triangles.ToArray ();
                vertices.Dispose ();
                normals.Dispose ();
                triangles.Dispose ();
                PostUpdateCommands.RemoveComponent<VoxelChunkChanged>(entity);
            });
        }
    }
}