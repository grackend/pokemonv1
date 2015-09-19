using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum BattleOps {
	Fight,
	Pokemon,
	Item,
	Run
}

public class BattleOptions : MonoBehaviour {

	public static BattleOptions S;

	public int activeOption;
	public List<GameObject> BattleOption;

	void Awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
		bool first = true;
		activeOption = 0;

		foreach (Transform child in transform) {
			BattleOption.Add (child.gameObject);
		}

		BattleOption = BattleOption.OrderByDescending (m => m.transform.position.y).ToList ();

		foreach (GameObject go in BattleOption) {
			GUIText itemText = go.GetComponent<GUIText>();
			if(first) itemText.color = Color.magenta;
			first = false;
		}

		gameObject.SetActive (true);
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z) && !Main.S.selectingAttack) {
			switch (activeOption) {
			case(int)BattleOps.Fight:
				print ("FIGHT selected");
				FIGHTscreen.S.gameObject.SetActive (true);
				AttackInfoScreen.S.gameObject.SetActive (true);
				Main.S.selectingAttack = true;
				break;
			case(int)BattleOps.Pokemon:
				print ("PKMN selected");
				break;
			case(int)BattleOps.Item:
				print ("ITEM selected");
				break;
			case(int)BattleOps.Run:
				print ("RUN selected");
				Battle b = transform.parent.GetComponent<Battle>();
				b.ChoseToRun = true;
				break;
			}
		} else if ((Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.LeftArrow)) &&
					!Main.S.selectingAttack)
			MoveHorizontalBattleOption ();
		else if ((Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.UpArrow)) &&
				!Main.S.selectingAttack)
			MoveVerticalBattleOption ();

	}

	void MoveHorizontalBattleOption() {
		BattleOption[activeOption].GetComponent<GUIText> ().color = Color.black;
		activeOption = activeOption % 2 == 0 ? ++activeOption : --activeOption;
		BattleOption [activeOption].GetComponent<GUIText> ().color = Color.magenta;
	}

	void MoveVerticalBattleOption() {
		int bottomRow = 2;
		BattleOption[activeOption].GetComponent<GUIText> ().color = Color.black;
		activeOption = activeOption < bottomRow ? activeOption += 2 : activeOption -= 2;
		BattleOption [activeOption].GetComponent<GUIText> ().color = Color.magenta;
	}
}
