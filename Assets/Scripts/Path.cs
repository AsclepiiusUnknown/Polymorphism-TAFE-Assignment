using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    #region Variables
    public List<Transform> waypoints; //List of transforms used as waypoins for the agents to travel to

    public float pointRadius;//Radius used as the min distance that each agent ahs to make to go to the next point
    public float returnPct = 0.9f; //The perectn of agents who succesfully make the journey

    public Vector3 gizmoSize = Vector3.one; //The size of the gizmos we use to visualise the paths and points
    #endregion

    #region Draw the Gizmos
    //Draw gizmos on all the paths and points
    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count == 0) //IS the waypoints are null or there are none
            return; //Return to avoid errors

        for (int index = 0; index < waypoints.Count; index++) //For each of the waypoints
        {
            #region Points
            Transform waypoint = waypoints[index]; //Set the transform waypoint to be this waypoints position

            if (waypoint == null) //if the waypoint is null
                continue; //continue through the script

            Gizmos.color = Color.magenta; //Set the color to Magneta
            Gizmos.DrawCube(waypoint.position, gizmoSize); //Draw a cube at the position of the waypoint using the specified size variable
            #endregion

            #region Paths
            if (index + 1 < waypoints.Count && waypoints[index + 1] != null) //Check if next waypoint is valid
            {
                Gizmos.color = Color.cyan;//Set the color to Cyan
                Gizmos.DrawLine(waypoint.position, waypoints[index + 1].position); //Draw a line between the two points
            }
            #endregion
        }
    }
    #endregion
}
