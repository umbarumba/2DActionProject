using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public float speed = 4f;//歩くスピード

	public float jumpPower = 700;//ジャンプ力
	public LayerMask groundLayer;//Linecastで判定するLayer

	public GameObject mainCamera;

	public GameObject bullet;

	public Life lifeScript;
	private Boss BossScript;
	private BossZone BossZoneScript;

	private Rigidbody2D RB2D;
	private Animator anim;

	private bool isGrounded;//着地判定

	private Renderer Rend;

    private bool _CameraFreeze = false;

	private bool gameClear = false;//ゲームクリアーしたら操作を無効にする
	public Text clearText;//ゲームクリアー時に表示するテキスト

	// Use this for initialization
	void Start () {
		//各コンポーネントをキャッシュしておく
		anim = GetComponent<Animator>();
		RB2D = GetComponent<Rigidbody2D> ();
		Rend = GetComponent<Renderer> ();

		BossZoneScript = GameObject.FindGameObjectWithTag ("BossZone").GetComponent<BossZone> ();
	}

	void Update () {
		//Linecastでユニティちゃんの足元に地面があるか判定
		isGrounded = Physics2D.Linecast (
			transform.position + transform.up * 1 - transform.right*1,
			transform.position - transform.up * 0.05f  - transform.right*1,
			groundLayer)
			||
			Physics2D.Linecast (
				transform.position + transform.up * 1 + transform.right*1,
				transform.position - transform.up * 0.05f  + transform.right*1,
				groundLayer)
			||
			Physics2D.Linecast (
				transform.position + transform.up * 1 ,
				transform.position - transform.up * 0.05f ,
				groundLayer)
			;

		//クリア時にジャンプさせない
		if (!gameClear) {
			//スペースキーを押した時
			if (Input.GetKeyDown (KeyCode.Space)) {
				//着地していた時
				if (isGrounded) {
					//Dashアニメーションを止めて、Jumpアニメーションを実行
					anim.SetBool ("Dash", false);
					anim.SetTrigger ("Jump");
					//着地判定をfalse
					isGrounded = false;
					//AddForceにて上方向へ力を加える
					RB2D.AddForce (Vector2.up * jumpPower);
				}
			}
		}
		//ジャンプ中に
		//上下の移動速度を取得
		float velY = RB2D.velocity.y;
		//移動速度が0.1より大きければ上昇
		bool isJumping = velY > 0.1f ? true:false;
		//移動速度が-0.1より小さければ下降
		//＜予定＞↓isGrounedがtrueの時にfalseにする
		bool isFalling = velY < -0.1f ? true:false;
		//結果をアニメータービューの変数へ反映する
		anim.SetBool("isJumping",isJumping);
		anim.SetBool("isFalling", isFalling);

		//ジャンプ中に
		if (isJumping) {
			//spaceキーが離されたら
			if (Input.GetKeyUp (KeyCode.Space)) {
				//速度を0にする
				RB2D.velocity = Vector2.zero;
			}
		}

		//クリア時に弾を撃たせない、ゲームオーバーにさせない
		if (!gameClear) {
			if (Input.GetKeyDown (KeyCode.Z)) {
				anim.SetTrigger ("Shot");
				Instantiate (bullet, transform.position + new Vector3 (0f, 1.2f, 0f), transform.rotation);
			}

			//現在のカメラの位置から8低くした位置を下回った時
			if (gameObject.transform.position.y < Camera.main.transform.position.y - 8) {
				//LifeScriptのGameOverメソッドを実行する
				lifeScript.GameOver ();
			}
		}
	}



	void FixedUpdate () {
		//クリア時に左右に移動させない、MainCameraを動かさない
		if (!gameClear) {
			//左キー: -1、右キー: 1
			float x = Input.GetAxisRaw ("Horizontal");
			//左か右を入力したら
			if (x != 0) {
				//入力方向へ移動
				RB2D.velocity = new Vector2 (x * speed, RB2D.velocity.y);
				//localScale.xを-1にすると画像が反転する
				Vector2 temp = transform.localScale;
				temp.x = x;
				transform.localScale = temp;
				//Wai->Dash
				anim.SetBool ("Dash", true);


				//画面中央から左に4移動した位置をユニティちゃんが超えたら	
				if (transform.position.x > mainCamera.transform.position.x - 4) {
						//カメラの位置を取得
					Vector3 cameraPos = mainCamera.transform.position;
                    //Bossが映っていないときだけ
					if (_CameraFreeze == false) {
						//ユニティちゃんの位置から右に4移動した位置を画面中央にする
						cameraPos.x = transform.position.x + 4;
						mainCamera.transform.position = cameraPos;
					}
				}
				//カメラ表示領域の左下をワールド座標に変換
				Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));
				//カメラ表示領域の右上をワールド座標に変換
				Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));
				//ユニティちゃんのポジションを取得
				Vector2 pos = transform.position;
				//ユニティちゃんのx座標の移動範囲をClampメソッドで制限
				pos.x = Mathf.Clamp (pos.x, min.x + 0.5f, max.x);
				transform.position = pos;

				//右も左も入力していなかったら
			} else {
				//横移動の速度を0にしてピタッと止まるようにする
				RB2D.velocity = new Vector2 (0, RB2D.velocity.y);
				//Dash->Wait
				anim.SetBool ("Dash", false);
			}
		} else {
			//クリアーテキストを表示
			clearText.enabled = true;
			//アニメーションは走り
			anim.SetBool ("Dash", true);
			//右に進み続ける
			RB2D.velocity = new Vector2 (speed, RB2D.velocity.y);
			//5秒後にタイトル画面へ戻るCallTitleメソッドを呼び出す
			Invoke ("CallTitle", 5);
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		//Debug.Log (col.gameObject.tag);
		//Enemyとぶつかった時にコルーチンを実行
		if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Boss" || col.gameObject.tag == "EnemyBullet") {
			
			StartCoroutine ("Damage");
		}
	}

	IEnumerator Damage () {
		//Debug.Log ("Damageコルーチン発動");
		//レイヤーをPlayerDamageに変更
		gameObject.layer = LayerMask.NameToLayer ("PlayerDamage");
		//while文を10回ループ
		int count = 10;
		while (count > 0) {
			//透明にする
			Rend.material.color = new Color (1, 1, 1, 0);
			//0.05秒待つ
			yield return new WaitForSeconds (0.05f);
			//元に戻す
			Rend.material.color = new Color (1, 1, 1, 1);
			//0.05秒待つ
			yield return new WaitForSeconds (0.05f);
			count--;
		}
		//レイヤーをPlayerに戻す
		gameObject.layer = LayerMask.NameToLayer ("Player");
	}

	void OnTriggerEnter2D (Collider2D col) {
		//タグがClearZoneであるTriggerにぶつかったら
		if (col.tag == "ClearZone") {
			//ゲームクリアー
			gameClear = true;
		}

		if (col.tag == "BossZone") {
			BossZoneScript.BossZoneEnter ();
		}

		if (col.tag == "EnemyBullet") {
			StartCoroutine ("Damage");
		}
	}
    
	void CallTitle () {
        //タイトル画面へ
        Application.LoadLevel("Title");
	}

    public void BossFinder () {
        BossScript = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
    }

    public void SwichingBool()
    {
        _CameraFreeze = true;
    }
}
