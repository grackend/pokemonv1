using UnityEngine;
using System.Collections;

public class PlayerMenuName : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GUIText gui = GetComponent<GUIText> ();
		gui.text = Player.S.Name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
