using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {

	public static Dialog S;

	void Awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
		HideDialogBox ();
	}

	public void ShowMessage(string message) {
		Main.S.inDialog = true;
		GameObject dialogBox = transform.Find ("Text").gameObject;
		Text goText = dialogBox.GetComponent<Text> ();
		goText.text = message;
	}

	// Update is called once per frame
	void Update () {
		if (Main.S.inDialog && Input.GetKeyDown (KeyCode.S)) {
			HideDialogBox ();
		}
	}


	void HideDialogBox() {
		Color noAlpha = GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color;
		noAlpha.a = 0;
		GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color = noAlpha;
		gameObject.SetActive (false);
		Main.S.inDialog = false;
	}
}
