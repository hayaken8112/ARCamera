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

		 #if UNITY_IPHONE && !UNITY_EDITOR
		 saving = InstantiateUI(savingPrefab);
         _ImageToAlbum(ARCamera.PathManager.GetVideoPath());
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
		}

}