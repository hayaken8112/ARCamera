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
	private Subject<GameObject> TextObjectSubject = new Subject<GameObject>();
	public IObservable<GameObject> OnTextObjectGenerated
	{
		get { return TextObjectSubject; }
	}
	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("Canvas");
		textButton = GameObject.Find("TextButton").GetComponent<Button>();
		textButton.OnClickAsObservable().Subscribe(_ => {
			inputFieldInstance = Instantiate(inputFieldPrefab);
			inputFieldInstance.transform.SetParent(canvas.transform, false);
		});
	}
	
	// Update is called once per frame
	void Update () {
	}
}
} // ARCamera
