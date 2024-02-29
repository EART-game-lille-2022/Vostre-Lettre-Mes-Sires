using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchesManager : MonoBehaviour
{
    public MapTouche map;
    public Vector2 ui;

    private void Awake()
    {
        map = new MapTouche();
        map.Controles.ToucheOnce.performed += ctx => Tir();
        map.Controles.JoystickMovement.performed += ctx => ui = map.Controles.JoystickMovement.ReadValue<Vector2>();
        map.Controles.JoystickMovement.canceled += ctx => ui = Vector2.zero;


    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Tir()
    {
        Debug.Log("tir");
    }
}
