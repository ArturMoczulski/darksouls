using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class PlayerAttacker : MonoBehaviour
    {
        public string lastAttack;

        AnimatorHandler animator;
        PlayerLocomotion playerLocomotion;
        AudioSource audioSource;
        AudioClips audioClips;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        PlayerStats playerStats;

        private void Awake()
        {
            animator = GetComponentInChildren<AnimatorHandler>();
            audioSource = GetComponent<AudioSource>();
            audioClips = GetComponent<AudioClips>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            inputHandler = GetComponent<InputHandler>();
            playerStats = GetComponent<PlayerStats>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        public void HandleWeaponCombo(WeaponItem weaponItem)
        {
            if (playerStats.currentStamina < weaponItem.baseStaminaCost * weaponItem.lightAttackMultiplier)
                return;

            if (inputHandler.comboFlag)
            {
                animator.animator.SetBool("canDoCombo", false);
                if (lastAttack == weaponItem.OH_Light_Attack_001)
                {
                    weaponSlotManager.attackingWeapon = weaponItem;
                    audioSource.clip = audioClips.attack001;
                    audioSource.Play();
                    animator.PlayTargetAnimation(weaponItem.OH_Light_Attack_002, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weaponItem)
        {

            if (playerStats.currentStamina < weaponItem.baseStaminaCost * weaponItem.lightAttackMultiplier)
                return;

            weaponSlotManager.attackingWeapon = weaponItem;
            audioSource.clip = audioClips.attack001;
            audioSource.Play();
            animator.PlayTargetAnimation(weaponItem.OH_Light_Attack_001, true);
            lastAttack = weaponItem.OH_Light_Attack_001;
        }

        public void HandleHeavyAttack(WeaponItem weaponItem)
        {
            if (playerStats.currentStamina < weaponItem.baseStaminaCost * weaponItem.heavyAttackMultiplier)
                return;

            weaponSlotManager.attackingWeapon = weaponItem;
            audioSource.clip = audioClips.attack001;
            audioSource.Play();
            animator.PlayTargetAnimation(weaponItem.OH_Heavy_Attack_001, true);
            lastAttack = weaponItem.OH_Heavy_Attack_001;
        }
    }
}
