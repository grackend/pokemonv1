using UnityEngine;
using System.Collections;

public class ShadowMarker : MonoBehaviour {

	public float shotDistance;
	public float shadowSpeed;
	Vector3 startPos;
	Vector3 direction;

	// Use this for initialization
	void Start () {
		print (transform.position);
		print (startPos);

		
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (transform.position, startPos) < shotDistance) {
			transform.position += direction * shadowSpeed * Time.deltaTime;
		}
	}

	public void setPosition(Vector3 pos) {
		startPos = pos;
	}

	public void setDirection(Vector3 dir) {
		direction = dir;
	}
}
