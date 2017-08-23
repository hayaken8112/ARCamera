using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

namespace ARCamera {
	public enum States {Main, PreviewPhoto, PreviewVideo};
public class StateManager: SingletonMonoBehaviour<StateManager>
{
	public States currentState {　get; set;　}
	void Start () {
		ARCamera.StateManager.Instance.currentState = ARCamera.States.Main;
	}
	
}
}