using UnityEngine;
using System.Collections;

public enum Direction {
	down, left, up, right
}

public class Player : MonoBehaviour {

	public static Player S;

	public float	moveSpeed;
	public int		tileSize;

	public Sprite	upSprite;
	public Sprite	downSprite;
	public Sprite	leftSprite;
	public Sprite	rightSprite;

	public SpriteRenderer 	sprend;

	public bool 	________________;

	public RaycastHit 	hitInfo;

	public float chanceToFindPokemon = .1f;

	public bool			moving = false;
	public Vector3		targetPos;
	public Direction	direction;
	public Vector3		moveVec;

	public Pokemon[]	Party;
	
	public bool checkedSpaceForPokemon = false;


	void Awake(){
		S = this;
		Party = new Pokemon[6];
	}

	void Start() {
		sprend = gameObject.GetComponent<SpriteRenderer> ();
	}

	new public Rigidbody rigidbody {
		get { return gameObject.GetComponent<Rigidbody> ();}
	}

	public Vector3 pos {
		get { return transform.position;}
		set { transform.position = value;}
	}

	void FixedUpdate() {

		if(Input.GetKeyDown (KeyCode.Z)) {
			CheckForAction();
		}

		if (!moving && !Main.S.inDialog && !Main.S.paused && !Main.S.inBattle) {
			if (Input.GetKey (KeyCode.RightArrow)) {
				moveVec = Vector3.right;
				direction = Direction.right;
				sprend.sprite = rightSprite;
				moving = true;
				checkedSpaceForPokemon = false;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				moveVec = Vector3.left;
				direction = Direction.left;
				sprend.sprite = leftSprite;
				moving = true;
				checkedSpaceForPokemon = false;
			} else if (Input.GetKey (KeyCode.UpArrow)) {
				moveVec = Vector3.up;
				direction = Direction.up;
				sprend.sprite = upSprite;
				moving = true;
				checkedSpaceForPokemon = false;
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				moveVec = Vector3.down;
				direction = Direction.down;
				sprend.sprite = downSprite;
				moving = true;
				checkedSpaceForPokemon = false;
			} else {
				moveVec = Vector3.zero;
				moving = false;
			}

			if(Physics.Raycast(GetRay(), out hitInfo, 1f, GetLayerMask(new string[] {"Immovable", "NPC"}))) {
				moveVec = Vector3.zero;
				moving = false;
				print("found immovable");
			}else if(Physics.Raycast(GetRay(), out hitInfo, 1f, GetLayerMask(new string[] {"Ledge"}))) {
				// if there is a ledge
				if (moveVec == Vector3.down) {
					moveVec.y *= 2;
				} else {
					moveVec = Vector3.zero;
					moving = false;
				}
			}

			//chance to find pokemon
			if (Physics.Raycast(GetRay(), out hitInfo, 1f, GetLayerMask(new string[] {"Grass", "NPC"}))) {
				if (!checkedSpaceForPokemon){
					if (Random.value < chanceToFindPokemon){
						print ("Found a pokemon");
					}
					checkedSpaceForPokemon = true;
				}
			}

			targetPos = pos + moveVec;

		} else {
			if ((targetPos - pos).magnitude < moveSpeed * Time.fixedDeltaTime) {
				pos = targetPos;
				moving = false;
			} else {
				pos += (targetPos - pos).normalized * moveSpeed * Time.fixedDeltaTime;
			}
		}
	}

	public void CheckForAction() {
		if (Physics.Raycast (GetRay (), out hitInfo, 1f, GetLayerMask (new string[] {"NPC"}))) {
			NPC npc = hitInfo.collider.gameObject.GetComponent<NPC>();
			npc.FacePlayer(direction);
			npc.PlayDialogue();
		}
		
	}
	
	Ray GetRay() {
		switch(direction) {
		case Direction.down:
			return new Ray(pos, Vector3.down);
		case Direction.up:
			return new Ray(pos, Vector3.up);
		case Direction.right:
			return new Ray(pos, Vector3.right);
		case Direction.left:
			return new Ray(pos, Vector3.left);
		default:
			return new Ray(pos, Vector3.down);
		}
	}

	int GetLayerMask(string[] layerNames) {
		int layerMask = 0;

		foreach (string layer in layerNames) {
			layerMask = layerMask | (1 << LayerMask.NameToLayer(layer));
		}

		return layerMask;
	}

	public void MoveThroughDoor(Vector3 doorLoc) {
		if (doorLoc.z <= 0) doorLoc.z = transform.position.z;
		moving = false;
		moveVec = Vector3.zero;
		transform.position = doorLoc;
		print ("thru door");
	}
}
