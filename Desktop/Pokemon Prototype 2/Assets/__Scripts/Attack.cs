using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	public string 		AttackName;
	public string		AttackType;
	public int			AttackPower;
	public int			AttackPP;
	public int			AttackPPRemaining;
	public bool			IsPhysical;
	public int 			Accuracy;
	public string		StatusCondition; //"None" if it doesn't change status
	public int			ChanceOfStatusCondition;
	public string		StatChange; //"None" if it doesn't change stats
	public int			StatChangeChance;
	public int			StatChangeAmount;
	public bool			StatChangeSelf;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
