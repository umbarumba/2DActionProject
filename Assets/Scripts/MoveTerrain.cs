using UnityEngine;
using System.Collections;

public class MoveTerrain : MonoBehaviour {

	Rigidbody2D RB2D;

	private float t, total;
	public float moveSpeed = 0.2f;

	private const string MAIN_CAMERA_TAG_NAME = "MainCamera";

	private bool _isRendered = false;

	void Start () {
		RB2D = GetComponent<Rigidbody2D> ();
		total = 0f;
	}

	// Update is called once per frame
	void Update () {
		//Sin関数で上下させる
		if (_isRendered) {
			t = Time.deltaTime;
			total += t;
			//ゲーム開始からの時間で-1~1の数値の波にmovespeedをかけた分だけ動かす
			RB2D.velocity = new Vector2 (0, moveSpeed * Mathf.Sin(total));
		}
	}

	void OnWillRenderObject() {
		//メインカメラに映った時だけ_isRenderedをtrue
		if (Camera.current.tag == MAIN_CAMERA_TAG_NAME) {
			_isRendered = true;
		}
	}
}
