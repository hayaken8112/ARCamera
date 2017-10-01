using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UniRx;
public class Tutorial : MonoBehaviour {

	public GameObject main_tutorial_panel_prefab;
	GameObject main_tutorial_panel;
	GameObject canvas;

	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("Canvas");
		Debug.Log(PlayerPrefs.GetInt("main_tutorial", -1));
		if(PlayerPrefs.GetInt("main_tutorial", -1) == -1){
		  main_tutorial_panel =  InstantiateUI(main_tutorial_panel_prefab);
		  PlayerPrefs.SetInt("main_tutorial", 1);
		  PlayerPrefs.Save();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	    GameObject InstantiateUI(GameObject prefab)
    {
		Debug.Log(prefab);
		Debug.Log(canvas);
        GameObject instance = Instantiate(prefab);
        instance.transform.SetParent(canvas.transform, false);
        return instance;
    }



}
