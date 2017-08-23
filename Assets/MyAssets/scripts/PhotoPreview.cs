using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UniRx;
using ARCamera;

public class PhotoPreview : MonoBehaviour 
{

    public string photoPath;
    GameObject previewPanel;
    GameObject videoPlayer;
    public GameObject videoPlayerPrefab;
    RawImage img;
    // Use this for initialization
    void Start()
    {
		previewPanel = GameObject.Find("PreviewPanel(Clone)");
		img = previewPanel.GetComponent<RawImage>();
        if (StateManager.Instance.currentState == States.PreviewPhoto) {
            img.texture = ReadTexture(PathManager.GetPhotoPath(), Screen.width, Screen.height);
        } else if (StateManager.Instance.currentState == States.PreviewVideo) {
            img.texture = Resources.Load("VideoPreviewRendererTexture") as RenderTexture;
            videoPlayer = Instantiate(videoPlayerPrefab);
            StartCoroutine(SetVideo()); // 次のフレームでVideoPlayerにURLをセットする
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    byte[] ReadPngFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

        bin.Close();

        return values;
    }

    Texture ReadTexture(string path, int width, int height)
    {
        byte[] readBinary = ReadPngFile(path);

        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(readBinary);

        return texture;
    }
    IEnumerator SetVideo()
    {
        yield return null;
        string videoPath = PathManager.GetVideoPath();
        videoPlayer.GetComponent<MyVideoPlayer>().SetVideoURL(videoPath);
    }
}
