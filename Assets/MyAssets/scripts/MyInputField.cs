using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

public class MyInputField : MonoBehaviour {

	InputField inputField;
	string inputText;
	
	// Use this for initialization
	void Start () {
		inputField = this.GetComponent<InputField>();	
		inputField.ActivateInputField();
		inputText = "";
		ARCamera.TextObjectGenarator.Instance.textObject =  FlyingText.GetObject(inputText, new Vector3(0,0,1), Quaternion.identity);
		inputField.OnValueChangedAsObservable()
		.Subscribe( text => 
		{
			inputText = text;
			Debug.Log(text);
			FlyingText.UpdateObject(ARCamera.TextObjectGenarator.Instance.textObject, inputText);
		});
		inputField.OnEndEditAsObservable()
		.Where(text => !String.IsNullOrEmpty(text))
		.Subscribe(_ => {
			ARCamera.ARObjectGenerator.Instance.kindOfnextObject = ARCamera.ARObjectGenerator.KindOfObject.Text;
			ARCamera.ARObjectGenerator.Instance.nextARObject = ARCamera.TextObjectGenarator.Instance.textObject;
			/*
			textObject = FlyingText.GetObject("" , new Vector3(0,0,1) , Quaternion.identity);
			inputField.text = "";
			*/
			Destroy(this.gameObject);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
