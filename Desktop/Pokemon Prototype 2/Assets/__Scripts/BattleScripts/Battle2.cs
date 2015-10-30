using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Battle2 : MonoBehaviour {
	/*
	static public Battle2 S;

	public Pokemon 		PlayerPokemon; //also accessed from Party.cs
	public Pokemon		EnemyPokemon;

	public bool			ChoseToAttack; 
	public bool			ChoseToSwitch;
	public static bool	Switching;
	public bool			ChoseToUseItem;
	public bool 		ChoseToRun;

	public bool			showingMessages;
	public int			currentMessage;
	public List<string>	battleMessages;

	public bool 		PlayerTurn;
	public bool 		EnemyTurn;
	public bool			PlayerFainted;
	public bool			EnemyFainted;

	public Attack		ChosenAttack; //updated in AttackBox.cs
	// Use this for initialization

	void Awake() {
		ChoseToAttack = ChoseToRun = ChoseToSwitch = ChoseToUseItem = Switching = false;
		PlayerPokemon = Player.S.Party [0];
		EnemyPokemon = BattleMain.S.enemy;
	}

	void UpdatePlayerPokemonInfo() {
		GameObject Go = GameObject.Find ("BattlePlayerPokemonInfo");
		PlayerPokemonInfoDisplay script = Go.GetComponent<PlayerPokemonInfoDisplay> ();
		script.poke = PlayerPokemon;
	}

	void UpdateEnemyPokemonInfo() {
		GameObject Go = GameObject.Find ("BattleEnemyPokemonInfo");
		EnemyPokemonInfoDisplay script = Go.GetComponent<EnemyPokemonInfoDisplay> ();
		script.poke = EnemyPokemon;
	}

	void Start () {

		UpdatePlayerPokemonInfo ();
		UpdateEnemyPokemonInfo ();
		OptionBox.S.gameObject.SetActive (false);

		showingMessages = true;
		BattleText.S.printToBattleText (BattleMain.S.EnemyName + " wants to battle!");
		currentMessage = 0;

		checkSpeeds ();

		PlayerFainted = false;
		EnemyFainted = false;

	}

	public void PlayerAttackEnemy() {
		int damg = Damage (PlayerPokemon, EnemyPokemon, ChosenAttack);
		print ("1 Health: " + EnemyPokemon.HealthCurrent);
		EnemyPokemon.HealthCurrent -= damg;
		print ("2 Health: " + EnemyPokemon.HealthCurrent);
		string s = (PlayerPokemon.Name + " used " + ChosenAttack.AttackName + "!");
		battleMessages.Add (s);
		currentMessage = 0;
	}

	public void EnemyAttackPlayer() {
		Attack att = EnemyAI (EnemyPokemon);
		int dmg = Damage (EnemyPokemon, PlayerPokemon, att);
		if (!Main.S.invincible)
			PlayerPokemon.HealthCurrent -= dmg;
		string s = (EnemyPokemon.Name + " used " + att.AttackName + "!");
		battleMessages.Add (s);
		currentMessage = 0;

	}

	
	// Update is called once per frame
	void Update () {

		if (!OptionBox.S.gameObject.activeSelf && !showingMessages && 
		    !PlayerFainted && !EnemyFainted && 
		    (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S))) {
			OptionBox.S.gameObject.SetActive(true);
		}

		if (ChoseToAttack) {
			bool first = false;
			//player initial attack if her pokemon is faster
			if(PlayerTurn && !showingMessages && !PlayerFainted) {
				first = true;
				PlayerAttackEnemy();
				PlayerTurn = false;
				EnemyTurn = true;
			}
			if(EnemyPokemon.HealthCurrent < 1) {
				battleMessages.Add (EnemyPokemon.Name + " fainted");
				battleMessages.Add (PlayerPokemon.Name + " gained " + EnemyPokemon.ExperienceDrop + " EXP");
				if(EnemyPokemon.ExperienceDrop + PlayerPokemon.ExperienceCurrent >= PlayerPokemon.ExperienceToNext)
					battleMessages.Add (PlayerPokemon.ExperienceString);
				PlayerPokemon.ExperienceCurrent += EnemyPokemon.ExperienceDrop;

				EnemyFainted = true;
			}

			//enemy can attack
			if(EnemyTurn && !showingMessages && !EnemyFainted) {
				EnemyAttackPlayer();
				EnemyTurn = false;
				PlayerTurn = true;
			}
			if(PlayerPokemon.HealthCurrent < 1) {
				battleMessages.Add ("Your " + PlayerPokemon.Name + " fainted");
				PlayerFainted = true;
			}

			//let player attack if her pokemon is slower
			if(PlayerTurn && !showingMessages && !first) {
				PlayerAttackEnemy();
				PlayerTurn = false;
				EnemyTurn = true;
			}
			if(EnemyPokemon.HealthCurrent < 1 && !EnemyFainted) {
				battleMessages.Add (EnemyPokemon.Name + " fainted");
				battleMessages.Add (PlayerPokemon.Name + " gained " + EnemyPokemon.ExperienceDrop + " EXP");
				if(EnemyPokemon.ExperienceDrop + PlayerPokemon.ExperienceCurrent >= PlayerPokemon.ExperienceToNext)
					battleMessages.Add (PlayerPokemon.ExperienceString);
				PlayerPokemon.ExperienceCurrent += EnemyPokemon.ExperienceDrop;

				EnemyFainted = true;
			}

			showingMessages = true;
			ChoseToAttack = false;

		}
		if (PlayerFainted) {
			if(/*(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)) && !showingMessages) {
				battleMessages.Clear();
				PlayerFainted = false;
				Main.S.CloseBattle();
			}
		}
		if (EnemyFainted) {
			if(/*(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)) && !showingMessages) {
				battleMessages.Clear();
				EnemyFainted = false;
				Main.S.CloseBattle();
			}
		}
		if (Switching) {
			battleMessages.Add (PlayerPokemon.Name + " return!");
			print (Party.ChosenPokemon.Name);
			PlayerPokemon = Party.ChosenPokemon;
			battleMessages.Add ("Go " + PlayerPokemon.Name + "!");

			AttackBox.S.SwitchTo = PlayerPokemon;
			Main.S.ClosePokemonParty();
			UpdatePlayerPokemonInfo();
			AttackBox.S.UpdatePokemonAttackInfo();

			Switching = false;
			EnemyAttackPlayer();
			checkSpeeds();
			showingMessages = true;
		}
		if (ChoseToRun && !Player.S.spottedByTrainer) {
			ChoseToRun = false;
			Main.S.CloseBattle();
		}

		showMessages ();

	}

	int Damage(Pokemon attacker, Pokemon defender, Attack attack) {
		float Dmg = ((2f * attacker.Level + 10f) / 250f) * attack.AttackPower;
		float attackStat = attack.IsPhysical == true ? attacker.AttackStat : attacker.SpecialStat;
		float defenseStat = attack.IsPhysical == true ? defender.DefenseStat : defender.SpecialStat;
		Dmg *= (attackStat/defenseStat);
		Dmg = (Dmg * 10) % 10 >= 5 ? Mathf.Ceil(Dmg) : Mathf.Floor(Dmg);
		int damg = (int)Dmg;
		return damg;
	}

	Attack EnemyAI(Pokemon enemy) { //picks a random attack
		int i = 0;
		foreach (Attack att in enemy.Attacks) {
			if(att.AttackName != "-") ++i;
		}
		i = Random.Range (0, i - 1);
		
		return enemy.Attacks [i];
	}

	void showMessages() {
		if (showingMessages) {
			if(battleMessages.Count == 0) {
				showingMessages = false;
			}
			else {
				BattleText.S.printToBattleText (battleMessages[currentMessage]);
				if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)) {
					if(++currentMessage == battleMessages.Count) {//if there are no more messages
						battleMessages.Clear();
						showingMessages = false;
						currentMessage = 0;
						BattleText.S.printToBattleText ("");
						//OptionBox.S.gameObject.SetActive(true);
					}
					else 
						BattleText.S.printToBattleText (battleMessages[currentMessage]); //print next message
				}
			}
		}
	}

	void checkSpeeds() {
		if (PlayerPokemon.SpeedStat >= EnemyPokemon.SpeedStat) {
			PlayerTurn = true;
			EnemyTurn = false;
		} else {
			PlayerTurn = false;
			EnemyTurn = true;
		}

	}
*/
}	