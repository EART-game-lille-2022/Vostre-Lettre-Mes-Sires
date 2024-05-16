using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

public class Character : MonoBehaviour/*, IDragHandler*/, IPointerDownHandler, IPointerUpHandler
{
    public StopPoint onto;


    public float speed = 10;
    
    // debug test
    // public Vector3 targetDirection;
    void Start()
    {
        // GoTo(onto.FindByDirection(targetDirection));
        transform.position = onto.transform.position;
    }

    bool isMoving { get { return onto == null; } }


    private void GoTo(Connection temp_conn)
    {
        if (temp_conn == null) return;

        bool isReverse = temp_conn.b == onto;

        // conn.spline.Evaluate(0, out Unity.Mathematics.float3 pos, out Unity.Mathematics.float3 tangeant, out Unity.Mathematics.float3 up);
        
        // float t = 1;
        onto = null;
        NelowGames.Tween.DoTween((t) => {
            if(temp_conn.spline.Evaluate(isReverse ? 1 - t : t, out Unity.Mathematics.float3 pos, out Unity.Mathematics.float3 tangeant, out Unity.Mathematics.float3 up))
                transform.position = pos;
        }, temp_conn.GetLength()/speed, () => {
            onto = isReverse ? temp_conn.a : temp_conn.b;
        });
        // temp_conn.spline.Evaluate(isReverse ? 1 - t : t, out Unity.Mathematics.float3 pos, out Unity.Mathematics.float3 tangeant, out Unity.Mathematics.float3 up);

        // transform.DOMove(pos, 1);

    }
    Vector3 startPointerPos;
    private void Update()
    {
            
         
    }
    /*public void OnDrag(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
        Debug.Log(eventData.pointerDrag);
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (plane.Raycast(ray, out float dist))
        {
            Vector3 pos = ray.GetPoint(dist);
            Debug.DrawLine(startPointerPos, pos, Color.red, 1);
        }

    }*/

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag);

        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (plane.Raycast(ray, out float dist))
        {
            startPointerPos = ray.GetPoint(dist);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isMoving) return;

        Debug.Log(eventData.pointerDrag);

        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (plane.Raycast(ray, out float dist))
        {
            Vector3 pos = ray.GetPoint(dist);
            Debug.DrawLine(startPointerPos, pos, Color.red, 1);
            if (Vector3.Distance(startPointerPos, pos) > 1f)
            {
                Connection c = onto.FindByDirection((pos - transform.position).normalized);
                Debug.Log(c, c.gameObject);
                GoTo(c);
            }
        }
    }
}
