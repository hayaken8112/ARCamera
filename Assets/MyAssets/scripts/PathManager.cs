using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace ARCamera {
public class PathManager : SingletonMonoBehaviour<PathManager> {
    public const string temporaryScreenshotFilename = "screenshot.png";
    public static string GetPhotoPath () {
		string path;
		#if UNITY_EDITOR
		path = Application.streamingAssetsPath + "/" + "screenshot.png";
        #endif
		#if UNITY_IOS

        path = Application.persistentDataPath + "/" + temporaryScreenshotFilename;
		#endif
		return path;
     }
	public static string GetVideoPath ()
     {
         #if UNITY_EDITOR
         return Application.streamingAssetsPath + "/" + "test.mp4";
         #endif
         string everyplayDir;
         #if UNITY_IOS
 
         var root = new DirectoryInfo(Application.persistentDataPath).Parent.FullName;
         everyplayDir = root + "/tmp/Everyplay/session";
 
         #endif
 
         var files = new DirectoryInfo(everyplayDir).GetFiles("*.mp4", SearchOption.AllDirectories);
         var videoLocation = "";
 
         // Should only be one video, if there is one at all
         foreach (var file in files) {
             #if UNITY_ANDROID
             videoLocation = "file://" + file.FullName;
             #else
             videoLocation = file.FullName;
             #endif
             break;
         }
 
         return videoLocation;
     }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
}
