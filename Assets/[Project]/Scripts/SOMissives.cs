using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Missions/New Mission", order = 1)]
public class SOMissives : ScriptableObject
{
    public string titleOfTheMission;
    public string content;
    public List<string> pathPointsTagsAcceptedForDeparture = new List<string>();
    public List<string> pathPointsTagsAcceptedForArrival = new List<string>();
    public GameObject whoHasIt;
}
