using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class EnemyStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        EnemyAnimatorHandler animatorHandler;
        AudioSource audioSource;
        Collider collider;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
            audioSource = GetComponentInChildren<AudioSource>();
            collider = GetComponent<Collider>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFormHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFormHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0)
                currentHealth = 0;

            animatorHandler.PlayTargetAnimation("Impact_001", true);
            audioSource.Play();

            if (currentHealth <= 0)
            {
                animatorHandler.PlayTargetAnimation("Death_001", true);
                collider.enabled = false;
            }
        }
    }

}
