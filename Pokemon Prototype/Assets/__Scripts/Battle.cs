using UnityEngine;
using System.Collections;

public class Battle : MonoBehaviour {

	public static Battle S;

	public Pokemon 		PlayersPokemon;
	public Pokemon		EnemyPokemon;
	public Attack		PlayerAttack;

	public bool			ChoseToAttack = false;
	public bool 		ChoseToRun = false;

	void awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
		//gameObject.SetActive (false);
		BattleOptions.S.gameObject.SetActive (true);
		//PlayersPokemon = Player.S.Party[0]
	}
	
	// Update is called once per frame
	void Update () {
		if (ChoseToAttack) {
			PlayerAttack = FIGHTscreen.S.ChosenAttack;
			ChoseToAttack = false;

			if(PlayersPokemon.SpeedStat > EnemyPokemon.SpeedStat) {
				int dmg = Damage (PlayersPokemon, EnemyPokemon, PlayerAttack);
				EnemyPokemon.HealthCurrent -= dmg;
				if(EnemyPokemon.HealthCurrent > 0) {
					Attack att = EnemyAI (EnemyPokemon);
					dmg = Damage(EnemyPokemon, PlayersPokemon, att);
					PlayersPokemon.HealthCurrent -= dmg;
				}
			} else {
				Attack att = EnemyAI (EnemyPokemon);
				int dmg = Damage(EnemyPokemon, PlayersPokemon, att);
				PlayersPokemon.HealthCurrent -= dmg;
				if(PlayersPokemon.HealthCurrent > 0) {
					dmg = Damage(PlayersPokemon, EnemyPokemon, PlayerAttack);
					EnemyPokemon.HealthCurrent -= dmg;
				}
			}
			print (PlayersPokemon.Name + " HP: " + PlayersPokemon.HealthCurrent + "/" + PlayersPokemon.HealthFull);
			print (EnemyPokemon.Name + " HP: " + EnemyPokemon.HealthCurrent + "/" + EnemyPokemon.HealthFull);
		} else if (ChoseToRun) {
			//say Ran Away
			ChoseToRun = false;
			gameObject.SetActive(false);
		}
	
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

}
