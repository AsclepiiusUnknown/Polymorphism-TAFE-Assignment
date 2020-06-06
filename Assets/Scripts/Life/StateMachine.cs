using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour
{
    /*#region Variables
    public enum State //Create an enumarator to store all of our states and control thier operations
    {
        Patrol,
        Seek,
        Attack,
        Flee,
    }

    [Header("General")]
    public State state; //creating a cariable for our local enum
    public GameObject AI; //The AI object in game
    public GameObject Player; //The player object in game
    private Enemy enemyScript; //The enemy script
    public Player playerScript; //the player script
    public float normalSpeed, runningSpeed; //Two different speeds used to alter the Nav Mesh's speed while in various states

    [Header("Patrol State")]
    private PatrolAI patrolAI; //the script for the patrol state

    [Header("Seek State")]
    private MoveAI moveAI; //The script for the seek state
    [Range(0, 25)]
    public float moveRadiusSize; //the size of the radius in which the player will give off information to the AI for seek state changes
    public bool isSeeking = false; //a variable used to check if the AI is in seeking state
    public float seekDistance; //a visual representation of the distance between the player and AI

    [Header("Attack State")]
    private AttackAI attackAI; //the script used for the attack state
    public bool isAttacking; //a bool used to check whether or not the AI is attacking the player
    public float attackRadiusSize; //the size of the radius in which the player will give off information to the AI for attack state changes

    [Header("FleeState")]
    private FleeAI fleeAI; //a variable used to check if the AI is in fleeing state
    #endregion

    private void Start()
    {
        //set all the variables to thier respective component values
        moveAI = GetComponent<MoveAI>();
        patrolAI = GetComponent<PatrolAI>();
        attackAI = GetComponent<AttackAI>();
        fleeAI = GetComponent<FleeAI>();
        enemyScript = GetComponent<Enemy>();
        isSeeking = GetComponent<MoveAI>().isSeeking;

        //Debugging to check if objects are attached correctly
        if (moveAI == null)
        {
            Debug.LogError("moveAI not attached to StateMachine");
        }
        if (patrolAI == null)
        {
            Debug.LogError("patrolAI not attached to StateMachine");
        }

        //start the program in the patrol state
        ChangeStateTo("Patrol");
    }

    private void Update()
    {
        //update the seek distance constantly to make it an accurate representation
        seekDistance = Vector3.Distance(AI.transform.position, Player.transform.position);
    }

    #region Patrol
    IEnumerator PatrolState()
    {
        Debug.Log("Patrol: Enter"); //log the state
        state = State.Patrol; //set the state 
        while (state == State.Patrol) //run the following code while the state is correct
        {
            patrolAI.isPatrolling = true; //the Ai is patrolling
            patrolAI.Patrol(); //call the seperate script to run the code for this state

            CheckForChange(); //check if there needs to be a change

            yield return 0; //Wait one frame
        }
        ChangeStateTo(state.ToString()); //set the state to whatever it currently is (changes are made in CheckForChange)
    }
    #endregion

    #region Seek
    public IEnumerator SeekState()
    {
        Debug.Log("Seek: Enter");//log the state
        state = State.Seek;//set the state 
        while (state == State.Seek)//run the following code while the state is correct
        {
            isSeeking = true; //the Ai is seeking
            moveAI.Move(); //call the seperate script to run the code for this state

            CheckForChange(); //check if there needs to be a change

            yield return 0; //Wait one frame
        }
        ChangeStateTo(state.ToString()); //set the state to whatever it currently is (changes are made in CheckForChange)
    }
    #endregion

    #region Attack
    IEnumerator AttackState()
    {
        Debug.Log("Attack: Enter");//log the state
        state = State.Attack;//set the state 
        while (state == State.Attack)//run the following code while the state is correct
        {
            isAttacking = true; //the Ai is attacking
            attackAI.Attack(); //call the seperate script to run the code for this state

            CheckForChange(); //check if there needs to be a change

            yield return 0; //Wait one frame
        }
        ChangeStateTo(state.ToString());//set the state to whatever it currently is (changes are made in CheckForChange)
    }
    #endregion

    #region Flee
    IEnumerator FleeState()
    {
        Debug.Log("Flee: Enter");//log the state
        state = State.Flee;//set the state 
        while (state == State.Flee)//run the following code while the state is correct
        {
            fleeAI.isFleeing = true;//the Ai is fleeing

            CheckForChange(); //check if there needs to be a change

            yield return 0; //Wait one frame
        }
        ChangeStateTo(state.ToString()); //set the state to whatever it currently is (changes are made in CheckForChange)
    }
    #endregion

    void ChangeStateTo(string methodName)
    {
        StartCoroutine(methodName + "State"); //change the state to the passed value plus "State"
    }

    void CheckForChange()
    {
        if ((enemyScript.health < (enemyScript.maxHealth / 4)) || enemyScript.health < playerScript.health) //check if there needs to be a change to flee state based on the requirements
        {
            Debug.Log("Exiting State"); //Log the state exit
            Debug.Log("Changing to Flee"); //Log what we are changing to

            state = State.Flee; //Set the state accordingly

            //we arent doing any of the following states
            patrolAI.isPatrolling = false;
            isSeeking = false;
            isAttacking = false;

            //we are fleeing
            fleeAI.isFleeing = true;
        }
        else if (Vector3.Distance(AI.transform.position, Player.transform.position) <= attackRadiusSize) //check if there needs to be a change to flee state based on the requirements
        {
            Debug.Log("Exiting State"); //Log the state exit
            Debug.Log("Changing to Attack"); //Log what we are changing to

            state = State.Attack; //Set the state accordingly

            //we arent doing any of the following states
            patrolAI.isPatrolling = false;
            isSeeking = false;
            fleeAI.isFleeing = false;

            //we are attacking
            isAttacking = true;
        }
        else if (Vector3.Distance(AI.transform.position, Player.transform.position) <= moveRadiusSize) //check if there needs to be a change to flee state based on the requirements
        {
            Debug.Log("Exiting State"); //Log the state exit
            Debug.Log("Changing to Seek"); //Log what we are changing to

            state = State.Seek; //Set the state accordingly

            //we arent doing any of the following states
            patrolAI.isPatrolling = false;
            isAttacking = false;
            fleeAI.isFleeing = false;

            //we are seeking
            isSeeking = true;
        }
        else //otherwise just return to patrol state
        {
            Debug.Log("Exiting State"); //Log the state exit
            Debug.Log("Changing to Patrol"); //Log what we are changing to

            state = State.Patrol; //Set the state accordingly

            //we arent doing any of the following states
            isSeeking = false;
            fleeAI.isFleeing = false;
            isAttacking = false;

            //we are patrolling
            patrolAI.isPatrolling = true;
        }
    }*/
}