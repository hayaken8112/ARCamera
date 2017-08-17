using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CancelButton : MonoBehaviour{

	GameObject shareBtn;
	GameObject saveBtn;
	GameObject previewImg;
	GameObject cancelBtn;

	GameObject videoPlayer;
	// Use this for initialization
	void Start () {
		shareBtn = GameObject.Find("ShareButton(Clone)");
		saveBtn = GameObject.Find("SaveButton(Clone)");
		cancelBtn = GameObject.Find("CancelButton(Clone)");
		previewImg = GameObject.Find("PreviewPanel(Clone)");
		videoPlayer = GameObject.Find("VideoPlayer(Clone)");

		Button cancelButton = this.GetComponent<Button>();
		cancelButton.onClick.AddListener(BackToMainScene);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void BackToMainScene(){
		Destroy(shareBtn);
		Destroy(saveBtn);
		Destroy(cancelBtn);
		Destroy(previewImg);
		Destroy(videoPlayer);
	}


}
