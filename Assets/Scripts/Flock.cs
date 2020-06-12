using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    #region Variables
    [Header("General")]
    public FlockAgent agentPrefab; //The prefab used as the agent
    [HideInInspector] //Hide within the unity inspector
    public List<FlockAgent> agents = new List<FlockAgent>(); //A list used to store all of the flocks agents
    public FlockBehaviour behaviour; //The Behaviour object that the flock will use to act appropriately
    public int agentsCount { get { return agents.Count; } } //Used to count how many agents there are in the flock

    [Header("Settings")]
    public bool randomiseSpeed = false; //Used to randomise the speed (currently not used)
    [Range(10, 500)]
    public int startingCount = 250; //How many agents the flock will spawn at the start
    const float AgentDensity = 0.08f; //How densly packed the agents will be in correspondance to eachother and their surroundings
    [Range(1f, 100f)]
    public float driveFactor = 10f; //Movement multiplier
    [Range(1f, 100f)]
    public float maxSpeed = 15f; //The max speed the agents can travel at (controls the speed more freely than the drive factor though)
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f; //The size of the radius used with behaviours such as avoidance top distance agents from eachother and avoid clusters
    [Range(1f, 50f)]
    public float areaRadius = 20f; //The size of the radious known as the area and used to calculate certain functions such as gathering data on the nearby objects
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f; //The multiplier used to effectively alter the avoidance radius

    //Square variables used to simplify the mathematical equations for caltulating certain data
    float squareMaxSpeed; //Max speed
    float squareNeighbourRadius; //Neighbourhood radius
    float squareAvoidanceRadius; //Avoidance radius

    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } } //Used to effectively gather and share data on the corresponding squared variable

    [HideInInspector]
    public List<Transform> context; //List of transforms which holds the surounding objects
    [HideInInspector]
    public List<Transform> areaContext; //Very much like the above list but instead being used in relation to the areaRadius in comparison with the NeighbourhoodRadius

    [Header("Color")]
    public ColorChangeState colorChangeState; //What kind of colors we want our agents to be (not currently used)
    public Color startColor; //the color the enemies will be when close to eachother
    public Color fadeToColor; //the color the enemies will be when further away from eachother
    public float colorLerpDivider = 6f; //The multiplier/divider used to calculate the Lerp rate for the gradient color effect
    #endregion

    #region Default
    void Start()
    {
        #region Square Setups
        //set up the squared values to be correct
        squareMaxSpeed = maxSpeed * maxSpeed; //max speed
        squareNeighbourRadius = neighbourRadius * neighbourRadius; //neighbourRadius
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier; //avoidance radius (kinda). 0.5 times bigger than neighbourRadius
        #endregion

        #region Setup each agent
        for (int i = 0; i < startingCount; i++) //For each agent in our flock
        {
            FlockAgent newAgent = Instantiate(agentPrefab, Random.insideUnitCircle * startingCount * AgentDensity, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), transform); //Create an agent prefab object within the scene at the correct position
            newAgent.Initialize(this); //Set up the agent with the FlockAgent script
            newAgent.name = "Agent " + 1; //Name it to its numarical correspondance
            agents.Add(newAgent); //Add a new agent to the list of agents for use later
        }
        #endregion
    }

    private void Update()
    {
        foreach (FlockAgent agent in agents) //For each FlockAgent within the list of all the agents (set up within the Start method)
        {
            #region Context Setup
            context = GetNearbyObjects(agent, neighbourRadius); //The context is the objects within the neighbourhood radius of the agent
            areaContext = GetNearbyObjects(agent, areaRadius); //The area context is the objects within the area radius of the agent
            #endregion

            #region Color Changing
            if (colorChangeState == ColorChangeState.Single) //If we want to use a single color
            {
                agent.GetComponent<SpriteRenderer>().color = startColor; //Set the agents color to be 
            }
            else if (colorChangeState == ColorChangeState.Gradient) //If we want a gradient effect
            {
                agent.GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, fadeToColor, context.Count / colorLerpDivider); //Fade between the two colors using the divider value as our time argument
            }
            #endregion

            #region Movement
            Vector2 move = behaviour.CalculateMove(agent, context, areaContext, this); //Calculate the agents movements for this frame and set move to its value
            move *= driveFactor; //Multiply the movements by the drive factor
            if (move.sqrMagnitude > squareMaxSpeed) //if the moves squared magnitude is greater than the squared max speed
            {
                if (randomiseSpeed) //if we want to randomise the speed
                {
                    float tempSpeedRandomiser = Random.Range(maxSpeed / 4, maxSpeed / 2); //set the temp speed to a random speed
                    move = move.normalized * (maxSpeed + tempSpeedRandomiser); //the move is set to a normalized value then mutliplied by the max speed plus our random speed
                }
                else //otherwise
                    move = move.normalized * maxSpeed; //the move is set to a normalized value then mutliplied by the max speed (no randomising)
            }
            agent.Move(move); //apply the movement to the agent
            #endregion
        }
    }
    #endregion

    #region Context Calculations
    List<Transform> GetNearbyObjects(FlockAgent agent, float radius) //Get the nearby objects in relation to the parameters
    {
        List<Transform> context = new List<Transform>(); //Make a new list called context
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, radius); //Make an array of collider 2Ds and set them to a circle at the agents position and the radius parameter

        foreach (Collider2D contextCollider in contextColliders) //For all of the colliders within our array of colliders
        {
            if (contextCollider != agent.AgentCollider) //If the created collider isnt the same as the agents collider
            {
                context.Add(contextCollider.transform); //Add the context collider we created to the context list
            }
        }
        return context; //Return the list context which contains our transforms
    }
    #endregion
}

public enum ColorChangeState
{
    Single,
    Gradient,
    None
}