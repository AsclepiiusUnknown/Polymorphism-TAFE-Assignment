using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flock))]
public class PreyStateMachine : Life
{
    #region Variables
    public enum PreyStates
    {
        FlockWander,
        EvadeHide
    }

    [Header("Prey Variables")]
    public bool useHurtColor = true;
    public Color hurtColor = Color.red;

    [Header("State Machine Variables")]
    public PreyStates preyState;

    [Header("Behaviour Objects")]
    [SerializeField] private FlockBehaviour FlockWanderBehaviour;
    [SerializeField] private FlockBehaviour EvadeHideBehaviour;
    #endregion

    #region Default
    protected override void Start()
    {
        base.Start();

        #region Debugging
        if (GetComponent<Flock>() != null && flock == null)
        {
            flock = GetComponent<Flock>();
        }

        if (flock == null)
        {
            Debug.LogError("Prey State Machine couldnt find flock");
        }
        #endregion

        ChangeStateTo(PreyStates.FlockWander.ToString());

    }

    void Update()
    {
        stateDisplay.text = preyState.ToString();
    }
    #endregion

    #region Prey Functionality
    public void PlayHurtColorEffectVoid(SpriteRenderer renderer, int repeatAmount, float waitTime)
    {
        PlayHurtColorEffect(renderer, repeatAmount, waitTime);
    }

    public IEnumerator PlayHurtColorEffect(SpriteRenderer renderer, int repeatAmount, float waitTime)
    {
        Color tempStartColor = renderer.color;

        for (int i = 0; i < repeatAmount; i++)
        {
            renderer.color = hurtColor;
            yield return new WaitForSeconds(waitTime);
            renderer.color = tempStartColor;
        }
    }
    #endregion

    #region State Machine Functionality
    // * //
    #region Flock/Wander
    public IEnumerator FlockWanderState()
    {
        Debug.Log("Flock/Wander: ENTER");

        preyState = PreyStates.FlockWander;
        flock.behaviour = FlockWanderBehaviour;

        while (preyState == PreyStates.FlockWander)
        {
            foreach (FlockAgent agent in flock.agents)
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext);

                if (filteredContext.Count > 0)
                {
                    preyState = PreyStates.EvadeHide;
                    ChangeStateTo(PreyStates.EvadeHide.ToString());
                    yield return null;
                }
            }

            yield return null;
        }

        Debug.Log("Flock/Wander: EXIT");
    }
    #endregion

    #region Evade/Hide
    public IEnumerator EvadeHideState()
    {
        Debug.Log("Evade/Hide: ENTER");

        preyState = PreyStates.EvadeHide;
        flock.behaviour = EvadeHideBehaviour;

        while (preyState == PreyStates.EvadeHide)
        {
            foreach (FlockAgent agent in flock.agents)
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext);

                if (filteredContext.Count <= 0)
                {
                    preyState = PreyStates.FlockWander;
                    ChangeStateTo(PreyStates.FlockWander.ToString());
                    Debug.Log(filteredContext.Count);
                    yield return null;
                }
            }

            yield return null;
        }

        Debug.Log("Evade: EXIT");
    }
    #endregion

    #region State Changing
    protected void ChangeStateTo(string methodName)
    {
        StartCoroutine(methodName + "State"); //change the State to the passed value plus "State"
    }
    #endregion
    #endregion
}