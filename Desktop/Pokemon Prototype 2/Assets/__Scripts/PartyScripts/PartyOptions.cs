using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyOptions : MonoBehaviour {

	public static PartyOptions 	S;
	
	public int 				activeItem;
	public List<GameObject> PartyOps;

	public bool				choseToSwitch;
	public bool				choseToMove;
	public bool				viewStats;
	
	void Awake() {
		S = this;
	}
	
	// Use this for initialization
	void Start () {
		bool first = true;
		activeItem = 0;
		
		foreach (Transform child in transform) {
			PartyOps.Add (child.gameObject);
		}
		
		foreach (GameObject go in PartyOps) {
			GUIText itemText = go.GetComponent<GUIText>();
			if(first) itemText.color = Color.red;
			first = false;
		}
		
		gameObject.SetActive (false);
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.DownArrow)) 
			MoveDownMenu ();
		else if (Input.GetKeyDown (KeyCode.UpArrow)) 
			MoveUpMenu ();
		else if (Input.GetKeyDown (KeyCode.S)) {
			Party.S.selectingPokemon = true;
			gameObject.SetActive(false);
			while (activeItem != 0)
				MoveUpMenu ();
		} else if (Input.GetKeyDown (KeyCode.A)) {
			if(activeItem == 0) {
				if(Main.S.inBattle) {
					choseToSwitch = true;
					BattleMain.S.intentToSwitchPokemon = true;
				}
				else 
					choseToMove = true;
				//gameObject.SetActive(false);
			} else if(activeItem == 2) {
				Party.S.selectingPokemon = true;
				gameObject.SetActive(false);
				while (activeItem != 0)
					MoveUpMenu ();
			} else if(activeItem == 1) {
				PokemonStats.S.gameObject.SetActive(true);
				PokemonStats.S.updt = true;
			}
		}
	}

	public void MoveDownMenu() {
		PartyOps [activeItem].GetComponent<GUIText> ().color = Color.black;
		activeItem = activeItem == PartyOps.Count - 1 ? 0 : ++activeItem;
		PartyOps [activeItem].GetComponent<GUIText> ().color = Color.red;
	}
	
	public void MoveUpMenu() {
		PartyOps [activeItem].GetComponent<GUIText> ().color = Color.black;
		activeItem = activeItem == 0 ? PartyOps.Count - 1 : --activeItem;
		PartyOps [activeItem].GetComponent<GUIText> ().color = Color.red;
	}
}
