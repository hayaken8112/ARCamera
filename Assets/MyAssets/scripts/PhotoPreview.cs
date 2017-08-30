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
    GameObject videoPlayer;
    public GameObject videoPlayerPrefab;
    RawImage img;
    // Use this for initialization
    void Start()
    {
		img = this.gameObject.GetComponent<RawImage>();
        if (StateManager.Instance.currentState == States.PreviewPhoto) {
            img.texture = ReadTexture(PathManager.GetPhotoPath(), Screen.width, Screen.height);
        } else if (StateManager.Instance.currentState == States.PreviewVideo) {
            img.texture = Resources.Load("VideoPreviewRendererTexture") as RenderTexture;
            videoPlayer = Instantiate(videoPlayerPrefab);
            StartCoroutine(SetVideo()); // 次のフレームでVideoPlayerにURLをセットする
            /* Observable.EveryUpdate().FirstOrDefault()
                                    .Subscribe(_ => {
                                        new WaitForSeconds(2);
                                        string videoPath = PathManager.GetVideoPath();
                                        videoPlayer.GetComponent<MyVideoPlayer>().SetVideoURL(videoPath);
                                    });*/
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
        yield return new WaitForSeconds(1);
        string videoPath = PathManager.GetVideoPath();
        videoPlayer.GetComponent<MyVideoPlayer>().SetVideoURL(videoPath);
    }
}
