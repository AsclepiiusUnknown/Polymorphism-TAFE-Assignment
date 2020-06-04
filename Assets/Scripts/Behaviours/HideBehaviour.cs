using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Hide")]
public class HideBehaviour : FilteredFlockBehaviour
{
    public ContextFilter obstaclesFilter;

    public float hideBehindObstacleDist = 2f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        //hide from enemies
        List<Transform> filteredContext = (filter == null) ? areaContext : filter.Filter(agent, areaContext);
        //Hide behind Obstacles
        List<Transform> obstaclesContext = (filter == null) ? areaContext : obstaclesFilter.Filter(agent, areaContext);

        if (filteredContext.Count == 0)
        {
            return Vector2.zero;
        }

        //Find nearest obstacle
        float nearestDist = float.MaxValue;
        Transform nearestObstacle = null;

        foreach (Transform item in obstaclesContext)
        {
            float Distance = Vector2.Distance(item.position, agent.transform.position);

            if (Distance < nearestDist)
            {
                nearestObstacle = item;
                nearestDist = Distance;
            }
        }

        //return if no obstacle
        if (nearestObstacle == null)
            return Vector2.zero;

        Vector2 move = Vector2.zero;
        foreach (Transform item in filteredContext)
        {
            //Dir from item to nearest obstacle
            Vector2 obstacleDir = nearestObstacle.position - item.position;

            //Add to that dir to get point behind obstacle
            obstacleDir += obstacleDir.normalized * hideBehindObstacleDist;

            //Get Pos
            Vector2 hidePos = ((Vector2)item.position) + obstacleDir;

            move += hidePos;
        }
        move /= filteredContext.Count;

        //DEBUGGING ONLY
        Debug.DrawRay(move, Vector2.up * 1f);

        //Dir AI wants to move in (the Offset)
        move -= (Vector2)agent.transform.position;

        return move;
    }
}
