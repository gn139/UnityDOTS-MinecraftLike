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
using static CharacterControllerUtilities;

namespace Systems {
    [UpdateAfter (typeof (EndFramePhysicsSystem))]
    public class PlayerAnimatorSystem : ComponentSystem {

        Animator animator;
        protected override void OnStartRunning () {
            animator = GameObject.FindGameObjectWithTag ("PlayerAnimator").GetComponent<Animator> ();
            animator.updateMode = AnimatorUpdateMode.AnimatePhysics;//dont know if its useful 不知道是否有用 
        }

        protected override void OnUpdate () {
            Entities.WithAllReadOnly<PlayerTag, CharacterControllerInternalData> ().ForEach ((Entity entity,
                ref CharacterControllerInternalData data) => {
                if (!animator)
                    return;
                if (data.SupportedState == CharacterSupportState.Unsupported)
                    animator.SetBool ("InGround", false);
                if (data.SupportedState == CharacterSupportState.Supported)
                    animator.SetBool ("InGround", true);
                PlayerOnGround (data);
                PlayerNotOnGround (data);

            });
        }

        void PlayerOnGround (CharacterControllerInternalData data) {
            if (!animator.GetBool ("InGround"))
                return;
            animator.SetBool ("IsLand", true);
            animator.SetBool ("IsJump", false);
            var sqrLength = math.lengthsq (data.LinearVelocity);
            // Debug.Log(sqrLength);
            if (sqrLength > 4.5f) {
                animator.SetBool ("IsRun", true);
                animator.SetBool ("isWalk", false);
            } else if (sqrLength > 0 && sqrLength <= 4.5f) {
                animator.SetBool ("isWalk", true);
                animator.SetBool ("IsRun", false);
            }

            if (data.LinearVelocity.x < 0.01f && data.LinearVelocity.z < 0.01f) {
                animator.SetBool ("IsRun", false);
                animator.SetBool ("isWalk", false);
            }
        }

        void PlayerNotOnGround (CharacterControllerInternalData data) {
            if (animator.GetBool ("InGround"))
                return;
            if (data.LinearVelocity.y < 0.5) {
                animator.SetBool ("IsFloat", true);
            } else {
                animator.SetBool ("IsJump", true);
                animator.SetBool ("IsLand", false);
            }

        }
    }
}