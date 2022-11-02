using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MissionObject", order = 1)]
public class MissionObject : ScriptableObject
{
    public string mission; //name of the mission
    public string text; //description of the mission
    public int progress; //progress
    public int completionProgress; //amount of progress for completion
    public bool completed;
    public MissionObject[] nextMissions; //missions to add after completion
}
