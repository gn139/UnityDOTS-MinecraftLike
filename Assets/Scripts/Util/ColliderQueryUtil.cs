using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Physics;

namespace Util {
    public class ColliderQueryUtil {
        [BurstCompile]
        public struct ColliderCastJob : IJobParallelFor {
            [ReadOnly] public CollisionWorld world;
            [ReadOnly] public NativeArray<ColliderCastInput> inputs;
            public NativeArray<ColliderCastHit> results;

            public unsafe void Execute (int index) {
                ColliderCastHit hit;
                world.CastCollider (inputs[index], out hit);
                results[index] = hit;
            }
        }

        public static JobHandle ScheduleBatchColliderCast (CollisionWorld world,
            NativeArray<ColliderCastInput> inputs, NativeArray<ColliderCastHit> results) {
            JobHandle rcj = new ColliderCastJob {
                inputs = inputs,
                    results = results,
                    world = world

            }.Schedule (inputs.Length, 5);
            return rcj;
        }

        [BurstCompile]
        public struct RaycastJob : IJobParallelFor {
            [ReadOnly] public CollisionWorld world;
            [ReadOnly] public NativeArray<RaycastInput> inputs;
            public NativeArray<RaycastHit> results;

            public unsafe void Execute (int index) {
                RaycastHit hit;
                world.CastRay (inputs[index], out hit);
                results[index] = hit;
            }
        }

        public static JobHandle ScheduleBatchRayCast (CollisionWorld world,
            NativeArray<RaycastInput> inputs, NativeArray<RaycastHit> results) {
            JobHandle rcj = new RaycastJob {
                inputs = inputs,
                    results = results,
                    world = world

            }.Schedule (inputs.Length, 5);
            return rcj;
        }

        public static void SingleColliderCast (CollisionWorld world, ColliderCastInput input,
            ref ColliderCastHit result) {
            var rayCommands = new NativeArray<ColliderCastInput> (1, Allocator.TempJob);
            var rayResults = new NativeArray<ColliderCastHit> (1, Allocator.TempJob);
            rayCommands[0] = input;
            var handle = ScheduleBatchColliderCast (world, rayCommands, rayResults);
            handle.Complete ();
            result = rayResults[0];
            rayCommands.Dispose ();
            rayResults.Dispose ();
        }

        public static void SingleRayCast (CollisionWorld world, RaycastInput input,
            ref RaycastHit result) {
            var rayCommands = new NativeArray<RaycastInput> (1, Allocator.TempJob);
            var rayResults = new NativeArray<RaycastHit> (1, Allocator.TempJob);
            rayCommands[0] = input;
            var handle = ScheduleBatchRayCast (world, rayCommands, rayResults);
            handle.Complete ();
            result = rayResults[0];
            rayCommands.Dispose ();
            rayResults.Dispose ();
        }
    }
}