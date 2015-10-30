using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum menuItem{
	pokedex,
	pokemon,
	item,
	player,
	save,
	option,
	exit
}

public class Menu : MonoBehaviour {

	public static Menu S;

	public int activeItem;
	public List<GameObject> menuItems;

	void Awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
		bool first = true;
		activeItem = 0;

		foreach (Transform child in transform) {
			menuItems.Add (child.gameObject);
		}

		menuItems = menuItems.OrderByDescending (m => m.transform.position.y).ToList ();
		foreach (GameObject go in menuItems) {
			GUIText itemText = go.GetComponent<GUIText>();
			if(first) itemText.color = Color.red;
			first = false;
		}

		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if(!Main.S.inBattle && !Main.S.viewingPokemon && Main.S.paused && !Main.S.viewingItems) {
			if (Input.GetKeyDown (KeyCode.S)) {
				gameObject.SetActive (false);
				Main.S.paused = false;
			} 
			if (Input.GetKeyDown (KeyCode.A)) {
				switch (activeItem) {
				case(int)menuItem.pokedex:
					print ("Pokedex menu selected");
					break;
				case(int)menuItem.pokemon:
					Main.S.OpenPokemonParty();
					break;
				case(int)menuItem.item:
					Main.S.OpenBag();
					break;
				case(int)menuItem.player:
					//PlayerStatus.S.gameObject.SetActive(true);
					PlayerStatus.S.gameObject.SetActive(true);
					Main.S.viewingItems = true;

					//Main.S.inBattle = true;
					break;
				case(int)menuItem.save:
					print ("Save menu selected");
					break;
				case(int)menuItem.option:
					print ("Option menu selected");
					break;
				case(int)menuItem.exit:
					gameObject.SetActive(false);
					Main.S.paused = false;
					break;
				}
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) 
				MoveDownMenu ();
			else if (Input.GetKeyDown (KeyCode.UpArrow)) 
				MoveUpMenu();
		}
	}

	public void MoveDownMenu() {
		menuItems [activeItem].GetComponent<GUIText> ().color = Color.black;
		activeItem = activeItem == menuItems.Count - 1 ? 0 : ++activeItem;
		menuItems [activeItem].GetComponent<GUIText> ().color = Color.red;
	}

	public void MoveUpMenu() {
		menuItems [activeItem].GetComponent<GUIText> ().color = Color.black;
		activeItem = activeItem == 0 ? menuItems.Count - 1 : --activeItem;
		menuItems [activeItem].GetComponent<GUIText> ().color = Color.red;
	}
}
