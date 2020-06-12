using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContextFilter : ScriptableObject //Script used to make one of the scriptable Filter objects
{
    public abstract List<Transform> Filter(FlockAgent agent, List<Transform> orignal); //Abstract list  of transforms used to filter objects
}
