using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Vector3 doorPos;

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Player") {
			print ("Entered door. Move to (" + doorPos.x + ", " + doorPos.y + ")");
			Player.S.MoveThroughDoor(doorPos);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
