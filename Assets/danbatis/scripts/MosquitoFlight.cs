using UnityEngine;
using System.Collections;

public class MosquitoFlight : MonoBehaviour {
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
	float maxForthSpeed;
	public float maxForwardSpeed=10f;
	float horizAx;
	public float rotateSpeed=10.0f;
	Animator myAnim;

	public bool lethal;
	float stamina=100.0f;
	bool recovering=false;

	public Texture staminaBarBack;
	public Texture staminaBarCore;


	// Use this for initialization
	void Awake(){
		myAnim = GetComponent<Animator> ();
		myControl = GetComponent<CharacterController> ();
		myTransform = transform;
		maxForthSpeed = maxForwardSpeed;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			//Debug.Log ("first slash move");
			StartCoroutine (Slash());
		}	
		if (Input.GetMouseButton(1) && !recovering) {
			Debug.Log ("holding right mouse button");
			stamina -= 2*acel * Time.deltaTime;
			maxForthSpeed = 2*maxForwardSpeed;
			if (stamina <= 0) {
				stamina = 0f;
				maxForthSpeed = maxForwardSpeed / 2;
				recovering = true;
			}
		}	
		if (!Input.GetMouseButton(1)) {
			stamina += acel * Time.deltaTime;
			if(!recovering)
				maxForthSpeed = maxForwardSpeed;

			if (stamina >= 100.0f) {
				stamina = 100.0f;
				maxForthSpeed = maxForwardSpeed;
				recovering = false;
			}		
		}

		//vertical axe
		if (Input.GetKey ("space")) {
			verticalAcel = acel;
			if (verticalSpeed == 0) {				
				StartCoroutine (TakeOff ());
				Debug.Log ("playing takeoff");
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
				//Debug.Log ("crossfading idle");
				myAnim.SetBool ("inAir", false);
				myAnim.SetBool ("grounded", true);
			}
		}
		else{
			forwardSpeed += acel * Time.deltaTime;
			if (forwardSpeed >= maxForthSpeed)
				forwardSpeed = maxForthSpeed;

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
	void OnGUI(){		
		GUI.Label (new Rect (Screen.width-110, Screen.height - 40, 100, 30), staminaBarBack);
		GUI.Label (new Rect (Screen.width-110, Screen.height - 40, stamina, 30), staminaBarCore);
	}

	IEnumerator TakeOff() {
		myAnim.SetBool ("grounded", false);
		yield return new WaitForSeconds (0.9f);
		myAnim.SetBool ("inAir", true);
	}
	IEnumerator Slash() {
		if (myAnim.GetBool ("inAir")) {
			myAnim.SetBool ("Slash1", true);
			lethal = true;
			float timeDuration = myAnim.GetCurrentAnimatorClipInfo (0) [0].clip.length;
			yield return new WaitForSeconds (0.9f*timeDuration);
			myAnim.SetBool ("Slash1", false);
			lethal = false;
		}
	}
	void OnTriggerEnter(Collider other){
		if (lethal) {			
			//check if the it is another guy or ourselves
			if (gameObject != other.gameObject) {
				mosquitoLocalSetup otherMosquito = other.GetComponent<mosquitoLocalSetup> ();

				if (otherMosquito != null) {
					Debug.Log (gameObject.name + " entered trigger: " + other.name);
					otherMosquito.CmdDamage();
				}
			}
		}
	}
}
