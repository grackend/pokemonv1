using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackBox : MonoBehaviour {
	
	public static AttackBox S;
	
	public int 				currentAttack;
	public List<GameObject> AtkList; //controls GUIText for name of attacks
	public Attack[]			AtkArray; //references actual attacks
	
	public Attack			ChosenAttack;
	public Pokemon			SwitchTo;

	public bool				updateForNewPokemon = false;
	public bool				lockOtherMovement = false;
	public bool				AttackChosen = false; //can be changed in Battle3

	public string			noPP = "There is not enough PP left";
	public bool				noPPbool = false;
	

	void Awake() {
		S = this;
		AtkArray = new Attack[4];
	}



	public void UpdatePokemonAttackInfo() {

		//GameObject GO = GameObject.Find ("Battle2");
		//Battle2 b = GO.GetComponent<Battle2> ();
		//Pokemon pkmn = b.PlayerPokemon;
		Pokemon pkmn = BattleMain.S.currentPokemon;

		//Pokemon pkmn = Battle2.S.PlayerPokemon;
		int i = 0;
		foreach (Attack att in pkmn.Attacks) {
			if (i < pkmn.Attacks.Length) {
				AtkArray [i] = att;
				++i;
			}
		}
		
		foreach (Transform child in transform) {
			AtkList.Add (child.gameObject);
		}
		
		i = 0;
		foreach (GameObject go in AtkList) {
			if (i < pkmn.Attacks.Length) {
				GUIText AttackText = go.GetComponent<GUIText> ();
				AttackText.text = AtkArray [i].AttackName;
				++i;
			}
		}
		
		bool first = true;
		currentAttack = 0;
		
		foreach (GameObject go in AtkList) {
			GUIText AttackText = go.GetComponent<GUIText> ();
			if (first)
				AttackText.color = Color.red;
			first = false;
		}
	}


	void Start () {
		UpdatePokemonAttackInfo ();
		gameObject.SetActive (false);
	}

	void Update () {

		lockOtherMovement = true; //disables movement in OptionBox

		if (updateForNewPokemon) {
			updateForNewPokemon = false;
			UpdatePokemonAttackInfo(); //change the attacks in AttackBox to match the current pokemon
		}

		if (Input.GetKeyDown (KeyCode.A)) {

			//Battle2 script = GameObject.Find("Battle2").GetComponent<Battle2>();
			//script.ChoseToAttack = true;
			//script.ChosenAttack = AtkArray [currentAttack];

			ChosenAttack = AtkArray[currentAttack]; //make ChosenAttack the selected attack

			if(ChosenAttack.AttackPPRemaining == 0) {
				noPPbool = true;
			}
			else {
				lockOtherMovement = false; //allow movement in OptionBox
				AttackChosen = true; //sends message to Battle3
				ResetAttack(); //resets first attack to red
				if(!Main.S.invincible)
					--ChosenAttack.AttackPPRemaining;

				//turn off all box displays
				OptionBox.S.gameObject.SetActive(false);
				AttackInfoBox.S.gameObject.SetActive(false);
				gameObject.SetActive(false);
			}
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			MoveDownAttack ();
		} 
		else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			MoveUpAttack();
		}
		//turn off attack
		if (Input.GetKeyDown (KeyCode.S)) {
			lockOtherMovement = false; //allow movement in OptionBox
			AttackChosen = false; //sends message to Battle3
			ResetAttack(); //resets first attack to red
			AttackInfoBox.S.gameObject.SetActive(false);
			gameObject.SetActive (false);
		}
	}
	
	void MoveDownAttack() {

		AtkList [currentAttack].GetComponent<GUIText> ().color = Color.black;
		if (currentAttack + 1 == 4 || AtkArray [currentAttack + 1].AttackName == "-")
			currentAttack = 0;
		else
			currentAttack++;
		AtkList [currentAttack].GetComponent<GUIText> ().color = Color.red;

	}
	
	void MoveUpAttack() {
		AtkList [currentAttack].GetComponent<GUIText> ().color = Color.black;
		if (currentAttack > 0)
			--currentAttack;
		else {
			currentAttack = 3;
			while(AtkArray[currentAttack].AttackName == "-") {
				--currentAttack;
			}
		}
		AtkList [currentAttack].GetComponent<GUIText> ().color = Color.red;
	}

	void ResetAttack() { //reset first attack to selected attack in red
		AtkList [currentAttack].GetComponent<GUIText> ().color = Color.black;
		currentAttack = 0;
		AtkList [currentAttack].GetComponent<GUIText> ().color = Color.red;
	}
}