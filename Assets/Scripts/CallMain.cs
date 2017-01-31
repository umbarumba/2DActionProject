using UnityEngine;
using System.Collections;

public class CallMain : MonoBehaviour {

	IEnumerator Start () {
		yield return new WaitForSeconds (2);
		Application.LoadLevel ("GameScene");
	}
}
