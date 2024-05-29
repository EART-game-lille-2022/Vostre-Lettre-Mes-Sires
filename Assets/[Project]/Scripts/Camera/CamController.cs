using UnityEngine;
using UnityEngine.InputSystem;

public class CamController : MonoBehaviour
{
    public Vector2 direction;
    public float movespeed;

    public Camera cam;
    public float minZoom = 5, maxZoom = 100, zoomSpeed = .1f, zoomDamp = 1f;
    float zoom, zoomVel;
    
    Vector3 rightClickPos;

    public void start()
    {
        zoom = cam.orthographicSize;
    }

    public void OnCameraMovement(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    void Update() {
        // transform.Translate(new Vector3(direction.x, direction.y,0) * movespeed * Time.deltaTime);
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0) * movespeed * Time.deltaTime);

        zoom = Mathf.Clamp(zoom * (1-Input.mouseScrollDelta.y * zoomSpeed), minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref zoomVel, zoomDamp);

        if(Input.GetMouseButtonDown(1))
        {
            rightClickPos = Input.mousePosition;
        }
        if(Input.GetMouseButton(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float dist))
            {
                Vector3 pos = ray.GetPoint(dist);
                
                Ray ray2 = cam.ScreenPointToRay(rightClickPos);
                if (plane.Raycast(ray2, out float dist2)) {
                    Vector3 pos2 = ray2.GetPoint(dist2);
                    Vector3 delta = pos2 - pos;
                    transform.Translate(delta);
                }
            }
            rightClickPos = Input.mousePosition;
        }
    }
}
