﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using ARCamera;

public class ARObjectEditor : MonoBehaviour {

	Vector3 defaultObjRot;
	GameObject lastARObject;
	Slider slider;
	// Use this for initialization
	void Start () {
		StateManager.Instance.currentMode = EditMode.Rotate;
		slider = this.gameObject.GetComponent<Slider>();
		GameObject sliderHandle = GameObject.Find("Handle");
		var beginDragTrigger = sliderHandle.AddComponent<ObservableBeginDragTrigger>();
		beginDragTrigger.OnBeginDragAsObservable().Where(_ => StateManager.Instance.currentMode == EditMode.Rotate)
												  .Subscribe(pointerEventData => defaultObjRot = lastARObject.transform.rotation.eulerAngles);
		slider.OnDragAsObservable().Where(_ => StateManager.Instance.currentMode == EditMode.Rotate).Subscribe(pointerEventData => 
		{
			if (lastARObject != null) {
				// スライドバーの中心からの変化分だけ回転させる。
				float diff = (slider.value - 0.5f) * 360;
				lastARObject.transform.rotation = Quaternion.Euler(defaultObjRot.x, defaultObjRot.y + diff, defaultObjRot.z);
			}

		});
		slider.OnDragAsObservable().Where(_ => StateManager.Instance.currentMode == EditMode.Zoom).Subscribe(pointerEventData => 
		{
			if (lastARObject != null) {
				// スライドバーの中心からの変化分だけ回転させる。
				float scale = 0.1f * (float)Math.Pow(100, slider.value);
				lastARObject.transform.localScale = new Vector3(scale, scale, scale);
			}

		});
		lastARObject = ARCamera.ARObjectGenerator.Instance.GetLastARObject();
		ARCamera.ARObjectGenerator.Instance.OnObjectGenerated
		.Subscribe(ARObj => 
		{
			// オブジェクトが増減すると、スライダーの位置を中心に戻す。
			slider.value = 0.5f;
			lastARObject = ARObj;
		});
	}
	public void InitSlider() {
		slider.value = 0.5f;
	}
	
}
