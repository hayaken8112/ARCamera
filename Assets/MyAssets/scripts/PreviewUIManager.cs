using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UniRx;

public class PreviewUIManager : MonoBehaviour {
    public GameObject previewPrefab;
    public GameObject saveButtonPrefab;
    public GameObject cancelButtonPrefab;
    public GameObject shareButtonPrefab;
	GameObject previewPanel;
	GameObject saveBtn;
	GameObject cancelBtn;
	GameObject shareBtn;
    GameObject canvas;
    GameObject managers;
    Tutorial tutorial;


	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
        managers = GameObject.Find("Managers");
        tutorial =  managers.GetComponent<Tutorial>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    GameObject InstantiateUI(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab);
        instance.transform.SetParent(canvas.transform, false);
        return instance;
    }

    public IEnumerator MakePreviewUI()
    {
        yield return new WaitForSeconds(0.1f);
        // プレビュー画面のインスタンス生成
        previewPanel =  InstantiateUI(previewPrefab);
        cancelBtn = InstantiateUI(cancelButtonPrefab);
        saveBtn = InstantiateUI(saveButtonPrefab);
        // shareBtn = InstantiateUI(shareButtonPrefab);
        tutorial.DoTutorial("save_tutorial");
    }

	public void DestroyPreviewUI()
	{
		Destroy(previewPanel);
		Destroy(cancelBtn);
		Destroy(saveBtn);
		// Destroy(shareBtn);
	}
}
