using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float 	speedToKill = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Suckable") {
			Rigidbody rb = coll.transform.GetComponent<Rigidbody>();
			if (rb.velocity.magnitude >= speedToKill)
				Destroy (this.gameObject);
		}
	}
}
