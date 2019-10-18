using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Voxel;
using Random = Unity.Mathematics.Random;

namespace Systems {
    public class TerrainSpawnerSystem : JobComponentSystem {
        BeginInitializationEntityCommandBufferSystem entityCommandBufferSystem;

        EntityQuery chunkQuery;
        EntityQuery spawnerQuery;

        EntityQuery playerQuery;
        EntityQuery prefabQuery;

        NativeArray<float3> playerPos;

        Random random;

        // Texture2D heightMap;

        protected override void OnCreate () {
            entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem> ();
            spawnerQuery = GetEntityQuery (new EntityQueryDesc {
                All = new [] { ComponentType.ReadOnly<TerrainSpawner> (), ComponentType.ReadOnly<TerrainSpawnerTag> () }
            });
            chunkQuery = GetEntityQuery (new EntityQueryDesc {
                All = new [] {
                    ComponentType.ReadOnly<VoxelChunkTag> (),
                        // ComponentType.ReadOnly<WithOutVoxel> (),
                        ComponentType.ReadOnly<VoxelCount> (),
                        ComponentType.ReadOnly<VoxelChunkRelativePosition> ()
                }
            });
            playerQuery = GetEntityQuery (ComponentType.ReadOnly<PlayerTag> (),
                ComponentType.ReadWrite<Translation> (), ComponentType.ReadOnly<PlayerNotInGround> ());
            prefabQuery = GetEntityQuery (typeof (Prefabs));
            random = new Unity.Mathematics.Random ();
            random.InitState ();

            // heightMap = PerlinNoiseUtil.GenerateHeightMap (random.NextFloat (0, 99999), random.NextFloat (0, 99999));

        }

        protected override void OnStartRunning () {
            if (spawnerQuery.CalculateEntityCount () < 0)
                return;
            var spawners = spawnerQuery.ToComponentDataArray<TerrainSpawner> (Allocator.TempJob);
            if (prefabQuery.CalculateEntityCount () == 0)
                EntityManager.CreateEntity (typeof (Prefabs));
            prefabQuery.SetSingleton (spawners[0].Prefabs);
            spawners.Dispose();
        }

        [RequireComponentTag (typeof (TerrainSpawnerTag))]
        struct SpawnChunkJob : IJobForEachWithEntity<TerrainSpawner> {
            public EntityCommandBuffer.Concurrent CommandBuffer;
            // public NativeArray<float3> PlayerPostions;
            public void Execute (Entity entity,
                int index, [ReadOnly] ref TerrainSpawner spawner) {
                // PlayerPostions[0] = new float3 (spawner.Count >> 1, 0, spawner.Count >> 1);
                for (int x = 0; x < spawner.Count; x++) {
                    for (int z = 0; z < spawner.Count; z++) {
                        // var voxelChunk = CommandBuffer.CreateEntity (index);
                        var voxelChunk = CommandBuffer.Instantiate (index, spawner.Prefabs.NonePrefab);
                        CommandBuffer.AddBuffer<Vertex> (index, voxelChunk);
                        CommandBuffer.AddBuffer<Uv> (index, voxelChunk);
                        CommandBuffer.AddBuffer<Normal> (index, voxelChunk);
                        CommandBuffer.AddBuffer<Triangle> (index, voxelChunk);
                        CommandBuffer.AddBuffer<BlockVoxel> (index, voxelChunk);
                        CommandBuffer.AddComponent (index, voxelChunk, new VoxelChunkTag ());
                        CommandBuffer.AddComponent (index, voxelChunk, new VoxelChunkChanged ());
                        CommandBuffer.AddComponent (index, voxelChunk, new VoxelChunkRelativePosition {
                            X = x,
                                Y = 0,
                                Z = z
                        });
                        CommandBuffer.AddComponent (index, voxelChunk, new VoxelCount {
                            Value = 20
                        });
                        CommandBuffer.SetComponent (index, voxelChunk, new Translation {
                            Value = new float3 (x * 20, 0, z * 20)
                        });
                    }
                }
                CommandBuffer.RemoveComponent<TerrainSpawnerTag> (index, entity);
            }
        }


        // [RequireComponentTag (typeof (PlayerNotInGround))]
        // struct PutPlayerInGroundJob : IJobForEachWithEntity<Translation> {
        //     public EntityCommandBuffer.Concurrent CommandBuffer;
        //     [DeallocateOnJobCompletion] public NativeArray<float3> PlayerPostions;
        //     public void Execute (Entity entity, int index, ref Translation position) {
        //         if (PlayerPostions[0].y == 0)
        //             return;
        //         position.Value = PlayerPostions[0];
        //         CommandBuffer.RemoveComponent<PlayerNotInGround> (index, entity);
        //     }
        // }

        protected override JobHandle OnUpdate (JobHandle inputDeps) {
            // playerPos = new NativeArray<float3> (1, Allocator.TempJob);
            var chunkSpwanerHandle = new SpawnChunkJob {
                CommandBuffer = entityCommandBufferSystem.CreateCommandBuffer ().ToConcurrent (),
                    // PlayerPostions = playerPos,
            }.Schedule (spawnerQuery, inputDeps);

            entityCommandBufferSystem.AddJobHandleForProducer (chunkSpwanerHandle);

            // var voxelSpwanerHandle = new SpawnVoxelJob {
            //     CommandBuffer = entityCommandBufferSystem.CreateCommandBuffer ().ToConcurrent (),
            //         PlayerPostions = playerPos,
            //         Prefabs = prefabQuery.GetSingleton<Prefabs> (),
            //         Random = random,
            //         Vertices = GetBufferFromEntity<Vertex> (),
            //         // Uvs = GetBufferFromEntity<Uv> (),
            //         Triangles = GetBufferFromEntity<Triangle> (),
            //         Normals = GetBufferFromEntity<Normal> (),
            //         BlockVoxels = GetBufferFromEntity<BlockVoxel> ()
            // }.Schedule (chunkQuery, chunkSpwanerHandle);

            // entityCommandBufferSystem.AddJobHandleForProducer (voxelSpwanerHandle);
            // voxelSpwanerHandle.Complete ();

            // var putPlayerHandle = new PutPlayerInGroundJob {
            //     CommandBuffer = entityCommandBufferSystem.CreateCommandBuffer ().ToConcurrent (),
            //         PlayerPostions = playerPos
            // }.Schedule (playerQuery, chunkSpwanerHandle);

            // entityCommandBufferSystem.AddJobHandleForProducer (putPlayerHandle);

            return chunkSpwanerHandle;
        }
    }
}