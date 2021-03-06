﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	//variables
	public Transform  target;
	public float smoothing = 5f;

	Vector3 offset ; 
	// Use this for initialization
	void Start () {
		//calculate initial offset position
		offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Create a postion the camera is aiming for based on the offset from the target.
		Vector3 targetCamPos = target.position + offset;

		//Smoothly interpolate between the camera's current position and it's target position 
		transform.position = Vector3.Lerp(transform.position ,targetCamPos , smoothing);

	}
}
