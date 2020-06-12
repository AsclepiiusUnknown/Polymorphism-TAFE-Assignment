using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))] //Needs a Collider2D component
public class FlockAgent : MonoBehaviour
{
    #region Varaibles
    Flock agentFlock; // the agents Flock script reference
    public Flock AgentFlock { get { return agentFlock; } } //Special reference to the agents flock script

    private Collider2D agentCollider; //The agents collider
    public Collider2D AgentCollider { get { return agentCollider; } } //Special reference to the agents collider
    #endregion

    #region Default
    void Start()
    {
        agentCollider = GetComponent<Collider2D>(); //The collider is equal to the collider ont he agent object
        if (agentCollider == null) //if there is no collider
        {
            Debug.LogError("FlockAgent.cs Cant find Collider2D"); //Log the error
            return; //Return out of the function
        }
    }
    #endregion

    #region Agent Functions
    public void Initialize(Flock flock)
    {
        agentFlock = flock; //Set this agent to equal the parameter passed through
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity; //Set the velocity up
        transform.position += (Vector3)velocity * Time.deltaTime; //Move the agent in the passed parameter
    }
    #endregion
}
