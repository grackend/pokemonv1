using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	public static PlayerStatus 	S;

	void Awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.S)) {
			gameObject.SetActive(false);
			Main.S.viewingItems = false;
		}
	}
}
