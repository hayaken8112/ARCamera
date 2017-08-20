using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ARCamera.StateManager.Instance.currentState = ARCamera.States.Main;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
