using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MySlider : Slider ,IPointerDownHandler, IPointerUpHandler{

	public bool isPressed = false;

	override public void OnPointerDown(PointerEventData eventData){
		isPressed = true;
	}
	override public void OnPointerUp(PointerEventData eventData){
		isPressed = false;
	}

	void Update() {
		//Debug.Log(this.IsPressed());
	}
}
