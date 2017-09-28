using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputTest : SingletonMonoBehaviour<InputTest>{

	Subject<Touch> onTouchStream = new Subject<Touch>();
	public IObservable<Touch> OnTouchUp
	{
		get {return onTouchStream;}
	}
	bool isOnGameObject;
	bool isPuttingObject;
	ReactiveProperty<int> touchCountRP = new ReactiveProperty<int>();
	// Use this for initialization
	void Start () {
		this.UpdateAsObservable().Subscribe(_ => touchCountRP.Value = Input.touchCount);
		touchCountRP.Where(x => x > 1).Subscribe(_ => isPuttingObject = false);
		this.UpdateAsObservable().Where(_ => Input.touchCount == 1).Subscribe(_ => {
			var touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				isOnGameObject =  EventSystem.current.IsPointerOverGameObject(touch.fingerId);
				isPuttingObject = true;
			} else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved){
			} else if (isPuttingObject && !isOnGameObject && touch.phase == TouchPhase.Ended) {
				Debug.Log("touch Ended:isOnGameObj");
				onTouchStream.OnNext(touch);
			}
		});
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
