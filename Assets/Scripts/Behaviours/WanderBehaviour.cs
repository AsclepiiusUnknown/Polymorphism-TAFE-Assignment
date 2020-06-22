using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Wander")]
public class WanderBehaviour : FilteredFlockBehaviour
{
    Path path = null;
    int? currentWaypoint = null;

    //Checks and returns both a vector 2 representing the centre position of the radius and a bool as to whether the radius is being used
    public (Vector2, bool) LimitedRadius(FlockAgent agent)
    {
        //Get dir towards center
        Vector2 centerOffset = (Vector2)path.waypoints[(int)currentWaypoint].position - (Vector2)agent.transform.position;

        //Dist to center
        float t = centerOffset.magnitude / path.pointRadius;

        if (t < path.returnPct)
        {
            return (Vector2.zero, true);
        }

        return (centerOffset, false);
    }

    // Used to Follow a chosen path and check if we need to return back to the first waypoint
    public Vector2 FollowPath(FlockAgent agent)
    {
        if (path == null)
            return Vector2.zero;

        if (currentWaypoint == null)
            currentWaypoint = 0;

        (Vector2 move, bool isAtRadius) = LimitedRadius(agent);

        if (isAtRadius)
        {
            currentWaypoint++;

            if (currentWaypoint >= path.waypoints.Count)
                currentWaypoint = 0;
        }
        return move;
    }

    //Used to find a path to follow and set it to our current path
    public void FindPath(FlockAgent agent, List<Transform> areaContext)
    {
        List<Transform> filteredContext = (filter == null) ? areaContext : filter.Filter(agent, areaContext);

        //if no neighbours maintain current allignment
        if (filteredContext.Count == 0)
        {
            return;
        }

        //get a radnom number, set path to that number
        int pathIndex = Random.Range(0, filteredContext.Count);
        path = filteredContext[pathIndex].GetComponentInParent<Path>(); //Biased towards the paths with more waypoints

    }

    //Another instance of the calculate move from within Flock Behaviour which checks for agents of the same flock in its surrounding context and uses the above functions to wander accordingly
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        if (path == null)
        {
            FindPath(agent, areaContext);
        }

        return FollowPath(agent);
    }


}
