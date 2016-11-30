using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {
	Transform myTransform;

	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.LookAt(Camera.main.transform);
		myTransform.Rotate(0f,180,0f);
	
	}
}
