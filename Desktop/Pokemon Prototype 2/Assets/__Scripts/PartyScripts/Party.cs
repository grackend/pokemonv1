using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Party : MonoBehaviour {

	public static Party		S;

	public static Pokemon	ChosenPokemon;
	public bool				switchingPokemon;
	public bool				usingPotion = false;
	public bool				selectingPokemon = true;
	public bool				movingPokemon = false;
	int						beginMovingPokemon;
	int						endMovingPokemon;

	public Pokemon[] 		party;
	public int 				activeItem;

	private bool 			alreadyActiveMessage = false;
	private bool			noWillToFightMessage = false;
	private bool			pokemonHasFullHPMessage = false;
	private bool			pokemonCantBeHealedMessage = false;
	GUIText 				gui;

	public bool				choseToSwitch;

	public List<GameObject> pkmn;

	void Awake() {
		S = this;
		//party = new Pokemon[6];
		int i = 0;
		print (Player.S.party [0].Name);
		foreach (Pokemon poke in Player.S.party) {
			party[i] = poke;
			++i;
		}
	}

	public void buildPartyGraphic(int startingPokemon) {
		int i = 0;
		pkmn.Clear ();
		foreach (Transform child in transform) {
			pkmn.Add (child.gameObject);
			if(party[i].Name != "-") {
				gui = child.GetComponent<GUIText>();
				gui.text = party[i].Name + " Lv " + party[i].Level + " HP " + party[i].HealthCurrent + " / " + party[i].HealthFull;
			}
			else {
				gui = child.GetComponent<GUIText>();
				gui.text = "-";
			}
			if(i == startingPokemon) gui.color = Color.red;
			else gui.color = Color.black;
			++i;
		}
	}

	public void PrintOverPokemon(int currentActivePokemon, string s) {
		int i = 0;
		pkmn.Clear ();
		foreach (Transform child in transform) {
			pkmn.Add (child.gameObject);
			gui = child.GetComponent<GUIText>();
			if(i == currentActivePokemon) {
				gui.text = s;
			}
			else if(party[i].Name != "-") {
				gui.text = party[i].Name + " Lv " + party[i].Level + " HP " + party[i].HealthCurrent + " / " + party[i].HealthFull;
			}
			else {
				gui.text = "-";
			}
			gui.color = Color.black;
			++i;
		}
	}

	void Start () {
		activeItem = 0;
		switchingPokemon = false;
		alreadyActiveMessage = false;
		buildPartyGraphic (0);
		
		//gameObject.SetActive (false);
	}
		
	
	// Update is called once per frame
	void Update () {
		if (selectingPokemon) {
			if (Input.GetKeyDown (KeyCode.DownArrow))
				MoveDownOne ();
			else if (Input.GetKeyDown (KeyCode.UpArrow))
				MoveUpOne ();
			else if (Input.GetKeyDown (KeyCode.S)) {
				if (Main.S.inBattle && !Battle3.S.playerFaintedChooseNext) {
					if (Main.S.usingPotion) {
						Bag.S.browsingItems = true;
						Main.S.usingPotion = Bag.S.usingPotion = false;
					}
					Main.S.ClosePokemonParty ();
				} else if (!Main.S.inBattle) {
					if (Main.S.usingPotion) {
						Bag.S.browsingItems = true;
						Main.S.usingPotion = Bag.S.usingPotion = false;
					}
					//else if (Main.S.viewingItems)
						//Bag.S.browsingItems = true; //problem here
					Main.S.ClosePokemonParty ();
				}
			} else if (Input.GetKeyDown (KeyCode.A)) {
				if(!Main.S.usingPotion) {
					PartyOptions.S.gameObject.SetActive (true);
					selectingPokemon = false;
					movingPokemon = false;
				}
				 else if (Main.S.usingPotion) {
					if (party [activeItem].HealthCurrent == party [activeItem].HealthFull) {
						pokemonHasFullHPMessage = true;
						selectingPokemon = false;
					} else if (party [activeItem].HealthCurrent == 0) {
						pokemonCantBeHealedMessage = true;
						selectingPokemon = false;
					} else {
						int temp = party [activeItem].HealthCurrent += Bag.S.ChosenItem.heal;
						if (temp > party [activeItem].HealthFull)
							temp = party [activeItem].HealthFull;
						party [activeItem].HealthCurrent = temp;
						Bag.S.ChosenItem.quantity--;
						Bag.S.updateBag ();
						buildPartyGraphic (activeItem);
						Main.S.usingPotion = Bag.S.usingPotion = false;
						if(Main.S.inBattle) {
							PlayerPokemonInfoDisplay.S.updateUI ();
							Battle3.S.usedPotion = true;
							Main.S.CloseBag();
							Main.S.viewingItems = false;
						}
						else 
							Bag.S.browsingItems = true;
						Main.S.ClosePokemonParty ();
						//Main.S.CloseBag ();
					}
				}
			}
		}
		else if(PartyOptions.S.choseToSwitch) {
			choseToSwitch = true;
			PartyOptions.S.choseToSwitch = false;
			PartyOptions.S.gameObject.SetActive(false);
		}
		else if (choseToSwitch) {
			choseToSwitch = false;
			if (BattleMain.S.intentToSwitchPokemon) {
				if (party [activeItem].HealthCurrent <= 0) {
					selectingPokemon = false;
					noWillToFightMessage = true;
				} else if (party [activeItem] == BattleMain.S.currentPokemon) {
					selectingPokemon = false;
					alreadyActiveMessage = true;
				} else {
					ChosenPokemon = party [activeItem];
					print (ChosenPokemon.Name);
					BattleMain.S.currentPokemon.resetStats();
					BattleMain.S.lastPokemon = BattleMain.S.currentPokemon; //For battle text purposes
					BattleMain.S.currentPokemon = ChosenPokemon; //For references
					BattleMain.S.switchingPokemon = true; //For references
					OptionBox.S.gameObject.SetActive (false); //Turn off Option Box to show text
					Battle3.S.playerFaintedChooseNext = false; //you've selected the next pokemon
					Main.S.ClosePokemonParty ();
				}
			} 
		} else if (PartyOptions.S.choseToMove) {
			movingPokemon = true;
			PartyOptions.S.choseToMove = false;
			PartyOptions.S.gameObject.SetActive (false);
			beginMovingPokemon = activeItem;
			gui = pkmn [beginMovingPokemon].GetComponent<GUIText> ();
			gui.color = Color.blue;
			selectingPokemon = false;
		} else if (movingPokemon) {
			if (Input.GetKeyDown (KeyCode.DownArrow))
				MoveDownOne ();
			else if (Input.GetKeyDown (KeyCode.UpArrow))
				MoveUpOne ();
			endMovingPokemon = activeItem;
			if (Input.GetKeyDown (KeyCode.S)) {
				movingPokemon = false;
				selectingPokemon = true;
				gui = pkmn [activeItem].GetComponent<GUIText> ();
				gui.color = Color.black;
				gui = pkmn[beginMovingPokemon].GetComponent<GUIText>();
				gui.color = Color.red;
			}
			else if(Input.GetKeyDown (KeyCode.A)) {
				movingPokemon = false;
				selectingPokemon = true;
				if(beginMovingPokemon == endMovingPokemon) {
					gui = pkmn [endMovingPokemon].GetComponent<GUIText> ();
					gui.color = Color.black;
				} else {
					gui = pkmn [endMovingPokemon].GetComponent<GUIText> ();
					gui.color = Color.black;
					Pokemon temp = Player.S.party[beginMovingPokemon];
					Player.S.party[beginMovingPokemon] = Player.S.party[endMovingPokemon];
					Player.S.party[endMovingPokemon] = temp;
					for(int i = 0; i < 6; ++i)
						party[i] = Player.S.party[i];
					activeItem = endMovingPokemon;
					buildPartyGraphic(endMovingPokemon);
				}
			}
		}

		else if (noWillToFightMessage) {
			PrintOverPokemon(activeItem, "There is no will to fight");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				noWillToFightMessage = false;
				selectingPokemon = true;
				buildPartyGraphic (activeItem);
			}
		} else if (alreadyActiveMessage) {
			PrintOverPokemon(activeItem, "Pokemon is already in use");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				alreadyActiveMessage = false;
				selectingPokemon = true;
				buildPartyGraphic (activeItem);
			}
		} else if (pokemonHasFullHPMessage) {
			PrintOverPokemon(activeItem, "Pokemon already has full HP");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				pokemonHasFullHPMessage = false;
				selectingPokemon = true;
				buildPartyGraphic (activeItem);
			}
		} else if(pokemonCantBeHealedMessage) {
			PrintOverPokemon(activeItem, "Pokemon has fainted and can't be healed");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				pokemonHasFullHPMessage = false;
				selectingPokemon = true;
				buildPartyGraphic (activeItem);
			}
		}
	}

	void MoveDownOne() {
		if(pkmn [activeItem].GetComponent<GUIText> ().color != Color.blue)
			pkmn [activeItem].GetComponent<GUIText> ().color = Color.black;
		if (activeItem == 5)
			activeItem = 0;
		else if (party [activeItem + 1].Name == "-")
			activeItem = 0;
		else
			++activeItem;
		if (pkmn [activeItem].GetComponent<GUIText> ().color != Color.blue)
			pkmn [activeItem].GetComponent<GUIText> ().color = Color.red;
	}
	void MoveUpOne() {
		if(pkmn [activeItem].GetComponent<GUIText> ().color != Color.blue)
			pkmn [activeItem].GetComponent<GUIText> ().color = Color.black;
		if (activeItem == 0) {
			activeItem = 5;
			while(party[activeItem].Name == "-")
				--activeItem;
		}
		else
			--activeItem;
		if (pkmn [activeItem].GetComponent<GUIText> ().color != Color.blue)
			pkmn [activeItem].GetComponent<GUIText> ().color = Color.red;
	}
}
