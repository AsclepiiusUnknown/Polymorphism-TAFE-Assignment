using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//[RequireComponent(typeof(Flock))]
public class Life : MonoBehaviour
{
    #region Variables
    [Header("Flock Components")]
    public Flock flock;
    public ContextFilter contextFilter;

    [Header("Health")]
    public float maxHealth = 100;
    [HideInInspector]
    public float health;

    [Header("Other")]
    public TextMeshProUGUI stateDisplay;
    #endregion

    #region Default
    protected virtual void Start()
    {
        health = maxHealth;
    }
    #endregion



    #region Testing - REMOVE LATER!!!
    public int Method1(string input)
    {
        print("its working for 1");
        //... do something
        return 0;
    }

    public bool RunTheMethod(Func<string, int> myMethodName)
    {
        print("its working for run");

        //... do stuff
        int i = myMethodName("My String");
        //... do more stuff
        return true;
    }

    public bool Test()
    {
        return RunTheMethod(Method1);
    }


    public IEnumerator CdToExecuteVoid(Func<int> methodName, float length)
    {
        yield return new WaitForSeconds(length);

        int m = methodName();
    }
    #endregion
}
