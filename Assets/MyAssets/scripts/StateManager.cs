using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

namespace ARCamera
{
    public enum States { Main, ObjectSelect, TextEdit, PreviewPhoto, PreviewVideo };
	public enum EditMode { Rotate, Zoom }
    public class StateManager : SingletonMonoBehaviour<StateManager>
    {
        public ReactiveProperty<States> currentState = new ReactiveProperty<States>();
	    public EditMode currentMode;
        void Start()
        {
            currentState.Value = States.Main; // 初期化
            currentMode = EditMode.Rotate;

			// ObjectSelectかTextEdit状態から、画面をタッチしてMainに戻る
            InputTest.Instance.OnTouchUp.Where(_ => currentState.Value == States.ObjectSelect || currentState.Value == States.TextEdit)
                .Subscribe(_ => {
                    currentState.Value = States.Main;
                });
            
            currentState.Where(state => state != States.Main).Subscribe(_ => {
                TextObjectGenarator.Instance.isEditting = false;
            });
        }

        

    }
}