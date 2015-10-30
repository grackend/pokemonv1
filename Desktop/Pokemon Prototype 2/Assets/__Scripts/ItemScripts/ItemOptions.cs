using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemOptions : MonoBehaviour {

	public static ItemOptions 	S;

	public int 				activeItem;
	public List<GameObject> ItemOps;

	void Awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
		bool first = true;
		activeItem = 0;

		foreach (Transform child in transform) {
			ItemOps.Add (child.gameObject);
		}

		foreach (GameObject go in ItemOps) {
			GUIText itemText = go.GetComponent<GUIText>();
			if(first) itemText.color = Color.red;
			first = false;
		}

		gameObject.SetActive (false);
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.S)) {
			Bag.S.browsingItems = true;
			while(activeItem != 0) MoveUpMenu();
			gameObject.SetActive (false);
		} 
		else if (Input.GetKeyDown (KeyCode.A)) {
			if(activeItem == 0) { //USE
				print ("You chose to use this item");

				if(Bag.S.ChosenItem.gameObject.tag == "Potion") { //Use Potion
					Bag.S.usingPotion = Main.S.usingPotion = true;
				}
				else if(Bag.S.ChosenItem.gameObject.tag == "Pokeball") {
					if(!Main.S.inBattle) {
						Bag.S.cantUseHere = true;
					}
					else if(Main.S.inBattle && !Player.S.spottedByTrainer)
						Bag.S.usingPokeball = true;
					else {
						Bag.S.cantUseHere = true;
					}
				}
				gameObject.SetActive(false);
			}
			else if(activeItem == 1) { //TOSS
				if(!Main.S.inBattle) {
					--Bag.S.ChosenItem.quantity;
					Bag.S.updateBag();
					if(Bag.S.ChosenItem.quantity == 0)
						gameObject.SetActive(false);
				}
				else {
					Bag.S.cantTossHere = true;
				}
			}
			else { //CANCEL
				Bag.S.browsingItems = true;;
				gameObject.SetActive (false);
				while(activeItem != 0) MoveUpMenu();
			}
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) 
			MoveDownMenu ();
		else if (Input.GetKeyDown (KeyCode.UpArrow)) 
			MoveUpMenu();
	}

	public void MoveDownMenu() {
		ItemOps [activeItem].GetComponent<GUIText> ().color = Color.black;
		activeItem = activeItem == ItemOps.Count - 1 ? 0 : ++activeItem;
		ItemOps [activeItem].GetComponent<GUIText> ().color = Color.red;
	}
	
	public void MoveUpMenu() {
		ItemOps [activeItem].GetComponent<GUIText> ().color = Color.black;
		activeItem = activeItem == 0 ? ItemOps.Count - 1 : --activeItem;
		ItemOps [activeItem].GetComponent<GUIText> ().color = Color.red;
	}
}
