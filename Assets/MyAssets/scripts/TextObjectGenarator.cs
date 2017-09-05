using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class TextObjectGenarator : MonoBehaviour {

	InputField inputField;
	string inputText = "";
	GameObject textObject;
	// Use this for initialization
	void Start () {
		textObject =  FlyingText.GetObject(inputText, new Vector3(-2.5f,0,5), Quaternion.identity);
		inputField = GameObject.Find("InputField").GetComponent<InputField>();	
		inputField.OnValueChangedAsObservable()
		.Subscribe( text => 
		{
			inputText = text;
			FlyingText.UpdateObject(textObject, inputText);
		});
	}
	
	// Update is called once per frame
	void Update () {
	}
}
