using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum BattleOps2 {
	Fight,
	Pokemon,
	Item,
	Run
}

public class OptionBox : MonoBehaviour {
	
	public static OptionBox S;
	
	public int 				activeOption;
	public List<GameObject> BattleOptions;

	public bool				chooseFight;
	public bool				chooseParty;
	public bool 			chooseItem;
	public bool				chooseRun;

	void Awake() {
		S = this;
	}
	
	// Use this for initialization
	void Start () {
		bool first = true;
		activeOption = 0;

		foreach (Transform child in transform) {
			BattleOptions.Add (child.gameObject);
		}
		
		BattleOptions = BattleOptions.OrderByDescending (m => m.transform.position.y).ToList ();
		
		foreach (GameObject go in BattleOptions) {
			GUIText itemText = go.GetComponent<GUIText>();
			if(first) itemText.color = Color.red;
			first = false;
		}
		
		gameObject.SetActive (false);
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!AttackBox.S.lockOtherMovement && !Main.S.viewingPokemon && !Main.S.viewingItems) {
			if (Input.GetKeyDown (KeyCode.A)) {
				switch (activeOption) {
				case(int)BattleOps2.Fight:
					AttackBox.S.lockOtherMovement = true; //so cursor does not move when choosing attack
					AttackBox.S.gameObject.SetActive (true);
					AttackInfoBox.S.gameObject.SetActive (true);
					break;
				case(int)BattleOps2.Pokemon:
					BattleMain.S.intentToSwitchPokemon = true;
					Main.S.OpenPokemonParty ();
				//Battle2.S.ChoseToSwitch = true;
					break;
				case(int)BattleOps2.Item:
					Main.S.OpenBag();
					break;
				case(int)BattleOps2.Run:
					chooseRun = true;
					break;
				}
			} else if ((Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.LeftArrow)))
				MoveHorizontalBattleOption ();
			else if ((Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.UpArrow)))
				MoveVerticalBattleOption ();
		}
		
	}
	
	void MoveHorizontalBattleOption() {
		BattleOptions[activeOption].GetComponent<GUIText> ().color = Color.black;
		activeOption = activeOption % 2 == 0 ? ++activeOption : --activeOption;
		BattleOptions [activeOption].GetComponent<GUIText> ().color = Color.red;
	}
	
	void MoveVerticalBattleOption() {
		int bottomRow = 2;
		BattleOptions[activeOption].GetComponent<GUIText> ().color = Color.black;
		activeOption = activeOption < bottomRow ? activeOption += 2 : activeOption -= 2;
		BattleOptions [activeOption].GetComponent<GUIText> ().color = Color.red;
	}
}