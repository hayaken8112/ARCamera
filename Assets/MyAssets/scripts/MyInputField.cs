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
		.Subscribe(text => {
			if (String.IsNullOrEmpty(text)) {
				Destroy(this.gameObject);
			} else {
				ARCamera.ARObjectGenerator.Instance.kindOfnextObject = ARCamera.ARObjectGenerator.KindOfObject.Text;
				ARCamera.ARObjectGenerator.Instance.nextARObjectRP.Value = ARCamera.TextObjectGenarator.Instance.textObject;
			}
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
