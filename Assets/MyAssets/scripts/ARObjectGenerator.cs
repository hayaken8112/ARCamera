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
        GameObject managers;
        Tutorial tutorial;
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
        void Start()
        {
            canvas = GameObject.Find("Canvas");
            managers = GameObject.Find("Managers");
            tutorial = managers.GetComponent<Tutorial>();
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
            Change_Gridcolor_Slidebutton();

            kindOfnextObject = KindOfObject.Object;
            // 次のTextObjectが変更されたときの処理
            nextARObjectRP.Value = null;
            nextARObjectRP.Subscribe(nextObj =>
            {
                ARObjectStack.Push(nextObj);
                ARObjectSubject.OnNext(nextObj);
            });

            // Main状態でのオブジェクトの配置処理

            InputTest.Instance.OnTouchUp.Where(_ => StateManager.Instance.currentState.Value == States.Main)
                .Subscribe(touch =>
                {
                    ARCamera.TextObjectGenarator.Instance.isEditting = false;
                    var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                    ARPoint point = new ARPoint {x = screenPosition.x, y = screenPosition.y};

                    // prioritize reults types
                    ARHitTestResultType[] resultTypes = {
                    ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                    // if you want to use infinite planes use this:
                    //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                    ARHitTestResultType.ARHitTestResultTypeHorizontalPlane,
                    ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    };
                    tutorial.DoTutorial("edit_object");
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
                Texture2D prefab_image = Resources.Load<Texture2D>("Captures/" + prefab_name);

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
        void Change_Gridcolor_Slidebutton()
        {
            //選択されたオブジェクトの色を変える
            GameObject slideButton = GameObject.Find("SlideButton");
            RawImage slideImage = slideButton.GetComponent<RawImage>();
            nextARObjIndex.Zip(nextARObjIndex.Skip(1), (x, y) => new { OldValue = x, NewValue = y })
    .Subscribe(t =>
    {
        //GridButtonList[t.NewValue].GetComponent<RawImage>().color = new Color(1, 1, 1, 0.5f);
        GridButtonList[t.OldValue].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
    });
            nextARObjIndex.Subscribe(x =>
            {
                GridButtonList[x].GetComponent<RawImage>().color = new Color(1, 1, 1, 0.5f); ;
                slideImage.texture = GridButtonList[x].GetComponent<RawImage>().texture;
            });
        }
    }
}
