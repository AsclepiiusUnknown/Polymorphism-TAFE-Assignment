using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Pursuit")]
public class PursuitBehaviour : FilteredFlockBehaviour
{
    //Another instance of the calculate move from within Flock Behaviour which checks for agents of an opposing flock and moves towards them in pursuit
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        List<Transform> areaFilteredContext = (filter == null) ? areaContext : filter.Filter(agent, areaContext);
        if (areaFilteredContext.Count == 0)
        {
            return Vector2.zero;
        }
        Vector2 move = Vector2.zero;
        foreach (Transform item in areaFilteredContext)
        {
            float distance = Vector2.Distance(item.position, agent.transform.position);
            float distancePercent = distance / flock.areaRadius;
            float inverseDistancePercent = 1 - distancePercent;
            float weight = inverseDistancePercent / areaFilteredContext.Count;// flock.agentsCount;
            Vector2 direction = (item.position - agent.transform.position) * weight;
            move += direction; //* weight;
        }
        return move;
    }
}
