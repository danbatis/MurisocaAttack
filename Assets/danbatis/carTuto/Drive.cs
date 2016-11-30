using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Drive : MonoBehaviour {

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
	void Update() {
		
		float translation = CrossPlatformInputManager.GetAxis("Vertical") * speed;
		float rotation = CrossPlatformInputManager.GetAxis("Horizontal") * rotationSpeed;
		translation *= Time.deltaTime;
		rotation *= Time.deltaTime;
		transform.Translate(0, 0, translation);
		transform.Rotate(0, rotation, 0);
	}
}
