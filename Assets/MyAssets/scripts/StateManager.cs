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
        private Subject<States> StateSubject = new Subject<States>();
        public IObservable<States> OnStatesChanged
        {
            get { return StateSubject; }
        }
        public States currentState { get; set; }
	    public EditMode currentMode;
        void Start()
        {
            currentState = States.Main; // 初期化
            currentMode = EditMode.Rotate;

			// ObjectSelectかTextEdit状態から、画面をタッチしてMainに戻る
            InputTest.Instance.OnTouchUp.Where(_ => currentState == States.ObjectSelect || currentState == States.TextEdit)
                .Subscribe(_ => {
                    currentState = States.Main;
                    StateSubject.OnNext(currentState); // 状態の変更を通知
                });
        }

    }
}