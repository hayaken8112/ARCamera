using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using UniRx;
using UnityEngine.SceneManagement;
using ARCamera;

public class CameraButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    GameObject manager;
    PreviewUIManager previewUIManager;
    ScreenShot screenshot;
    [SerializeField] Color recordingColor;


 //   #if UNITY_IPHONE
    // クラスの最初でインポート


    [DllImport("__Internal")]
    private static extern void _CameraSound ();
    [DllImport("__Internal")]
    private static extern void _MovieStartSound ();
    [DllImport("__Internal")]
    private static extern void _MovieEndSound ();

//#endif



	public GameObject button;
    Image button_color;
	[SerializeField]
    [Tooltip("How long must pointer be down on this object to trigger a long press")]

    
    
    private float holdTime = 1f;
    private bool this_is_video = false;
 
	// Use this for initialization

	void Start () {
        manager = GameObject.Find("Managers");
        previewUIManager = manager.GetComponent<PreviewUIManager>();
        screenshot = this.gameObject.GetComponent<ScreenShot>();
		button = GameObject.Find("Button");
        button_color = button.GetComponent<Image>();
		this.onClick.AddListener(TakeShot);
		this.onLongPress.AddListener(Record);
	}
	
	void TakeShot(){
		screenshot.CaptchaScreen();
		Debug.Log("short tap");
		// _PlaySystemShutterSound ();
        #if UNITY_IOS && !UNITY_EDITOR
        _CameraSound ();
        #endif
        StartCoroutine(previewUIManager.MakePreviewUI());
        StateManager.Instance.currentState = States.PreviewPhoto;
	}


	void Record(){
		Debug.Log("long tap");
        this_is_video = true;
		Everyplay.StartRecording();
        button_color.color = recordingColor;
        #if UNITY_IOS && !UNITY_EDITOR
        _MovieStartSound ();
        #endif
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
            button_color.color = new Color(1, 1, 1, 1);
            #if UNITY_IOS && !UNITY_EDITOR 
            _MovieEndSound ();
            #endif
		    Everyplay.StopRecording();
            StateManager.Instance.currentState = States.PreviewVideo;
            StartCoroutine(previewUIManager.MakePreviewUI());
            this_is_video = false;
        }
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
