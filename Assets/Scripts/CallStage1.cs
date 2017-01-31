using UnityEngine;
using System.Collections;

public class CallStage1 : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Application.LoadLevel ("Stage1");
		}
	}
}
