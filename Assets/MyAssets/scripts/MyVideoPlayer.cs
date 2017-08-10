using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyVideoPlayer : MonoBehaviour
{
    UnityEngine.Video.VideoPlayer videoPlayer;
    GameObject slider;
    MySlider sld;
    private bool sldIsPressed = false;
    void Start()
    {
        // Will attach a VideoPlayer to the main camera.
        GameObject camera = GameObject.Find("Main Camera");
        slider = GameObject.Find("Slider");
        sld = slider.GetComponent<MySlider>();

        // VideoPlayer automatically targets the camera backplane when it is added
        // to a camera object, no need to change videoPlayer.targetCamera.
        videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();

        // By default, VideoPlayers added to a camera will use the far plane.
        // Let's target the near plane instead.
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

        // This will cause our scene to be visible through the video being played.
        videoPlayer.targetCameraAlpha = 1.0f;

        // Set the video to play. URL supports local absolute or relative paths.
        // Here, using absolute.
        videoPlayer.url = "/Users/hayashikentarou/Desktop/test.mp4";

        // Skip the first 100 frames.
        videoPlayer.frame = 100;

        // Restart from beginning when done.
        videoPlayer.isLooping = true;

        // Each time we reach the end, we slow down the playback by a factor of 10.
        videoPlayer.loopPointReached += EndReached;

        // Start playback. This means the VideoPlayer may have to prepare (reserve
        // resources, pre-load a few frames, etc.). To better control the delays
        // associated with this preparation one can use videoPlayer.Prepare() along with
        // its prepareCompleted event.
        videoPlayer.Play();
    }

    void Update(){
        //sld.value = videoPlayer.time
        //Debug.Log(videoPlayer.frameCount);
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

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;
    }

    void SetSlider(long currentFrame, ulong numOfFrame)
    {
        sld.value = (float)currentFrame / numOfFrame;
    }

    void MoveSlider()
    {
        videoPlayer.frame = (long)(sld.value * videoPlayer.frameCount);
    }


}