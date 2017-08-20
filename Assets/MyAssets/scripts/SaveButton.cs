using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using ARCamera;

public class SaveButton : PreviewUI {
    #if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern void _MovieToAlbum (string path);
    [DllImport("__Internal")]
    private static extern void _ImageToAlbum (string path);
	#endif
	Button saveBtn;
	// Use this for initialization
	void Start () {
		saveBtn = this.gameObject.GetComponent<Button>();
		StartCoroutine(InitPreviewUI());
		if (StateManager.Instance.currentState == States.PreviewPhoto){
			saveBtn.OnClickAsObservable().Subscribe(_ => _ImageToAlbum (PathManager.GetPhotoPath()));
		} else if (StateManager.Instance.currentState == States.PreviewVideo){
			saveBtn.OnClickAsObservable().Subscribe(_ => StartCoroutine(WaitUntilFinishedWriting()));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
     IEnumerator WaitUntilFinishedWriting(){
         yield return new WaitForSeconds( 2 );
        _MovieToAlbum (ARCamera.PathManager.GetVideoPath ());
    }
}