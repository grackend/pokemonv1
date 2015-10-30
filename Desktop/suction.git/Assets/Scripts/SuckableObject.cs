using UnityEngine;
using System.Collections;

public class SuckableObject : MonoBehaviour {

	private Rigidbody	rb;

	void Awake() {
		rb = transform.GetComponent<Rigidbody> ();
	}



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		            
	}

	public void ObjectLocation(Vector3 vec) {
		transform.position = vec;
	}

	public void stopRegularMovement() { //when being held
		Rigidbody rb = this.transform.GetComponent<Rigidbody> ();
		rb.velocity = Vector3.zero; //stop all movement
		rb.angularVelocity = Vector3.zero; //stop ALL movement
		rb.useGravity = false; //so it can be carried freely
		rb.constraints = RigidbodyConstraints.FreezeAll;  //no rotation or strange movement
		transform.rotation = Quaternion.identity;
		Renderer rend = gameObject.GetComponent<Renderer> ();
		//rend.material.color = new Vector4 (rend.material.color.r, rend.material.color.g, rend.material.color.b, .5f);
	}

	public void startRegularMovement() { //when released
		Rigidbody rb = this.transform.GetComponent<Rigidbody> ();
		rb.useGravity = true; //gravity again
		rb.constraints = RigidbodyConstraints.None;  //can move again

	}

	void OnTriggerEnter (Collider coll) {
		Player.S.addToObjectList (this.gameObject);
	}

	void OnTriggerExit(Collider coll) {
		Player.S.removeFromObjectList (this.gameObject);
	}

	/*
	public void makeTransparent() {
		Renderer rend = this.transform.GetComponent<Renderer> ();
		rend.material.color.a = -5f.
	}

	public void makeOpaque() {
		Renderer rend = this.transform.GetComponent<Renderer> ();
		rend.material.color = new Vector4 (rend.material.color.r, rend.material.color.g, rend.material.color.b, 1f);
	}*/

}
