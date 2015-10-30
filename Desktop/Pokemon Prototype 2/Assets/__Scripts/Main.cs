using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	static public Main S;

	public bool	CustomLevel;

	public bool inDialog;
	public bool paused;
	public bool inBattle;
	public bool viewingPokemon;
	public bool invincible;
	public bool	viewingItems;
	public bool	usingPotion;
	bool		noMusic;
	public bool	gotPokeFromOak;

	public Pokemon 	Alakazam, Arcanine, Dragonite, Dugtrio, Electabuzz, Gengar, Gyarados, Hitmonchan;
	public Pokemon  Jynx, Kangaskhan, Muk, Rhydon, Scyther, Snorlax;

	void Awake() {
		S = this;
	}

	void Start(){
		inDialog = false;
		paused = false;
		inBattle = false;
		viewingPokemon = false;
		invincible = false;
		noMusic = false;

	}
	
	// Update is called once per frame
	void Update () {

		/*if (viewingPokemon && Input.GetKeyDown (KeyCode.S)) {
			ClosePokemonParty();
		}*/
		if (!inDialog && Input.GetKeyDown (KeyCode.Return) && !inBattle && !viewingItems && !viewingPokemon) {
			Menu.S.gameObject.SetActive(true);
			paused = true;
		}
		if (Input.GetKeyDown (KeyCode.I)) {
			if (invincible)
				invincible = false;
			else 
				invincible = true;
		}
		if(Input.GetKeyDown (KeyCode.M)) {
			if(!noMusic) {
				Music.S.gameObject.SetActive(false);
				noMusic = true;
			}
			else {
				Music.S.gameObject.SetActive(true);
				noMusic = false;
			}
		}
	}

	public void OpenPokemonParty() {
		Application.LoadLevelAdditive ("_Party");
		viewingPokemon = true;
	}
	public void ClosePokemonParty() {
		Application.UnloadLevel ("_Party");
		Main.S.viewingPokemon = false;
		BattleMain.S.givingPokemonItem = false;
		BattleMain.S.intentToSwitchPokemon = false;
	}
	public void OpenBattle(Pokemon enemy) {
		BattleMain.S.setFirstPokemon ();
		Application.LoadLevelAdditive ("_Battle");
		BattleMain.S.enemy = enemy;
		//BattleMain.S.EnemyName = enemy.Name;
		Main.S.inBattle = true;
	}
	public void CloseBattle() {
		for (int i = 0; i < 4; ++i) 
			Destroy (BattleMain.S.enemy.Attacks[i].gameObject);
		for (int i = 0; i < 6; ++i) {
			Player.S.party[i].resetStats();
		}
		Destroy (BattleMain.S.enemy.gameObject);
		OptionBox.S.gameObject.SetActive (false);
		PlayerPokemonInfoDisplay.S.gameObject.SetActive (false);
		Application.UnloadLevel ("_Battle");
		Main.S.inBattle = false;
		Player.S.spottedByTrainer = false;
	}
	public void OpenBag() {
		Application.LoadLevelAdditive ("_Bag");
		viewingItems = true;
		//Bag.S.browsingItems = true;
	}
	public void CloseBag() {
		viewingItems = false;
		Bag.S.browsingItems = false;
		Application.UnloadLevel ("_Bag");
	}

	public Pokemon wild(int rndm) {
		switch (rndm) {
		case 0:
			return Instantiate(Alakazam);
		case 1:
			return Instantiate(Arcanine);
		case 2:
			return Instantiate(Dragonite);
		case 3:
			return Instantiate(Dugtrio);
		case 4:
			return Instantiate(Gengar);
		case 5:
			return Instantiate(Electabuzz);
		case 6:
			return Instantiate(Gyarados);
		case 7:
			return Instantiate(Jynx);
		case 8:
			return Instantiate(Scyther);
		case 9:
			return Instantiate(Snorlax);
		case 10:
			return Instantiate(Muk);
		case 11:
			return Instantiate(Rhydon);
		case 12:
			return Instantiate(Kangaskhan);
		case 13:
			return Instantiate(Hitmonchan);
		default:
			return Instantiate (Alakazam);
		}
	}
}