using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Request/New Request", order = 1)]
public class SORequest : ScriptableObject
{
    public string titleOfTheRequest;
    public string content;
    public List<string> pathPointsTagsAcceptedForDeparture = new List<string>();
    public List<string> pathPointsTagsAcceptedForArrival = new List<string>();
    public GameObject whoHasIt;
}

