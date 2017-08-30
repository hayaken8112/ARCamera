﻿using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine;

public class ScreenShot : MonoBehaviour {
	public Camera ArCam;

	public void CaptchaScreen()
	{
		Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		RenderTexture rt = new RenderTexture(screenShot.width, screenShot.height, 24);
		RenderTexture prev = ArCam.targetTexture;
		ArCam.targetTexture = rt;
		ArCam.Render();
		ArCam.targetTexture = prev;
		RenderTexture.active = rt;
		screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0, 0);
		screenShot.Apply();

		byte[] bytes = screenShot.EncodeToPNG();
		UnityEngine.Object.Destroy(screenShot);

		string fileName = "screenshot.png";

		File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);
	}
}