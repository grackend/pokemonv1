using UnityEngine;
using System.Collections;

public class AirBall : MonoBehaviour {
	
	Rigidbody rb;
	bool suckBalls, blowBalls;
	Vector3 moveToward;

	void Awake() {
		rb = this.transform.GetComponent<Rigidbody> ();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine (wreck ());

	}
	
	// Update is called once per frame
	void Update () {
		//don't make it a force, make it a velocity

		rb.AddForce((moveToward - transform.position) * Time.deltaTime);
		//print ((moveToward - transform.position) * Time.deltaTime);
		//transform.RotateAround (player.transform.position, player.transform.forward, 500 * Time.deltaTime);
	}

	void OnTriggerExit(Collider coll) {
		if (coll.gameObject.tag == "SuckCone"/* || coll.gameObject.tag == "BlowCone"*/) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter(Collision coll) {
		if ((coll.gameObject.tag == "Player" || coll.gameObject.tag == "AirBallDestructor") && suckBalls) {
			Destroy (this.gameObject);
		}                                
	}

	IEnumerator wreck() {
		yield return new WaitForSeconds (1);
		Destroy (this.gameObject);
	}

	public void setSucking(bool b) {
		suckBalls = b;
		blowBalls = !b;
	}

	public void setMoveToward(Vector3 move) {
		moveToward = move;
	}


}
