using Systems;
using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Voxel;

[UpdateAfter (typeof (TerrainSpawnerSystem))]
[UpdateBefore (typeof (ChunkMeshSystem))]
public class ChunkUpdateSystem : JobComponentSystem {

    EntityQuery voxelChunkQuery;
    private EntityQuery hideVoxelChunkQuery;
    EntityQuery playerQuery;
    EntityQuery countQuery;
    BeginInitializationEntityCommandBufferSystem entityCommandBufferSystem;
    protected override void OnCreate () {
        entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem> ();
        playerQuery = GetEntityQuery (new EntityQueryDesc {
            All = new ComponentType[] {
                    typeof (Translation),
                    typeof (PlayerTag)
                },
                None = new ComponentType[] {
                    typeof (PlayerNotInGround)
                }
        });
        voxelChunkQuery = GetEntityQuery (new EntityQueryDesc {
            All = new ComponentType[] {
                typeof (VoxelChunkRelativePosition),
                typeof (VoxelChunkTag),
                typeof (VoxelCount),
                typeof (VoxelChunkChanged)
            }
        });
        countQuery = GetEntityQuery (typeof (CountComponent));
        // hideVoxelChunkQuery = GetEntityQuery (new EntityQueryDesc {
        //     All = new ComponentType[] {
        //         typeof (VoxelChunkRelativePosition),
        //         typeof (VoxelChunkTag),
        //         typeof (VoxelCount),
        //         typeof (VoxelChunkHidden)
        //     },
        // });
        // voxelQuery = GetEntityQuery (new EntityQueryDesc {
        //     All = new ComponentType[] {
        //         typeof (VoxelPosition),
        //         typeof (VoxelTag),
        //         typeof (VoxelParentIndex)
        //     }
        // });
    }

    [BurstCompile]
    [RequireComponentTag (typeof (VoxelChunkTag), typeof (VoxelChunkChanged))]
    struct ChunkMeshDataJob : IJobForEachWithEntity<VoxelCount, VoxelChunkRelativePosition> {

        // public NativeArray<float3> PlayerPostions;

        [NativeDisableParallelForRestriction] public BufferFromEntity<Vertex> Vertices;
        // [NativeDisableParallelForRestriction] public BufferFromEntity<Uv> Uvs;
        [NativeDisableParallelForRestriction] public BufferFromEntity<Normal> Normals;
        [NativeDisableParallelForRestriction] public BufferFromEntity<Triangle> Triangles;
        [NativeDisableParallelForRestriction] public BufferFromEntity<BlockVoxel> BlockVoxels;

        public Random Random;
        public int ChunkCount;
        // [ReadOnly] public Texture2D heightMap;
        // [ReadOnly] public Prefabs Prefabs;

