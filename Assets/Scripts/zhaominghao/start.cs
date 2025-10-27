using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // �� Inspector ����ק VideoPlayer ���������
    public Canvas uiCanvas;         // �� Inspector ����ק������ʼ��ť�� Canvas

    public void StartGame()
    {
        Debug.Log("��ʼ������Ƶ...");

        // �������� Canvas���� GameObject.Find ���ɿ���
        if (uiCanvas != null)
        {
            uiCanvas.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("MainMenuManager: uiCanvas δ���䣬�޷����� Canvas��");
        }

        if (videoPlayer == null)
        {
            Debug.LogError("MainMenuManager: videoPlayer δ���䣬�޷�������Ƶ��");
            return;
        }

        // ��ֹ�ظ����Ļص�
        videoPlayer.loopPointReached -= OnVideoFinished;
        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("��Ƶ������ϣ���ת������");
        SceneManager.LoadScene("startscene");
    }
}