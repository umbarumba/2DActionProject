﻿using UnityEngine;
using System.Collections;

public class Enemy1 : MonoBehaviour {

	Rigidbody2D RB2D;
	public int speed = -3;

	public GameObject explosion;

	public int attakPoint = 10;

	private Life lifeScript;

	public GameObject item;
	//メインカメラのタグ名　constは定数
	private const string MAIN_CAMERA_TAG_NAME = "MainCamera";
	//カメラに映っているかの判定
	private bool _isRendered = false;

	// Use this for initialization
	void Start () {
		RB2D = GetComponent<Rigidbody2D> ();

		lifeScript = GameObject.FindGameObjectWithTag ("HP").GetComponent<Life> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_isRendered) {
			RB2D.velocity = new Vector2 (speed, RB2D.velocity.y);
		}

		if (gameObject.transform.position.y < Camera.main.transform.position.y - 8 ||
			gameObject.transform.position.x < Camera.main.transform.position.x - 10) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (_isRendered) {
			if (col.tag == "Bullet") {
				Destroy (gameObject);
				Instantiate (explosion, transform.position, transform.rotation);
				//四分の一の確率で回復アイテムを落とす
				if (Random.Range (0, 4) == 0) {
					Instantiate (item, transform.position, transform.rotation);
				}
			}
		}
	}

	//Rendererがカメラに映ってる間に呼ばれ続ける
	void OnWillRenderObject() {
		//メインカメラに映った時だけ_isRenderedをtrue
		if (Camera.current.tag == MAIN_CAMERA_TAG_NAME) {
			_isRendered = true;
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		//UnityChanとぶつかった時
		if (col.gameObject.tag == "UnityChan") {
			//LifeScriptのLifeDownメソッドを実行
			lifeScript.LifeDown (attakPoint);
		}
	}
}
