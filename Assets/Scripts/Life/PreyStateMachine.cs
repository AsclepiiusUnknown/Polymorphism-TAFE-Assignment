using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Flock))]
public class PreyStateMachine : Life
{
    #region Variables
    public enum PreyStates
    {
        Flock,
        Wander,
        Evade,
        Hide,
    }

    [Header("Prey Variables")]
    public bool useHurtColor = true;
    public Color hurtColor = Color.red;

    [Header("State Machine Variables")]
    public PreyStates preyState;
    //public bool ChangeState = false;//

    [Header("Behaviour Objects")]
    [SerializeField] private FlockBehaviour FlockBehaviour;
    [SerializeField] private FlockBehaviour WanderBehaviour;
    [SerializeField] private FlockBehaviour EvadeBehaviour;
    [SerializeField] private FlockBehaviour HideBehaviour;
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

        ChangeStateTo(preyState.ToString());
    }
    void Update()
    {
        stateDisplay.text = preyState.ToString();
    }
    #endregion

    #region Prey Functionality
    public void TakeDamage(float damage)
    {
        health -= damage;
    }

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
    ///

    #region Flock
    public IEnumerator FlockState()
    {
        Debug.Log("Flock: ENTER");

        preyState = PreyStates.Flock;
        flock.behaviour = FlockBehaviour;

        StartCoroutine("DelayStateChangeFW");

        while (preyState == PreyStates.Flock)
        {
            foreach (FlockAgent agent in flock.agents)
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext);

                if (filteredContext.Count > 0)
                {
                    StopCoroutine("DelayStateChangeFW");
                    ChangeStateTo("Evade");
                    preyState = PreyStates.Evade;
                }
            }

            yield return null;
        }

        Debug.Log("Flock: EXIT");
    }
    #endregion

    #region Wander
    public IEnumerator WanderState()
    {
        Debug.Log("Wander: ENTER");

        preyState = PreyStates.Wander;
        flock.behaviour = WanderBehaviour;

        StartCoroutine("DelayStateChangeFW");

        while (preyState == PreyStates.Wander)
        {
            foreach (FlockAgent agent in flock.agents)
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext);

                if (filteredContext.Count > 0)
                {
                    StopCoroutine("DelayStateChangeFW");
                    ChangeStateTo("Evade");
                    preyState = PreyStates.Evade;
                }
            }

            yield return null;
        }

        Debug.Log("Wander: EXIT");
    }
    #endregion

    #region Evade
    public IEnumerator EvadeState()
    {
        Debug.Log("Evade: ENTER");

        preyState = PreyStates.Evade;
        flock.behaviour = EvadeBehaviour;

        StartCoroutine("DelayStateChangeEH");

        while (preyState == PreyStates.Evade)
        {
            foreach (FlockAgent agent in flock.agents)
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext);

                if (filteredContext.Count <= 0)
                {
                    StopCoroutine("DelayStateChangeEH");
                    ChangeStateTo("Flock");
                    preyState = PreyStates.Flock;
                }
            }

            yield return null;
        }

        Debug.Log("Evade: EXIT");
    }
    #endregion

    #region Hide
    public IEnumerator HideState()
    {
        Debug.Log("Hide: ENTER");

        preyState = PreyStates.Hide;
        flock.behaviour = HideBehaviour;

        StartCoroutine("DelayStateChangeEH");

        while (preyState == PreyStates.Hide)
        {
            foreach (FlockAgent agent in flock.agents)
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext);

                if (filteredContext.Count <= 0)
                {
                    StopCoroutine("DelayStateChangeEH");
                    ChangeStateTo("Flock");
                    preyState = PreyStates.Flock;
                }
            }

            yield return null;
        }

        Debug.Log("Evade: EXIT");
    }
    #endregion

    #region State Changing
    void ChangeStateTo(string methodName)
    {
        StartCoroutine(methodName + "State"); //change the state to the passed value plus "State"
    }

    public IEnumerator DelayStateChangeFW()
    {
        yield return new WaitForSeconds(10);

        if (preyState == PreyStates.Flock)
        {
            ChangeStateTo("Wander");
        }
        else
        {
            ChangeStateTo("Flock");
        }
    }

    public IEnumerator DelayStateChangeEH()
    {
        Debug.Log("Counting...");

        yield return new WaitForSeconds(10);

        if (preyState == PreyStates.Evade)
        {
            ChangeStateTo("Hide");
        }
        else
        {
            ChangeStateTo("Evade");
        }
    }
    #endregion
    #endregion
}