using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class CancelButton : MonoBehaviour {

	Button cancelBtn;
	GameObject managers;
	PreviewUIManager previewUIManager;
	// Use this for initialization
	void Start () {
		managers = GameObject.Find("Managers");
		previewUIManager = managers.GetComponent<PreviewUIManager>();
		cancelBtn = this.gameObject.GetComponent<Button>();
		cancelBtn.OnClickAsObservable().Subscribe(_ => {
			previewUIManager.DestroyPreviewUI();
			ARCamera.StateManager.Instance.currentState.Value = ARCamera.States.Main;
		});
	}
	

}
