using UnityEngine;
using System.Collections;

public class PlayerMenuMoney : MonoBehaviour {

	GUIText gui;

	// Use this for initialization
	void Start () {
		gui = GetComponent<GUIText> ();
	}
	
	// Update is called once per frame
	void Update () {
		string cashmoney = (Player.S.wallet).ToString ();
		gui.text = "WALLET: $" + cashmoney;
	}
}
