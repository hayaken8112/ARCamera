using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

public class CameraButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

    #if UNITY_IPHONE
    // クラスの最初でインポート
    [DllImport("__Internal")]
    private static extern void _ImageToAlbum (string path);

    [DllImport("__Internal")]
    private static extern void _MovieToAlbum (string path);

    [DllImport("__Internal")]
    private static extern void _CameraSound ();
    [DllImport("__Internal")]
    private static extern void _MovieStartSound ();
    [DllImport("__Internal")]
    private static extern void _MovieEndSound ();

#endif


    //画像の保存パス
    string TemporaryScreenshotPath () {
        return Application.persistentDataPath + "/" + temporaryScreenshotFilename;
     }

     //動画の保存パス
    static string GetVideoPath ()
     {
         #if UNITY_IOS
 
         var root = new DirectoryInfo(Application.persistentDataPath).Parent.FullName;
         var everyplayDir = root + "/tmp/Everyplay/session";
 
         #elif UNITY_ANDROID
 
         var root = new DirectoryInfo(Application.temporaryCachePath).FullName;
         var everyplayDir = root + "/sessions";
 
         #endif
 
         var files = new DirectoryInfo(everyplayDir).GetFiles("*.mp4", SearchOption.AllDirectories);
         var videoLocation = "";
 
         // Should only be one video, if there is one at all
         foreach (var file in files) {
             #if UNITY_ANDROID
             videoLocation = "file://" + file.FullName;
             #else
             videoLocation = file.FullName;
             #endif
             break;
         }
 
         return videoLocation;
     }





	public GameObject button;
	[SerializeField]
    [Tooltip("How long must pointer be down on this object to trigger a long press")]

    
    
    private float holdTime = 1f;
    private bool this_is_video = false;
 
	// Use this for initialization

    const string temporaryScreenshotFilename = "screenshot.jpg";
	void Start () {
		button = GameObject.Find("Button");
		this.onClick.AddListener(TakeShot);
		this.onLongPress.AddListener(Record);
	}
	
	void TakeShot(){
		ScreenCapture.CaptureScreenshot(temporaryScreenshotFilename);
		//ScreenCapture.CaptureScreenshot("screenshot.png");
		Debug.Log("short tap");
		//_PlaySystemShutterSound ();
        Debug.Log("pic :" + TemporaryScreenshotPath());
		_ImageToAlbum (TemporaryScreenshotPath());
        _CameraSound ();
	}
	void Record(){
		Debug.Log("long tap");
        this_is_video = true;
		Everyplay.StartRecording();
        _MovieStartSound ();

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
        if(this_is_video){
            _MovieEndSound ();
		    Everyplay.StopRecording();
            StartCoroutine(WaitUntilFinishedWriting());
            this_is_video = false;
        }
    }
 
     IEnumerator WaitUntilFinishedWriting(){
         yield return new WaitForSeconds( 2 );
         Debug.Log("video: "+GetVideoPath ());
        _MovieToAlbum (GetVideoPath ());
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
