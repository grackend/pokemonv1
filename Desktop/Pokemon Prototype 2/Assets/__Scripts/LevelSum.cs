using UnityEngine;
using System.Collections;

public class LevelSum : MonoBehaviour {

	GUIText gui;

	// Use this for initialization
	void Start () {
		gui = this.GetComponent<GUIText> ();
	}
	
	// Update is called once per frame
	void Update () {
		int sum = 0;
		for (int i = 0; i < 6; ++i) {
			if (Player.S.party[i].Name != "-" && Player.S.party[i].HealthCurrent > 0)
				sum += Player.S.party[i].Level;
		}
		gui.text = "Level Sum: " + sum;
	}
}
