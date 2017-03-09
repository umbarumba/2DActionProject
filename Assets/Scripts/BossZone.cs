using UnityEngine;
using System.Collections;

public class BossZone : MonoBehaviour {

    private Player PlayerScript;
	public GameObject BossPrefab;
    private Rigidbody2D RB2D;
	public GameObject MainCamera;

	private void Start () {
		RB2D = GetComponent<Rigidbody2D> ();
        PlayerScript = GameObject.FindGameObjectWithTag("UnityChan").GetComponent<Player>();
	}

    public void BossZoneEnter() {
        Debug.Log("Boss投下");
        Instantiate(BossPrefab, new Vector3(105, 7, 0), transform.rotation);
        GetComponent<BoxCollider2D>().enabled = false;
        PlayerScript.BossFinder();
    }
}
