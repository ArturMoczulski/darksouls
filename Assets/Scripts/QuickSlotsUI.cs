using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkSouls
{
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image leftWeaponImage;
        public Image rightWeaponImage;

        public void UpdateWeaponQuickSlotUI(bool isLeft, WeaponItem weapon)
        {
            if (isLeft)
            {
                UpdateWeaponImage(leftWeaponImage, weapon);
            }
            else
            {
                UpdateWeaponImage(rightWeaponImage, weapon);
            }
        }

        public void UpdateWeaponImage(Image weaponImage, WeaponItem weaponItem)
        {
            if (weaponItem.itemIcon != null)
            {
                weaponImage.sprite = weaponItem.itemIcon;
                weaponImage.enabled = true;
            }
            else
            {
                weaponImage.sprite = null;
                weaponImage.enabled = false;
            }
        }
    }
}
