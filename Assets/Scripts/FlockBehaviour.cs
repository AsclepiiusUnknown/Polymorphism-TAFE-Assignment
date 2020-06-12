using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehaviour : ScriptableObject //Script used as a root of making the scriptable Behaviour objects
{
    public abstract Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock); //Calculate the movement for the given parameters
}
