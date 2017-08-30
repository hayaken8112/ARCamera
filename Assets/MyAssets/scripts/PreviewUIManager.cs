﻿using System.Collections;
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

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
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

    public void MakePreviewUI()
    {
        // プレビュー画面のインスタンス生成
        previewPanel =  InstantiateUI(previewPrefab);
        cancelBtn = InstantiateUI(cancelButtonPrefab);
        saveBtn = InstantiateUI(saveButtonPrefab);
        shareBtn = InstantiateUI(shareButtonPrefab);

    }

	public void DestroyPreviewUI()
	{
		Destroy(previewPanel);
		Destroy(cancelBtn);
		Destroy(saveBtn);
		Destroy(shareBtn);
	}
}
