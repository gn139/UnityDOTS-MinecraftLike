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
                        ComponentType.ReadOnly<WithOutVoxel> (),
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
        }

        [RequireComponentTag (typeof (TerrainSpawnerTag))]
        struct SpawnChunkJob : IJobForEachWithEntity<TerrainSpawner> {
            public EntityCommandBuffer.Concurrent CommandBuffer;
            public NativeArray<float3> PlayerPostions;
            public void Execute (Entity entity,
                int index, [ReadOnly] ref TerrainSpawner spawner) {
                PlayerPostions[0] = new float3 (spawner.Count >> 1, 0, spawner.Count >> 1);
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
                        CommandBuffer.AddComponent (index, voxelChunk, new WithOutVoxel ());
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

        [RequireComponentTag (typeof (VoxelChunkTag), typeof (WithOutVoxel))]
        struct SpawnVoxelJob : IJobForEachWithEntity<VoxelCount> {
            public EntityCommandBuffer.Concurrent CommandBuffer;

            public NativeArray<float3> PlayerPostions;

            [NativeDisableParallelForRestriction] public BufferFromEntity<Vertex> Vertices;
            // [NativeDisableParallelForRestriction] public BufferFromEntity<Uv> Uvs;
            [NativeDisableParallelForRestriction] public BufferFromEntity<Normal> Normals;
            [NativeDisableParallelForRestriction] public BufferFromEntity<Triangle> Triangles;
            [NativeDisableParallelForRestriction] public BufferFromEntity<BlockVoxel> BlockVoxels;

            public Random Random;
            // [ReadOnly] public Texture2D heightMap;
            [ReadOnly] public Prefabs Prefabs;

            [NativeDisableParallelForRestriction] DynamicBuffer<float3> vertices;
            // [NativeDisableParallelForRestriction] DynamicBuffer<float2> uvs;
            [NativeDisableParallelForRestriction] DynamicBuffer<float3> normals;
            [NativeDisableParallelForRestriction] DynamicBuffer<int> triangles;
            [NativeDisableParallelForRestriction] DynamicBuffer<BlockVoxel> blocks;
            public void Execute (Entity entity,
                int index, [ReadOnly] ref VoxelCount count) {

                vertices = Vertices[entity].Reinterpret<float3> ();
                // uvs = Uvs[entity].Reinterpret<float2> ();
                normals = Normals[entity].Reinterpret<float3> ();
                triangles = Triangles[entity].Reinterpret<int> ();
                blocks = BlockVoxels[entity];

                for (int x = 0; x < count.Value; x++) {
                    for (int z = 0; z < count.Value; z++) {
                        // int hightlevel = (int) (heightMap.GetPixel (x + position.X * count.Value,
                        //     z + position.Z * count.Value).r * 100) - y;
                        // int worldX = x + position.X * count.Value;
                        // int worldZ = z + position.Z * count.Value;
                        // float height = noise.snoise (new float2 (worldX, worldZ) * 0.08F) * 2 + 5;
                        // height = height >= count.Value ? count.Value : height;
                        for (int y = 0; y < count.Value; y++) {
                            blocks.Add (new BlockVoxel (VoxelState.Already));

                            // var voxel = RandomCreateVoxel (index, Prefabs, y, height);
                            // var voxel = CommandBuffer.CreateEntity(index);
                            // CommandBuffer.AddComponent (index, voxel, new VoxelTag ());
                            // CommandBuffer.AddComponent(index, voxel, new VoxelType{
                            //     Value = VoxelState.Already
                            // });
                            // CommandBuffer.SetComponent (index, voxel, new Translation {
                            //     Value = new float3 (worldX, y, worldZ)
                            // });
                            // CommandBuffer.AddComponent (index, voxel, new VoxelParentIndex {
                            //     Value = entity.Index
                            // });
                            // CommandBuffer.AddComponent (index, voxel, new VoxelPosition {
                            //     Value = new float3 (x, y, z)
                            // });
                            if (y == count.Value - 1) {
                                AddFace (x, y, z, Face.Up);
                            }
                            if (y == 0) {

                                AddFace (x, y, z, Face.Down);
                            }
                            if (x == 0) {

                                AddFace (x, y, z, Face.Left);
                            }
                            if (x == count.Value - 1) {

                                AddFace (x, y, z, Face.Right);
                            }
                            if (z == count.Value - 1) {

                                AddFace (x, y, z, Face.Front);
                            }
                            if (z == 0) {

                                AddFace (x, y, z, Face.Back);
                            }
                        }
                        // if (PlayerPostions[0].x == position.X &&
                        //     PlayerPostions[0].z == position.Z &&
                        //     x == count.Value >> 1 && z == count.Value >> 1) {
                        //     PlayerPostions[0] = new float3 (worldX, height + 1, worldZ);
                        // }
                        // for (int y = 0; y < count.Value; y++) {
                        //     var voxel = CommandBuffer.Instantiate (index, prefab.Prefab);
                        //     CommandBuffer.AddComponent (index, voxel, new VoxelTag ());
                        //     CommandBuffer.SetComponent (index, voxel, new Translation {
                        //         Value = new float3 (worldX, y, worldZ)
                        //     });
                        // }
                    }
                }
                CommandBuffer.RemoveComponent<WithOutVoxel> (index, entity);
                CommandBuffer.AddComponent<VoxelChunkChanged> (index, entity);
            }

            void AddFace (int x, int y, int z, Face face) {
                int n = vertices.Length;
                triangles.Add (n);
                triangles.Add (n + 2);
                triangles.Add (n + 1);
                triangles.Add (n);
                triangles.Add (n + 3);
                triangles.Add (n + 2);

                switch (face) {
                    case Face.Up:
                        {
                            vertices.Add (new float3 (x, y + 1, z));
                            vertices.Add (new float3 (x + 1, y + 1, z));
                            vertices.Add (new float3 (x + 1, y + 1, z + 1));
                            vertices.Add (new float3 (x, y + 1, z + 1));

                            break;
                        }
                    case Face.Down:
                        {
                            vertices.Add (new float3 (x, y, z));
                            vertices.Add (new float3 (x, y, z + 1));
                            vertices.Add (new float3 (x + 1, y, z + 1));
                            vertices.Add (new float3 (x + 1, y, z));
                            break;
                        }
                    case Face.Left:
                        {
                            vertices.Add (new float3 (x, y + 1, z));
                            vertices.Add (new float3 (x, y + 1, z + 1));
                            vertices.Add (new float3 (x, y, z + 1));
                            vertices.Add (new float3 (x, y, z));
                            break;
                        }
                    case Face.Right:
                        {
                            vertices.Add (new float3 (x + 1, y + 1, z + 1));
                            vertices.Add (new float3 (x + 1, y + 1, z));
                            vertices.Add (new float3 (x + 1, y, z));
                            vertices.Add (new float3 (x + 1, y, z + 1));
                            break;
                        }
                    case Face.Front:
                        {
                            vertices.Add (new float3 (x, y + 1, z + 1));
                            vertices.Add (new float3 (x + 1, y + 1, z + 1));
                            vertices.Add (new float3 (x + 1, y, z + 1));
                            vertices.Add (new float3 (x, y, z + 1));
                            break;
                        }
                    case Face.Back:
                        {
                            vertices.Add (new float3 (x + 1, y + 1, z));
                            vertices.Add (new float3 (x, y + 1, z));
                            vertices.Add (new float3 (x, y, z));
                            vertices.Add (new float3 (x + 1, y, z));
                            break;
                        }
                }
                var normal = math.cross (vertices[n + 2] - vertices[n], vertices[n + 1] - vertices[n]);
                normals.Add (normal);
                normals.Add (normal);
                normals.Add (normal);
                normals.Add (normal);
            }

            // Entity RandomCreateVoxel (int jobIndex, Prefabs prefabs, int y, float height) {
            //     if (y + 1 > height) {
            //         var randomFactroy = Random.NextInt (0, 39);
            //         if (randomFactroy < 10)
            //             return CommandBuffer.Instantiate (jobIndex, prefabs.GroundGreen);
            //         if (randomFactroy < 20)
            //             return CommandBuffer.Instantiate (jobIndex, prefabs.GroundBrown);
            //         if (randomFactroy < 30)
            //             return CommandBuffer.Instantiate (jobIndex, prefabs.GreyStone);
            //         // if (randomFactroy < 40)
            //         //     return CommandBuffer.Instantiate (jobIndex, prefabs.Ground3);
            //         // if (randomFactroy < 50)
            //         //     return CommandBuffer.Instantiate (jobIndex, prefabs.Ground4);
            //         // if (randomFactroy < 60)
            //         //     return CommandBuffer.Instantiate (jobIndex, prefabs.Ground5);
            //         // if (randomFactroy < 70)
            //         //     return CommandBuffer.Instantiate (jobIndex, prefabs.GroundVegetation0);
            //         // if (randomFactroy < 80)
            //         //     return CommandBuffer.Instantiate (jobIndex, prefabs.GroundVegetation1);
            //         // if (randomFactroy < 90)
            //         //     return CommandBuffer.Instantiate (jobIndex, prefabs.GroundVegetation2);
            //         // if (randomFactroy < 100)
            //         //     return CommandBuffer.Instantiate (jobIndex, prefabs.GroundVegetation3);
            //         // if (randomFactroy < 110)
            //         //     return CommandBuffer.Instantiate (jobIndex, prefabs.GroundVegetation4);
            //         // if (randomFactroy < 120)
            //         //     return CommandBuffer.Instantiate (jobIndex, prefabs.GroundVegetation5);
            //     }
            //     if (y >= 0) {
            //         var randomFactroy = Random.NextInt (0, 39);
            //         if (randomFactroy < 10)
            //             return CommandBuffer.Instantiate (jobIndex, prefabs.GroundBrown);
            //         if (randomFactroy < 20)
            //             return CommandBuffer.Instantiate (jobIndex, prefabs.Sand);
            //         if (randomFactroy < 30)
            //             return CommandBuffer.Instantiate (jobIndex, prefabs.GreyStone);
            //     }

            //     return CommandBuffer.Instantiate (jobIndex, prefabs.GroundBrown);

            // }
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
            playerPos = new NativeArray<float3> (1, Allocator.TempJob);
            var chunkSpwanerHandle = new SpawnChunkJob {
                CommandBuffer = entityCommandBufferSystem.CreateCommandBuffer ().ToConcurrent (),
                    PlayerPostions = playerPos,
            }.Schedule (spawnerQuery, inputDeps);

            entityCommandBufferSystem.AddJobHandleForProducer (chunkSpwanerHandle);

            var voxelSpwanerHandle = new SpawnVoxelJob {
                CommandBuffer = entityCommandBufferSystem.CreateCommandBuffer ().ToConcurrent (),
                    PlayerPostions = playerPos,
                    Prefabs = prefabQuery.GetSingleton<Prefabs> (),
                    Random = random,
                    Vertices = GetBufferFromEntity<Vertex> (),
                    // Uvs = GetBufferFromEntity<Uv> (),
                    Triangles = GetBufferFromEntity<Triangle> (),
                    Normals = GetBufferFromEntity<Normal> (),
                    BlockVoxels = GetBufferFromEntity<BlockVoxel> ()
            }.Schedule (chunkQuery, chunkSpwanerHandle);

            entityCommandBufferSystem.AddJobHandleForProducer (voxelSpwanerHandle);
            voxelSpwanerHandle.Complete ();

            // var putPlayerHandle = new PutPlayerInGroundJob {
            //     CommandBuffer = entityCommandBufferSystem.CreateCommandBuffer ().ToConcurrent (),
            //         PlayerPostions = playerPos
            // }.Schedule (playerQuery, voxelSpwanerHandle);

            // entityCommandBufferSystem.AddJobHandleForProducer (putPlayerHandle);

            return voxelSpwanerHandle;
        }
    }
}