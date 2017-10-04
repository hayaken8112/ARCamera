using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

namespace ARCamera {
public class TextObjectGenarator : SingletonMonoBehaviour<TextObjectGenarator> {

	GameObject canvas;
	[SerializeField] private GameObject cameraChild;
	public GameObject inputFieldPrefab;
	GameObject inputFieldInstance;
	Button textButton;
	GameObject managers;
    Tutorial tutorial;
	GameObject undobutton;
	public GameObject textObject { get; set; }
	public bool isEditting = false;
	// Use this for initialization
	bool noUndoButton = false;
	Vector3 undobuttonvector;
	void Start () {
		undobutton = GameObject.Find("UndoButton");
		undobuttonvector = undobutton.GetComponent<RectTransform>().localPosition;
		canvas = GameObject.Find("Canvas");
		managers = GameObject.Find("Managers");
        tutorial =  managers.GetComponent<Tutorial>();

		textButton = GameObject.Find("TextButton").GetComponent<Button>();
		// textButtonが押されたときの処理
		textButton.OnClickAsObservable().Subscribe(_ => {
			undobutton.GetComponent<RectTransform>().localPosition =  new Vector3 (1000, 1000, 1000);
			noUndoButton = true;
			if (ARCamera.StateManager.Instance.currentState.Value == ARCamera.States.TextEdit){
				inputFieldInstance.GetComponent<InputField>().ActivateInputField();
			} else {
				inputFieldInstance = Instantiate(inputFieldPrefab);
				inputFieldInstance.transform.SetParent(canvas.transform, false);
				ARCamera.StateManager.Instance.currentState.Value = ARCamera.States.TextEdit;
				tutorial.DoTutorial("string_select");
			}
		});
		this.UpdateAsObservable().Where(_ => textObject != null && isEditting).Subscribe(_ => {
			textObject.transform.position = cameraChild.transform.position;
			textObject.transform.rotation = cameraChild.transform.rotation;
		});
		// Mainに戻ったとき
		ARCamera.StateManager.Instance.currentState
		.Where(state => state == ARCamera.States.Main)
		.Subscribe(_ => {
			if(noUndoButton){
			  undobutton.GetComponent<RectTransform>().localPosition = undobuttonvector;
			  noUndoButton = false;
			}
			Destroy(inputFieldInstance);
			tutorial.DoTutorial("put_object");
		});
	}
}
} // ARCamera
