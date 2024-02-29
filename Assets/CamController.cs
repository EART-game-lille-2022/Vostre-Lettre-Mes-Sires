using UnityEngine;
using UnityEngine.InputSystem;

public class CamController : MonoBehaviour
{
    public Vector2 direction;
    public float movespeed;
    


    public void Awake()
    {
        
    }

    public void OnCameraMovement(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    void Update()
    {
        transform.Translate(new Vector3(direction.x, direction.y,0) * movespeed * Time.deltaTime);
    }
}
