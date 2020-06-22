using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Allignment")]
public class AlligntmentBehaviour : FilteredFlockBehaviour
{
    //Another instance of the calculate move from within Flock Behaviour which checks for agents of the same flock in its surrounding context and then moves accorningly to align itself with the others
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
