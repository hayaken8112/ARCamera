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

	public GameObject textObject { get; set; }
	public bool isEditting = false;
	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("Canvas");
		managers = GameObject.Find("Managers");
        tutorial =  managers.GetComponent<Tutorial>();

		textButton = GameObject.Find("TextButton").GetComponent<Button>();
		// textButtonが押されたときの処理
		textButton.OnClickAsObservable().Subscribe(_ => {
			inputFieldInstance = Instantiate(inputFieldPrefab);
			inputFieldInstance.transform.SetParent(canvas.transform, false);
			ARCamera.StateManager.Instance.currentState = ARCamera.States.TextEdit;
			tutorial.DoTutorial("string_select");
		});
		this.UpdateAsObservable().Where(_ => textObject != null && isEditting).Subscribe(_ => {
			textObject.transform.position = cameraChild.transform.position;
			textObject.transform.rotation = cameraChild.transform.rotation;
		});
		// Mainに戻ったとき
		ARCamera.StateManager.Instance.OnStatesChanged
		.Where(state => state == ARCamera.States.Main)
		.Subscribe(_ => {Destroy(inputFieldInstance);
		                 tutorial.DoTutorial("put_object");});
	}
}
} // ARCamera
