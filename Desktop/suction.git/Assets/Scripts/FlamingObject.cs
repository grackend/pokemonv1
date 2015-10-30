using UnityEngine;
using System.Collections;

public class FlamingObject : SuckableObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Player" && !Player.S.onFire) {
			StartCoroutine(Player.S.toggleOnFire());
		}
	}
}
