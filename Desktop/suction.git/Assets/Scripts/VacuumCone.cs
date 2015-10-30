using UnityEngine;
using System.Collections;

public class VacuumCone : MonoBehaviour {

	public float		forwardGrowthRate = .02f;
	public float 		coneThinness = 4f;
	public float		coneFlatness = 1f;
	private float 		horizontalGrowthRate;
	public float		maxForward;
	public GameObject	airPrefab;
	public int			airBallSpawnEveryNFrame = 10;
	
	public bool			__________________;	
	
	public Mesh			mesh; //mesh of this cone
	public int			airBallSpawnCount = 0;

	void Awake() {
		mesh = transform.GetComponent<MeshFilter> ().mesh;
		horizontalGrowthRate = forwardGrowthRate / coneThinness;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale.y < maxForward) {
			transform.localScale = new Vector3(transform.localScale.x + (horizontalGrowthRate * 2), 
			                                   transform.localScale.y + horizontalGrowthRate / coneFlatness,
			                                   transform.localScale.z + forwardGrowthRate);
		}

		if (airBallSpawnCount % airBallSpawnEveryNFrame == 0) {
			airBallSpawnCount = 0;
			Vector3 conePos = transform.position;
			Vector3 coneDirection = transform.forward;
			Quaternion coneRotation = transform.rotation;
			float spawnDistance = transform.localScale.z * mesh.bounds.size.z;

			float radius = transform.localScale.x * mesh.bounds.size.x / 2;

			Vector3 spawnPos = conePos + coneDirection * spawnDistance;

			Vector3 pointOnCone = spawnPos + Random.insideUnitSphere * radius;
			if(gameObject.tag == "SuckCone") {
				GameObject air = Instantiate(airPrefab, pointOnCone, coneRotation) as GameObject;
				AirBall airB = air.transform.GetComponent<AirBall>();
				airB.setMoveToward(conePos);
				airB.setSucking(true);
			} else if (gameObject.tag == "BlowCone") {
				GameObject air = Instantiate(airPrefab, conePos, coneRotation) as GameObject;
				AirBall airB = air.transform.GetComponent<AirBall>();
				airB.setMoveToward(pointOnCone);
				airB.setSucking(false);
			}
		}
		++airBallSpawnCount;
	}

}
