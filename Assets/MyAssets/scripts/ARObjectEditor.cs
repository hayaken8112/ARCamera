using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class ARObjectEditor : MonoBehaviour {

	Vector3 dragStartPos;
	GameObject lastARObject;
	// Use this for initialization
	void Start () {
		var eventTrigger = this.gameObject.AddComponent<ObservableEventTrigger>();
		eventTrigger.OnPointerDownAsObservable().Subscribe(_ => lastARObject = ARCamera.ARObjectGenerator.Instance.GetLastARObject());
		eventTrigger.OnBeginDragAsObservable().Subscribe(pointerEventData => dragStartPos = pointerEventData.position);
		eventTrigger.OnDragAsObservable().Subscribe(pointerEventData => 
		{
			float dragWidth = pointerEventData.position.x - dragStartPos.x;
			Debug.Log(dragWidth);
			if (lastARObject != null) {
				lastARObject.transform.Rotate(0, dragWidth, 0);
			}
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
}