        [NativeDisableParallelForRestriction] DynamicBuffer<float3> vertices;
        // [NativeDisableParallelForRestriction] DynamicBuffer<float2> uvs;
        [NativeDisableParallelForRestriction] DynamicBuffer<float3> normals;
        [NativeDisableParallelForRestriction] DynamicBuffer<int> triangles;
        [NativeDisableParallelForRestriction] DynamicBuffer<BlockVoxel> blocks;
        public void Execute (Entity entity,
            int index, [ReadOnly] ref VoxelCount count, [ReadOnly] ref VoxelChunkRelativePosition position) {

            vertices = Vertices[entity].Reinterpret<float3> ();
            // uvs = Uvs[entity].Reinterpret<float2> ();
            normals = Normals[entity].Reinterpret<float3> ();
            triangles = Triangles[entity].Reinterpret<int> ();
            blocks = BlockVoxels[entity];

            for (int x = 0; x < count.Value; x++) {
                for (int z = 0; z < count.Value; z++) {
                    // int hightlevel = (int) (heightMap.GetPixel (x + position.X * count.Value,
                    //     z + position.Z * count.Value).r * 100) - y;
                    int worldX = x + position.X * count.Value;
                    int worldZ = z + position.Z * count.Value;
                    float height = noise.snoise (new float2 (worldX, worldZ) * 0.08F) * 2 + 5;
                    height = height >= count.Value ? count.Value : height;
                    for (int y = 0; y < height; y++) {
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
                        if (y == count.Value - 1 && position.Y == ChunkCount - 1) {
                            AddFace (x, y, z, Face.Up);
                        }
                        if (y == 0 && position.Y == 0) {

                            AddFace (x, y, z, Face.Down);
                        }
                        if (x == 0 && position.X == 0) {

                            AddFace (x, y, z, Face.Left);
                        }
                        if (x == count.Value - 1 && position.X == ChunkCount - 1) {

                            AddFace (x, y, z, Face.Right);
                        }
                        if (z == 0 && position.Z == 0) {

                            AddFace (x, y, z, Face.Front);
                        }
                        if (z == count.Value - 1 && position.Z == ChunkCount - 1) {

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
        }

        void AddFace (int x, int y, int z, Face face) {
            int n = vertices.Length;
            triangles.Add (n);
            triangles.Add (n + 1);
            triangles.Add (n + 2);
            triangles.Add (n);
            triangles.Add (n + 2);
            triangles.Add (n + 3);

            switch (face) {
                case Face.Up:
                    {
                        vertices.Add (new float3 (x, y + 1, z));
                        vertices.Add (new float3 (x, y + 1, z + 1));
                        vertices.Add (new float3 (x + 1, y + 1, z + 1));
                        vertices.Add (new float3 (x + 1, y + 1, z));
                        break;
                    }
                case Face.Down:
                    {
                        vertices.Add (new float3 (x, y, z));
                        vertices.Add (new float3 (x + 1, y, z));
                        vertices.Add (new float3 (x + 1, y, z + 1));
                        vertices.Add (new float3 (x, y, z + 1));
                        break;
                    }
                case Face.Left:
                    {
                        vertices.Add (new float3 (x, y, z));
                        vertices.Add (new float3 (x, y, z + 1));
                        vertices.Add (new float3 (x, y + 1, z + 1));
                        vertices.Add (new float3 (x, y + 1, z));
                        break;
                    }
                case Face.Right:
                    {
                        vertices.Add (new float3 (x + 1, y, z));
                        vertices.Add (new float3 (x + 1, y + 1, z));
                        vertices.Add (new float3 (x + 1, y + 1, z + 1));
                        vertices.Add (new float3 (x + 1, y, z + 1));
                        break;
                    }
                case Face.Front:
                    {
                        vertices.Add (new float3 (x, y, z));
                        vertices.Add (new float3 (x, y + 1, z));
                        vertices.Add (new float3 (x + 1, y + 1, z));
                        vertices.Add (new float3 (x + 1, y, z));
                        break;
                    }
                case Face.Back:
                    {
                        vertices.Add (new float3 (x, y, z + 1));
                        vertices.Add (new float3 (x + 1, y, z + 1));
                        vertices.Add (new float3 (x + 1, y + 1, z + 1));
                        vertices.Add (new float3 (x, y + 1, z + 1));
                        break;
                    }
            }
            var normal = math.normalize (math.cross (vertices[n + 2] - vertices[n], vertices[n + 1] - vertices[n]));
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

    [RequireComponentTag (typeof (PlayerTag))]
    struct ChunkHideJob : IJobForEachWithEntity<Translation> {
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public int ViewChunk;
        [ReadOnly] public NativeArray<Entity> VoxelChunks;
        [ReadOnly] public ComponentDataFromEntity<VoxelCount> VoxelCounts;
        [ReadOnly] public ComponentDataFromEntity<VoxelChunkRelativePosition> RelativePostions;
        public void Execute (Entity entity, int index, [ReadOnly] ref Translation position) {
            if (VoxelChunks.Length < 1)
                return;

            int cx = (int) position.Value.x / VoxelCounts[VoxelChunks[0]].Value;
            int cz = (int) position.Value.z / VoxelCounts[VoxelChunks[0]].Value;
            foreach (var item in VoxelChunks) {
                //hide voxelChunk in the distance
                if (RelativePostions[item].X >= cx - ViewChunk || RelativePostions[item].X < cx + ViewChunk ||
                    RelativePostions[item].Z >= cx - ViewChunk || RelativePostions[item].Z < cz + ViewChunk) { }
                // CommandBuffer.AddComponent (index, entity, new VoxelChunkNeedHide ());
            }
        }
    }

    [RequireComponentTag (typeof (PlayerTag))]
    struct ChunkShowJob : IJobForEachWithEntity<Translation> {
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public int ViewChunk;
        [ReadOnly] public NativeArray<Entity> VoxelChunks;
        [ReadOnly] public ComponentDataFromEntity<VoxelCount> VoxelCounts;
        [ReadOnly] public ComponentDataFromEntity<VoxelChunkRelativePosition> RelativePostions;
        public void Execute (Entity entity, int index, [ReadOnly] ref Translation position) {
            if (VoxelChunks.Length < 1)
                return;

            int cx = (int) position.Value.x / VoxelCounts[VoxelChunks[0]].Value;
            int cz = (int) position.Value.z / VoxelCounts[VoxelChunks[0]].Value;
            for (int x = cx - ViewChunk; x < cx + ViewChunk; x++) {
                for (int z = cz - ViewChunk; z < cz + ViewChunk; z++) {
                    foreach (var item in VoxelChunks) {
                        //show voxelChunk at hand
                        if (x != RelativePostions[item].X && z != RelativePostions[item].Z) {

                            continue;
                        }
                        // CommandBuffer.RemoveComponent<VoxelChunkHidden> (index, item);
                    }
                }
            }

        }

    }

    int chunkCount;

    protected override void OnStartRunning () {
        chunkCount = countQuery.GetSingleton<CountComponent> ().ChunkCount;
    }

    protected override JobHandle OnUpdate (JobHandle inputDependencies) {
        inputDependencies = new ChunkMeshDataJob {
            // Random = random,
            Vertices = GetBufferFromEntity<Vertex> (),
                // Uvs = GetBufferFromEntity<Uv> (),
                Triangles = GetBufferFromEntity<Triangle> (),
                Normals = GetBufferFromEntity<Normal> (),
                BlockVoxels = GetBufferFromEntity<BlockVoxel> (),
                ChunkCount = chunkCount
        }.Schedule (voxelChunkQuery, inputDependencies);
        return inputDependencies;
    }
}