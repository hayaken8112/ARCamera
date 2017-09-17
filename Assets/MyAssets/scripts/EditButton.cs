using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using ARCamera;
public class EditButton : MonoBehaviour {

	GameObject canvas;
	public GameObject sliderPrefab;
	public GameObject rotateButtonPrefab;
	public GameObject zoomButtonPrefab;
	bool isEditting = false;
	GameObject sliderObj;
	GameObject rotateButtonObj;
	GameObject zoomButtonObj;
	GameObject undoButton;
	Button editButton;
	Button rotateButton;
	Button zoomButton;
	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("Canvas");
		undoButton = GameObject.Find("UndoButton");
		editButton = this.GetComponent<Button>();
		editButton.OnClickAsObservable().Take(1).Subscribe(_ => {
			sliderObj = Instantiate(sliderPrefab);
			sliderObj.transform.SetParent(canvas.transform, false);
			rotateButtonObj = Instantiate(rotateButtonPrefab);
			rotateButtonObj.transform.SetParent(canvas.transform, false);
			zoomButtonObj = Instantiate(zoomButtonPrefab);
			zoomButtonObj.transform.SetParent(canvas.transform, false);
			rotateButton = rotateButtonObj.GetComponent<Button>();
			zoomButton = zoomButtonObj.GetComponent<Button>();
			rotateButton.OnClickAsObservable().Subscribe(t => {
				StateManager.Instance.currentMode = EditMode.Rotate;
				Debug.Log("Rotate");
			});
			zoomButton.OnClickAsObservable().Subscribe(t => {
				StateManager.Instance.currentMode = EditMode.Zoom;
			});
			undoButton.SetActive(false);

			isEditting = !isEditting;
		});
		editButton.OnClickAsObservable().Skip(1).Subscribe(_ => {
			if (isEditting) {
				sliderObj.SetActive(false);
				rotateButtonObj.SetActive(false);
				zoomButtonObj.SetActive(false);
				undoButton.SetActive(true);
				isEditting = !isEditting;
			} else {
				sliderObj.SetActive(true);
				rotateButtonObj.SetActive(true);
				zoomButtonObj.SetActive(true);
				undoButton.SetActive(false);
				isEditting = !isEditting;
			}
		});

	}

	void InstantiateUI (GameObject prefab, GameObject obj) {
		obj = Instantiate(prefab);
		obj.transform.SetParent(canvas.transform, false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
