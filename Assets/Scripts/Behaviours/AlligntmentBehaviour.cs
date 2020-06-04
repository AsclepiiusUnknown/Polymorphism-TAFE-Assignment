using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Allignment")]
public class AlligntmentBehaviour : FilteredFlockBehaviour
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
        Vector2 allignmentMove = Vector2.zero;
        foreach (Transform item in filteredContext)
        {
            allignmentMove += (Vector2)item.transform.up;
        }

        allignmentMove /= filteredContext.Count;

        return allignmentMove;
    }
}
