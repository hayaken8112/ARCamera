using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using ARCamera;

public class ObjectEditor : MonoBehaviour {

	GameObject lastARObject;
	[SerializeField] private float zoomRate;
	[SerializeField] private float rotateSpeed;
	// Use this for initialization
	void Start () {
		lastARObject = ARCamera.ARObjectGenerator.Instance.GetLastARObject();
		ARCamera.ARObjectGenerator.Instance.OnObjectGenerated
		.Subscribe(ARObj => 
		{
			// オブジェクトが増減すると、スライダーの位置を中心に戻す。
			lastARObject = ARObj;
		});

		this.UpdateAsObservable().Where(_ => Input.touchCount == 2)
		.Subscribe(_ => {
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			float deltaMagnitudediff = touchDeltaMag - prevTouchDeltaMag;

			float aveDiff = (touchZero.deltaPosition.x + touchOne.deltaPosition.x)/2;
			Debug.Log("diff1" + deltaMagnitudediff);
				if (lastARObject != null) {
					float scale = lastARObject.transform.localScale.x + deltaMagnitudediff * zoomRate;
					if (scale > 0.005) {
						lastARObject.transform.localScale = new Vector3(scale, scale, scale);
					}
					if (touchZero.deltaPosition.x * touchOne.deltaPosition.x > 0) {
						Debug.Log("aveDiff:" + aveDiff);
						Vector3 rot = lastARObject.transform.rotation.eulerAngles;
						rot.y -= aveDiff * rotateSpeed;
						lastARObject.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
					}
				}
		});
	}
	
}
