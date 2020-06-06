using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    #region Variables
    public Flock flock;

    public float maxHealth = 100;
    [HideInInspector]
    public float health;
    #endregion

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {

    }
}
