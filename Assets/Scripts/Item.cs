using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public int healPoint = 20;
	//Prefab化するとInspectorから指定できないためprivate化
	private Life lifeScript;

	void Start () {
		//HPタグの付いているオブジェクトのLifeScriptを取得
		lifeScript = GameObject.FindGameObjectWithTag ("HP").GetComponent<Life> ();
	}

	void OnCollisionEnter2D (Collision2D col) {
		//ユニティちゃんと衝突した時
		if (col.gameObject.tag == "UnityChan") {
			//lifeUpメソッドを呼び出す　引数はhealPoint
			lifeScript.LifeUp (healPoint);
			//アイテムを削除する
			Destroy (gameObject);
		}
	}
}
