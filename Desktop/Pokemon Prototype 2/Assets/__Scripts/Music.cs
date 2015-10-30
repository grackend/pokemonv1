using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Music : MonoBehaviour {

	public static Music 		S;

	public List<GameObject>		songs;
	public AudioSource			townSong, battleSong, thunderstruck, lavender;

	void Awake () {
		S = this;
		foreach (Transform child in transform) {
			songs.Add(child.gameObject);
		}
		townSong = songs [0].GetComponent<AudioSource> ();
		battleSong = songs [1].GetComponent<AudioSource> ();
		thunderstruck = songs [2].GetComponent<AudioSource> ();
		lavender = songs [3].GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		thunderstruck.gameObject.SetActive (false);
		lavender.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Main.S.inBattle) {
			//lavender.gameObject.SetActive (false);
			townSong.gameObject.SetActive(false);
			battleSong.gameObject.SetActive (true);
		} else {
			battleSong.gameObject.SetActive(false);
			townSong.gameObject.SetActive(true);
		}
	
	}
}
