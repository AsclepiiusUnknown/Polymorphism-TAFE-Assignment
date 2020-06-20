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
    public Flock flock; //Flock component reference used to control the behaviour object being used
    public ContextFilter contextFilter; //Context filter (different flock) used to check if there are other agents nearby

    [Header("Other")]
    public TextMeshProUGUI stateDisplay; //Text element used to display the state the flock is in
    #endregion
}
