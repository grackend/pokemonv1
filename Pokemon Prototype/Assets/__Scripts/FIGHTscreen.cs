using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class FIGHTscreen : MonoBehaviour {

	public static FIGHTscreen S;

	public int 				activeAttack;
	public List<GameObject> AttackList; //controls GUIText for name of attacks
	public Attack[]			AttackArray; //references actual attacks

	public Attack			ChosenAttack;
	
	void Awake() {

		S = this;
		AttackArray = new Attack[4];
	}
	

	// Use this for initialization
	void Start () {

		Battle Parent = transform.parent.transform.parent.transform.parent.GetComponent<Battle> ();
		Pokemon playersPokemon = Parent.PlayersPokemon;
		int i = 0;
		foreach (Attack att in playersPokemon.Attacks) {
			if(i < playersPokemon.Attacks.Length) {
				AttackArray[i] = att;
				++i;
			}
		}

		foreach (Transform child in transform) {
			AttackList.Add (child.gameObject);
		}

		i = 0;
		foreach (GameObject go in AttackList) {
			if (i < playersPokemon.Attacks.Length) {
				GUIText AttackText = go.GetComponent<GUIText>();
				AttackText.text = AttackArray[i].AttackName;
				++i;
			}
		}

		bool first = true;
		activeAttack = 0;

		foreach (GameObject go in AttackList) {
			GUIText AttackText = go.GetComponent<GUIText>();
			if(first) AttackText.color = Color.magenta;
			first = false;
		}
		gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			print (AttackArray [activeAttack].AttackName);
			Battle b = transform.parent.transform.parent.transform.parent.GetComponent<Battle>();
			b.ChoseToAttack = true;
			ChosenAttack = AttackArray [activeAttack];
			closeAttackMenus();
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			MoveDownMenu ();
		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			MoveUpMenu();
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			closeAttackMenus();
		}
	}

	void MoveDownMenu() {
		AttackList [activeAttack].GetComponent<GUIText> ().color = Color.black;
		if (activeAttack + 1 == 4 || AttackArray [activeAttack + 1].AttackName == "-")
			activeAttack = 0;
		else
			activeAttack++;
		AttackList [activeAttack].GetComponent<GUIText> ().color = Color.magenta;
	}
	
	void MoveUpMenu() {
		AttackList [activeAttack].GetComponent<GUIText> ().color = Color.black;
		if (activeAttack > 0)
			--activeAttack;
		else {
			activeAttack = 3;
			while(AttackArray[activeAttack].AttackName == "-") {
				--activeAttack;
			}
		}
		AttackList [activeAttack].GetComponent<GUIText> ().color = Color.magenta;
	}

	void closeAttackMenus() {
		Main.S.selectingAttack = false;
		AttackInfoScreen.S.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}
}