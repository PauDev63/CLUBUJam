using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "Scriptable Objects/Task")]
public class Task : ScriptableObject
{
    public TaskStep[] resourcesNeededSteps;
    public TaskStep[] workingSteps;
    public TaskStep[] resourcesGeneratedSteps;
    //[NonSerialized] public Building targetBuilding;
}
