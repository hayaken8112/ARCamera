﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class CancelButton : PreviewUI {

	// Use this for initialization
	void Start () {
		StartCoroutine(InitPreviewUI());
	}
	

}
