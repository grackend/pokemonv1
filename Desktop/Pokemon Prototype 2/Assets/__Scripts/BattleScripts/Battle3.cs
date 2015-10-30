using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Battle3 : MonoBehaviour {

	public static Battle3 		S;

	Pokemon						playerPokemon;
	Pokemon						enemyPokemon;

	public List<string>			messageQueue; //holds all strings that need to be displayed
	public string				currentMessageString;
	public int					currentMessagesNumber; //the message that will be next displayed in messageQueue
	public bool					messageBattleStart;	

	public bool					displayInitialUI;

	public bool					Fight;
	public bool					PKMN;
	public bool					Item;
	public bool					Run;

	public Item					ball; //set in Bag.cs

	bool 						attackChosen;
	bool						playerDoesDamage;
	bool 						enemyDoesDamage;
	public bool					enemyFainted;
	public bool					playerFainted;
	
	bool						gainEXP;
	bool						gainLevelNum;
	bool						gainLevel;
	bool						showStatChanges;
	bool						playerTurn;
	bool						enemyTurn;
	bool						superEffective;
	bool						notVeryEffective;
	bool						notEffective;
	bool						checkEffectiveness;
	bool						sendNextPokemon;
	public bool					usedPotion;
	public bool					usedPokeball; //changed in Bag.cs
	bool						caught;
	bool						shake1, shake2, shake3;
	bool						itBrokeFree;
	bool						addPokemonToParty;
	public bool					playerFaintedChooseNext;
	bool 						allPokemonFainted;
	bool						moneyDrop;
	bool						askForKillingBlow;
	bool						enemyKilled;
	bool						morePokemon;
	bool						endBattle;

	int 						whoAttacksNext;

	bool						statChange;
	string						statChangeStat;
	string						statChangeTarget;
	string						statChangeAmount;
	float						statChangeChance;
	Attack 						atk; //enemy's attack. This is a quick fix for a strange bug


	void Awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
		messageBattleStart = true;
		Fight = false;
		PKMN = false;
		Item = false;
		Run = false;
		attackChosen = false;
		playerDoesDamage = true;
		enemyDoesDamage = true;
		displayInitialUI = false;
		playerTurn = false;
		enemyFainted = false;
		playerFainted = false;
		gainEXP = false;
		gainLevelNum = false;
		gainLevel = false;
		enemyTurn = false;
		sendNextPokemon = false;
		usedPotion = false;
		usedPokeball = shake1 = shake2 = shake3 = caught = false;
		itBrokeFree = false;
		addPokemonToParty = false;
		playerFaintedChooseNext = false;
		allPokemonFainted = false;
		endBattle = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		playerPokemon = BattleMain.S.currentPokemon;
		enemyPokemon = BattleMain.S.enemy;

		if (messageBattleStart) { //displays initial message at beginning of battle
			currentMessageString = BattleMain.S.EnemyName + " wants to fight!";
			BattleText.S.printToBattleText (currentMessageString);
			EnemyPokemonInfoDisplay.S.updateUI ();
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				messageBattleStart = false;
				displayInitialUI = true;
			}
		} else if (displayInitialUI) {
			BattleText.S.printToBattleText ("Go " + playerPokemon.Name + "!");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				PlayerPokemonInfoDisplay.S.gameObject.SetActive (true);
				PlayerPokemonInfoDisplay.S.updateUI ();
				displayInitialUI = false;
				OptionBox.S.gameObject.SetActive (true);
			}
		} else if (AttackBox.S.AttackChosen) {
			AttackBox.S.AttackChosen = false;
			attackChosen = true;
		} else if (attackChosen) {//you chose an attack to use with PP
			//if you are faster
			if (playerPokemon.SpeedStat >= enemyPokemon.SpeedStat) {
				BattleText.S.printToBattleText (playerPokemon.Name + " used " + AttackBox.S.ChosenAttack.AttackName + "!");
				if (playerDoesDamage) { //does damage once
					enemyPokemon.HealthCurrent -= Damage (playerPokemon, enemyPokemon, AttackBox.S.ChosenAttack);
					playerDoesDamage = false;
				}
				EnemyPokemonInfoDisplay.S.updateUI ();
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {

					whoAttacksNext = 0;
					playerDoesDamage = true;
					attackChosen = false;

					if (AttackBox.S.ChosenAttack.StatChangeChance >= Random.value * 100) {
						statChangeStat = AttackBox.S.ChosenAttack.StatChange; //set string to be printed later
						if (AttackBox.S.ChosenAttack.StatChangeSelf) {
							statChangeFunc(AttackBox.S.ChosenAttack, playerPokemon);
							statChangeTarget = playerPokemon.Name;
						} else {
							statChangeFunc (AttackBox.S.ChosenAttack, enemyPokemon);
							statChangeTarget = enemyPokemon.Name;
						} 
						if (AttackBox.S.ChosenAttack.StatChangeAmount == 1) statChangeAmount = "rose";
						else if(AttackBox.S.ChosenAttack.StatChangeAmount == 2) statChangeAmount = "greatly rose";
						else if(AttackBox.S.ChosenAttack.StatChangeAmount == -1) statChangeAmount = "fell";
						else if(AttackBox.S.ChosenAttack.StatChangeAmount == -2) statChangeAmount = "harshly fell";
						statChange = true;
					}
					else 
						checkEffectiveness = true;
					//check if Enemy fainted went second
				}
			} else { //enemy is faster
				if (enemyDoesDamage) {
					atk = EnemyAI (enemyPokemon);
					BattleText.S.printToBattleText ("Enemy " + enemyPokemon.Name + " used " + atk.AttackName + "!");
					if (!Main.S.invincible)
						playerPokemon.HealthCurrent -= Damage (enemyPokemon, playerPokemon, atk);
					enemyDoesDamage = false;
				}
				PlayerPokemonInfoDisplay.S.updateUI ();
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
					whoAttacksNext = 1;
					enemyDoesDamage = true;
					attackChosen = false;
					if (atk.StatChangeChance >= Random.value * 100) {
						statChangeStat = atk.StatChange; //set string to be printed later
						if (atk.StatChangeSelf) {
							statChangeFunc(atk, enemyPokemon);
							statChangeTarget = enemyPokemon.Name;
						} else {
							statChangeFunc (atk, playerPokemon);
							statChangeTarget = playerPokemon.Name;
						} 
						if (atk.StatChangeAmount == 1) statChangeAmount = "rose";
						else if(atk.StatChangeAmount == 2) statChangeAmount = "greatly rose";
						else if(atk.StatChangeAmount == -1) statChangeAmount = "fell";
						else if(atk.StatChangeAmount == -2) statChangeAmount = "harshly fell";
						statChange = true;
					}
					else 
						checkEffectiveness = true;
					//check if player fainted went second
				}
			}
		} else if (playerTurn) {//when the player attacks second
			BattleText.S.printToBattleText (playerPokemon.Name + " used " + AttackBox.S.ChosenAttack.AttackName + "!");
			if (playerDoesDamage) {
				enemyPokemon.HealthCurrent -= Damage (playerPokemon, enemyPokemon, AttackBox.S.ChosenAttack);
				playerDoesDamage = false;
			}
			EnemyPokemonInfoDisplay.S.updateUI ();
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {

				playerDoesDamage = true;
				whoAttacksNext = 2;

				if (AttackBox.S.ChosenAttack.StatChangeChance >= Random.value * 100) {
					statChangeStat = AttackBox.S.ChosenAttack.StatChange; //set string to be printed later
					if (AttackBox.S.ChosenAttack.StatChangeSelf) {
						statChangeFunc(AttackBox.S.ChosenAttack, playerPokemon);
						statChangeTarget = playerPokemon.Name;
					} else {
						statChangeFunc (AttackBox.S.ChosenAttack, enemyPokemon);
						statChangeTarget = enemyPokemon.Name;
					} 
					if (AttackBox.S.ChosenAttack.StatChangeAmount == 1) statChangeAmount = "rose";
					else if(AttackBox.S.ChosenAttack.StatChangeAmount == 2) statChangeAmount = "greatly rose";
					else if(AttackBox.S.ChosenAttack.StatChangeAmount == -1) statChangeAmount = "fell";
					else if(AttackBox.S.ChosenAttack.StatChangeAmount == -2) statChangeAmount = "harshly fell";
					statChange = true;
				}
				else 
					checkEffectiveness = true;
				playerTurn = false;
			}
		} else if (enemyTurn) {//when the enemy attacks second
			if (enemyDoesDamage) {
				atk = EnemyAI (enemyPokemon);
				BattleText.S.printToBattleText ("Enemy " + enemyPokemon.Name + " used " + atk.AttackName + "!");
				if (!Main.S.invincible)
					playerPokemon.HealthCurrent -= Damage (enemyPokemon, playerPokemon, atk);
				enemyDoesDamage = false;
			}
			PlayerPokemonInfoDisplay.S.updateUI ();
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				enemyTurn = false;
				whoAttacksNext = 3;
				enemyDoesDamage = true;
				if (atk.StatChangeChance >= Random.value * 100) {
					statChangeStat = atk.StatChange; //set string to be printed later
					if (atk.StatChangeSelf) {
						statChangeFunc(atk, enemyPokemon);
						statChangeTarget = enemyPokemon.Name;
					} else {
						statChangeFunc (atk, playerPokemon);
						statChangeTarget = playerPokemon.Name;
					} 
					if (atk.StatChangeAmount == 1) statChangeAmount = "rose";
					else if(atk.StatChangeAmount == 2) statChangeAmount = "greatly rose";
					else if(atk.StatChangeAmount == -1) statChangeAmount = "fell";
					else if(atk.StatChangeAmount == -2) statChangeAmount = "harshly fell";
					statChange = true;
				}
				else 
					checkEffectiveness = true;
			}
		} else if (statChange) {
			BattleText.S.printToBattleText (statChangeTarget + "'s " + statChangeStat + " " + statChangeAmount + "!");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				statChange = false;
				checkEffectiveness = true;
			}
		}

		else if (checkEffectiveness) {
			printEffectiveness (whoAttacksNext);
			//checkEffectiveness = false;
		} else if (playerFainted) {
			BattleText.S.printToBattleText (playerPokemon.Name + " fainted!");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				playerFainted = false;

				int numPokemonFainted = 0, numPokemonChecked = 0; //checking if all pokemon have fainted
				while (numPokemonChecked < 6) {
					if (Player.S.party [numPokemonChecked].HealthCurrent == 0)
						numPokemonFainted++;
					numPokemonChecked++;
				}
				if (numPokemonFainted == 6) { //if all have fainted
					allPokemonFainted = true;
				} else { //if not all have fainted
					playerFaintedChooseNext = true;
					BattleMain.S.intentToSwitchPokemon = true;
					Main.S.OpenPokemonParty ();
				}
			}
		} else if (enemyFainted) {
			BattleText.S.printToBattleText (enemyPokemon.Name + " fainted!");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				if (Main.S.CustomLevel) {
					if (BattleMain.S.EXPmultiplier > .2)
						BattleMain.S.EXPmultiplier -= .2f;
					BattleMain.S.MoneyMultiplier += .2f;
				}
				if(Player.S.spottedByTrainer) { //if you're in a trainer battle
					++BattleMain.S.currentNPCPoke;
					if(Player.S.spottedByTrainer && BattleMain.S.moreNPCPokemon ()) {
						BattleMain.S.enemy = BattleMain.S.enemyList[BattleMain.S.currentNPCPoke];
					}
				}
				gainEXP = true;
				enemyFainted = false;
			}
		} else if (enemyKilled) {
			BattleText.S.printToBattleText ("You murdered " + enemyPokemon.Name);
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				if(Player.S.spottedByTrainer) { //if you're in a trainer battle
					++BattleMain.S.currentNPCPoke;
					if(Player.S.spottedByTrainer && BattleMain.S.moreNPCPokemon ()) {
						BattleMain.S.enemy = BattleMain.S.enemyList[BattleMain.S.currentNPCPoke];
					}
				}
				gainEXP = true;
				enemyKilled = false;
				BattleMain.S.EXPmultiplier += .2f;
				if (BattleMain.S.MoneyMultiplier > .2)
					BattleMain.S.MoneyMultiplier -= .2f;

			}
		} else if (gainEXP) {
			BattleText.S.printToBattleText (playerPokemon.Name + " gained " + 
				(enemyPokemon.ExperienceDrop * BattleMain.S.EXPmultiplier) + " EXP!");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				playerPokemon.ExperienceCurrent += (int)Mathf.Ceil (enemyPokemon.ExperienceDrop * BattleMain.S.EXPmultiplier);
				//if you gained a level
				if (playerPokemon.ExperienceCurrent >= playerPokemon.ExperienceToNext) {
					playerPokemon.ExperienceCurrent = 
						playerPokemon.ExperienceToNext - playerPokemon.ExperienceCurrent;
					playerPokemon.ExperienceToNext = playerPokemon.Level * playerPokemon.Level - playerPokemon.Level; 
					gainLevelNum = true;
					gainEXP = false;
				} else {
					gainEXP = false;
					if (Player.S.spottedByTrainer && BattleMain.S.moreNPCPokemon ())
						morePokemon = true;
					else
						moneyDrop = true;
				}
			}
		} else if (gainLevelNum) {
			gainLevelNum = false;
			gainLevel = true;
			playerPokemon.Level++;
			PlayerPokemonInfoDisplay.S.updateUI ();
		} else if (gainLevel) {
			BattleText.S.printToBattleText (playerPokemon.Name + " grew to level " + 
				(playerPokemon.Level) + "!");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				playerPokemon.HealthCurrent += 2;
				playerPokemon.HealthFull += 2;
				PlayerPokemonInfoDisplay.S.updateUI ();
				gainLevel = false;
				showStatChanges = true;
			}
		} else if (showStatChanges) {
			StatIncrease.S.gameObject.SetActive (true);
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				++playerPokemon.AttackStat;
				++playerPokemon.DefenseStat;
				++playerPokemon.SpecialStat;
				++playerPokemon.SpeedStat;
				showStatChanges = false;
				StatIncrease.S.gameObject.SetActive (false);
				if (Player.S.spottedByTrainer && BattleMain.S.moreNPCPokemon ())
					morePokemon = true;
				else
					moneyDrop = true;
			}
		} else if (morePokemon) {
			BattleText.S.printToBattleText (BattleMain.S.EnemyName + " sent out " + BattleMain.S.enemy.Name);
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				EnemyPokemonInfoDisplay.S.updateUI();
				morePokemon = false;
				OptionBox.S.gameObject.SetActive (true);
			} 
		}
		else if (moneyDrop) {
			if (BattleMain.S.moneyDrop > 0) {
				BattleText.S.printToBattleText (BattleMain.S.EnemyName + " dropped $" + (BattleMain.S.moneyDrop * BattleMain.S.MoneyMultiplier));
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
					moneyDrop = false;
					Player.S.wallet += (int)Mathf.Ceil(BattleMain.S.moneyDrop * BattleMain.S.MoneyMultiplier);
					endBattle = true;
					BattleMain.S.moneyDrop = 0;
				}
			} else {
				endBattle = true;
				moneyDrop = false;
			}

		} else if (sendNextPokemon) {
			BattleText.S.printToBattleText (playerPokemon.Name + " I choose you!");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				AttackBox.S.UpdatePokemonAttackInfo ();
				sendNextPokemon = false;
				enemyTurn = true;
			}
		} else if (PKMN) { //switching Pokemon is handled in OptionBox and Party and BattleMain
			BattleText.S.printToBattleText (BattleMain.S.lastPokemon.Name + " return!");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				PlayerPokemonInfoDisplay.S.updateUI ();
				sendNextPokemon = true;
				PKMN = false;
			}
		} else if (BattleMain.S.switchingPokemon) {
			PKMN = true;
			BattleMain.S.switchingPokemon = false;
		} else if (OptionBox.S.chooseRun) {
			OptionBox.S.gameObject.SetActive (false);
			if (!Player.S.spottedByTrainer) { //can run from wild pokemon
				BattleText.S.printToBattleText ("You got away safely");
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
					endBattle = true;
					OptionBox.S.chooseRun = false;
				}
			} else {
				BattleText.S.printToBattleText ("You can't run from a trainer battle");
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
					OptionBox.S.chooseRun = false;
					OptionBox.S.gameObject.SetActive (true);
				}
			}
		} else if (usedPotion) {
			BattleText.S.printToBattleText ("Pokemon was healed");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				usedPotion = false;
				enemyTurn = true;
			}
		} else if (usedPokeball) {
			BattleText.S.printToBattleText ("You threw a " + ball.itemName);
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				int rate = Pokeball (ball, enemyPokemon);
				usedPokeball = false;
				if (rate == 0) {
					shake1 = shake2 = shake3 = caught = true;
				} else if (rate == 1)
					shake1 = true;
				else if (rate == 2)
					shake1 = shake2 = true;
				else if (rate == 3)
					shake1 = shake2 = shake3 = true;
			}
		} else if (shake1) {
			BattleText.S.printToBattleText ("The ball shakes");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				if (!shake2) {
					itBrokeFree = true;
				}
				shake1 = false;
			}
		} else if (shake2 && !shake1) {
			BattleText.S.printToBattleText ("It shakes again");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				if (!shake3) {
					itBrokeFree = true;
				}
				shake2 = false;
			}
		} else if (shake3 && !shake2) {
			BattleText.S.printToBattleText ("It shakes a third time!");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				if (!caught) {
					itBrokeFree = true;
				}
				shake3 = false;
			}
		} else if (!shake3 && caught) {
			BattleText.S.printToBattleText ("Gotcha! " + enemyPokemon.Name + " was caught!");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				caught = false;
				addPokemonToParty = true;
			}
		} else if (addPokemonToParty) {
			if (Player.S.partySize () == 6) {
				BattleText.S.printToBattleText (enemyPokemon.Name + " was sent to your PC");
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
					addPokemonToParty = false;
					endBattle = true;
				}

			} else {
				BattleText.S.printToBattleText (enemyPokemon.Name + " was added to your party");
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
					addPokemonToParty = false;
					Player.S.party [Player.S.partySize ()] = enemyPokemon;
					endBattle = true;
				}
			}
		} else if (itBrokeFree) {
			BattleText.S.printToBattleText ("It broke free!");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				BattleText.S.printToBattleText ("");
				itBrokeFree = false;
				enemyTurn = true;
			}
		} else if (allPokemonFainted) {
			BattleText.S.printToBattleText ("You have no remaining Pokemon. You lose");
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
				allPokemonFainted = false;
				Application.LoadLevel ("_Scene_Map");
			}
		} else if (askForKillingBlow) {
			BattleText.S.printToBattleText ("Press A to kill. Press S for mercy");
			if(Input.GetKeyDown(KeyCode.A)) {
				askForKillingBlow = false;
				enemyKilled = true;
			} else if(Input.GetKeyDown(KeyCode.S)) {
				askForKillingBlow = false;
				enemyFainted = true;
			}
		} else if (endBattle) {
			endBattle = false;
			Main.S.CloseBattle ();
		}
		//BattleMain.S.currentPokemon = playerPokemon;
		//BattleMain.S.enemy = enemyPokemon;
	}


	//MEMBER FUNCTIONS BEGIN HERE


	int Damage(Pokemon attacker, Pokemon defender, Attack attack) {
		float Dmg = ((2f * attacker.Level + 10f) / 250f) * attack.AttackPower;
		float attackStat = attack.IsPhysical == true ? attacker.AttackStat : attacker.SpecialStat;
		float defenseStat = attack.IsPhysical == true ? defender.DefenseStat : defender.SpecialStat;
		Dmg *= (attackStat / defenseStat);
		Dmg *= TypeEffectiveness (attack.AttackType, defender);
		if(attack.AttackType == attacker.Type1 || attack.AttackType == attacker.Type2)
			Dmg *= 1.5f;
		Dmg = (Dmg * 10) % 10 >= 5 ? Mathf.Ceil (Dmg) : Mathf.Floor (Dmg);
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
	int Pokeball(Item ball, Pokemon enemy) {
		//((( 3 * Max HP - 2 * HP ) * (Catch Rate * Ball Modifier ) / (3 * Max HP) ) * Status Modifier
		float catchValue = ((3 * enemy.HealthFull - 2 * enemy.HealthCurrent) * (enemy.CatchRate * ball.catchQuality));
		catchValue /= (3 * enemy.HealthFull);
		if (catchValue >= 255) {
			return 0; //caught
		}
		int M = Mathf.CeilToInt(Random.Range (0.0001f, 3f));
		return M;
	}

	float superEffectiveMult(string hypotheticalType, Pokemon defender) {
		if (defender.Type1 == hypotheticalType || defender.Type2 == hypotheticalType) {
			superEffective = true;
			return 2f;
		} else
			return 1f;
	}

	float notVeryEffectiveMult(string hypotheticalType, Pokemon defender) {
		if (defender.Type1 == hypotheticalType || defender.Type2 == hypotheticalType) {
			notVeryEffective = true;
			return .5f;
		} else
			return 1f;
	}

	float notEffectiveMult(string hypotheticalType, Pokemon defender) {
		if (defender.Type1 == hypotheticalType || defender.Type2 == hypotheticalType) {
			notEffective = true;
			return 0f;
		} else
			return 1f;
	}

	float TypeEffectiveness(string atkType, Pokemon defender) {
		float damage = 1f;
		if (atkType == "Normal") {
			damage *= notVeryEffectiveMult ("Rock", defender);
			damage *= notEffectiveMult ("Ghost", defender);
		} else if (atkType == "Fire") {
			damage *= superEffectiveMult ("Grass", defender);
			damage *= superEffectiveMult ("Ice", defender);
			damage *= superEffectiveMult ("Bug", defender);
			damage *= notVeryEffectiveMult ("Fire", defender);
			damage *= notVeryEffectiveMult ("Water", defender);
			damage *= notVeryEffectiveMult ("Rock", defender);
			damage *= notVeryEffectiveMult ("Dragon", defender);
		} else if (atkType == "Water") {
			damage *= superEffectiveMult ("Fire", defender);
			damage *= superEffectiveMult ("Ground", defender);
			damage *= superEffectiveMult ("Rock", defender);
			damage *= notVeryEffectiveMult ("Water", defender);
			damage *= notVeryEffectiveMult ("Grass", defender);
			damage *= notVeryEffectiveMult ("Dragon", defender);
		} else if (atkType == "Electric") {
			damage *= superEffectiveMult ("Water", defender);
			damage *= superEffectiveMult ("Flying", defender);
			damage *= notVeryEffectiveMult ("Electric", defender);
			damage *= notVeryEffectiveMult ("Grass", defender);
			damage *= notVeryEffectiveMult ("Dragon", defender);
			damage *= notEffectiveMult ("Ground", defender);
		} else if (atkType == "Grass") {
			damage *= superEffectiveMult ("Water", defender);
			damage *= superEffectiveMult ("Ground", defender);
			damage *= superEffectiveMult ("Rock", defender);
			damage *= notVeryEffectiveMult ("Fire", defender);
			damage *= notVeryEffectiveMult ("Grass", defender);
			damage *= notVeryEffectiveMult ("Poison", defender);
			damage *= notVeryEffectiveMult ("Flying", defender);
			damage *= notVeryEffectiveMult ("Bug", defender);
			damage *= notVeryEffectiveMult ("Dragon", defender);
		} else if (atkType == "Ice") {
			damage *= superEffectiveMult ("Grass", defender);
			damage *= superEffectiveMult ("Ground", defender);
			damage *= superEffectiveMult ("Flying", defender);
			damage *= superEffectiveMult ("Dragon", defender);
			damage *= notVeryEffectiveMult ("Water", defender);
			damage *= notVeryEffectiveMult ("Ice", defender);
		} else if (atkType == "Fighting") {
			damage *= superEffectiveMult ("Normal", defender);
			damage *= superEffectiveMult ("Ice", defender);
			damage *= superEffectiveMult ("Rock", defender);
			damage *= notVeryEffectiveMult ("Poison", defender);
			damage *= notVeryEffectiveMult ("Flying", defender);
			damage *= notVeryEffectiveMult ("Psychic", defender);
			damage *= notVeryEffectiveMult ("Bug", defender);
			damage *= notEffectiveMult ("Ghost", defender);
		} else if (atkType == "Poison") {
			damage *= superEffectiveMult ("Grass", defender);
			damage *= superEffectiveMult ("Bug", defender);
			damage *= notVeryEffectiveMult ("Poison", defender);
			damage *= notVeryEffectiveMult ("Ground", defender);
			damage *= notVeryEffectiveMult ("Rock", defender);
			damage *= notVeryEffectiveMult ("Ghost", defender);
		} else if (atkType == "Ground") {
			damage *= superEffectiveMult ("Fire", defender);
			damage *= superEffectiveMult ("Electric", defender);
			damage *= superEffectiveMult ("Poison", defender);
			damage *= superEffectiveMult ("Rock", defender);
			damage *= notVeryEffectiveMult ("Grass", defender);
			damage *= notVeryEffectiveMult ("Bug", defender);
			damage *= notEffectiveMult ("Flying", defender);
		} else if (atkType == "Flying") {
			damage *= superEffectiveMult ("Grass", defender);
			damage *= superEffectiveMult ("Fighting", defender);
			damage *= superEffectiveMult ("Bug", defender);
			damage *= notVeryEffectiveMult ("Electric", defender);
			damage *= notVeryEffectiveMult ("Rock", defender);
		} else if (atkType == "Psychic") {
			damage *= superEffectiveMult ("Fighting", defender);
			damage *= superEffectiveMult ("Poison", defender);
			damage *= notVeryEffectiveMult ("Psychic", defender);
		} else if (atkType == "Bug") {
			damage *= superEffectiveMult ("Grass", defender);
			damage *= superEffectiveMult ("Psychic", defender);
			damage *= superEffectiveMult ("Poison", defender);
			damage *= notVeryEffectiveMult ("Fire", defender);
			damage *= notVeryEffectiveMult ("Fighting", defender);
			damage *= notVeryEffectiveMult ("Flying", defender);
		} else if (atkType == "Rock") {
			damage *= superEffectiveMult ("Fire", defender);
			damage *= superEffectiveMult ("Ice", defender);
			damage *= superEffectiveMult ("Flying", defender);
			damage *= superEffectiveMult ("Bug", defender);
			damage *= notVeryEffectiveMult ("Fighting", defender);
			damage *= notVeryEffectiveMult ("Ground", defender);
		} else if (atkType == "Rock") {
			damage *= superEffectiveMult ("Ghost", defender);
			damage *= notEffectiveMult ("Normal", defender);
			damage *= notEffectiveMult ("Psychic", defender);	
		} else if (atkType == "Dragon") {
			damage *= superEffectiveMult ("Dragon", defender);
		} else
			print ("Type not recognized");
		return damage;
	}

	private void printEffectiveness (int whatNext) {
		//whatNext = 0 if you need to check if the Enemy has fainted. If not, enemy can attack
		//whatNext = 1 if you need to check if the Player has fainted. If not, player can attack
		//whatNext = 2 if you need to check if the Enemy has fainted and has already attacked
		//whatNext = 3 if you need to check if the Player has fainted and has already attacked
		if (superEffective || notVeryEffective || notEffective) {
			if (notEffective) {
				BattleText.S.printToBattleText ("It is not effective");
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
					superEffective = notVeryEffective = notEffective = checkEffectiveness = false;
					if (whatNext == 0)
						checkEnemyFaintedWentSecond ();
					if (whatNext == 1)
						checkPlayerFaintedWentSecond ();
					if (whatNext == 2)
						checkEnemyFaintedWentFirst ();
					if (whatNext == 3)
						checkPlayerFaintedWentFirst ();
				}
			} else if (notVeryEffective && !superEffective) {
				BattleText.S.printToBattleText ("It is not very effective");
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
					superEffective = notVeryEffective = notEffective = checkEffectiveness = false;
					if (whatNext == 0)
						checkEnemyFaintedWentSecond ();
					if (whatNext == 1)
						checkPlayerFaintedWentSecond ();
					if (whatNext == 2)
						checkEnemyFaintedWentFirst ();
					if (whatNext == 3)
						checkPlayerFaintedWentFirst ();
				}
			} else if (!notVeryEffective && superEffective) {
				BattleText.S.printToBattleText ("It is super effective!");
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) {
					superEffective = notVeryEffective = notEffective = checkEffectiveness = false;
					if (whatNext == 0)
						checkEnemyFaintedWentSecond ();
					if (whatNext == 1)
						checkPlayerFaintedWentSecond ();
					if (whatNext == 2)
						checkEnemyFaintedWentFirst ();
					if (whatNext == 3)
						checkPlayerFaintedWentFirst ();
				}
			} else {
				checkEffectiveness = false;
				if(whatNext == 0) checkEnemyFaintedWentSecond();
				if(whatNext == 1) checkPlayerFaintedWentSecond();
				if(whatNext == 2) checkEnemyFaintedWentFirst();
				if(whatNext == 3) checkPlayerFaintedWentFirst();
			}
		} else {
			checkEffectiveness = false;
			if(whatNext == 0) checkEnemyFaintedWentSecond();
			if(whatNext == 1) checkPlayerFaintedWentSecond();
			if(whatNext == 2) checkEnemyFaintedWentFirst();
			if(whatNext == 3) checkPlayerFaintedWentFirst();
		}
	}

	void checkPlayerFaintedWentSecond() {
		if (playerPokemon.HealthCurrent <= 0) {
			playerFainted = true;
		} else {
			playerTurn = true;
			playerDoesDamage = true;
		}
	}

	void checkPlayerFaintedWentFirst() {
		if (playerPokemon.HealthCurrent <= 0)
			playerFainted = true;
		else 
			OptionBox.S.gameObject.SetActive (true);
	}

	void checkEnemyFaintedWentSecond() {
		if (enemyPokemon.HealthCurrent <= 0) {
			if(Main.S.CustomLevel)
				askForKillingBlow = true;
			else 
				enemyFainted = true;
		} else {
			enemyTurn = true;
			enemyDoesDamage = true;
		}
	}

	void checkEnemyFaintedWentFirst() {
		if (enemyPokemon.HealthCurrent <= 0) {
			if(Main.S.CustomLevel)
				askForKillingBlow = true;
			else
				enemyFainted = true;
		} else 
			OptionBox.S.gameObject.SetActive (true);
	}

	void statChangeFunc(Attack atk, Pokemon target) {
		string stat = atk.StatChange;
		if (stat == "None") {
			return;
		} else if (stat == "Attack") {
			if (atk.StatChangeAmount == 1)
				target.AttackStatMult *= 1.2f;
			else if (atk.StatChangeAmount == 2)
				target.AttackStatMult *= 1.2f * 1.2f;
			else if (atk.StatChangeAmount == -1)
				target.AttackStatMult /= 1.2f;
			else if (atk.StatChangeAmount == -2)
				target.AttackStatMult /= (1.2f * 1.2f);
			target.AttackStat *= target.AttackStatMult;
		} else if (stat == "Defense") {
			if (atk.StatChangeAmount == 1) 
				target.DefenseStatMult *= 1.2f;
			else if (atk.StatChangeAmount == 2)
				target.DefenseStatMult *= 1.2f * 1.2f;
			else if (atk.StatChangeAmount == -1)
				target.DefenseStatMult /= 1.2f;
			else if (atk.StatChangeAmount == -2)
				target.DefenseStatMult /= (1.2f * 1.2f);
			target.DefenseStat *= target.DefenseStatMult;
		} else if (stat == "Special") {
			if (atk.StatChangeAmount == 1) 
				target.SpecialStatMult *= 1.2f;
			else if (atk.StatChangeAmount == 2)
				target.SpecialStatMult *= 1.2f * 1.2f;
			else if (atk.StatChangeAmount == -1)
				target.SpecialStatMult /= 1.2f;
			else if (atk.StatChangeAmount == -2)
				target.SpecialStatMult /= (1.2f * 1.2f);
			target.SpecialStat *= target.SpecialStatMult;
		} else if (stat == "Speed") {
			if (atk.StatChangeAmount == 1) 
				target.SpeedStatMult *= 1.2f;
			else if (atk.StatChangeAmount == 2)
				target.SpeedStatMult *= 1.2f * 1.2f;
			else if (atk.StatChangeAmount == -1)
				target.SpeedStatMult /= 1.2f;
			else if (atk.StatChangeAmount == -2)
				target.SpeedStatMult /= (1.2f * 1.2f);
			target.SpeedStat *= target.SpeedStatMult;
		}

	}

}
