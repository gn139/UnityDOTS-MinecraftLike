using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

namespace Systems {
    [UpdateAfter (typeof (CharacterControllerSystem))]
    public class BoxWireframeSystem : ComponentSystem {
        EntityQuery playerQuery;
        private EntityQuery characterQuery;

        EntityQuery prefabsQuery;

        Entity voxel;
        Entity? lastHit;

        protected override void OnCreate () {
            characterQuery = GetEntityQuery (typeof (CharacterControllerInput));
            playerQuery = GetEntityQuery (typeof (PlayerTag));
            prefabsQuery = GetEntityQuery (typeof (Prefabs));
        }

        [BurstCompile]
        public struct RaycastJob : IJob {
            public RaycastInput RaycastInput;
            public NativeList<RaycastHit> RaycastHits;
            public bool CollectAllHits;
            [ReadOnly] public PhysicsWorld World;

            public void Execute () {
                if (CollectAllHits) {
                    World.CastRay (RaycastInput, ref RaycastHits);
                } else if (World.CastRay (RaycastInput, out RaycastHit hit)) {
                    RaycastHits.Add (hit);
                }
            }
        }

        protected override void OnStartRunning () {
            voxel = prefabsQuery.GetSingleton<Prefabs> ().GreyStone;
        }

        protected override void OnUpdate () {
            Entities.WithAllReadOnly<WireframeSpawner> ().ForEach ((Entity entity, ref WireframeSpawner spawner) => {
                PostUpdateCommands.Instantiate (spawner.Prefab);
                PostUpdateCommands.DestroyEntity (entity);
            });

            var input = characterQuery.GetSingleton<CharacterControllerInput> ();

            var players = playerQuery.ToEntityArray (Allocator.TempJob);
            if (players.Length < 0) {
                players.Dispose ();
                return;
            }
            var playerPos = EntityManager.GetComponentData<Translation> (players[0]);
            var cameraRotation = GameObject.FindGameObjectWithTag ("MainCamera").transform.rotation;
            var cameraPosition = GameObject.FindGameObjectWithTag ("MainCamera").transform.position;
            Entities.WithAllReadOnly<CubePointerTag> ().ForEach ((Entity entity, ref Translation position, ref Rotation rotation) => {
                if (!input.IsBuild && math.lengthsq (playerPos.Value - position.Value) > 1.8f) {
                    //只有在建筑模式且离玩家位置不超过2f时显示
                    position.Value = new float3 (cameraPosition.x, cameraPosition.y + 100f, cameraPosition.z);
                    return;
                }
                ref PhysicsWorld world = ref World.Active.GetExistingSystem<BuildPhysicsWorld> ().PhysicsWorld;
                var raycastHits = new NativeList<RaycastHit> (Allocator.TempJob);
                var startPos = input.Aimed ? (float3)cameraPosition : playerPos.Value;
                var raycastInput = new RaycastInput {
                    Start = startPos,
                    End = playerPos.Value + math.forward (cameraRotation) * 2,
                    Filter = CollisionFilter.Default
                };
                new RaycastJob {
                    RaycastInput = raycastInput,
                        RaycastHits = raycastHits,
                        CollectAllHits = false,
                        World = world
                }.Schedule ().Complete ();
                if (raycastHits.Length > 0) {
                    var index = raycastHits[0].RigidBodyIndex;
                    var hit = world.Bodies[ index != -1 ? index : 0].Entity;
                    var currentPos = EntityManager.GetComponentData<Translation> ((Entity) hit).Value;
                    var targetPos = new float3 (currentPos.x, currentPos.y + 0.5f, currentPos.z);
                    var isEquel = targetPos != playerPos.Value;
                    if (isEquel.x && isEquel.y && isEquel.z) {
                        position.Value = targetPos;
                        lastHit = hit;
                    }
                    if (input.Fire) {
                        PostUpdateCommands.AddComponent ((Entity) lastHit, new Disabled ());
                    }

                } else if (lastHit != null) {
                    var lastPos = EntityManager.GetComponentData<Translation> ((Entity) lastHit).Value;
                    var posChange = raycastInput.End - lastPos;
                    var x = math.abs (posChange.x);
                    var y = math.abs (posChange.y);
                    var z = math.abs (posChange.z);
                    float max = math.max (math.max (x, y), z);
                    if (max == x && x != 0) {
                        position.Value = new float3 (lastPos.x + posChange.x / x,
                            lastPos.y + 0.5f, lastPos.z);
                    }

                    if (max == y && y != 0) {
                        position.Value = new float3 (lastPos.x,
                            lastPos.y + posChange.y / y + 0.5f, lastPos.z);
                    }

                    if (max == z && z != 0) {
                        position.Value = new float3 (lastPos.x,
                            lastPos.y + 0.5f, lastPos.z + posChange.z / z);
                    }

                    if (input.Fire) {
                        var copy = PostUpdateCommands.Instantiate (voxel);
                        PostUpdateCommands.SetComponent (copy, new Translation {
                            Value = new float3 (position.Value.x, position.Value.y - 0.5f, position.Value.z)
                        });
                        PostUpdateCommands.RemoveComponent<Disabled> (copy);
                    }

                    // lastHit = null;

                    // if (input.Fire && !EntityManager.HasComponent<Translation> (lastHit)) {
                    //     PostUpdateCommands.SetComponent (PostUpdateCommands.Instantiate (lastHit), new Translation {
                    //         Value = new float3 (position.Value.x, position.Value.y - 0.5f, position.Value.z)
                    //     });
                    // }

                }

                rotation.Value = quaternion.identity;
                raycastHits.Dispose ();
            });
        }
    }
}