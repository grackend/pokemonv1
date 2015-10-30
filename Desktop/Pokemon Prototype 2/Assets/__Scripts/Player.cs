using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction {
	down, left, up, right
}

public class Player : MonoBehaviour {

	public Sprite graveyard;
	public static Player S;
	Animator animator;

	public string 	Name;
	public float	moveSpeed;
	public int		tileSize;
	
	public Sprite	upSprite;
	public Sprite	downSprite;
	public Sprite	leftSprite;
	public Sprite	rightSprite;
	
	public SpriteRenderer 	sprend;
	public Direction		NPCDir;
	
	public bool 	________________;
	
	public RaycastHit 	hitInfo;
	
	public float chanceToFindPokemon = .1f;
	
	public bool			moving = false;
	public Vector3		targetPos;
	public Direction	direction;
	public Vector3		moveVec;
	
	public Pokemon[]	PrefabParty;
	public Pokemon[] 	party;
	public List<Item>	bag;
	public int			wallet;

	public bool 		checkedSpaceForPokemon = false;
	public Pokemon 		wildPokemon;

	public bool			spottedByTrainer = false;

	GameObject 			tileUnderPlayer;
	int					currentFloor = 0; //tracks what floor the players is on to spawn the right pokemon
	
	void Awake(){
		S = this;
		party = new Pokemon[6];
		int i = 0;
		while (i < 6) {
			party[i] = Instantiate(PrefabParty[i]);
			++i;
		}
		graveyard = Resources.Load<Sprite> ("graveyard");
	}


	void Start() {
		sprend = gameObject.GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();

	}
	
	new public Rigidbody rigidbody {
		get { return gameObject.GetComponent<Rigidbody> ();}
	}
	
	public Vector3 pos {
		get { return transform.position;}
		set { transform.position = value;}
	}

	public void FaceNPC(Direction NPCDir) {
		switch(NPCDir) {
		case Direction.down:
			sprend.sprite = upSprite;
			break;
		case Direction.up:
			sprend.sprite = downSprite;
			break;
		case Direction.right:
			sprend.sprite = leftSprite;
			break;
		case Direction.left:
			sprend.sprite = rightSprite;
			break;
		}
	}
	
	void FixedUpdate() {
		if (!Main.S.inBattle && 
		    !Main.S.viewingPokemon && 
		    !Main.S.viewingItems && 
		    !Main.S.paused) {
			if (Input.GetKeyDown (KeyCode.A)) {
				CheckForAction ();
			}

			if (!moving && !Main.S.inDialog && !spottedByTrainer) {
				if (Input.GetKey (KeyCode.RightArrow)) {
					moveVec = Vector3.right;
					direction = Direction.right;
					sprend.sprite = rightSprite;
					moving = true;
					checkedSpaceForPokemon = false;
					//animator.SetTrigger ("WalkRight");
				} else if (Input.GetKey (KeyCode.LeftArrow)) {
					moveVec = Vector3.left;
					direction = Direction.left;
					sprend.sprite = leftSprite;
					moving = true;
					checkedSpaceForPokemon = false;
					//animator.SetTrigger ("WalkLeft");
				} else if (Input.GetKey (KeyCode.UpArrow)) {
					moveVec = Vector3.up;
					direction = Direction.up;
					sprend.sprite = upSprite;
					moving = true;
					checkedSpaceForPokemon = false;
					//animator.SetTrigger ("WalkUp");
				} else if (Input.GetKey (KeyCode.DownArrow)) {
					moveVec = Vector3.down;
					direction = Direction.down;
					sprend.sprite = downSprite;
					moving = true;
					checkedSpaceForPokemon = false;
					//animator.SetTrigger ("WalkDown");
				} else {
					moveVec = Vector3.zero;
					moving = false;
				}
				
				if (Physics.Raycast (GetRay (), out hitInfo, 1f, GetLayerMask (new string[] {"Immovable", "NPC", "Item", "Pokemon"}))) {
					moveVec = Vector3.zero;
					moving = false;
					//print("found immovable");
				} else if (Physics.Raycast (GetRay (), out hitInfo, 1f, GetLayerMask (new string[] {"Ledge"}))) {
					// if there is a ledge
					if (moveVec == Vector3.down) {
						moveVec.y *= 2;
					} else {
						moveVec = Vector3.zero;
						moving = false;
					}
				}
				if (spottedByTrainer) {
					moving = false;
					FaceNPC (NPCDir);
				}

				targetPos = pos + moveVec;

				//chance to find pokemon
				int xPos = (int)pos.x;
				int yPos = (int)pos.y;
				string xString; string yString;
				if(xPos.ToString().Length == 1){
					xString = "00"+xPos.ToString();
				}else if(xPos.ToString().Length == 2){
					xString = "0"+xPos.ToString();
				}else{
					xString = xPos.ToString();
				}
				if(yPos.ToString().Length == 1){
					yString = "00"+yPos.ToString();
				}else if(yPos.ToString().Length == 2){
					yString = "0"+yPos.ToString();
				}else{
					yString = yPos.ToString();
				}
				string position = xString+'x'+yString;

				tileUnderPlayer = GameObject.Find(position);
				/*
				if (tileUnderPlayer.layer == 9) {
					if (!checkedSpaceForPokemon){
						float rndm = Random.value;
						
						if (rndm < chanceToFindPokemon){
							// randomly create an instance of one of three pokemon
							// assign it to a variable in 
							/*
							float x = Random.value;
							x *= 100;
							x = ((int)x) % 11;

							if(x % 3 == 0){
								//wildPokemon = Instantiate(SpearowPrefab);
							}else if(x % 3 == 1){
								//wildPokemon = Instantiate (PidgeyPrefab);
							}else{
								//wildPokemon = Instantiate (RattataPrefab);
							}*/
				/*
							float bottom = 0f, top = 14.999f;
							switch (currentFloor) {
							case 1: {
								bottom = 0f;
								top = 2.999f;
								break;
							}
							case 2: {
								bottom = 3f;
								top = 6.999f;
								break;
							}
							case 3: {
								bottom = 7f;
								top = 10.999f;
								break;
							}
							case 4: {
								bottom = 11f;
								top = 14.999f;
								break;
							}
							}
							Main.S.OpenBattle(Main.S.wild ((int)Random.Range (bottom, top)));
							
						}
						checkedSpaceForPokemon = true;
					}
				}
				*/
				
			} else {
				if ((targetPos - pos).magnitude < moveSpeed * Time.fixedDeltaTime) {
					pos = targetPos;
					moving = false;
				} else {
					pos += (targetPos - pos).normalized * moveSpeed * Time.fixedDeltaTime;
				}
			}
		}
	}
	
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
		//print ("thru door");
		currentFloor++;
	}

	public int partySize() {
		int size = 0;
		while (size < 6) {
			if(party[size].Name != "-")
				++size;
			else
				break;
		}
		return size;
	}

	void addToBag(Item itm) {
		for(int i = 0; i < Player.S.bag.Count; ++i) {
			if(bag[i].itemName == itm.itemName) {
				++bag[i].quantity;
				return;
			}
		}
		bag.Add (itm);
	}
}
