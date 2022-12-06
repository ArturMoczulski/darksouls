using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownTime : MonoBehaviour
{
    [SerializeField]
    float timeScale = 1;

    void Awake()
    {
        Time.timeScale = timeScale;
    }
}
