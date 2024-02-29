using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Character : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
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
        float t = 1;
        conn.spline.Evaluate(isReverse ? 1-t : t, out Unity.Mathematics.float3 pos, out Unity.Mathematics.float3 tangeant, out Unity.Mathematics.float3 up);

        transform.position = pos;

        onto = isReverse ? conn.a : conn.b;
    }
    Vector3 startPointerPos;
    public void OnDrag(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
        /*Debug.Log(eventData.pointerDrag);
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (plane.Raycast(ray, out float dist))
        {
            Vector3 pos = ray.GetPoint(dist);
            Debug.DrawLine(startPointerPos, pos, Color.red, 1);
        }*/

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        /*Debug.Log(eventData.pointerDrag);*/
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (plane.Raycast(ray, out float dist)) {
            startPointerPos = ray.GetPoint(dist);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       /* Debug.Log(eventData.pointerDrag);*/
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (plane.Raycast(ray, out float dist))
        {
            Vector3 pos = ray.GetPoint(dist);
            Debug.DrawLine(startPointerPos, pos, Color.red, 1);
            if(Vector3.Distance(startPointerPos, pos) > .5f)
            {
                Connection c = onto.FindByDirection((pos - startPointerPos).normalized);
                Debug.Log(c);
                GoTo(c);
            }
        }
    }
}
