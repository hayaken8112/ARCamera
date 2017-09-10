using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using ARCamera;

public class SaveButton : MonoBehaviour {
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
		if (StateManager.Instance.currentState == States.PreviewPhoto){
			saveBtn.OnClickAsObservable().Subscribe(_ => StartCoroutine(WaitUntilFinishedWritingPicture()));
		}
		else if (StateManager.Instance.currentState == States.PreviewVideo){
			saveBtn.OnClickAsObservable().Subscribe(_ => StartCoroutine(WaitUntilFinishedWritingMovie()));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

     IEnumerator WaitUntilFinishedWritingPicture(){
		 GameObject saving = GameObject.Find("saving");
		 GameObject saved = GameObject.Find("saved");
		 saving.transform.SetAsLastSibling();
		 saved.transform.SetAsLastSibling();		 
		 PanelSlider saved_slider = saved.GetComponent<PanelSlider>();
		 PanelSlider saving_slider = saving.GetComponent<PanelSlider>();
		 saving_slider.SlideIn();
         yield return new WaitForSeconds( 1 );
		 saving_slider.SlideOut();
		 saved_slider.SlideIn();
		yield return new WaitForSeconds( 1 );
		saved_slider.SlideOut();
		_ImageToAlbum (PathManager.GetPhotoPath());
		
    }

     IEnumerator WaitUntilFinishedWritingMovie(){
		 GameObject saving = GameObject.Find("saving");
		 GameObject saved = GameObject.Find("saved");
		 saving.transform.SetAsLastSibling();
		 saved.transform.SetAsLastSibling();		 
		 PanelSlider saved_slider = saved.GetComponent<PanelSlider>();
		 PanelSlider saving_slider = saving.GetComponent<PanelSlider>();
		 saving_slider.SlideIn();
         yield return new WaitForSeconds( 2 );
		 saving_slider.SlideOut();
		 saved_slider.SlideIn();
		yield return new WaitForSeconds( 1 );
		saved_slider.SlideOut();
        _MovieToAlbum (ARCamera.PathManager.GetVideoPath ());

		
    }
}