using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour {

	public static NPC 	N;

	public bool			trainer = false;
	public string 		Name;
	public string 		speech;
	public string		preBattleSpeech;
	public int			moneyDrop;
	
	public float 		chanceToMove = .1f;
	public bool 		moving = false;

	public Sprite		upSprite;
	public Sprite		downSprite;
	public Sprite		rightSprite;
	public Sprite		leftSprite;
	public string		setDirection;

	public SpriteRenderer 	sprend;	

	public RaycastHit		hitInfo;
	public Direction		direction;
	public Vector3			moveVec = Vector3.zero;
	public Vector3			targetPos = Vector3.zero;
	public float			moveSpeed = 1.2f;
	public float			sight;
	public bool				fought = false;
	public bool				startBattle;
	public List<Pokemon>	prefabPokeList;
	

	// Use this for initialization
	void Start () {
		sprend = gameObject.GetComponent<SpriteRenderer> ();
		SetDirection ();
	}

	public void PlayDialogue() {
		Dialog.S.gameObject.SetActive (true);
		Color noAlpha = GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color;
		noAlpha.a = 255;
		GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color = noAlpha;
		if(trainer && !fought)
			Dialog.S.ShowMessage (preBattleSpeech);
		else
			Dialog.S.ShowMessage (speech);
	}
	
	public void FacePlayer(Direction playerDir) {
		switch(playerDir) {
		case Direction.down:
			sprend.sprite = upSprite;
			direction = Direction.up;
			break;
		case Direction.up:
			sprend.sprite = downSprite;
			direction = Direction.down;
			break;
		case Direction.right:
			sprend.sprite = leftSprite;
			direction = Direction.left;
			break;
		case Direction.left:
			sprend.sprite = rightSprite;
			direction = Direction.right;
			break;

		}
	}

	void SetDirection() {
		if (setDirection == "up") {
			sprend.sprite = upSprite;
			direction = Direction.up;
			moveVec = Vector3.up;
		} else if (setDirection == "down") {
			sprend.sprite = downSprite;
			direction = Direction.down;
			moveVec = Vector3.down;
		} else if (setDirection == "right") {
			sprend.sprite = rightSprite;
			direction = Direction.right;
			moveVec = Vector3.right;
		} else if (setDirection == "left") {
			sprend.sprite = leftSprite;
			direction = Direction.left;
			moveVec = Vector3.left;
		} else {
			sprend.sprite = leftSprite;
			direction = Direction.left;
			moveVec = Vector3.zero;
		}
	}

	public Vector3 pos {
		get { return transform.position;}
		set { transform.position = value;}
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

	bool SeePlayer(float sight) {
		if (Physics.Raycast (GetRay (), out hitInfo, sight, GetLayerMask (new string[] {"Player"}))) {
			Player.S.spottedByTrainer = true;
			return true;
		}
		return false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(this.tag == "MovingNPC"){
			if(!moving && !Main.S.inDialog){
				if(Random.value < chanceToMove){
					if (Mathf.Floor(this.transform.position.y) % 2 == 0){
						moveVec = Vector3.down;
						moving = true;
						sprend.sprite = downSprite;
					}else{
						moveVec = Vector3.up;
						moving = true;
						sprend.sprite = upSprite;
					}
					targetPos = pos + moveVec;
				}
			}else{
				if ((targetPos - pos).magnitude < moveSpeed * Time.fixedDeltaTime) {
					pos = targetPos;
					moving = false;
				} else {
					pos += (targetPos - pos).normalized * moveSpeed * Time.fixedDeltaTime;
				}
			}
		}
		if (trainer && !fought && !Main.S.inBattle) {
			targetPos = pos + moveVec;
			if (SeePlayer (sight) && !SeePlayer (.5f)) {
				Player.S.FaceNPC(direction);
				Player.S.spottedByTrainer = true;
				Player.S.NPCDir = direction;
				pos += (targetPos - pos).normalized * moveSpeed * Time.fixedDeltaTime;
			} else if (SeePlayer (.5f)) {
				if(!startBattle) {
					BattleMain.S.EnemyName = Name;
					startBattle = true;
				}
				ActivateTrainerBattle();
			}
		}
	}
	public void ActivateTrainerBattle() {
		PlayDialogue();
		if(Input.GetKeyDown(KeyCode.S))
		   startBattle = false;
		if(!startBattle) {
			BattleMain.S.enemyList.Clear();
			for(int i = 0; i < prefabPokeList.Count; ++i) {
				BattleMain.S.enemyList.Add(Instantiate(prefabPokeList[i]));
			}
			//BattleMain.S.EnemyName = Name;
			BattleMain.S.currentNPCPoke = 0;
			Main.S.OpenBattle (BattleMain.S.enemyList[0]);
			BattleMain.S.moneyDrop = moneyDrop;
			fought = true;
		}
	}
}
