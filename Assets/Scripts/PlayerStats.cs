using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Player Stats")]
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public int maxStamina;
        public int currentStamina;
        public int staminaRegenarationRate = 1;

        [Header("UI")]
        public HealthBar healthBar;
        public StaminaBar staminaBar;

        private float lastStaminaRegenDelta;
        AnimatorHandler animatorHandler;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFormHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
        }

        private int SetMaxHealthFormHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0)
                currentHealth = 0;

            healthBar.SetCurrentHealth(currentHealth);
            animatorHandler.PlayTargetAnimation("Paladin_Impact_001", true);

            if (currentHealth <= 0)
            {
                animatorHandler.PlayTargetAnimation("Paladin_Death_001", true);
            }
        }

        public void TakeStaminaDamage(int staminaDamage)
        {
            AdjustStamina(-staminaDamage);
        }

        public void UpdateStaminaBar()
        {
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void AdjustStamina(int amount)
        { 
            currentStamina += amount;

            if (currentStamina > maxStamina)
                currentStamina = maxStamina;

            if (currentStamina < 0)
                currentStamina = 0;
        }

        public void RegenerateStaminaOverTime(float delta)
        {
            lastStaminaRegenDelta += delta;
            if (lastStaminaRegenDelta > 1)
            {
                AdjustStamina(staminaRegenarationRate);
                lastStaminaRegenDelta = 0;
            }
        }
    }

}
