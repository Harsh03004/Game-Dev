using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class YourScriptName : MonoBehaviour
{
    //component
    CharacterController controller;

    //moving 
    private float inputx;
    private float intputz;
    private Vector3 vec3;
    private float velocity;

    void Start()
    {
        velocity = 0.3f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        inputx = Input.GetAxis("Horizontal");
        intputz = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        vec3 = new Vector3(inputx*velocity, 0, intputz*velocity);
        controller.Move(vec3);
    }
}
