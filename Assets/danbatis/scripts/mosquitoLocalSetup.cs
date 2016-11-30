using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class mosquitoLocalSetup : NetworkBehaviour {
	//syncvar works only one way, if the variable changes in the server, the change is propagated to the clients
	[SyncVar]
	public string pname="player";


	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			GetComponent<MosquitoFlight> ().enabled = true;
			Camera.main.GetComponent<ThirdPersonCamera> ().enabled = true;
			Debug.Log ("Third Person Cam reactivated!");
			Camera.main.GetComponent<ThirdPersonCamera> ().target = transform;
			GetComponent<MosquitoFlight> ().camTransform = Camera.main.transform;
		}	
	}


	void OnGUI(){
		if (isLocalPlayer) {
			pname = GUI.TextField (new Rect (25, Screen.height - 40, 100, 30), pname);
			if (GUI.Button (new Rect (130, Screen.height - 40, 80, 30), "Change")) {
				CmdChangeName (pname);
			}
		}
	}

	//Command works only one way, if the variable changes in the client, the change is propagated to the host
	[Command]
	public void CmdChangeName(string newName){
		pname = newName;
	}

	public void ListenGenericCmd(string cmdInstruction){
		if (!isLocalPlayer) {
			StartCoroutine (GenericCmd ());
		}
	}

	void Update() {
		this.GetComponentInChildren<TextMesh> ().text = pname;
	}

	IEnumerator GenericCmd(){
		string oldpname = pname;
		pname="zetachange";
		yield return new WaitForSeconds(2.0f);
		pname = oldpname;
	}
}
