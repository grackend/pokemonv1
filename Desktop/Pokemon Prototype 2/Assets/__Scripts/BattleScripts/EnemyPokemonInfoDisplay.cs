using UnityEngine;
using System.Collections;

public class EnemyPokemonInfoDisplay : MonoBehaviour {

	public static EnemyPokemonInfoDisplay S;

	public Pokemon 		poke;
	private GUIText 	Text;



	void Awake() {
		S = this;
	}

	void Start () {
		GameObject go = GameObject.Find ("EnemyPokemonInfoText");
		Text = go.GetComponent<GUIText> ();
	}
		// Update is called once per frame
	void Update () {

	}

	public void updateUI() {
		poke = BattleMain.S.enemy;
		if (poke.HealthCurrent < 0)
			poke.HealthCurrent = 0;
		if (poke.HealthCurrent > poke.HealthFull)
			poke.HealthCurrent = poke.HealthFull;
		Text.text = poke.Name + " Lv " + poke.Level + "\n";
		int i = 0;
		while (i < poke.HealthCurrent) {
			Text.text += "|";
			++i;
		}
		while (i < poke.HealthFull) {
			Text.text += ".";
			++i;
		}
	}
}
