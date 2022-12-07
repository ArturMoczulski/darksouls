using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class WeaponPickUp : Interactable
    {
        public WeaponItem weaponItem;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUp(playerManager);
        }

        private void PickUp(PlayerManager playerManager)
        { 
            PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();
            PlayerLocomotion playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            AnimatorHandler animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

            playerLocomotion.rigidbody.velocity = Vector3.zero;
            animatorHandler.PlayTargetAnimation("Paladin_PickUp_001", true);
            playerInventory.weaponItems.Add(weaponItem);

            playerManager.interactableUI.itemPickedUpText.text = weaponItem.itemName;
            playerManager.interactableUI.itemPickedUpIcon.sprite = weaponItem.itemIcon;
            playerManager.interactableUI.itemPickedUpPopUp.SetActive(true);

            Destroy(gameObject);
        }
    }
}
