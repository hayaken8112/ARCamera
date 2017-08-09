using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraButton : MonoBehaviour {
	public GameObject button;
	// Use this for initialization
	void Start () {
		button = GameObject.Find("Button");
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TakeShot);
	}
	
	void TakeShot(){}
}
