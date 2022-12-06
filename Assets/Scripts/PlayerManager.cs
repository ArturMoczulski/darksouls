using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class PlayerManager : MonoBehaviour
    {
        public bool isInteracting;
        public bool canDoCombo;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;

        InputHandler inputHandler;
        Animator animator;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;
        PlayerStats playerStats;

        // Start is called before the first frame update
        void Start()
        {

            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            animator = GetComponentInChildren<Animator>();
            cameraHandler = CameraHandler.singleton;
            playerStats = GetComponent<PlayerStats>();
        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;

            playerStats.RegenerateStaminaOverTime(delta);
            playerStats.UpdateStaminaBar();
        }

        void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");

			inputHandler.TickInput(delta);

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

			playerLocomotion.HandleMovement(delta);
			playerLocomotion.HandleRollingAndSprinting(delta);
			playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);

            inputHandler.StateReset();

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }

        private void LateUpdate()
        {
        }

    }
}
