using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class ARObjectEditor : MonoBehaviour {

	Vector3 dragStartPos;
	Vector3 defaultObjRot;
	GameObject lastARObject;
	GameObject sliderHandle;
	Slider slider;
	// Use this for initialization
	void Start () {
		slider = GameObject.Find("Slider").GetComponent<Slider>();
		sliderHandle = GameObject.Find("Handle");
		var pointerDownTrigger = sliderHandle.AddComponent<ObservablePointerDownTrigger>();
		var beginDragTrigger = sliderHandle.AddComponent<ObservableBeginDragTrigger>();
		beginDragTrigger.OnBeginDragAsObservable().Subscribe(pointerEventData => defaultObjRot = lastARObject.transform.rotation.eulerAngles);
		slider.OnDragAsObservable().Subscribe(pointerEventData => 
		{
			if (lastARObject != null) {
				// スライドバーの中心からの変化分だけ回転させる。
				float diff = (slider.value - 0.5f) * 360;
				lastARObject.transform.rotation = Quaternion.Euler(defaultObjRot.x, defaultObjRot.y + diff, defaultObjRot.z);
			}

		});
		ARCamera.ARObjectGenerator.Instance.OnObjectGenerated
		.Subscribe(ARObj => 
		{
			// オブジェクトが増減すると、スライダーの位置を中心に戻す。
			slider.value = 0.5f;
			lastARObject = ARObj;
		});
	}
	
}
