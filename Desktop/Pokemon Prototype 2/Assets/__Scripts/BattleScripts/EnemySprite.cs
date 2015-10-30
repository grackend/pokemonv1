using UnityEngine;
using System.Collections;

public class EnemySprite : MonoBehaviour {

	public SpriteRenderer			sprit;
	public GUITexture				gui;
	Battle3							b;

	// Use this for initialization
	void Start () {
		sprit = gameObject.GetComponent<SpriteRenderer> ();
		gui = gameObject.GetComponent<GUITexture> ();

	}
	
	// Update is called once per frame
	void Update () {
		//sprit.sprite = b.EnemyPokemon.BackSprite;
		//sprit.sprite = BattleMain.S.enemy.FrontSprite;
		gui.texture = BattleMain.S.currentPokemon.GetComponent<Texture> ();

	}
}
