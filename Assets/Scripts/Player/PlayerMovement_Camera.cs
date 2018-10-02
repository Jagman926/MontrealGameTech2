using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Camera : MonoBehaviour 
{
	public float speedH = 2.0f;
    private float yaw = 0.0f;

    // Update is called once per frame
    void Update()
    {
        yaw -= speedH * Input.GetAxis("Mouse Y");
		Mathf.Clamp(yaw, -90.0f, 90.0f);
        transform.eulerAngles = new Vector3(yaw, transform.eulerAngles.y, transform.eulerAngles.z);
    }

}
