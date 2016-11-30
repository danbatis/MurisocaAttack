using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SetupLocalClient : NetworkBehaviour {
	//syncvar works only one way, if the variable changes in the server, the change is propagated to the clients
	[SyncVar]
	public string pname="player";


	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			GetComponent<Drive> ().enabled = true;
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

	void Update() {
		this.GetComponentInChildren<TextMesh> ().text = pname;
	}
}
