using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using Util;

namespace Systems {
    [UpdateAfter (typeof (CharacterControllerSystem))]
    public unsafe class CameraControlSystem : ComponentSystem {
        EntityQuery playerQuery;
        EntityQuery inputQuery;
        // PhysicsWorld physicsWorld;
        float3 aimPivotOffset = new float3 (0f, 0.5f, 0f);
        float3 aimCamOffset = new float3 (0f, 0.5f, 0.2f);

        float aimFOV = 80f;

        // [BurstCompile]
        // public struct ColliderCastJob : IJob {
        //     [NativeDisableUnsafePtrRestriction] public Unity.Physics.Collider * Collider;
        //     public quaternion Orientation;
        //     public float3 Start;
        //     public float3 End;
        //     public NativeArray<ColliderCastHit> ColliderCastHits;
        //     // public bool CollectAllHits;
        //     [ReadOnly] public PhysicsWorld World;

        //     public void Execute () {
        //         ColliderCastInput colliderCastInput = new ColliderCastInput {
        //             Collider = Collider,
        //             Orientation = Orientation,
        //             Start = Start,
        //             End = End
        //         };

        //         // if (CollectAllHits) {
        //         //     World.CastCollider (colliderCastInput, ref ColliderCastHits);
        //         // } else 
        //         if (World.CastCollider (colliderCastInput, out ColliderCastHit hit)) {
        //             ColliderCastHits[0];
        //         }
        //     }
        // }

        protected override void OnCreate () {
            playerQuery = GetEntityQuery (typeof (PlayerTag));
            inputQuery = GetEntityQuery (typeof (CharacterControllerInput));
        }

        protected override void OnUpdate () {
            var players = playerQuery.ToEntityArray (Allocator.TempJob);
            if (players.Length < 0) {
                players.Dispose ();
                return;
            }
            var playerPos = EntityManager.GetComponentData<Translation> (players[0]);
            // var input = EntityManager.GetComponentData<CharacterControllerInternalData> (players[0]).Input;
            var input = inputQuery.GetSingleton<CharacterControllerInput> ();
            var cameraObj = GameObject.FindGameObjectWithTag ("MainCamera");
            Entities.WithAll<Translation, Rotation, CameraControlData> ().ForEach ((Entity cameraEntity,
                ref Translation cameraPosition,
                ref Rotation cameraRotation, ref CameraControlData data) => {
                var targetH = input.Aimed? data.HorizontalAimingSpeed / 2 : data.HorizontalAimingSpeed;
                var targetV = input.Aimed? data.VerticalAimingSpeed / 2 : data.VerticalAimingSpeed;
                var angleHorizontal = math.clamp (input.Looking.x, -1, 1) * targetH * Time.deltaTime;
                data.AngleHorizontal += angleHorizontal;
                var angleVertical = math.clamp (input.Looking.y, -1, 1) * targetV * Time.deltaTime;
                data.AngleVertical += angleVertical;
                angleVertical = math.clamp (data.AngleVertical, data.MinVerticalAngle, data.MaxVerticalAngle);
                data.AngleVertical = angleVertical;

                var camera = cameraObj.GetComponent<Camera> ();
                if (input.Aimed) {
                    data.TargetCameraOffset = aimCamOffset;
                    data.TargetPivotOffset = aimPivotOffset;
                    data.TargetFOV = aimFOV;
                } else {
                    data.TargetCameraOffset = data.CameraOffset;
                    data.TargetPivotOffset = data.PivotOffset;
                    data.TargetFOV = camera.fieldOfView;
                }
                // data.TargetCameraOffset = data.CameraOffset;
                // data.TargetPivotOffset = data.PivotOffset;
                var cameraYRot = quaternion.Euler (0, data.AngleHorizontal, 0);
                var aimRot = quaternion.Euler (-data.AngleVertical, data.AngleHorizontal, 0);
                cameraRotation.Value = aimRot;
                cameraObj.transform.rotation = aimRot;
                camera.fieldOfView = math.lerp (camera.fieldOfView, data.TargetFOV, Time.deltaTime);

                var baseTempPosition = playerPos.Value + math.rotate (cameraYRot, data.TargetMaxVerticalAngle);
                var noCollisionOffset = data.TargetCameraOffset;
                for (float zOffset = data.TargetCameraOffset.z; zOffset <= 0; zOffset += 0.5f) {
                    noCollisionOffset.z = zOffset;
                    if (DoubleViewingPosCheck (players[0], baseTempPosition + math.rotate (aimRot, noCollisionOffset),
                            math.abs (zOffset), data.ShapeHeight,
                            playerPos, cameraPosition, cameraRotation, data.RelCameraPosLength) || zOffset == 0) {
                        break;
                    }
                }

                // Repostition the camera
                var smoothPivotOffset = math.lerp (data.SmoothPivotOffset, data.TargetPivotOffset,
                    math.clamp (data.Smooth * Time.deltaTime, 0, 1));
                data.SmoothPivotOffset = smoothPivotOffset;
                var smoothCameraOffset = math.lerp (data.SmoothCameraOffset, noCollisionOffset,
                    math.clamp (data.Smooth * Time.deltaTime, 0, 1));
                data.SmoothCameraOffset = smoothCameraOffset;

                var position = playerPos.Value +
                    math.rotate (cameraYRot, data.SmoothPivotOffset) + math.rotate (aimRot, data.SmoothCameraOffset);
                cameraPosition.Value = position;
                cameraObj.GetComponent<Transform> ().position = position;
            });

            players.Dispose ();
        }
        // Double check for collisions: concave objects doesn't detect hit from outside, so cast in both directions.
        bool DoubleViewingPosCheck (Entity player, float3 checkPos,
            float offset, float height,
            Translation playerPos, Translation cameraPosition,
            Rotation cameraRotation, float relCameraPosMag) {

            ref var physicsWorld = ref World.Active.GetExistingSystem<BuildPhysicsWorld> ().PhysicsWorld;
            float playerFocusHeight = height * 0.75f;
            return ViewingPosCheck (player, checkPos, playerFocusHeight, playerPos, cameraRotation, relCameraPosMag, physicsWorld) &&
                ReverseViewingPosCheck (player, cameraPosition,
                    checkPos, playerFocusHeight, offset, playerPos, cameraRotation, physicsWorld);
        }

