using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class SceneTransition : SingletonMonoBehaviour<SceneTransition>
{
    public static void MainToPhotoPreview()
    {
		SceneManager.LoadScene("PhotoPreview");
		/* 
        SceneManager.LoadSceneAsync("PhotoPreview", LoadSceneMode.Single).AsObservable()
					.Subscribe(_Activator =>
					{
						var photoPreView = FindObjectOfType<PhotoPreview>() as PhotoPreview;
					// photoPreView.photoPath = TemporaryScreenshotPath();
					});
					*/
		Resources.UnloadUnusedAssets();
		GC.Collect();
    }

}