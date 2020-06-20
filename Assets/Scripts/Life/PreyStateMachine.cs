using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flock))]
public class PreyStateMachine : Life
{
    #region Variables
    public enum PreyStates//States used for the control of the Preys behaviour in the environment
    {
        Wander, //Default state - wanders around the environment
        EvadeHide//when being chased - evades and hides when able
    }

    [Header("State Machine Variables")]
    public PreyStates preyState; //instance of the above states for control behaviour

    [Header("Behaviour Objects")]
    [SerializeField] private FlockBehaviour WanderBehaviour; //behaviour for wandering
    [SerializeField] private FlockBehaviour EvadeHideBehaviour; //behaviour for evading enemies
    #endregion

    #region Default
    void Start()
    {
        #region Debugging
        if (GetComponent<Flock>() != null && flock == null) //IF the flock exists and we dont already have one
        {
            flock = GetComponent<Flock>(); //setup the flock component
        }

        if (flock == null)//if the flock is still null
        {
            Debug.LogError("Predator State Machine couldnt find flock"); //debug the error
        }
        #endregion

        ChangeStateTo(PreyStates.Wander.ToString()); //Start in the wander state

    }

    void Update()
    {
        stateDisplay.text = preyState.ToString();
    }
    #endregion

    #region State Machine Functionality
    // * //
    #region Wander
    public IEnumerator WanderState() //used to wander
    {
        Debug.Log("Wander: ENTER"); //log entering this state

        preyState = PreyStates.Wander; //set the state
        flock.behaviour = WanderBehaviour; //set the behaviour object

        while (preyState == PreyStates.Wander) //while we are in this state
        {
            foreach (FlockAgent agent in flock.agents) //for each agent in our flock
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext); //make a list of agents nearby from other flocks

                if (filteredContext.Count > 0) //if there are agents on the list (aka enemies nearby)
                {
                    preyState = PreyStates.EvadeHide; //change to evade state
                    ChangeStateTo(PreyStates.EvadeHide.ToString()); //change states
                    yield return null; //return out of the loop
                }
            }

            yield return null; //return out of the loop
        }

        Debug.Log("Wander: EXIT"); //log the state exit
    }
    #endregion

    #region Evade/Hide
    public IEnumerator EvadeHideState() //used to evade
    {
        Debug.Log("Evade/Hide: ENTER"); //log entering the state

        preyState = PreyStates.EvadeHide; //set the state
        flock.behaviour = EvadeHideBehaviour; //set the behaviour object

        while (preyState == PreyStates.EvadeHide) //while we are in this state
        {
            foreach (FlockAgent agent in flock.agents) //for each agent in our flock
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext); //make a list of agents neaby from other flocks

                if (filteredContext.Count <= 0) //if there arent agents on the list (aka no enemies nearby)
                {
                    preyState = PreyStates.Wander; //change to wander state
                    ChangeStateTo(PreyStates.Wander.ToString()); //change states
                    //Debug.Log(filteredContext.Count); 
                    yield return null; //return out of the loop
                }
            }

            yield return null; //return out of the loop
        }

        Debug.Log("Evade: EXIT"); //log exiting the state
    }
    #endregion

    #region State Changing
    protected void ChangeStateTo(string methodName) //used to change states
    {
        StartCoroutine(methodName + "State"); //change the State to the passed value plus "State"
    }
    #endregion
    #endregion
}