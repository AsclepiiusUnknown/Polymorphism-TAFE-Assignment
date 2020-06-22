using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class CohesionBehaviour : FilteredFlockBehaviour
{
    //Another instance of the calculate move from within Flock Behaviour which checks for agents of the same flock in its surrounding context and then move as a single group to appear to be communicating rather than going in random directions
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);

        //if no neighbours stay still
        if (filteredContext.Count == 0)
        {
            return Vector2.zero;
        }

        //Add all points together and average
        Vector2 cohesionMove = Vector2.zero;
        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector2)item.position;
        }

        cohesionMove /= filteredContext.Count;
        //cohesionMove = cohesionMove / filteredContext.Count

        //Create offset from agent position
        cohesionMove -= (Vector2)agent.transform.position;

        return cohesionMove;
    }
}
