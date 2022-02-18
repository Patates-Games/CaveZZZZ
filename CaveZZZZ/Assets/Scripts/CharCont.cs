using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCont : MonoBehaviour
{
    CharacterController controller;
    public float speed = 5;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift)) speed = 5f;
        if (Input.GetKeyDown(KeyCode.LeftShift)) speed = 7f;


        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.deltaTime * speed;
        controller.Move(move);
    }
}
