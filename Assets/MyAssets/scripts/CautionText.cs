using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;
using UniRx;

public class CautionText : MonoBehaviour {

	ReactiveProperty<int> numOfPlane = new ReactiveProperty<int>();

	private UnityARAnchorManager unityARAnchorManager;
	// Use this for initialization
	void Start () {
		unityARAnchorManager = new UnityARAnchorManager();
		numOfPlane.Value = 0;
		numOfPlane.Subscribe(num => {
			var textColor = this.gameObject.GetComponent<Text>().color;
			textColor.a = (num == 0) ? 1 : 0;
			this.gameObject.GetComponent<Text>().color = textColor;
			this.gameObject.GetComponentInChildren<RawImage>().color = textColor;
		});
	}

	public void Update() {
		numOfPlane.Value = unityARAnchorManager.GetCurrentPlaneAnchors().Count;
	}




        void OnDestroy()
        {
            unityARAnchorManager.Destroy ();
        }
	
}
