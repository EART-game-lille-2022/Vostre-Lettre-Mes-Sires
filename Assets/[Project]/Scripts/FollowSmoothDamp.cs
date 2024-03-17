using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowSmoothDamp : MonoBehaviour {

	public Transform followed;
	private Vector3 vpos;
	public float smoothTime = 0.1F, maxVelocity = 10;
	private Vector3 velocity = Vector3.zero;

	void LateUpdate () {
		if(followed)
			vpos = followed.position;
		transform.position = Vector3.SmoothDamp(transform.position, vpos, ref velocity, smoothTime, maxVelocity);
	}
}

