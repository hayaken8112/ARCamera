using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class TextObjectGenarator : MonoBehaviour {

	Button textButton;
	InputField inputField;
	string inputText = "";
	GameObject textObject;
	// Use this for initialization
	void Start () {
		textButton = GameObject.Find("TextButton").GetComponent<Button>();
		textButton.OnClickAsObservable().Subscribe(_ => {
			inputField.ActivateInputField();
			textObject =  FlyingText.GetObject(inputText, new Vector3(0,0,1), Quaternion.identity);
		});
		inputField = GameObject.Find("InputField").GetComponent<InputField>();
		inputField.OnValueChangedAsObservable()
		.Subscribe( text => 
		{
			inputText = text;
			FlyingText.UpdateObject(textObject, inputText);
		});
		inputField.OnEndEditAsObservable()
		.Where(text => !String.IsNullOrEmpty(text))
		.Subscribe(_ => {
			ARCamera.ARObjectGenerator.Instance.kindOfnextObject = ARCamera.ARObjectGenerator.KindOfObject.Text;
			ARCamera.ARObjectGenerator.Instance.nextARObject = textObject;
			textObject = FlyingText.GetObject("" , new Vector3(0,0,1) , Quaternion.identity);
			inputField.text = "";
		});
	}
	
	// Update is called once per frame
	void Update () {
	}
}
