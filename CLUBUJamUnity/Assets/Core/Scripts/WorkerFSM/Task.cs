using System.Collections;
using UnityEngine;

public class Task
{

    public Task(Building _targetBuilding)
    {
        resourcesNeededSteps = new TaskStep[3]{ TaskStep.Return, TaskStep.Fetch, TaskStep.Drop };
        workingSteps = new TaskStep[1]{ TaskStep.Work };
        resourcesGeneratedSteps = new TaskStep[3] { TaskStep.Fetch, TaskStep.Return, TaskStep.Drop };

        targetBuilding = _targetBuilding;
    }
    
    public TaskStep[] resourcesNeededSteps;
    public TaskStep[] workingSteps;
    public TaskStep[] resourcesGeneratedSteps;
    public Building targetBuilding;
}
