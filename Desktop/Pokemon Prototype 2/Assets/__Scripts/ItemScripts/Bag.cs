using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bag : MonoBehaviour {

	public static Bag 			S;
	public int 					activeItem;
	public List<GameObject> 	menuItems;

	public List<Item> 			bag;
	public List<GameObject>		bagOfGameObjects;
	public Item					ChosenItem;

	public bool					browsingItems = true;
	public bool					usingPotion = false; //changed in ItemOptions.cs
	public bool					usingPokeball = false;
	public bool					cantUseHere = false; //changed in ItemOptions.cs
	public bool					cantTossHere = false;//changed in ItemOptions.cs

	public void updateBag() {
		GUIText gui;
		int i = 0;
		for (int j = 0; j < bag.Count; ++j) {
			if(bag[j].quantity < 1) { //get rid of items w/ no quantity
				bag.RemoveAt(j);
				--j;
				activeItem = 0;
			}
		}
		if (bag.Count > 0) {
			bagOfGameObjects.Clear ();
			foreach (Transform child in transform) {
				bagOfGameObjects.Add (child.gameObject);
				gui = child.GetComponent<GUIText> ();
				if (i < bag.Count) {
					gui.text = bag [i].itemName + " " + bag [i].quantity + "x";
					if (i == activeItem) {
						gui.color = Color.red;
					}
				} else {
					gui.text = "";
				}
				++i;
			}
		} else {
			bagOfGameObjects.Clear ();
			foreach (Transform child in transform) {
				gui = child.GetComponent<GUIText> ();
				gui.text = "";
			}
		}
	}

	void Awake() {
		S = this;
		bag.Clear ();
		for (int i = 0; i < Player.S.bag.Count; ++i) {
			bag.Add (Player.S.bag[i]);
		}
	}

	// Use this for initialization
	void Start () {
		activeItem = 0;
		updateBag ();
	}
	
	// Update is called once per frame
	void Update () {
		if (bag.Count == 0) {
			if (Input.GetKeyDown (KeyCode.S)) {
				Main.S.CloseBag ();
			}
		}
		else if (browsingItems) {
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				MoveDownItems ();
			} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
				MoveUpItems ();
			} else if (Input.GetKeyDown (KeyCode.S)) {
				Main.S.CloseBag ();
			} else if (Input.GetKeyDown (KeyCode.A)) {
				ChosenItem = bag [activeItem];
				browsingItems = false;
				print (ChosenItem.gameObject.tag);
				ItemOptions.S.gameObject.SetActive (true);
			}
		} else if (usingPotion) {
			BattleMain.S.givingPokemonItem = true;
			Main.S.OpenPokemonParty ();
			usingPotion = false;
		} else if (usingPokeball) {
			usingPokeball = false;
			Battle3.S.usedPokeball = true;
			--ChosenItem.quantity;
			Battle3.S.ball = ChosenItem;
			updateBag ();
			Main.S.CloseBag ();
		} else if (cantUseHere) {
			ItemOptions.S.gameObject.SetActive (false);
			GUIText gui = bagOfGameObjects [activeItem].GetComponent<GUIText> ();
			gui.text = "You can't use that here";
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				updateBag ();
				cantUseHere = false;
				ItemOptions.S.gameObject.SetActive (true);
			}
		} else if (cantTossHere) {
			ItemOptions.S.gameObject.SetActive (false);
			GUIText gui = bagOfGameObjects [activeItem].GetComponent<GUIText> ();
			gui.text = "You can't toss that here";
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				updateBag ();
				cantTossHere = false;
				ItemOptions.S.gameObject.SetActive (true);
			}
		}
	}

	public void MoveDownItems() {
		bagOfGameObjects [activeItem].GetComponent<GUIText> ().color = Color.black;
		activeItem = activeItem == bag.Count - 1 ? 0 : ++activeItem;
		bagOfGameObjects [activeItem].GetComponent<GUIText> ().color = Color.red;
	}
	
	public void MoveUpItems() {
		bagOfGameObjects [activeItem].GetComponent<GUIText> ().color = Color.black;
		activeItem = activeItem == 0 ? bag.Count - 1 : --activeItem;
		bagOfGameObjects [activeItem].GetComponent<GUIText> ().color = Color.red;
	}
}
