using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Flock))]
public class PredatorStateMachine : Life
{
    #region Variables
    public enum PredatorStates
    {
        Wander,
        Seek,
        Attack,
        CollisionAvoidance,
        OffsetPursuit
        //Search. Taken out due to ambuity between Search and Seek, authorised by Andrew on 11/06/20
    }

    [Header("Predator Variables")]
    public float attackDamage = 10;


    [Header("State Machine Variables")]
    public PredatorStates predatorState;
    //public bool ChangeState = false;//

    [Header("Behaviour Objects")]
    [SerializeField] private FlockBehaviour WanderBehaviour;
    [SerializeField] private FlockBehaviour SeekBehaviour;
    [SerializeField] private FlockBehaviour AttackBehaviour;
    [SerializeField] private FlockBehaviour ColAvoidanceBehaviour;
    [SerializeField] private FlockBehaviour OffsetPursuitBehaviour;
    #endregion

    #region Default
    // Start is called before the first frame update
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
            Debug.LogError("Predator State Machine couldnt find flock");
        }
        #endregion

        ChangeStateTo(predatorState.ToString());
    }
    void Update()
    {
        stateDisplay.text = predatorState.ToString();
    }
    #endregion

    #region Predator Functionality
    public void DamageEnemy(PreyStateMachine prey)
    {
        prey.TakeDamage(attackDamage);

        if (prey.useHurtColor)
        {
            prey.PlayHurtColorEffectVoid(prey.gameObject.GetComponent<SpriteRenderer>(), 3, 1);
        }
    }
    #endregion

    #region Sate Machine Functionality

    #region Wander
    public IEnumerator WanderState()
    {
        Debug.Log("Wander: ENTER");

        flock.behaviour = WanderBehaviour;

        while (predatorState == PredatorStates.Wander)
        {
            foreach (FlockAgent agent in flock.agents)
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext);

                if (filteredContext.Count > 0)
                {
                    predatorState = PredatorStates.Seek;
                }
            }

            yield return null;      ///2:24:24///
        }

        Debug.Log("Wander: EXIT");
        ChangeStateTo(predatorState.ToString());
    }
    #endregion

    #region Seek
    public IEnumerator SeekState()
    {
        Debug.Log("Seek: ENTER");

        while (predatorState == PredatorStates.Seek)
        {
            if (flock.behaviour != SeekBehaviour)
            {
                flock.behaviour = SeekBehaviour;
            }

            yield return null;
        }

        Debug.Log("Seek: EXIT");
        ChangeStateTo(predatorState.ToString());
    }
    #endregion

    #region Attack

    #endregion

    #region Collision Avoidance

    #endregion

    #region Offset Pursuit

    #endregion

    #region State Changing
    void ChangeStateTo(string methodName)
    {
        StartCoroutine(methodName + "State"); //change the state to the passed value plus "State"
    }
    #endregion
    #endregion
}