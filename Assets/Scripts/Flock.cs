using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public enum ColorChangeState { Single, Gradient, None };

    public int agentsCount { get { return agents.Count; } }

    [Header("General")]
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour; //The Behaviour

    [Header("Settings")]
    public bool randomiseSpeed = false;
    [Range(10, 500)]
    public int startingCount = 250;
    const float AgnetDensity = 0.08f;
    [Range(1f, 100f)]
    public float driveFactor = 10f; //Speed
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;
    [Range(1f, 50f)]
    public float areaRadius = 20f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f; //Multiplier

    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squareAvoidanceRadius; //Radius

    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    [Header("Color")]
    public ColorChangeState colorChangeState;
    public Color startColor;
    public Color fadeToColor;
    public float colorLerpDivider = 6f;


    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier; //0.5 times bigger than neighbourRadius

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(agentPrefab, Random.insideUnitCircle * startingCount * AgnetDensity, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), transform);
            newAgent.Initialize(this);
            newAgent.name = "Agent " + 1;
            agents.Add(newAgent);
        }
    }

    private void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent, neighbourRadius);
            List<Transform> areaContext = GetNearbyObjects(agent, areaRadius);

            //FOR TESTING ONLY
            if (colorChangeState == ColorChangeState.Single)
            {
                agent.GetComponent<SpriteRenderer>().color = startColor;
            }
            else if (colorChangeState == ColorChangeState.Gradient)
            {
                agent.GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, fadeToColor, context.Count / colorLerpDivider);
            }
            /*Color spriteColor = agent.GetComponent<SpriteRenderer>().color;
            spriteColor = Color.Lerp(spriteColor, new Color(0, spriteColor.g, spriteColor.b, spriteColor.a), context.Count / 6f);*/

            Vector2 move = behaviour.CalculateMove(agent, context, areaContext, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                if (randomiseSpeed)
                {
                    float tempSpeedRandomiser = Random.Range(maxSpeed / 4, maxSpeed / 2);
                    move = move.normalized * (maxSpeed + tempSpeedRandomiser);
                }
                else
                    move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent, float radius)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, radius);

        foreach (Collider2D contextCollider in contextColliders)
        {
            if (contextCollider != agent.AgentCollider)
            {
                context.Add(contextCollider.transform);
            }
        }
        return context;
    }

}
