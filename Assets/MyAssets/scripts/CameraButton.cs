using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using UniRx;
using UnityEngine.SceneManagement;

public class CameraButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

    public GameObject previewPrefab;
    public GameObject saveButtonPrefab;
    public GameObject cancelButtonPrefab;
    public GameObject shareButtonPrefab;
    GameObject canvas;
    #if UNITY_IPHONE
    // クラスの最初でインポート
    [DllImport("__Internal")]
    private static extern void _ImageToAlbum (string path);
   // private static extern void _PlaySystemShutterSound ();
#endif

	public GameObject button;
	[SerializeField]
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    
    private float holdTime = 1f;
 
	// Use this for initialization

    const string temporaryScreenshotFilename = "screenshot.png";
	void Start () {
		button = GameObject.Find("Button");
		this.onClick.AddListener(TakeShot);
		this.onLongPress.AddListener(Record);
        canvas = GameObject.Find("Canvas");
	}
	
	void TakeShot(){
		// ScreenCapture.CaptureScreenshot(temporaryScreenshotFilename);
		//ScreenCapture.CaptureScreenshot("screenshot.png");
		Debug.Log("short tap");
		//_PlaySystemShutterSound ();
	    // _ImageToAlbum (TemporaryScreenshotPath());
        GameObject preViewImage = InstantiateUI(previewPrefab);
        preViewImage.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        preViewImage.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        preViewImage.GetComponent<PhotoPreview>().photoPath = TemporaryScreenshotPath();
        GameObject cancelBtn = InstantiateUI(cancelButtonPrefab);
        cancelBtn.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(50, -50, 0);
        InstantiateUI(saveButtonPrefab);
        InstantiateUI(shareButtonPrefab);
	}

    GameObject InstantiateUI(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab);
        instance.transform.SetParent(canvas.transform);
        return instance;
    }

	void Record(){
		Debug.Log("long tap");
		Everyplay.StartRecording();
	}

    public string TemporaryScreenshotPath () {
         return Application.persistentDataPath + "/" + temporaryScreenshotFilename;
     }



	// Remove all comment tags (except this one) to handle the onClick event!
    private bool held = false;
    public UnityEvent onClick = new UnityEvent();
 
    public UnityEvent onLongPress = new UnityEvent();
 
    public void OnPointerDown(PointerEventData eventData)
    {
        held = false;
        Invoke("OnLongPress", holdTime);
		Debug.Log("0");
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");
 
        if (!held)
            onClick.Invoke();
		Debug.Log("1");
		Everyplay.StopRecording();
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");
		Debug.Log("2");
    }
 
    private void OnLongPress()
    {
        held = true;
        onLongPress.Invoke();
		Debug.Log("3");
    }


}
