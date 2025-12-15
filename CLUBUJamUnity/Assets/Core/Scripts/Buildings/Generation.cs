using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Generation", menuName = "Scriptable Objects/Generation")]
public class Generation : ScriptableObject
{
    public Resource[] resourcesRequiredForGeneration;
    public int[] quantitiesRequiredForGeneration;
    [Space]
    public Resource[] resourcesRequiredForUpgrading;
    public int[] quantitiesRequiredForUpgrading;
    [Space]
    public Resource resourceGenerated;
    public int quantityGenerated;
    [Space]
    public Sprite buildingSprite;
}
