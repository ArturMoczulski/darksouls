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
        PlayerInventory playerInventory;

        public InteractableUI interactableUI;

        // Start is called before the first frame update
        void Start()
        {

            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            animator = GetComponentInChildren<Animator>();
            cameraHandler = CameraHandler.singleton;
            playerStats = GetComponent<PlayerStats>();
            interactableUI = FindObjectOfType<InteractableUI>();
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

            CheckForInteractableObjects();

            inputHandler.StateReset();

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }

        void CheckForInteractableObjects()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();

                    if (interactable != null)
                    {
                        string interactableText = interactable.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUI.interactablePopUp.SetActive(true);

                        if (inputHandler.interactInput)
                        {
                            interactable.Interact(this);
                        }
                    }
                }
            }
            else
            {
                interactableUI.interactablePopUp.SetActive(false);

                if (inputHandler.interactInput && interactableUI.itemPickedUpPopUp.activeSelf)
                {
                    interactableUI.itemPickedUpPopUp.SetActive(false);
                }
            }

        }

    }
}
