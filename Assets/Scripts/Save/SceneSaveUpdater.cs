using UnityEngine;
using UnityEngine.SceneManagement;
using SaveSystemTutorial;

public class SceneSaveUpdater : MonoBehaviour
{
    const string PLAYER_DATA_FILE_NAME = "PlayerData";

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���Ҵ���Save��ǩ�Ķ���
        GameObject saveObj = GameObject.FindGameObjectWithTag("Save");
        if (saveObj != null)
        {
            // �����µĴ浵����
            Vector3 pos = saveObj.transform.position;
            Quaternion rot = saveObj.transform.rotation;
            string sceneName = scene.name;

            // ֻ����λ�úͳ�����������������ݿ���չ��
            var frame = new PlayerActionFrame(
                Time.time, pos, rot, Vector2.zero, false, 0, false, Vector2.zero, sceneName
            );
            SaveSystem.SaveByJson(PLAYER_DATA_FILE_NAME, frame);
            Debug.Log($"���Զ������³����浵��λ�ã�{pos}��������{sceneName}");
        }
        else
        {
            Debug.LogWarning("δ�ҵ�����Save��ǩ�Ķ���δ�Զ����泡����Ϣ��");
        }
    }
}
