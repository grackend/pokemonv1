using UnityEngine;
using System.Collections;

public class PokemonStats : MonoBehaviour {

	public static PokemonStats S;

	public Pokemon			poke;
	public bool				updt;
	public GameObject		nam, atk1, atk2, atk3, atk4, atk, def, spl, spd, exp;

	// Use this for initialization
	void Awake() {
		S = this;
	}

	void Start () {
		//updatePokemonStats ();
		gameObject.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {
		if (updt) {
			updt = false;
			updatePokemonStats ();
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			PartyOptions.S.viewStats = false;
			gameObject.SetActive(false);
		}
	}


	void updatePokemonStats() {
		poke = Party.S.party [Party.S.activeItem];
		nam = this.transform.Find ("NameAndLevel").gameObject;
		atk1 = this.transform.Find ("Attack1").gameObject;
		atk2 = this.transform.Find ("Attack2").gameObject;
		atk3 = this.transform.Find ("Attack3").gameObject;
		atk4 = this.transform.Find ("Attack4").gameObject;
		atk = this.transform.Find ("Attack").gameObject;
		def = this.transform.Find ("Defense").gameObject;
		spl = this.transform.Find ("Special").gameObject;
		spd = this.transform.Find ("Speed").gameObject;
		exp = this.transform.Find ("EXPtoNext").gameObject;
		
		GUIText gui = nam.GetComponent<GUIText> ();
		gui.text = poke.Name + " lvl " + poke.Level;
		
		gui = atk1.GetComponent<GUIText> ();
		gui.text = poke.Attacks [0].AttackName + " " + poke.Attacks [0].AttackType + " " + 
			poke.Attacks [0].AttackPPRemaining + "/" + poke.Attacks [0].AttackPP;
		
		gui = atk2.GetComponent<GUIText> ();
		if (poke.Attacks [1].AttackName == "-") {
			gui.text = "-";
		} else {
			gui.text = poke.Attacks [1].AttackName + " " + poke.Attacks [1].AttackType + " " + 
				poke.Attacks [1].AttackPPRemaining + "/" + poke.Attacks [1].AttackPP;
		}


		gui = atk3.GetComponent<GUIText> ();
		if (poke.Attacks [2].AttackName == "-") {
			gui.text = "-";
		} else {
			gui.text = poke.Attacks [2].AttackName + " " + poke.Attacks [2].AttackType + " " + 
				poke.Attacks [2].AttackPPRemaining + "/" + poke.Attacks [2].AttackPP;
		}
		
		gui = atk4.GetComponent<GUIText> ();
		if (poke.Attacks [3].AttackName == "-") {
			gui.text = "-";
		} else {
			gui.text = poke.Attacks [3].AttackName + " " + poke.Attacks [3].AttackType + " " + 
				poke.Attacks [3].AttackPPRemaining + "/" + poke.Attacks [3].AttackPP;
		}
		
		gui = atk.GetComponent<GUIText> ();
		gui.text = "Attack: " + poke.AttackStat;
		
		gui = def.GetComponent<GUIText> ();
		gui.text = "Defense: " + poke.DefenseStat;
		
		gui = spl.GetComponent<GUIText> ();
		gui.text = "Special: " + poke.SpecialStat;
		
		gui = spd.GetComponent<GUIText> ();
		gui.text = "Speed: " + poke.SpeedStat;
		
		gui = exp.GetComponent<GUIText> ();
		gui.text = "Experience to next level: " + (poke.ExperienceToNext - poke.ExperienceCurrent);
	}

}
