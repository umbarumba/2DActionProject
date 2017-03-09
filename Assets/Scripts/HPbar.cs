using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPbar : MonoBehaviour {

	private Life LifeScript;
	RectTransform rt;

	private float BarMax;

	// Use this for initialization
	void Start () {
		LifeScript = GameObject.FindGameObjectWithTag ("HP").GetComponent<Life> ();
		rt = GetComponent<RectTransform> ();
		BarMax = rt.sizeDelta.y;
	}

	public void GageUpDown (float per) {
		rt.sizeDelta = new Vector2 (50, BarMax * per);
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
