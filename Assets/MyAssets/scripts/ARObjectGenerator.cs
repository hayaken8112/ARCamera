using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

namespace ARCamera
{
    public class ARObjectGenerator : SingletonMonoBehaviour<ARObjectGenerator>
    {
        GameObject[] prefabs;
        Stack<GameObject> ARObjectStack = new Stack<GameObject>();
		private Subject<GameObject> ARObjectSubject = new Subject<GameObject>();
		public IObservable<GameObject> OnObjectGenerated
		{
			get { return ARObjectSubject; }
		}
        Button btn1;
        Button btn2;
        Button undoBtn;
        int nextARObjIndex = 0;
        bool isOnGameObject = true;
        void Start()
        {
            btn1 = GameObject.Find("ObjectButton1").GetComponent<Button>();
            btn2 = GameObject.Find("ObjectButton2").GetComponent<Button>();
            undoBtn = GameObject.Find("UndoButton").GetComponent<Button>();

            btn1.OnClickAsObservable().Subscribe(_ => nextARObjIndex = 0);
            btn2.OnClickAsObservable().Subscribe(_ => nextARObjIndex = 1);
            undoBtn.OnClickAsObservable().Subscribe(_ => {
				Destroy(ARObjectStack.Pop());
				ARObjectSubject.OnNext(GetLastARObject());
				});
            prefabs = Resources.LoadAll<GameObject>("Prefab"); // Resources/Prefab内のPrefabをロード

            this.UpdateAsObservable()
			.Where(_ => Input.touchCount == 1)
			.Subscribe(_ =>
            {
                isOnGameObject = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
                if (isOnGameObject) return;

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
                        if (HitTestWithResultType(point, resultType, nextARObjIndex))
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

        public GameObject GetLastARObject()
		{
			if (ARObjectStack.Count == 0) return null;
			return ARObjectStack.Peek();
		}
        /* 
		// Update is called once per frame
		void Update () {
			RawImage btnImage1 = GameObject.Find("ObjectButton1").GetComponent<RawImage>();
			btnImage1.texture =  AssetPreview.GetAssetPreview(prefabs[0]);
			if (Input.touchCount > 0)
			{
				isOnGameObject = EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
				if (isOnGameObject == false){
				isOnGameObject = true;
				var touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began)
				{
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
					ARPoint point = new ARPoint {
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
						if (HitTestWithResultType (point, resultType, nextARObjIndex))
						{
							return;
						}
					}
				}
				}
			}
		}
		*/
    }
}
