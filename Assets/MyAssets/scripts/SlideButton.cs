using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using System;
using UnityEngine.XR.iOS;
using UniRx.Triggers;
using System.Linq;


public class SlideButton : MonoBehaviour {

	GameObject scrollview;
	Button slideButton;

	PanelSlider panelslider;
	bool slidein = true;
	public void Slide(){
		if(slidein){
			panelslider.SlideIn();
			slidein = false;
			ARCamera.StateManager.Instance.currentState = ARCamera.States.ObjectSelect;
		} else {
			panelslider.SlideOut();
			slidein = true;
			ARCamera.StateManager.Instance.currentState = ARCamera.States.Main;
		}
	}

	void Start () {
		scrollview = GameObject.Find("Scroll View");
		slideButton = this.gameObject.GetComponent<Button>();
		panelslider = scrollview.GetComponent<PanelSlider>();
		// slideButtonが押されたときの処理
		slideButton.OnClickAsObservable().Subscribe(_ => {
			panelslider.SlideIn();
			ARCamera.StateManager.Instance.currentState = ARCamera.States.ObjectSelect;
		});

		// Main状態に戻ったときの処理
		ARCamera.StateManager.Instance.OnStatesChanged
		.Where(state => state == ARCamera.States.Main)
		.Subscribe(_ => panelslider.SlideOut());


	}

	
}
