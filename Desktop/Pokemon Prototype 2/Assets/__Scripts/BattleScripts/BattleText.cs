using UnityEngine;
using System.Collections;

public class BattleText : MonoBehaviour {

	public static 	BattleText S;
	string	message;

	void Awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (AttackBox.S.noPPbool) { //turn off all boxes to display message;
			OptionBox.S.gameObject.SetActive(false);
			AttackBox.S.gameObject.SetActive(false);
			AttackInfoBox.S.gameObject.SetActive(false);
			printToBattleText(AttackBox.S.noPP);
			if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)) {
				OptionBox.S.gameObject.SetActive(true);
				AttackBox.S.gameObject.SetActive(true);
				AttackInfoBox.S.gameObject.SetActive(true);
				AttackBox.S.noPPbool = false;
			}
			                              
		}
		//printToBattleText (message);
		/*
		if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
			OptionBox.S.gameObject.SetActive(true);
			printToBattleText("");
		}*/
	}

	public void printToBattleText(string s) {
		OptionBox.S.gameObject.SetActive (false);
		GUIText gui = BattleText.S.GetComponent<GUIText> ();
		gui.text = s;
	}
}
