using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class mosquitoCombat : MonoBehaviour {
	public Texture lifeBarBack;
	public Texture lifeBarCore;

	public int healthCount = 10;
	Animator myAnim;
	bool busy;
	public bool localPlayer;
	Renderer myrender;

	// Use this for initialization
	void Awake () {		
		myAnim = GetComponent<Animator> ();
		myrender = GetComponentInChildren<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnGUI(){
		Vector2 lifeBarPosition = new Vector2();
		if (localPlayer) {
			lifeBarPosition.x = 25;
			lifeBarPosition.y = Screen.height - 60;
		} else {
			Vector3 mosquitoPos = Camera.main.WorldToScreenPoint(transform.position);
			lifeBarPosition.x = mosquitoPos.x-50; 
			lifeBarPosition.y = Screen.height - mosquitoPos.y; 
		}
		if (myrender.isVisible) {
			GUI.Label (new Rect (lifeBarPosition, new Vector2 (100, 30)), lifeBarBack);
			GUI.Label (new Rect (lifeBarPosition, new Vector2 (healthCount * 10, 30)), lifeBarCore);
		}
	}
	public void Damage(){
		if (!busy) {
			if (healthCount > 1) {
				healthCount -= 1;

				StartCoroutine (damageAnim ());
			} else {
				Death ();
			}
		}
	}
	IEnumerator damageAnim() {
		busy = true;
		myAnim.SetBool ("damage",true);
		//float timeDuration = myAnim.GetCurrentAnimatorClipInfo (0) [0].clip.length;
		yield return new WaitForSeconds (0.5f);
		myAnim.SetBool ("damage", false);
		busy = false;
	}
	public void Death(){
		Time.timeScale = 0;
		Debug.Log ("I: "+gameObject.name+" died!");
	}
}
