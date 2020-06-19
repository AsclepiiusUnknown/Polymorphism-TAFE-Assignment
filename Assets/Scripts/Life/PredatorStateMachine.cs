using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Flock))]
public class PredatorStateMachine : Life
{
    #region Variables
    public enum PredatorStates
    {
        WanderSeek,
        Attack,
        OffsetPursuit
        //Search. Taken out due to ambuity between Search and Seek, authorised by Andrew on 11/06/20
    }
    [Header("Prepare Attack Timer")]
    public float prepareAttackTimeValue = 5;
    private float prepareAttackTimer;
    private bool isAttacking;

    [Header("Max Attack Timer")]
    public float maxAttackTimeValue = 5;
    private float maxAttackTimer;

    [Header("State Machine Variables")]
    public PredatorStates predatorState;

    [Header("Behaviour Objects")]
    [SerializeField] private FlockBehaviour WanderSeekBehaviour;
    [SerializeField] private FlockBehaviour AttackBehaviour;
    [SerializeField] private FlockBehaviour OffsetPursuitBehaviour;
    #endregion

    #region Default
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        isAttacking = false;

        #region Debugging
        if (GetComponent<Flock>() != null && flock == null)
        {
            flock = GetComponent<Flock>();
        }

        if (flock == null)
        {
            Debug.LogError("Predator State Machine couldnt find flock");
        }
        #endregion

        ChangeStateTo(PredatorStates.WanderSeek.ToString());
    }
    void Update()
    {
        stateDisplay.text = predatorState.ToString();

        if (!isAttacking && predatorState == PredatorStates.OffsetPursuit)
        {
            prepareAttackTimer -= Time.deltaTime;
        }

        if (isAttacking && predatorState == PredatorStates.Attack)
        {
            maxAttackTimer -= Time.deltaTime;
        }

        if (maxAttackTimer <= 0)
        {
            isAttacking = false;
            predatorState = PredatorStates.WanderSeek;
            ChangeStateTo(PredatorStates.WanderSeek.ToString());
            maxAttackTimer = maxAttackTimeValue;
        }
    }
    #endregion

    #region Predator Functionality
    public void KillEnemy(FlockAgent prey)
    {
        prey.Die();
    }
    #endregion

    #region Sate Machine Functionality

    #region Wander Seek
    public IEnumerator WanderSeekState()
    {
        Debug.Log("Wander: ENTER");

        flock.behaviour = WanderSeekBehaviour;

        while (predatorState == PredatorStates.WanderSeek)
        {
            foreach (FlockAgent agent in flock.agents)
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext);

                if (filteredContext.Count > 0)
                {
                    predatorState = PredatorStates.OffsetPursuit;
                }
            }

            yield return null;
        }

        Debug.Log("Wander: EXIT");
        ChangeStateTo(predatorState.ToString());
    }
    #endregion

    #region Attack
    public IEnumerator AttackState()
    {
        Debug.Log("Attack: ENTER");

        flock.behaviour = AttackBehaviour;

        prepareAttackTimer = prepareAttackTimeValue;
        isAttacking = true;
        maxAttackTimer = maxAttackTimeValue;

        while (predatorState == PredatorStates.Attack)
        {
            foreach (FlockAgent agent in flock.agents)
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext);

                if (filteredContext.Count <= 0)
                {
                    isAttacking = false;
                    predatorState = PredatorStates.WanderSeek;
                    // ChangeStateTo("WanderSeek");
                }
            }

            yield return null;
        }

        Debug.Log("Attack: EXIT");
        ChangeStateTo(predatorState.ToString());
    }
    #endregion

    #region Offset Pursuit
    public IEnumerator OffsetPursuitState()
    {
        Debug.Log("Offset Pursuit: ENTER");

        flock.behaviour = OffsetPursuitBehaviour;

        prepareAttackTimer = prepareAttackTimeValue;

        while (predatorState == PredatorStates.OffsetPursuit)
        {
            foreach (FlockAgent agent in flock.agents)
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext);

                if (filteredContext.Count <= 0)
                {
                    predatorState = PredatorStates.WanderSeek;
                    // ChangeStateTo("WanderSeek");
                }
            }

            if (prepareAttackTimer <= 0 && !isAttacking)
            {
                predatorState = PredatorStates.Attack;
                // ChangeStateTo("Attack");
            }

            yield return null;
        }

        Debug.Log("Offset Pursuit: EXIT");
        ChangeStateTo(predatorState.ToString());
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