using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{

    public class AnimatorHandler : MonoBehaviour
    {
        PlayerManager playerManager;
        public Animator animator;
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        int horizontal;
        int vertical;
        public bool canRotate = true;

        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            animator = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f); 
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f) {
                v = 0.5f;
            }
            else if (verticalMovement >= 0.55f) {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f) {
                v = -0.5f;
            }
            else if (verticalMovement <= 0.55f) {
                v = -1;
            }

            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f) {
                h = 0.5f;
            }
            else if (horizontalMovement >= 0.55f) {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f) {
                h = -0.5f;
            }
            else if (horizontalMovement <= 0.55f) {
                h = -1;
            }
            #endregion

            if (isSprinting)
            {

                v = 2;
                h = horizontalMovement;
            }

            animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
                return;

            float delta = Time.fixedDeltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;

            Vector3 velocity = deltaPosition / delta * 3;

            playerLocomotion.rigidbody.velocity = velocity;

        }

        public void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }

    }

}