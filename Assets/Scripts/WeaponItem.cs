using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    [CreateAssetMenu(menuName = "Items/WeaponItem")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Stamina Costs")]
        public int baseStaminaCost;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;

        [Header("One Handed Attack Animations")]
        public string OH_Light_Attack_001;
        public string OH_Light_Attack_002;
        public string OH_Heavy_Attack_001;
    }
}
