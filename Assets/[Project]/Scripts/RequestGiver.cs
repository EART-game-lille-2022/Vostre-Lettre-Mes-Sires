using System.Collections.Generic;
using UnityEngine;

public class RequestGiver : MonoBehaviour
{
    Collider collider;
    public RequestPocket questGiverPocket;
    public List<RequestPocket> pawnsInZonePockets;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        RequestPocket _collisionRT = collision.GetComponent<RequestPocket>();
        print("in");
        if (_collisionRT != null)
        {
            pawnsInZonePockets.Add(_collisionRT);

        }
    }

    private void OnTriggerExit(Collider collision)
    {
        RequestPocket _collisionRT = collision.GetComponent<RequestPocket>();
        pawnsInZonePockets.Remove(_collisionRT);
    }
    void ShowRequests() { }

    void GiveRequest(SOQuickRequest _requestSelected)
    {
        GameObject _selectedPawn = null;//GetSelectedPawn();
        _selectedPawn.GetComponent<RequestPocket>().RequestList.Add(_requestSelected);
    }



}

