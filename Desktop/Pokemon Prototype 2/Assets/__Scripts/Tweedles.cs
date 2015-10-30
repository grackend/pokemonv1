using UnityEngine;
using System.Collections;

public class Tweedles : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Main.S.gotPokeFromOak)
			Destroy (this.gameObject);
	}
}
