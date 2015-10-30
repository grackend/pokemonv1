using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleMain : MonoBehaviour {

	static public BattleMain S;

	public Pokemon 			enemy;
	public List<Pokemon>	enemyList;
	public int				currentNPCPoke;
	public string			EnemyName;
	public int				FirstPokemon;
	public Pokemon			currentPokemon;
	public Pokemon			lastPokemon;
	public bool				switchingPokemon;
	public bool				givingPokemonItem;
	public bool				intentToSwitchPokemon = false;
	public float 			EXPmultiplier = 1f, MoneyMultiplier = 1f;
	public int				moneyDrop;


	
	void Awake() {
		S = this;
		switchingPokemon = false;
	}

	// Use this for initialization
	void Start () {
		if(Main.S.gotPokeFromOak)
			setFirstPokemon ();
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void setFirstPokemon() {
		int i = 0;
		while (Player.S.party[i].HealthCurrent < 1) {
			++i;
		}
		FirstPokemon = i;
		currentPokemon = Player.S.party [FirstPokemon];
	}

	public bool	moreNPCPokemon () {
		return currentNPCPoke < enemyList.Count;
	}
}
