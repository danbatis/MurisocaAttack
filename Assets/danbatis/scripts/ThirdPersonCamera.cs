using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class ThirdPersonCamera : MonoBehaviour {
	public float smoothRot = 10.0f;
	public float smoothTrans = 15.0f;

	private Transform myTransform;
	public Transform target;
	private Vector3 targetVirtual;
	public float camDistance = 3.0f;
	public float camDistanceClose = 1.0f;

	public float approachCameraSpeed = 200.0f;
	public float camHeight = 3.0f;
	public float targetHeight = 3.0f;

	private float deltaCam = 0.0f;
	private float effectiveCamDistance;
	public bool useMouse;
	public float XSensitivity=0.5f;
	public float YSensitivity=0.5f;

	public float perceptibleDist=10f;
	public bool cameraOcluded = false;
	public List<string> ocludingElements;

	void Start(){
		myTransform = transform;
		effectiveCamDistance = camDistance;

		if (target == null) {
			GameObject targetGO = GameObject.FindGameObjectWithTag ("Player");
			if(targetGO != null)
				target = targetGO.transform;
		}
		if (target == null) {
			Debug.Log ("Error! The target was not assigned, ThirdPersonCam script is deactivated!");
			this.enabled = false;
		}
	}

	void Update ()
	{
		/*
		if (Input.GetKey ("left shift")) {
			targetVirtual = target.position + camDistance * target.forward + camHeight*target.up;
			myTransform.forward = Vector3.Lerp (myTransform.forward, -1 * target.forward, Time.deltaTime * smoothRot);	
			Debug.Log ("pressing left shift");
		} else {
		*/
		Vector3 newForth = target.forward;
		if (useMouse) {
			float xRot = CrossPlatformInputManager.GetAxis ("Mouse X") * XSensitivity;
			float yRot = CrossPlatformInputManager.GetAxis ("Mouse Y") * YSensitivity;
			//Debug.Log ("rotx: "+xRot.ToString()+" roty: "+yRot.ToString());
			Vector3 rotx = myTransform.forward * Mathf.Cos (xRot) + myTransform.right * Mathf.Sin (xRot);
			Vector3 roty = myTransform.forward * Mathf.Cos (yRot) + myTransform.up * Mathf.Sin (yRot);
			newForth = rotx + roty;
			newForth.Normalize ();
		}

		myTransform.forward = Vector3.Lerp (myTransform.forward, newForth, Time.deltaTime * smoothRot);
		targetVirtual = target.position - effectiveCamDistance * newForth + camHeight*target.up;

		RaycastHit hit;
		Vector3 visionTarget = target.position + Vector3.up * targetHeight;
		Debug.DrawRay(myTransform.position, visionTarget - myTransform.position, new Color (0.0f, 0.5f, 0.5f));
		if (Physics.Raycast(myTransform.position, visionTarget - myTransform.position, out hit)) {
			//Debug.Log ("camera ray hiting: "+hit.transform.gameObject.name);
			if (hit.transform.gameObject.tag == "Player") {
				//check if futurePosition will be ocluded before going for it:
				float futureEffectiveCamDistance = effectiveCamDistance + approachCameraSpeed * Time.deltaTime;
				if (futureEffectiveCamDistance >= camDistance)
					futureEffectiveCamDistance = camDistance;
				Vector3 futurePos;
				futurePos = target.position - futureEffectiveCamDistance * newForth + camHeight * target.up;

				if (Physics.Raycast (futurePos, visionTarget - futurePos, out hit)) {
					if(hit.transform.gameObject.tag=="Player")
						effectiveCamDistance = futureEffectiveCamDistance;
				}
			} 
			else {
				//Time.timeScale = 0;
				effectiveCamDistance -= approachCameraSpeed*Time.deltaTime;
				if (effectiveCamDistance <= camDistanceClose)
					effectiveCamDistance = camDistanceClose;
			}
		}
		else {
			Debug.Log ("camera ray not hitting anything!");
			//Time.timeScale = 0;
			effectiveCamDistance -= approachCameraSpeed;
			if (effectiveCamDistance <= camDistanceClose)
				effectiveCamDistance = camDistanceClose;
		}

		//targetVirtual = target.position - effectiveCamDistance * newForth + camHeight*target.up;

		myTransform.position = Vector3.Lerp (myTransform.position, targetVirtual, Time.deltaTime * smoothTrans);


	}

	/*
	void OnTriggerEnter(Collider other){
		Debug.Log (gameObject.name+"trigger with: "+other.gameObject.name);
		if (!ocludingElements.Contains (other.gameObject.name)) {
			ocludingElements.Add (other.gameObject.name);
		}
		cameraOcluded = true;
	}

	void OnTriggerExit(Collider other){
		Debug.Log (gameObject.name+"trigger out: "+other.gameObject.name);
		if (ocludingElements.Contains (other.gameObject.name)) {
			ocludingElements.Remove (other.gameObject.name);
		} else {
			Debug.Log ("Strange situation, received trigger out of an element without triggger enter: "+other.gameObject.name);
		}
		if(ocludingElements.Count==0)
			cameraOcluded = false;
	}


	void OnCollisionEnter(Collision collision){
		Debug.Log (gameObject.name+"collided with: "+collision.gameObject.name);
	}
	*/

}
