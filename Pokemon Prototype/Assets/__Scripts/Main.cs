using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	static public Main S;
	
	public bool inDialog = false;
	public bool paused = false;
	public bool inBattle = false;
	public bool selectingAttack = false;

	void Awake() {
		S = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (!inDialog && Input.GetKeyDown (KeyCode.Space)) {
			Menu.S.gameObject.SetActive(true);

			paused = true;
		}
	}
}