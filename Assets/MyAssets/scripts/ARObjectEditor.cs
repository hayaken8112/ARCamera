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
		// pointerDownTrigger.OnPointerDownAsObservable().Subscribe(_ => lastARObject = ARCamera.ARObjectGenerator.Instance.GetLastARObject());
		beginDragTrigger.OnBeginDragAsObservable().Subscribe(pointerEventData => defaultObjRot = lastARObject.transform.rotation.eulerAngles);
		slider.OnDragAsObservable().Subscribe(pointerEventData => 
		{
			Debug.Log("is dragged");
			/* 
			float dragWidth = pointerEventData.position.x - dragStartPos.x;
			Debug.Log(dragWidth);
			if (lastARObject != null) {
				lastARObject.transform.Rotate(0, dragWidth, 0);
			}
			*/
			if (lastARObject != null) {
				float diff = (slider.value - 0.5f) * 360;
				lastARObject.transform.rotation = Quaternion.Euler(defaultObjRot.x, defaultObjRot.y + diff, defaultObjRot.z);
			}

		});
		ARCamera.ARObjectGenerator.Instance.OnObjectGenerated
		.Subscribe(ARObj => 
		{
			slider.value = 0.5f;
			lastARObject = ARObj;
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
}
