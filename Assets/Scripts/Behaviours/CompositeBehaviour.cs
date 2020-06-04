using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{
    [System.Serializable]
    public class FlockClass
    {
        public FlockBehaviour behaviour;
        public float weight; //how much its prioritised (more is more)
    }

    public FlockClass[] Flocks;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        //setup move
        Vector2 move = Vector2.zero;

        //Iterate through behaviours
        for (int i = 0; i < Flocks.Length; i++)
        {
            Vector2 partialMove = Flocks[i].behaviour.CalculateMove(agent, context, areaContext, flock) * Flocks[i].weight;


            if (partialMove != Vector2.zero)
            {
                if (partialMove.SqrMagnitude() > Flocks[i].weight * Flocks[i].weight)
                {
                    partialMove.Normalize();
                    partialMove *= Flocks[i].weight;
                }
                move += partialMove;
            }
        }

        return move;
    }
}
