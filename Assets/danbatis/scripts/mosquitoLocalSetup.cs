using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class mosquitoLocalSetup : NetworkBehaviour {
	//syncvar works only one way, if the variable changes in the server, the change is propagated to the clients
	[SyncVar]
	public string pname="player";
	mosquitoCombat mosquitoBattle;
	TextMesh playerName;


	// Use this for initialization
	void Start () {
		playerName = GetComponentInChildren<TextMesh> ();
		mosquitoBattle = GetComponent<mosquitoCombat> ();

		if (isLocalPlayer) {
			GetComponent<MosquitoFlight> ().enabled = true;
			Camera.main.GetComponent<ThirdPersonCamera> ().enabled = true;
			Debug.Log ("Third Person Cam reactivated!");
			Camera.main.GetComponent<ThirdPersonCamera> ().target = transform;
			GetComponent<MosquitoFlight> ().camTransform = Camera.main.transform;
			mosquitoBattle.localPlayer = true;
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
	[Command]
	public void CmdDamage(){
		mosquitoBattle.Damage ();
	}

	void Update() {
		playerName.text = pname;
	}

	IEnumerator GenericCmd(string cmdInstruction){
		string oldpname = pname;
		pname=cmdInstruction;
		yield return new WaitForSeconds(2.0f);
		pname = oldpname;
	}
}