        // Check for collision from camera to player.
        unsafe bool ViewingPosCheck (Entity player, float3 checkPos,
            float deltaPlayerHeight, Translation playerPos,
            Rotation cameraRotation, float relCameraPosMag, PhysicsWorld physicsWorld) {

            float3 target = playerPos.Value + (math.up () * deltaPlayerHeight);
            ColliderCastHit result = new ColliderCastHit ();
            ColliderQueryUtil.SingleColliderCast (physicsWorld.CollisionWorld, new ColliderCastInput {
                Collider = (Unity.Physics.Collider * ) (Unity.Physics.SphereCollider.Create (new SphereGeometry {
                        Center = checkPos,
                            Radius = 0.2f
                    }).GetUnsafePtr ()),
                    Orientation = cameraRotation.Value,
                    Start = checkPos,
                    End = math.normalize (target - checkPos) * relCameraPosMag + checkPos
            }, ref result);

            // If a raycast from the check position to the player hits something...
            if (result.RigidBodyIndex != -1) {
                var hitBody = physicsWorld.Bodies[result.RigidBodyIndex];
                // ... if it is not the player...
                if (hitBody.Entity != player && hitBody.HasCollider) {
                    // This position isn't appropriate.
                    return false;
                }
            }
            // If we haven't hit anything or we've hit the player, this is an appropriate position.
            return true;
        }

        // Check for collision from player to camera.
        unsafe bool ReverseViewingPosCheck (Entity player, Translation cameraPos,
            float3 checkPos, float deltaPlayerHeight,
            float maxDistance, Translation playerPos, Rotation cameraRotation, PhysicsWorld physicsWorld) {
            // Cast origin.
            float3 origin = playerPos.Value + (math.up () * deltaPlayerHeight);
            ColliderCastHit result = new ColliderCastHit ();
            ColliderQueryUtil.SingleColliderCast (physicsWorld.CollisionWorld, new ColliderCastInput {
                Collider = (Unity.Physics.Collider * ) (Unity.Physics.SphereCollider.Create (new SphereGeometry {
                        Center = origin,
                            Radius = 0.2f
                    }).GetUnsafePtr ()),
                    Orientation = cameraRotation.Value,
                    Start = origin,
                    End = math.normalize (checkPos - origin) * maxDistance + origin,
            }, ref result);

            if (result.RigidBodyIndex != -1) {
                var hitBody = physicsWorld.Bodies[result.RigidBodyIndex];
                var floatBool = result.Position != cameraPos.Value;
                if (hitBody.Entity != player && floatBool.x && floatBool.y && floatBool.z && hitBody.HasCollider) {
                    return false;
                }
            }
            return true;
        }
    }
}