using UnityEngine;
using System.Collections;

public class Pokemon : MonoBehaviour {

	public string		Name;
	public int			Level;

	public Sprite 		FrontSprite;
	public Sprite		BackSprite;
	public Texture		textur;
	public string		Type1;
	public string 		Type2;

	public Attack[]		PrefabAttacks;
	public Attack[]		Attacks;

	public int			HealthFull;
	public int			HealthCurrent;
	public float		AttackStat;
	public float		DefenseStat;
	public float		SpecialStat;
	public float		SpeedStat;

	public float				AttackStatMult = 1;
	public float				DefenseStatMult = 1;
	public float				SpecialStatMult = 1;
	public float				SpeedStatMult = 1;


	public string		Status;
	public int 			ExperienceCurrent;
	public int			ExperienceToNext;
	public int			ExperienceDrop;
	public int			MoneyDrop;
	public int			CatchRate;


	void Awake() {
		Attacks = new Attack[4];
		int i = 0;
		foreach (Attack atk in PrefabAttacks) {
			Attacks[i] = Instantiate(atk);
			++i;
		}
	}

	// Use this for initialization
	void Start () {
		//ExperienceString = Name + " grew to level " + (Level + 1) + "!";
	}
	
	// Update is called once per frame
	void Update () {

		if (ExperienceCurrent >= ExperienceToNext) {
			/*Level++;
			ExperienceCurrent -= ExperienceToNext;
			ExperienceToNext = Level * Level;
			//ExperienceString = Name + " grew to level " + (Level + 1) + "!";*/
		}
		if (HealthCurrent < 0)
			HealthCurrent = 0;
		if (HealthCurrent > HealthFull)
			HealthCurrent = HealthFull;
	}

	public void resetStats() {
		AttackStatMult = 1;
		DefenseStatMult = 1;
		SpecialStatMult = 1;
		SpeedStatMult = 1;
	}
}
