using UnityEngine;
using System.Collections;

public class AttackInfoBox : MonoBehaviour {
	
	public static AttackInfoBox S;
	
	private int 	CurrentAttack;
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
		CurrentAttack = AttackBox.S.currentAttack;
		CurrentAttackType = AttackBox.S.AtkArray [CurrentAttack].AttackType;
		
		CurrentAttackPPFull = AttackBox.S.AtkArray[CurrentAttack].AttackPP;
		CurrentAttackPPRemaining = AttackBox.S.AtkArray[CurrentAttack].AttackPPRemaining;
		
		AttackTypeText = GameObject.Find ("TypeText");
		AttackPPText = GameObject.Find ("PPText");
		
		GUIText AttackTypeGUIText = AttackTypeText.GetComponent<GUIText>();
		GUIText AttackPPGUIText = AttackPPText.GetComponent<GUIText>();
		
		AttackTypeGUIText.text = "Type: " + CurrentAttackType;
		AttackPPGUIText.text = CurrentAttackPPRemaining + " / " + CurrentAttackPPFull;
		
	}
}