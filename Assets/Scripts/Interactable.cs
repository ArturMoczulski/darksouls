using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class Interactable : MonoBehaviour
    {
        public float groundOffset = 0.3f;
        public float radius = 0.6f;
        public string interactableText;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(
                transform.position +
                radius * Vector3.up +
                groundOffset * Vector3.up,
                radius
            );
        }

        public virtual void Interact(PlayerManager playerManager)
        {
            Debug.Log("Player interacted with " + gameObject.name);
        }
    }
}
