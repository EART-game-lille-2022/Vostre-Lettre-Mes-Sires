using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour, IDragHandler
{
    public StopPoint onto;

    public Vector3 targetDirection;
    void Start()
    {
        GoTo(onto.FindByDirection(targetDirection));
    }


    private void GoTo(Connection conn) {
        if (conn == null) return;

        bool isReverse = conn.b == onto;

        // conn.spline.Evaluate(0, out Unity.Mathematics.float3 pos, out Unity.Mathematics.float3 tangeant, out Unity.Mathematics.float3 up);
        conn.spline.Evaluate(0, out Unity.Mathematics.float3 pos, out Unity.Mathematics.float3 tangeant, out Unity.Mathematics.float3 up);

        transform.position = pos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
        Debug.Log(eventData.pointerDrag);
    }
}
