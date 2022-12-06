using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{

    public class EnemyAnimatorHandler : MonoBehaviour
    {
        public Animator animator;

        public void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            animator.CrossFade(targetAnim, 0.2f); 
        }

    }

}