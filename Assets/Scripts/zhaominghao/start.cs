using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 在 Inspector 中拖拽 VideoPlayer 组件到这里
    public Canvas uiCanvas;         // 在 Inspector 中拖拽包含开始按钮的 Canvas

    public void StartGame()
    {
        Debug.Log("开始播放视频...");

        // 隐藏整个 Canvas（比 GameObject.Find 更可靠）
        if (uiCanvas != null)
        {
            uiCanvas.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("MainMenuManager: uiCanvas 未分配，无法隐藏 Canvas。");
        }

        if (videoPlayer == null)
        {
            Debug.LogError("MainMenuManager: videoPlayer 未分配，无法播放视频。");
            return;
        }

        // 防止重复订阅回调
        videoPlayer.loopPointReached -= OnVideoFinished;
        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("视频播放完毕，跳转到场景");
        SceneManager.LoadScene("startscene");
    }
}