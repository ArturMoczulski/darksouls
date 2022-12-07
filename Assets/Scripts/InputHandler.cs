using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{

    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;
        
        public bool b_Input;
        public bool changeRightWeaponInput;
        public bool changeLeftWeaponInput;
        public bool lightAttackInput;
        public bool heavyAttackInput;
        public bool interactInput;

        public bool sprintFlag;
        public bool rollFlag;
        public bool comboFlag;
        public float rollInputTimer;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
        }

        private void OnEnable()
        {
            if (inputActions == null)
            {

                inputActions = new PlayerControls();

                inputActions.PlayerMovement.Movement.performed +=
                    inputActions => movementInput = inputActions.ReadValue<Vector2>();

                inputActions.PlayerMovement.Camera.performed +=
                    inputActions => cameraInput = inputActions.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta) 
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotsInput();
            HandleInteractingButtonInput();
        }

        private void MoveInput(float delta)
        {

            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

            if (b_Input)
            {
                // sprint
                rollInputTimer += delta;

                if (moveAmount > 0)
                { 
                    sprintFlag = true;
                }
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    // roll
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        public void HandleAttackInput(float delta)
        {
            inputActions.PlayerActions.LightAttack.performed += i => lightAttackInput = true;
            inputActions.PlayerActions.HeavyAttack.performed += i => heavyAttackInput = true;

            if (lightAttackInput)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else 
                {
                    if (playerManager.isInteracting)
                        return;

                    if (playerManager.canDoCombo)
                        return;

                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
            }

            if (heavyAttackInput)
            {
                if (playerManager.canDoCombo)
                    return;

                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        public void HandleQuickSlotsInput()
        {
            inputActions.PlayerInventory.ChangeLeftWeapon.performed += i => changeLeftWeaponInput = true;
            inputActions.PlayerInventory.ChangeRightWeapon.performed += i => changeRightWeaponInput = true;

            if (changeLeftWeaponInput)
            {
                playerInventory.ChangeLeftWeapon();
            }

            if (changeRightWeaponInput)
            {
                playerInventory.ChangeRightWeapon();
            }
        }

        public void HandleInteractingButtonInput()
        {
            inputActions.PlayerActions.PickUpItem.performed += i => interactInput = true;
        }

        public void StateReset()
        {
            rollFlag = false;
            sprintFlag = false;
            lightAttackInput = false;
            heavyAttackInput = false;
            changeRightWeaponInput = false;
            changeLeftWeaponInput = false;
            interactInput = false;
        }
    }

}