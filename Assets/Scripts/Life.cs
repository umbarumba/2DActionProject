﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Life : MonoBehaviour {

	RectTransform rt;

	public GameObject unityChan;//ユニティちゃん
	public GameObject explosion;//爆発アニメーション
	public Text gameOverText;//ゲームオーバーの文字
	private bool gameOver = false;//ゲームオーバー判定

	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform> ();
	}

	void Update () {
		//ライフが0以下になった時
		if (rt.sizeDelta.y <= 0) {
			//ゲームオーバー判定がfalseなら爆発アニメーションを生成
			//GameOverメソッドでtrueになるので、1回のみ実行
			if (gameOver == false) {
				Instantiate (explosion, unityChan.transform.position + new Vector3 (0, 1, 0), unityChan.transform.rotation);
			}
			//ゲームオーバー判定をtrueにし、ユニティちゃんを消去
			GameOver ();
		}
		//ゲームオーバー判定がtrueの時
		if (gameOver) {
			//ゲームオーバーの文字を表示
			gameOverText.enabled = true;
			//画面をクリックすると
			if (Input.GetMouseButtonDown (0)) {
				//タイトルに戻る
				Application.LoadLevel ("Title");
			}
		}
	}

	public void LifeDown (int ap) {
		//RectTransformのサイズを取得し、マイナスする
		rt.sizeDelta -= new Vector2 (0, ap);
	}

	public void LifeUp (int hp) {
		//RectTranceformのサイズを取得し、プラスする
		rt.sizeDelta += new Vector2 (0, hp);
		//最大値を超えたら、最大値で上書きする
		if (rt.sizeDelta.y > 240f) {
			rt.sizeDelta = new Vector2 (51f, 240f);
		}
	}

	public void GameOver () {
		gameOver = true;
		Destroy (unityChan);
	}
}
