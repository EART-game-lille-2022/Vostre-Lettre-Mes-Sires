using UnityEngine;

[CreateAssetMenu(fileName = "New Quick Request", menuName = "Request/New Quick Request", order = 2)]
public class SOQuickRequest : ScriptableObject
{
    public string titleOfTheRequest;
    public string content;
    public int value;
    public RequestPocket whoHasIt;
    public float timeLeftForPerfect;
}