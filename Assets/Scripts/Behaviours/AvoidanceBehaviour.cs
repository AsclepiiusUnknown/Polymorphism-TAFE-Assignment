using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);

        //if no neighbours maintain current allignment
        if (filteredContext.Count == 0)
        {
            return agent.transform.up;
        }

        //Add all points together and average
        Vector2 avoidanceMove = Vector2.zero;
        int avoidCount = 0;
        foreach (Transform item in filteredContext)
        {
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                avoidCount++;
                avoidanceMove += (Vector2)(agent.transform.position - item.position);
            }
        }
        if (avoidCount > 0)
        {
            avoidanceMove /= avoidCount;
        }

        return avoidanceMove;
    }
}
