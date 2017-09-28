using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
using System.Linq;

namespace ARCamera
{
    public class ARObjectGenerator : SingletonMonoBehaviour<ARObjectGenerator>
    {
        public enum KindOfObject { Object, Text };
        public KindOfObject kindOfnextObject { get; set; }
        GameObject[] prefabs;
        GameObject canvas;
        Stack<GameObject> ARObjectStack = new Stack<GameObject>();
        private Subject<GameObject> ARObjectSubject = new Subject<GameObject>();
        private List<GameObject> GridButtonList = new List<GameObject>();
        public IObservable<GameObject> OnObjectGenerated
        {
            get { return ARObjectSubject; }
        }
        public GameObject objBtnPrefab;
        Button undoBtn;
        ReactiveProperty<int> nextARObjIndex = new ReactiveProperty<int>();
		public ReactiveProperty<GameObject> nextARObjectRP = new ReactiveProperty<GameObject>();
        bool isOnGameObject = true;
        void Start()
        {
            canvas = GameObject.Find("Canvas");

            undoBtn = GameObject.Find("UndoButton").GetComponent<Button>();
			// undoButtonの処理
            undoBtn.OnClickAsObservable().Subscribe(_ =>
            {
                if (ARObjectStack.Count > 0)
                {
                    Destroy(ARObjectStack.Pop()); // Stackから一つ消す
                    ARObjectSubject.OnNext(GetLastARObject()); // Objectが減ったことを通知
                }
            });

            LoadPrefab();
            
            //選択されたオブジェクトの色を変える
            nextARObjIndex.Subscribe(x => {
              GridButtonList[x].GetComponent<RawImage>().color = new Color(1, 1, 1, 0.5f);;
            });

			kindOfnextObject = KindOfObject.Object;
			// 次のTextObjectが変更されたときの処理
            nextARObjectRP.Value = null;
			nextARObjectRP.Subscribe(nextObj => {
				ARObjectStack.Push(nextObj);
				ARObjectSubject.OnNext(nextObj);
			});

            // Update
			// Main状態でのオブジェクトの配置処理
            this.UpdateAsObservable()
            .Where(_ => Input.touchCount == 1 && StateManager.Instance.currentState == States.Main)
            .Subscribe(_ =>
            {
                isOnGameObject = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
				Debug.Log("main isOngameObj" + isOnGameObject);
                if (isOnGameObject) return; // UI上をタッチした場合は何もしない

                isOnGameObject = true;
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                    ARPoint point = new ARPoint
                    {
                        x = screenPosition.x,
                        y = screenPosition.y
                    };

                    // prioritize reults types
                    ARHitTestResultType[] resultTypes = {
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
						// if you want to use infinite planes use this:
						//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
						ARHitTestResultType.ARHitTestResultTypeHorizontalPlane,
                        ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    };

                    foreach (ARHitTestResultType resultType in resultTypes)
                    {
                        if (kindOfnextObject == KindOfObject.Object)
                        {
                            if (HitTestWithResultType(point, resultType, nextARObjIndex.Value))
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (HitTestWithResultType(point, resultType, nextARObjectRP.Value))
                            {
                                return;
                            }
                        }
                    }
                }

            });
        }

        bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes, int prefabIndex)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
            if (hitResults.Count > 0)
            {
                foreach (var hitResult in hitResults)
                {
                    Debug.Log("Got hit!");
                    Vector3 pos;
                    Quaternion rot;
                    pos = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                    rot = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                    GameObject newGameObject = Instantiate(prefabs[prefabIndex], pos, rot);
                    // ARObjectStack.Push(Instantiate(prefabs[prefabIndex], pos, rot)); // Generate an ARObject and push to the stack
                    ARObjectStack.Push(newGameObject); // Generate an ARObject and push to the stack
                    ARObjectSubject.OnNext(newGameObject);
                    Debug.Log(string.Format("x:{0:0.######} y:{1:0.######} z:{2:0.######}", pos.x, pos.y, pos.z));
                    return true;
                }
            }
            return false;
        }
        bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes, GameObject nextObject)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
            if (hitResults.Count > 0)
            {
                foreach (var hitResult in hitResults)
                {
                    Debug.Log("Got hit!");
                    Vector3 pos;
                    Quaternion rot;
                    pos = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                    rot = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                    nextObject.transform.position = pos;
                    nextObject.transform.rotation = rot;
                    // ARObjectStack.Push(Instantiate(prefabs[prefabIndex], pos, rot)); // Generate an ARObject and push to the stack
                    Debug.Log(string.Format("x:{0:0.######} y:{1:0.######} z:{2:0.######}", pos.x, pos.y, pos.z));
                    return true;
                }
            }
            return false;
        }

        public GameObject GetLastARObject()
        {
            if (ARObjectStack.Count == 0) return null;
            return ARObjectStack.Peek();
        }
        void LoadPrefab()
        {
            GameObject content;
            content = canvas.transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject;
            //content = this.transform.Find("Canvas/Scroll View/Viewport/Content").gameObject;
            //Debug.Log(content.tag);
            prefabs = Resources.LoadAll<GameObject>("Prefab"); // Resources/Prefab内のPrefabをロード
            for (int i = 0; i < prefabs.Count(); i++)
            {
                string prefab_name = prefabs[i].name;
                Texture2D prefab_image = Resources.Load<Texture2D>( "Captures/" + prefab_name);

                GameObject btn = Instantiate(objBtnPrefab); // Buttonをインスタンス化
                RawImage img = btn.GetComponent<RawImage>();
                img.texture = prefab_image;
                btn.transform.SetParent(content.transform, false);//ボタンをcontentの子に入れる
                GridButtonList.Add(btn); //ボタンをリストに入れる

                int temp = i;
                // 各オブジェクトボタンがクリックされたときの処理
                btn.GetComponent<Button>().OnClickAsObservable()
                .Subscribe(_ =>
                {
                    nextARObjIndex.Value = temp; // 即時値を代入、iだとクリック時に評価されてしまう。
					kindOfnextObject = KindOfObject.Object;
                });
            }

        }
    }
}
