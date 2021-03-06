﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Attack")]
public class AttackBehaviour : FilteredFlockBehaviour
{
    public PredatorStateMachine thisPredator;

    //Another instance of the calculate move from within Flock Behaviour which checks for agents of a different flock in its surrounding context and then attempts to chasxe them and eliminate them where possible
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        List<Transform> areaFilteredContext = (filter == null) ? areaContext : filter.Filter(agent, areaContext);

        if (areaContext.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 move = Vector2.zero;

        foreach (Transform item in areaFilteredContext)
        {
            if (item.gameObject.GetComponent<PreyStateMachine>() != null)
            {
                FlockAgent itemPrey = item.GetComponent<FlockAgent>();
                thisPredator.KillEnemy(itemPrey);
            }

            float distance = Vector2.Distance(item.position, agent.transform.position);
            float distancePct = distance / flock.areaRadius;
            float inverseDistPct = 1 - distancePct;
            float weight = inverseDistPct / flock.agentsCount;

            Vector2 direction = (item.position - agent.transform.position) * weight;

            if (direction.SqrMagnitude() > weight * weight)
            {
                direction.Normalize();
                direction *= weight;
            }
            move += direction * weight;
        }
        return move;
    }
}
