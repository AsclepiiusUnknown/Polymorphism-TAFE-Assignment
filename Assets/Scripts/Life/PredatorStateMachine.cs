using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Flock))]
public class PredatorStateMachine : Life
{
    #region Variables
    public enum PredatorStates //States used for the control of the Predators behaviour in the environment
    {
        WanderSeek, //Default state - the predator wanders around looking for prey to chase
        Attack, //When criteriaa is met - Chase and attack enemies
        OffsetPursuit //When enemies are nearby - Chase nearby prey
        //Search. Taken out due to ambuity between Search and Seek, authorised by Andrew on 11/06/20
    }

    [Header("Speed")]
    public float speedMultiplier = 1.3f; //We multiply the flocks max speed by this when attacking
    private float normalSpeed; //Used to store the max speed and restore this normal max after attacking
    private bool isSpeedy = false; //used to check if we have already sped up during this attack phase

    [Header("Prepare Attack Timer")]
    public float prepareAttackTimeValue = 5; //The amount of time from starting pursuit till we go into attack
    private float prepareAttackTimer; //Timer for the above
    private bool isAttacking = false; //Bool to check and control if we are/arent attacking

    [Header("Max Attack Timer")]
    public float maxAttackTimeValue = 5; //the amount of time from starting attack till we end attacking
    private float maxAttackTimer; //Timer for the above

    [Header("State Machine Variables")]
    public PredatorStates predatorState; //Instance of the above predator states

    [Header("Behaviour Objects")]
    [SerializeField] private FlockBehaviour WanderSeekBehaviour; //behaviour object for wandering
    [SerializeField] private FlockBehaviour AttackBehaviour; //behaviour object for attacking
    [SerializeField] private FlockBehaviour OffsetPursuitBehaviour; //behaviour object for pursuing
    #endregion

    #region Default
    // Start is called before the first frame update
    void Start()
    {
        normalSpeed = flock.maxSpeed; //Store the normal speed

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

        ChangeStateTo(PredatorStates.WanderSeek.ToString()); //Start in the wander state
    }

    void Update()
    {
        stateDisplay.text = (stateDisplay != null) ? predatorState.ToString() : null; //Display the state using the text element if it exists

        #region TIMERS
        if (!isAttacking && predatorState == PredatorStates.OffsetPursuit) //if we arent currently attacking and we are in pursuit
        {
            prepareAttackTimer -= Time.deltaTime; //Countdown to attacking
        }

        #region Speeding Up
        if (isAttacking && predatorState == PredatorStates.Attack) //if we are attacking and in the attacking state
        {
            maxAttackTimer -= Time.deltaTime; //countdown to the end of attacking
            if (!isSpeedy) //if we havent already sped up
            {
                flock.maxSpeed *= speedMultiplier; //speed up
                isSpeedy = true; //we are now going faster
            }
        }
        else //otherwise
        {
            flock.maxSpeed = normalSpeed; //set our speed to the normal speed
            isSpeedy = false;
        }
        #endregion

        if (maxAttackTimer <= 0) //if we are out of attack time
        {
            isAttacking = false; //we are no longer attacking
            predatorState = PredatorStates.WanderSeek; //set wander state
            ChangeStateTo(PredatorStates.WanderSeek.ToString()); //change to wander state
            maxAttackTimer = maxAttackTimeValue; //reset the timer
        }
        #endregion
    }
    #endregion

    #region Predator Functionality
    public void KillEnemy(FlockAgent prey) //used to kill the given prey (not currently functional)
    {
        prey.Die(); //Call them to die
    }
    #endregion

    #region Sate Machine Functionality
    // * //
    #region Wander Seek
    public IEnumerator WanderSeekState() //used to wander
    {
        Debug.Log("Wander: ENTER"); //log entering this state

        flock.behaviour = WanderSeekBehaviour; //set the correc behaviour

        while (predatorState == PredatorStates.WanderSeek) //while we are in this state
        {
            foreach (FlockAgent agent in flock.agents) //for each of the agents in our flock
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext); //make a list of agents nearby from other flocks

                if (filteredContext.Count > 0) //if there are agents on the list (aka enemies nearby)
                {
                    predatorState = PredatorStates.OffsetPursuit; //change to pursuit
                }
            }

            yield return null; //return out of the while loop
        }

        Debug.Log("Wander: EXIT"); //log the state exit
        ChangeStateTo(predatorState.ToString()); //change state
    }
    #endregion

    #region Offset Pursuit
    public IEnumerator OffsetPursuitState() //used to pursue
    {
        Debug.Log("Offset Pursuit: ENTER"); //log entering this state

        flock.behaviour = OffsetPursuitBehaviour; //set the correct behaviour

        prepareAttackTimer = prepareAttackTimeValue; //set the prepare attack timer

        while (predatorState == PredatorStates.OffsetPursuit) //while we are in pursuit
        {
            foreach (FlockAgent agent in flock.agents) //for each of the agents in our flock
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext); //make a list of agents nearby from other flocks

                if (filteredContext.Count <= 0) //if there are no enemies nearby
                {
                    predatorState = PredatorStates.WanderSeek; //change back to wander
                }
            }

            if (prepareAttackTimer <= 0 && !isAttacking) //if we are ready to attack and arent already attacking
            {
                predatorState = PredatorStates.Attack; //change to attack state
            }

            yield return null; //return out of the while loop
        }

        Debug.Log("Offset Pursuit: EXIT"); //log exiting the state
        ChangeStateTo(predatorState.ToString()); //change state
    }
    #endregion

    #region Attack
    public IEnumerator AttackState() //used to attack
    {
        Debug.Log("Attack: ENTER"); //log entering the state

        flock.behaviour = AttackBehaviour; //set the behaviour

        prepareAttackTimer = prepareAttackTimeValue; //reset the prepare timer
        isAttacking = true; //we are now attacking
        maxAttackTimer = maxAttackTimeValue; //start the max attack timer

        while (predatorState == PredatorStates.Attack) //while we are attacking
        {
            foreach (FlockAgent agent in flock.agents) //for each agent in our flock
            {
                List<Transform> filteredContext = (contextFilter == null) ? flock.areaContext : contextFilter.Filter(agent, flock.areaContext); //make a list of agents nearby from other flocks

                //? KILLING // functionality for killing the enemies that was extra and causing bugs so it was sadly taken out due to a lack of time
                /** int randomAgent = Random.Range(0, filteredContext.Count - 1);

                for (int i = 0; i < filteredContext.Count; i++)
                {
                    FlockAgent flockAgent = filteredContext[i].gameObject.GetComponent<FlockAgent>();

                    if (flockAgent != null && i == randomAgent)
                    {
                        flockAgent.Die();
                        print("killed agent No: " + i);
                    }
                }*/

                if (filteredContext.Count <= 0) //if there are no enemies nearby
                {
                    isAttacking = false; //we are no longer attacking
                    predatorState = PredatorStates.WanderSeek; //change to wander state
                }
            }

            yield return null; //return out of the while loop
        }

        Debug.Log("Attack: EXIT"); //log exiting the state
        ChangeStateTo(predatorState.ToString()); //change state
    }
    #endregion

    #region State Changing
    protected void ChangeStateTo(string methodName) //used to change state with a string
    {
        StartCoroutine(methodName + "State"); //change the State to the passed value plus "State"
    }
    #endregion
    #endregion
}