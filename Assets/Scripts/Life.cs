using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Life : MonoBehaviour {

	RectTransform rt;

	private HPbar HPbar;

	public GameObject unityChan;//ユニティちゃん
	public GameObject explosion;//爆発アニメーション
	public Text gameOverText;//ゲームオーバーの文字
	private bool gameOver = false;//ゲームオーバー判定

	public int HitPoint;//HP残量
	public int MaxHitPoint = 20;//HPの最大値
	public float HPper = 1;//HP残量パーセント

	// Use this for initialization
	void Start () {
		HitPoint = MaxHitPoint;
		HPbar = GameObject.FindGameObjectWithTag ("HP").GetComponent<HPbar> ();
	}

	void Update () {
		//ライフが0以下になった時
		if (HitPoint <= 0) {
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
		//自分のHP-相手の攻撃力
		HitPoint -= ap;
		Debug.Log (ap);
		HPper = (float)HitPoint / (float)MaxHitPoint;//HPのパーセントを計算
		HPbar.GageUpDown (HPper);
	}

	public void LifeUp (int hp) {
		//自分のHP+アイテムの回復量
		HitPoint += hp;
		//最大値を超えたら、最大値で上書きする
		if (HitPoint > MaxHitPoint) {
			HitPoint = MaxHitPoint;
		}
		HPper = (float) HitPoint / (float)MaxHitPoint;//HPのパーセントを計算
		HPbar.GageUpDown(HPper);
	}

	public void GameOver () {
		gameOver = true;
		Destroy (unityChan);
	}
}
