using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class MyVideoPlayer : MonoBehaviour
{
    UnityEngine.Video.VideoPlayer videoPlayer;

    private Subject<string> videoURLSubject = new Subject<string>();
    public IObservable<string> OnVideoURLChanged
    {
        get { return videoURLSubject; }
    }
    /* slider
    // public GameObject slider;
    // MySlider sld;
    // private bool sldIsPressed = false;
    */
    void Start()
    {
        // Will attach a VideoPlayer to the main camera.
        GameObject panel = GameObject.Find("PreviewPanel(Clone)");
        // slider = GameObject.Find("Slider");
        // sld = slider.GetComponent<MySlider>();

        // VideoPlayer automatically targets the camera backplane when it is added
        // to a camera object, no need to change videoPlayer.targetCamera.
        videoPlayer = panel.AddComponent<UnityEngine.Video.VideoPlayer>();

        // By default, VideoPlayers added to a camera will use the far plane.
        // Let's target the near plane instead.
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;
        // videoPlayer.targetTexture = Resources.Load("VideoPreviewRendererTexture") as RenderTexture;

        // This will cause our scene to be visible through the video being played.
        videoPlayer.targetCameraAlpha = 1.0f;
        videoPlayer.aspectRatio = UnityEngine.Video.VideoAspectRatio.Stretch;

        // Set the video to play. URL supports local absolute or relative paths.
        // Here, using absolute.
        // videoPlayer.url = "Assets/MyAssets/videos/test.mp4";
        videoURLSubject.Subscribe(videoURL => 
        {
            videoPlayer.url = videoURL;
            videoPlayer.Play();
            Debug.Log("Subject,"+ videoPlayer.url);
        });

        // Skip the first 100 frames.
        videoPlayer.frame = 100;

        // Restart from beginning when done.
        videoPlayer.isLooping = true;

        // Start playback. This means the VideoPlayer may have to prepare (reserve
        // resources, pre-load a few frames, etc.). To better control the delays
        // associated with this preparation one can use videoPlayer.Prepare() along with
        // its prepareCompleted event.
        // videoPlayer.Play();
    }

    void Update(){
        //sld.value = videoPlayer.time
        //Debug.Log(videoPlayer.frameCount);
        /* 
        if (slider != null && videoPlayer.isPlaying){
        if (sld.isPressed != true && videoPlayer.frameCount > 0) {
            SetSlider(videoPlayer.frame, videoPlayer.frameCount);
        } else if (sld.isPressed) {
            videoPlayer.Pause();
            MoveSlider();
        }
        if (Input.GetMouseButtonDown(0)){
            Debug.Log("clicked");
            videoPlayer.Prepare();
            videoPlayer.Play();
        }
        Debug.Log(sld.isPressed);
    }
        */
    }

/*  slider
    void SetSlider(long currentFrame, ulong numOfFrame)
    {
        sld.value = (float)currentFrame / numOfFrame;
    }

    void MoveSlider()
    {
        videoPlayer.frame = (long)(sld.value * videoPlayer.frameCount);
    }
*/

    public void SetVideoURL(string s){
        videoURLSubject.OnNext(s);
    }
    public string GetVideoURL() {
        return videoPlayer.url;
    }
    public void SetTargetTexture(RenderTexture tex) {
        videoPlayer.targetTexture = tex;
    }

}