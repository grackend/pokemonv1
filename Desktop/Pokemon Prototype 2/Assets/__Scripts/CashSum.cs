using UnityEngine;
using System.Collections;

public class CashSum : MonoBehaviour {

	GUIText gui;

	void Start () {
		gui = this.GetComponent<GUIText> ();
	}
	
	// Update is called once per frame
	void Update () {
		gui.text = "Money: $" + Player.S.wallet;
	}
}
