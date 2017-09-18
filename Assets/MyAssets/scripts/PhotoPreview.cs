using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UniRx;
using ARCamera;

public class PhotoPreview : MonoBehaviour 
{

    GameObject previewPanel;
    GameObject videoPlayerObj;
    public GameObject videoPlayerPrefab;
    RawImage img;
    // Use this for initialization
    void Start()
    {
        var tex = new RenderTexture(512, 512, 16);
		img = this.gameObject.GetComponent<RawImage>();
        if (StateManager.Instance.currentState == States.PreviewPhoto) {
            img.texture = ReadTexture(PathManager.GetPhotoPath(), Screen.width, Screen.height);
        } else if (StateManager.Instance.currentState == States.PreviewVideo) {
            videoPlayerObj = Instantiate(videoPlayerPrefab);
            StartCoroutine(SetVideo(tex)); // 次のフレームでVideoPlayerにURLをセットする
            img.texture = tex;
        }
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
    IEnumerator SetVideo(RenderTexture tex)
    {
        var videoPlayer = videoPlayerObj.GetComponent<MyVideoPlayer>();
        yield return null;
        videoPlayer.SetTargetTexture(tex);
        string videoPath = PathManager.GetVideoPath();
        videoPlayer.SetVideoURL(videoPath);
    }
}
