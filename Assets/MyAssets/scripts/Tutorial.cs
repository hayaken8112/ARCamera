using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UniRx;
public class Tutorial : MonoBehaviour {

	public GameObject main_tutorial_panel_prefab;
	public GameObject object_select_tutorial_panel_prefab;
	public GameObject string_select_tutorial_panel_prefab;
	public GameObject put_object_tutorial_panel_prefab;
	public GameObject edit_object_tutorial_panel_prefab;
	GameObject main_tutorial_panel;
	GameObject canvas;

	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("Canvas");
		Debug.Log(PlayerPrefs.GetInt("main_tutorial", -1));
		DoTutorial("main_tutorial");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DoTutorial(string tutorial){
		if(PlayerPrefs.GetInt(tutorial, -1) == -1){
		  GameObject tutorial_panel_prefab;
		  switch (tutorial){
            case "main_tutorial":
              tutorial_panel_prefab = main_tutorial_panel_prefab;
              break;
            case "object_select":
              tutorial_panel_prefab = object_select_tutorial_panel_prefab;
              break;
            case "string_select":
              tutorial_panel_prefab = string_select_tutorial_panel_prefab;
              break;
            case "put_object":
              tutorial_panel_prefab = put_object_tutorial_panel_prefab;
              break;
            case "edit_object":
              tutorial_panel_prefab = edit_object_tutorial_panel_prefab;
              break;
			default:
              tutorial_panel_prefab = null;
			  Debug.Log("tutorial name is incorrect");
			  break;
		  }
		  InstantiateUI(tutorial_panel_prefab);
		  PlayerPrefs.SetInt(tutorial, 1);
		  PlayerPrefs.Save();
		}
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
