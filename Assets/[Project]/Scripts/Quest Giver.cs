using System.Collections.Generic;
using UnityEngine;

public class RequestGiver : MonoBehaviour
{
    RequestPocket questGiverPocket;
    List<RequestPocket> pawnsInZonePockets;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pawnsInZonePockets.Add(collision.GetComponent<RequestPocket>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RequestPocket _collisionRT = collision.GetComponent<RequestPocket>();
        pawnsInZonePockets.Remove(_collisionRT);
    }
    void ShowRequests() { }

    void GiveRequest(SOQuickRequest _requestSelected)
    {
        GameObject _selectedPawn = GetSelectedPawn();
        _selectedPawn.GetComponent<RequestPocket>().RequestList.Add(_requestSelected);
    }



}

