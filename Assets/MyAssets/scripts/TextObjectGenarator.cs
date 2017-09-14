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
	public GameObject inputFieldPrefab;
	GameObject inputFieldInstance;
	Button textButton;
	public GameObject textObject { get; set; }
	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("Canvas");
		textButton = GameObject.Find("TextButton").GetComponent<Button>();
		// textButtonが押されたときの処理
		textButton.OnClickAsObservable().Subscribe(_ => {
			inputFieldInstance = Instantiate(inputFieldPrefab);
			inputFieldInstance.transform.SetParent(canvas.transform, false);
			ARCamera.StateManager.Instance.currentState = ARCamera.States.TextEdit;
		});
		// Mainに戻ったとき
		ARCamera.StateManager.Instance.OnStatesChanged
		.Where(state => state == ARCamera.States.Main)
		.Subscribe(_ => Destroy(inputFieldInstance));
	}
}
} // ARCamera
