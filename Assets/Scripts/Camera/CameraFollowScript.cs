using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {

    public Transform cameraPositionTarget;
    public Transform forwardLook;
    [Range(0, 1)] public float smoothTime;
    [Range(0, 1)] public float rotationTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = Vector3.Lerp(transform.position, cameraPositionTarget.position, smoothTime);

        Quaternion destRot = Quaternion.LookRotation(forwardLook.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, destRot, rotationTime);
	}
}
