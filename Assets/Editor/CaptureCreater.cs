using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CaptureCreater : EditorWindow
{

    // このディレクトリ以下のprefabのキャプチャを全て取得
    UnityEngine.Object searchDirectory;
    List<GameObject> objList = new List<GameObject>();
    string dirPath = "Captures/"; // 出力先ディレクトリ(Assets/Captures/以下に出力されます)
    int width = 100; // キャプチャ画像の幅
    int height = 100; // キャプチャ画像の高さ

    [MenuItem("Window/CaptureCreater")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow (typeof (CaptureCreater));
    }

    void OnGUI()
    {
        // Unity EditorのUI
        GUILayout.BeginHorizontal();
        GUILayout.Label("Search Directory : ", GUILayout.Width(110));
        searchDirectory = EditorGUILayout.ObjectField(searchDirectory, typeof(UnityEngine.Object), true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Save directory : ", GUILayout.Width(110));
        dirPath = (string)EditorGUILayout.TextField(dirPath);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Width : ", GUILayout.Width(110));
        width = EditorGUILayout.IntField(width);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Height : ", GUILayout.Width(110));
        height = EditorGUILayout.IntField(height);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button(new GUIContent("Capture")))
        {

            if(searchDirectory == null) return;

            // 出力先ディレクトリを生成
            if (!System.IO.File.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }

             objList.Clear();

            // 指定ディレクトリ内のprefabを全て取り出してListに入れる
            string replaceDirectoryPath = AssetDatabase.GetAssetPath(searchDirectory);
            string[] filePaths = Directory.GetFiles( replaceDirectoryPath , "*.*" );
            foreach(string filePath in filePaths)
            {
                GameObject obj =  AssetDatabase.LoadAssetAtPath( filePath , typeof(GameObject)) as GameObject;
                if(obj != null){
                     objList.Add(obj);
                }
            }

            EditorCoroutine.Start(Exec(objList));

            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
    }

    // List内のGameObjectを配置しつつ、キャプチャを取得
    IEnumerator Exec(List<GameObject> objList){
        foreach(GameObject obj in objList)
        {
            Debug.Log(obj.name);
            Texture2D texture2D = AssetPreview.GetAssetPreview(obj);
            byte[] bytes = texture2D.EncodeToPNG();
            System.IO.File.WriteAllBytes( dirPath  + obj.name + ".png", bytes );


            yield return new EditorCoroutine.WaitForSeconds(1.0f);

        }
    }

}