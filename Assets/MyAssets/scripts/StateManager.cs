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
    public class StateManager : SingletonMonoBehaviour<StateManager>
    {
        bool isOnGameObject = true;
        private Subject<States> StateSubject = new Subject<States>();
        public IObservable<States> OnStatesChanged
        {
            get { return StateSubject; }
        }
        public States currentState { get; set; }
        void Start()
        {
            ARCamera.StateManager.Instance.currentState = ARCamera.States.Main; // 初期化

			// ObjectSelectかTextEdit状態から、画面をタッチしてMainに戻る
            this.UpdateAsObservable()
                .Where(_ => (currentState == States.ObjectSelect || currentState == States.TextEdit) && Input.touchCount == 1)
                .Subscribe(_ => {
                    isOnGameObject = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
                    if (isOnGameObject) return; // UI上をタッチした場合は何もしない

                    isOnGameObject = true;
					var touch = Input.GetTouch(0);
					if (touch.phase == TouchPhase.Began) {
						currentState = States.Main;
						StateSubject.OnNext(currentState); // 状態の変更を通知
					}
                });
        }

    }
}