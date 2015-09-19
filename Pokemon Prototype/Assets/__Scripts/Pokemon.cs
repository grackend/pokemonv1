using UnityEngine;
using System.Collections;

public class Pokemon : MonoBehaviour {

	public string		Name;
	public int			Level;

	public Sprite 		FrontSprite;
	public Sprite		BackSprite;
	public string		Type1;
	public string 		Type2;

	public Attack[]		Attacks;
	public int			HealthFull;
	public int			HealthCurrent;
	public string		Status;
	public int 			ExperienceCurrent;
	public int			ExperienceToNext;

	public float			AttackStat;
	public float			DefenseStat;
	public float			SpecialStat;
	public float			SpeedStat;

	void Awake() {

		Attacks = new Attack[4];
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
