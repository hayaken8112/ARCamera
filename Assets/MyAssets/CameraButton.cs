using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CameraButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
	public GameObject button;
	[SerializeField]
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    private float holdTime = 1f;
 
	// Use this for initialization
	void Start () {
		button = GameObject.Find("Button");
		this.onClick.AddListener(TakeShot);
		this.onLongPress.AddListener(Record);
	}
	
	void TakeShot(){
		ScreenCapture.CaptureScreenshot("screenshot.png");
		Debug.Log("short tap");
	}
	void Record(){
		Debug.Log("long tap");
	}


	// Remove all comment tags (except this one) to handle the onClick event!
    private bool held = false;
    public UnityEvent onClick = new UnityEvent();
 
    public UnityEvent onLongPress = new UnityEvent();
 
    public void OnPointerDown(PointerEventData eventData)
    {
        held = false;
        Invoke("OnLongPress", holdTime);
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");
 
        if (!held)
            onClick.Invoke();
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");
    }
 
    private void OnLongPress()
    {
        held = true;
        onLongPress.Invoke();
    }


}
