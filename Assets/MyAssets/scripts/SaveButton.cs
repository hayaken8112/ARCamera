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
	GameObject cancelBtnObj;
	public GameObject savingPrefab;
	public GameObject savedPrefab;
	GameObject saving;
	GameObject saved;


	GameObject InstantiateUI(GameObject prefab)
    {
		GameObject canvas = GameObject.Find("Canvas");
        GameObject instance = Instantiate(prefab);
        instance.transform.SetParent(canvas.transform, false);
        return instance;
    }


	// Use this for initialization
	void Start () {
		saveBtn = this.gameObject.GetComponent<Button>();
		cancelBtnObj = GameObject.Find("CancelButton(Clone)");
		if (StateManager.Instance.currentState.Value == States.PreviewPhoto){
			saveBtn.OnClickAsObservable().Subscribe(_ => {
				this.transform.Translate(0, -200, 0);
				cancelBtnObj.transform.Translate(0, 200, 0);
				StartCoroutine(WaitUntilFinishedWritingPicture());
			});
		}
		else if (StateManager.Instance.currentState.Value == States.PreviewVideo){
			saveBtn.OnClickAsObservable().Subscribe(_ => {
				this.transform.Translate(0, -200, 0);
				cancelBtnObj.transform.Translate(0, 200, 0);
				StartCoroutine(WaitUntilFinishedWritingMovie());
			});
		}
	}
	

/* 
     IEnumerator WaitUntilFinishedWritingPicture(){

		 #if UNITY_IPHONE && !UNITY_EDITOR
		 saving = InstantiateUI(savingPrefab);
		 yield return new WaitForSeconds( 0 );
         _ImageToAlbum(ARCamera.PathManager.GetVideoPath());
		 #else
		 Debug.Log(this.transform.name);

		 saving = InstantiateUI(savingPrefab);
         yield return new WaitForSeconds( 2 );
		 Destroy(saving);
		 saved = InstantiateUI(savedPrefab);
		 yield return new WaitForSeconds( 1 );
		 Destroy(saved);
		 #endif
    }
	*/
	     IEnumerator WaitUntilFinishedWritingPicture(){

		 saving = InstantiateUI(savingPrefab);
		 #if UNITY_IPHONE && !UNITY_EDITOR
		 _ImageToAlbum (PathManager.GetPhotoPath());
		 #endif
         yield return new WaitForSeconds( 1 );
		 Destroy(saving);
		 saved = InstantiateUI(savedPrefab);
		 yield return new WaitForSeconds( 1 );
		 Destroy(saved);
		 cancelBtnObj.transform.Translate(0, -200, 0);
		
    }



     IEnumerator WaitUntilFinishedWritingMovie(){
		 #if UNITY_IPHONE && !UNITY_EDITOR
		 saving = InstantiateUI(savingPrefab);
         _MovieToAlbum (ARCamera.PathManager.GetVideoPath());
		 yield return new WaitForSeconds( 0 );
		 #else
		 Debug.Log(this.transform.name);

		 saving = InstantiateUI(savingPrefab);
         yield return new WaitForSeconds( 2 );
		 Destroy(saving);
		 saved = InstantiateUI(savedPrefab);
		 yield return new WaitForSeconds( 1 );
		 Destroy(saved);
		 #endif

		
    }

		public void AfterSaved (string message) {

		Debug.Log(message);
		Destroy(saving);
		saved = InstantiateUI(savedPrefab);
		StartCoroutine(DestroySaved());
	}
		IEnumerator DestroySaved() {
			yield return new WaitForSeconds(1);
			if(saved != null) Destroy(saved);
			cancelBtnObj.transform.Translate(0, -200, 0);
		}

}