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

	Vector3 dragStartPos;
	Vector3 defaultObjRot;
	Vector3 defaultObjScale;
	GameObject lastARObject;
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
			Debug.Log("diff1" + deltaMagnitudediff);
				if (lastARObject != null) {
					float scale = lastARObject.transform.localScale.x + deltaMagnitudediff * 0.001f;
					if (scale < 0.005) return;
					lastARObject.transform.localScale = new Vector3(scale, scale, scale);
				}
		});
	}
	public void MapPinch()
    {
       #if UNITY_EDITOR
        var pinch = this.UpdateAsObservable().Select (pos_dist => Input.mousePosition.x);
        var stop = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonUp(0)).First();
       #elif UNITY_IOS || UNITY_ANDROID
		var pinch = this.UpdateAsObservable()
			.Where(_ => Input.touchCount == 2)
			.Select (pos_dist => Vector2.Distance(Input.GetTouch (0).position, Input.GetTouch (1).position));
        var stop = this.UpdateAsObservable().Where (_ => Input.touchCount != 2).First();
       #endif

        IDisposable onPinch = pinch.Buffer(3)
            .TakeUntil(stop)
            .Subscribe(distanceParam => {
				Debug.Log("pinch");
            	float diff = distanceParam.Last() - distanceParam.First();

              	#if UNITY_EDITOR
			  	Debug.Log(diff);
              	#elif UNITY_IOS || UNITY_ANDROID
				if (lastARObject != null) {
					float scale = lastARObject.transform.localScale.x + diff;
					lastARObject.transform.localScale = new Vector3(scale, scale, scale);
				}
			    Debug.Log("diff" + diff);
                #endif
            });

    }
	
}
