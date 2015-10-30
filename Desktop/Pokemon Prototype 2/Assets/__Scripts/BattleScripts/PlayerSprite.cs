using UnityEngine;
using System.Collections;

public class PlayerSprite : MonoBehaviour {

	public SpriteRenderer			sprit;
	public GUITexture				gui;
	Battle3 						b;
	
	// Use this for initialization
	void Start () {
		sprit = gameObject.GetComponent<SpriteRenderer> ();
		gui = gameObject.GetComponent<GUITexture> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		//sprit.sprite = b.EnemyPokemon.BackSprite;
		//print (Battle2.S.PlayerPokemon.Attacks [0].AttackName);
		//sprit.sprite = BattleMain.S.currentPokemon.BackSprite;
		gui.texture = BattleMain.S.currentPokemon.GetComponent<Texture> ();

		
	}
}
