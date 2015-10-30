using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction {
	down, left, up, right
}

public class EECSGirl: MonoBehaviour {
	
	public static EECSGirl S;
	//Animator animator;
	
	public string 	Name;
	public float	moveSpeed;
	public int		tileSize;
	
	public Sprite	upSprite;
	public Sprite	downSprite;
	public Sprite	leftSprite;
	public Sprite	rightSprite;
	
	public SpriteRenderer 	sprend;

	public bool 	________________;
	
	public RaycastHit 	hitInfo;
	
	public Vector3		targetPos;
	public Direction	direction;
	public Vector3		moveVec;

	void Awake(){
		S = this;
	}
	
	
	void Start() {
		sprend = gameObject.GetComponent<SpriteRenderer> ();
		//animator = GetComponent<Animator> ();
		
	}
	
	new public Rigidbody rigidbody {
		get { return gameObject.GetComponent<Rigidbody> ();}
	}
	
	public Vector3 pos {
		get { return transform.position;}
		set { transform.position = value;}
	}
	
	void FixedUpdate() {

		if (Input.GetKeyDown (KeyCode.A)) {
			// CheckForAction ();
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			moveVec = Vector3.right;
			direction = Direction.right;
			sprend.sprite = rightSprite;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			moveVec = Vector3.left;
			direction = Direction.left;
			sprend.sprite = leftSprite;
		} else if (Input.GetKey (KeyCode.UpArrow)) {
			moveVec = Vector3.up;
			direction = Direction.up;
			sprend.sprite = upSprite;
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			moveVec = Vector3.down;
			direction = Direction.down;
			sprend.sprite = downSprite;
		} else {
			moveVec = Vector3.zero;
		}

		targetPos = pos + moveVec;

		if ((targetPos - pos).magnitude < moveSpeed * Time.fixedDeltaTime) {
			pos = targetPos;
		} else {
			pos += (targetPos - pos).normalized * moveSpeed * Time.fixedDeltaTime;
		}
	}

	/*
	public void CheckForAction() {
		if (Physics.Raycast (GetRay (), out hitInfo, 1f, GetLayerMask (new string[] {"NPC"}))) {
			NPC npc = hitInfo.collider.gameObject.GetComponent<NPC>();
			npc.FacePlayer(direction);
			if(!npc.trainer || npc.fought)
				npc.PlayDialogue();
		}
		if (Physics.Raycast (GetRay (), out hitInfo, 1f, GetLayerMask (new string[] {"Item"}))) {
			GameObject itm = hitInfo.collider.gameObject.GetComponentInChildren<Item>().gameObject;
			Item item = Instantiate(itm.GetComponent<Item>());
			addToBag(item);
			Destroy (hitInfo.collider.gameObject);
			Dialog.S.gameObject.SetActive (true);
			Color noAlpha = GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color;
			noAlpha.a = 255;
			GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color = noAlpha;
			Dialog.S.ShowMessage("You found a " + item.itemName + "!");
		}
		if (Physics.Raycast (GetRay (), out hitInfo, 1f, GetLayerMask (new string[] {"Pokemon"})) && !Main.S.gotPokeFromOak) {
			GameObject charm = hitInfo.collider.gameObject.GetComponentInChildren<Pokemon>().gameObject;
			Pokemon charmander = Instantiate(charm.GetComponent<Pokemon>());
			party[0] = charmander;
			Destroy (hitInfo.collider.gameObject);
			Main.S.gotPokeFromOak = true;
		}
	}
	*/

}
