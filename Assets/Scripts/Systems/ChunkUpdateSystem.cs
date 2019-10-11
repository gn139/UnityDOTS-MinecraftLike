using Systems;
using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

// [UpdateAfter (typeof (TerrainSpawnerSystem))]
// public class ChunkUpdateSystem : JobComponentSystem {

//     EntityQuery voxelChunkQuery;
//     private EntityQuery hideVoxelChunkQuery;
//     EntityQuery playerQuery;
//     EntityQuery voxelQuery;
//     EntityQuery terrainSpawnerQuery;
//     BeginInitializationEntityCommandBufferSystem entityCommandBufferSystem;
//     protected override void OnCreate () {
//         entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem> ();
//         playerQuery = GetEntityQuery (new EntityQueryDesc {
//             All = new ComponentType[] {
//                     typeof (Translation),
//                     typeof (PlayerTag)
//                 },
//                 None = new ComponentType[] {
//                     typeof (PlayerNotInGround)
//                 }
//         });
//         voxelChunkQuery = GetEntityQuery (new EntityQueryDesc {
//             All = new ComponentType[] {
//                 typeof (VoxelChunkRelativePosition),
//                 typeof (VoxelChunkTag),
//                 typeof (VoxelCount)
//             }
//         });
//         hideVoxelChunkQuery = GetEntityQuery (new EntityQueryDesc {
//             All = new ComponentType[] {
//                 typeof (VoxelChunkRelativePosition),
//                 typeof (VoxelChunkTag),
//                 typeof (VoxelCount),
//                 typeof (VoxelChunkHidden)
//             },
//         });
//         voxelQuery = GetEntityQuery (new EntityQueryDesc {
//             All = new ComponentType[] {
//                 typeof (VoxelPosition),
//                 typeof (VoxelTag),
//                 typeof (VoxelParentIndex)
//             }
//         });
//         Ent
//     }

//     [RequireComponentTag (typeof (PlayerTag))]
//     struct ChunkHideJob : IJobForEachWithEntity<Translation> {
//         public EntityCommandBuffer.Concurrent CommandBuffer;
//         public int ViewChunk;
//         [ReadOnly] public NativeArray<Entity> VoxelChunks;
//         [ReadOnly] public ComponentDataFromEntity<VoxelCount> VoxelCounts;
//         [ReadOnly] public ComponentDataFromEntity<VoxelChunkRelativePosition> RelativePostions;
//         public void Execute (Entity entity, int index, [ReadOnly] ref Translation position) {
//             if (VoxelChunks.Length < 1)
//                 return;

//             int cx = (int) position.Value.x / VoxelCounts[VoxelChunks[0]].Value;
//             int cz = (int) position.Value.z / VoxelCounts[VoxelChunks[0]].Value;
//             foreach (var item in VoxelChunks) {
//                 //hide voxelChunk in the distance
//                 if (RelativePostions[item].X >= cx - ViewChunk || RelativePostions[item].X < cx + ViewChunk ||
//                     RelativePostions[item].Z >= cx - ViewChunk || RelativePostions[item].Z < cz + ViewChunk)
//                     CommandBuffer.AddComponent (index, entity, new VoxelChunkNeedHide ());
//             }
//         }
//     }

//     [RequireComponentTag (typeof (PlayerTag))]
//     struct ChunkShowJob : IJobForEachWithEntity<Translation> {
//         public EntityCommandBuffer.Concurrent CommandBuffer;
//         public int ViewChunk;
//         [ReadOnly] public NativeArray<Entity> VoxelChunks;
//         [ReadOnly] public ComponentDataFromEntity<VoxelCount> VoxelCounts;
//         [ReadOnly] public ComponentDataFromEntity<VoxelChunkRelativePosition> RelativePostions;
//         public void Execute (Entity entity, int index, [ReadOnly] ref Translation position) {
//             if (VoxelChunks.Length < 1)
//                 return;

//             int cx = (int) position.Value.x / VoxelCounts[VoxelChunks[0]].Value;
//             int cz = (int) position.Value.z / VoxelCounts[VoxelChunks[0]].Value;
//             for (int x = cx - ViewChunk; x < cx + ViewChunk; x++) {
//                 for (int z = cz - ViewChunk; z < cz + ViewChunk; z++) {
//                     foreach (var item in VoxelChunks) {
//                         //show voxelChunk at hand
//                         if (x != RelativePostions[item].X && z != RelativePostions[item].Z) {

//                             continue;
//                         }
//                         CommandBuffer.RemoveComponent<VoxelChunkHidden> (index, item);
//                     }
//                 }
//             }

//         }

//     }

//     protected override JobHandle OnUpdate (JobHandle inputDependencies) {
//         var job = new ChunkUpdateSystemJob ();

//         // Assign values to the fields on your job here, so that it has
//         // everything it needs to do its work when it runs later.
//         // For example,
//         //     job.deltaTime = UnityEngine.Time.deltaTime;

//         // Now that the job is set up, schedule it to be run. 
//         return job.Schedule (this, inputDependencies);
//     }
// }