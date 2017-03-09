using UnityEngine;
using System.Collections;

public class BossZone : MonoBehaviour {

	public GameObject BossPrefab;
	private Rigidbody2D RB2D;
	public GameObject MainCamera;

	private void Start () {
		RB2D = GetComponent<Rigidbody2D> ();

	}

	void BossZoneEnter () {
		Instantiate (BossPrefab, new Vector3 (105, 7, 0), transform.rotation);
	}
}
