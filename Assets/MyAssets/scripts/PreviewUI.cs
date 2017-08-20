using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UniRx;

public class PreviewUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	protected IEnumerator InitPreviewUI(){
		yield return null;
		GameObject cancelBtn = GameObject.Find("CancelButton(Clone)");	
		Button button = cancelBtn.GetComponent<Button>();
		button.onClick.AsObservable().Subscribe(_ => 
									{
										Destroy(this.gameObject);
										ARCamera.StateManager.Instance.currentState = ARCamera.States.Main;
									});

	}
}
