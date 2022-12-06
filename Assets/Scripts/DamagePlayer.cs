using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class DamagePlayer : MonoBehaviour
    {
        public int damage;

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats)
            {
                playerStats.TakeDamage(damage);
            }
        }
    }
}
