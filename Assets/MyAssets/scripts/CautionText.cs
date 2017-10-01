using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;
using UniRx;

public class CautionText : MonoBehaviour {

	ReactiveProperty<int> numOfPointCloud = new ReactiveProperty<int>();
	// Use this for initialization
	void Start () {
		numOfPointCloud.Value = 0;
        UnityARSessionNativeInterface.ARFrameUpdatedEvent += ARFrameUpdated;
		numOfPointCloud.Subscribe(num => {
			var textColor = this.gameObject.GetComponent<Text>().color;
			textColor.a = num == 0 ? 1 : 0;
			this.gameObject.GetComponent<Text>().color = textColor;
		});
	}
    public void ARFrameUpdated(UnityARCamera camera)
    {
		if (camera.pointCloudData == null) return;
        numOfPointCloud.Value = camera.pointCloudData.Length;
    }

	
}
