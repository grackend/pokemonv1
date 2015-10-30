using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public static Player S;

	public float 		speed, turnSpeed, hangTime, jumpPower;
	public float		suckPower = 0; //will use this later
	public GameObject	conePrefab;
	private GameObject	cone;
	public ShadowMarker shadowMarkerPrefab;
	private ShadowMarker shadow;
	public bool			______________;
	public bool			onFire;
	private bool 		sucking, blowing;
	private bool 		carryingObject;
	private bool		settingLaunch;
	public  bool		canSuck = true, canBlow = false;
	private bool		onGround = true;
	private List<GameObject> objectList = new List<GameObject> ();
	private GameObject	objectBeingCarried;

	private Rigidbody rb;


	void Awake() {
		S = this;
	}
	// Use this for initialization
	void Start () {
		rb = transform.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel("_Scene_0");
		}
		if (Input.GetKeyDown (KeyCode.S) && !sucking && !blowing) {
			canSuck = !canSuck;
			canBlow = !canBlow;
			if(canSuck) changeToSuckColor();
			else changeToBlowColor();
		}
		if (Input.GetKey (KeyCode.RightArrow) && !settingLaunch) {
			transform.Rotate(new Vector3(0, turnSpeed, 0));
		}
		if (Input.GetKey (KeyCode.LeftArrow) && !settingLaunch) {
			transform.Rotate(new Vector3(0, -turnSpeed, 0));
		}
		if (Input.GetKey (KeyCode.UpArrow) && !settingLaunch) {
			transform.position += transform.forward * speed * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.DownArrow) && !settingLaunch) {
			transform.position -= transform.forward * speed * Time.deltaTime;
		}

		if (Input.GetKeyDown (KeyCode.Space) && carryingObject) { //only called when carrying an object
			carryingObject = false;
			objectBeingCarried.transform.parent = null;
			SuckableObject suck = objectBeingCarried.GetComponent<SuckableObject>();
			suck.startRegularMovement();
			Rigidbody rbo = objectBeingCarried.GetComponent<Rigidbody>();
			if (!settingLaunch) {
				//launch the object forward
				rbo.AddForce((transform.forward + new Vector3(0, .1f, 0)) * 1500);
				rb.AddForce((-transform.forward + new Vector3(0, .2f, 0)) * 400);
				suckPower = 0;
				//StartCoroutine(waitToSuck());
			}
			else {
				//lob the object
				settingLaunch = false;
				float velocityY = (0 - (-9.8f * hangTime / 2)); //Vi = Vf - (a * t)
				float velocityX = (shadow.transform.position.x - transform.position.x) / (hangTime);
				float velocityZ = (shadow.transform.position.z - transform.position.z) / (hangTime);
				rbo.velocity = new Vector3(velocityX, velocityY, velocityZ);
				Destroy(GameObject.Find ("ShadowMarker(Clone)"));
				rb.AddForce((-transform.forward + new Vector3(0, .2f, 0)) * 300);
				//StartCoroutine(waitToSuck());
			}
		}
		else if (Input.GetKeyDown (KeyCode.Space)) {
			if (canBlow || (canSuck && !carryingObject)) {
				Vector3 playerPos = transform.position;
				Vector3 playerDirection = transform.forward;
				Quaternion playerRotation = transform.rotation;
				float spawnDistance = transform.GetComponent<MeshFilter>().mesh.bounds.size.z / 2;

				Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
				
				cone = Instantiate(conePrefab, spawnPos, playerRotation) as GameObject;
				//sucking = true;

				if (canSuck) {
					cone.tag = "SuckCone";
					foreach(Transform t in cone.transform) t.tag = "SuckCone";
					sucking = true;
				} else if (canBlow) {
					cone.tag = "BlowCone";
					foreach(Transform t in cone.transform) t.tag = "BlowCone";
					blowing = true;
				}

				cone.transform.parent = transform;
			}
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			Destroy (cone.gameObject);
			objectList.Clear ();
			if(sucking) {
				sucking = false;
			} else if (blowing) {
				blowing = false;
			}              
		}

		if (Input.GetKeyDown (KeyCode.LeftShift) && carryingObject) {
			settingLaunch = true;
			shadow = Instantiate (shadowMarkerPrefab, objectBeingCarried.transform.position, Quaternion.identity) 
				as ShadowMarker;
			shadow.setDirection (transform.forward);
			shadow.setPosition (transform.position);
		} else if (Input.GetKeyDown (KeyCode.LeftShift) && onGround) {
			rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
			if (jumpPower > 0)
				onGround = false;
		}
		if (Input.GetKeyUp (KeyCode.LeftShift) && carryingObject) {
			settingLaunch = false;
			Destroy(GameObject.Find ("ShadowMarker(Clone)"));
		}

		if (sucking) {
			addForceToObjectsInList(objectList, 2f);
		} else if (blowing) {
			addForceToObjectsInList(objectList, -1f);
		}

	}
	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Ground") {
			onGround = true;
		} else if (coll.gameObject.tag == "Suckable" && canSuck) {
			for (int i = 0; i < objectList.Count; i += 1) {
				if (objectList[i] == coll.gameObject) {
					Destroy (cone.gameObject);
					SuckableObject suck = coll.transform.GetComponent<SuckableObject>();
					suck.stopRegularMovement();
					coll.transform.GetComponent<SuckableObject>().ObjectLocation(transform.position + 
						transform.forward + new Vector3(transform.forward.x * 1.2f, 0, transform.forward.z * 1.2f));
					coll.transform.parent = this.transform; //make player parent of the object
					//set parameters for the player
					rb.velocity = Vector3.zero; //remove all forces so no knockback
					rb.angularVelocity = Vector3.zero; //no knockback, again
					objectList.Clear (); //can only suck one thing at a time
					carryingObject = true;
					objectBeingCarried = coll.gameObject;
					break;
				}
			}
		}
	}
	IEnumerator waitToSuck() {
		canSuck = false;
		yield return new WaitForSeconds (.5f);
		canSuck = true;
	}
	public void addToObjectList(GameObject go) {
		objectList.Add (go);
	}
	public void removeFromObjectList (GameObject go) {
		objectList.Remove (go);
	}
	void addForceToObjectsInList(List<GameObject> list, float force) {
		for (int i = 0; i < list.Count; ++i) {
			//need to do something with suckPower here
			Rigidbody rbo = list [i].GetComponent<Rigidbody> ();
			rbo.AddForce ((transform.position - list [i].transform.position) * force); //2 is substitute for suckPower
		}
	}
	void changeToSuckColor() {
		Renderer rend = transform.GetComponent<Renderer> ();
		rend.material.color = Color.magenta;
	}
	void changeToBlowColor() {
		Renderer rend = transform.GetComponent<Renderer> ();
		rend.material.color = Color.cyan;
	}
	void changeToFireColor() {
		Renderer rend = transform.GetComponent<Renderer> ();
		rend.material.color = Color.black;
	}

	public IEnumerator toggleOnFire() {
 		if (!onFire) {
			onFire = true;
			changeToFireColor();
		}
		else {
			yield return new WaitForSeconds(3);
			onFire = false;
			if (canSuck) 
				changeToSuckColor ();
			else if (canBlow)
				changeToBlowColor ();
		}
	}
}
