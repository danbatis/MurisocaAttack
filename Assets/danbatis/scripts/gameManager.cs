using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {
	bool running=true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("escape")) {
			if (running) {
				running = false;
				Time.timeScale = 0;
				Debug.Log ("game paused");
			} else {
				running = true;
				Time.timeScale = 1;
				Debug.Log ("game running");
			}
		}		
	
	}
}
