using UnityEngine;
using System.Collections;

public class AttackInfoScreen : MonoBehaviour {

	public static AttackInfoScreen S;

	private int 	ActiveAttack;
	private string 	CurrentAttackType;

	private int		CurrentAttackPPFull;
	private int		CurrentAttackPPRemaining;

	private GameObject AttackTypeText;
	private GameObject AttackPPText;

	void Awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		ActiveAttack = FIGHTscreen.S.activeAttack;
		CurrentAttackType = FIGHTscreen.S.AttackArray [ActiveAttack].AttackType;
		
		CurrentAttackPPFull = FIGHTscreen.S.AttackArray[ActiveAttack].AttackPP;
		CurrentAttackPPRemaining = FIGHTscreen.S.AttackArray[ActiveAttack].AttackPPRemaining;

		AttackTypeText = GameObject.Find ("AttackTypeText");
		AttackPPText = GameObject.Find ("AttackPPText");

		GUIText AttackTypeGUIText = AttackTypeText.GetComponent<GUIText>();
		GUIText AttackPPGUIText = AttackPPText.GetComponent<GUIText>();

		AttackTypeGUIText.text = "Type: " + CurrentAttackType;
		AttackPPGUIText.text = CurrentAttackPPRemaining + " / " + CurrentAttackPPFull;

	}
}
