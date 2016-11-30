using UnityEngine;
using System.Collections;

public class MosquitoFlightLegacy : MonoBehaviour {
	CharacterController myControl;
	Transform myTransform;

	public Transform camTransform;

	public float leanForward=45f;
	float verticalSpeed;
	public float acel=10f;
	float verticalAcel;

	public bool limitVerticalSpeed;
	public float maximumSpeed = 10f;
	Vector3 vertMove;

	Vector3 moveDirection;

	Vector3 planarMove;
	float forwardSpeed;
	public float maxForwardSpeed=10f;
	float horizAx;
	public float rotateSpeed=10.0f;
	Animation myAnim;

	bool busy;


	// Use this for initialization
	void Awake(){
		myAnim = GetComponentInChildren<Animation> ();
		myAnim["Slash1"].speed = 2.0f;
	}
	void Start () {
		myControl = GetComponent<CharacterController> ();
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (!busy) {
			if (Input.GetMouseButtonDown (0)) {
				myAnim.Play("Slash1");
			}		
		}
		
		//vertical axe
		if (Input.GetKey ("space")) {
			verticalAcel = acel;
			if (verticalSpeed == 0) {
				busy = true;
				myAnim.Play ("takeOff");
				Debug.Log ("playing takeoff");
			}

			if (!myAnim.isPlaying) {
				busy = false;
				myAnim.CrossFade ("flying", 0.3f);
			}
			
		} 
		else {
			verticalAcel = -acel;
		}


		verticalSpeed += verticalAcel * Time.deltaTime;
		if (limitVerticalSpeed) {
			if (verticalSpeed >= maximumSpeed) {
				verticalSpeed = maximumSpeed;
			} else {
				if (verticalSpeed <= -maximumSpeed) {
					verticalSpeed = -maximumSpeed;
				}
			}
		}
		if (myControl.isGrounded) {
			verticalSpeed = 0;
			forwardSpeed = 0;
			if (!Input.GetKey ("space")) {
				myAnim.CrossFade ("idle", 0.3f);
				Debug.Log ("crossfading idle");
			}
		}
		else{
			forwardSpeed += acel * Time.deltaTime;
			if (forwardSpeed >= maxForwardSpeed)
				forwardSpeed = maxForwardSpeed;

			horizAx = Input.GetAxis("Horizontal");
		}

		if (camTransform != null) {				
			Vector3 auxForward = Vector3.ProjectOnPlane(camTransform.forward,Vector3.up);
			Vector3 auxRight = Vector3.ProjectOnPlane(camTransform.right,Vector3.up);
			planarMove = (auxForward * forwardSpeed + horizAx * forwardSpeed * auxRight)*Time.deltaTime;

			if (planarMove.magnitude > 0) {
				//Debug.Log ("planar magnitude > zero");
				myTransform.forward = Vector3.Lerp (myTransform.forward, planarMove, Time.deltaTime * rotateSpeed);
			} else {
				//Debug.Log ("planar = zero");
			}
		} 
		else {
			planarMove = myTransform.forward * forwardSpeed * Time.deltaTime;
			myTransform.Rotate(0f,horizAx*rotateSpeed*Time.deltaTime,0f);
		}

		//Debug.Log ("current orientation: "+myTransform.localRotation.x.ToString());
		vertMove = Vector3.up * verticalSpeed * Time.deltaTime;

		moveDirection = vertMove + planarMove;
		myControl.Move (moveDirection);	
	}
}
