using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/LimitedRadius")]
public class LimitedRadiusBehaviour : FlockBehaviour
{
    public Vector2 center;
    public float radius = 15f;
    public float returnPercent = 0.9f;

    //Another instance of the calculate move from within Flock Behaviour which checks for the center of the game and the size of the predetermined radius
    // it will then use this data to try and stay within that radius using the return percent to decide how successful this is and how often
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        //Direction towards center
        Vector2 centreOffset = center - (Vector2)agent.transform.position;

        //Distance to center
        float t = centreOffset.magnitude / radius;
        if (t < returnPercent)
        {
            return Vector2.zero;
        }

        return centreOffset;
        // * Variations:
        //or
        //return centreOffset * t;
        //or
        //return centreOffset * t * t;
    }
}
