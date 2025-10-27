using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 继续游戏按钮脚本
/// 从最近的存档文件中加载场景和玩家位置
/// </summary>
public class ContinueGameButton : MonoBehaviour
{
    [Header("淡入淡出设置")]
    [SerializeField] private float fadeDuration = 1f; // 淡化时长

    private Button continueButton;
    private CanvasGroup fadeCanvasGroup;
    private SceneTransitionRunner runner; // 独立的协程承载者，避免按钮被销毁后无法启动协程
    private const string PLAYER_DATA_FILE_NAME = "PlayerData";

    private void Start()
    {
        continueButton = GetComponent<Button>();
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueGameClicked);
        }
        else
        {
            Debug.LogError("ContinueGameButton 脚本必须挂载在 Button 组件上!");
        }
    }

    private void OnContinueGameClicked()
    {
        // 检查是否存在存档
        var saveData = SaveSystemTutorial.SaveSystem.LoadFromJson<PlayerActionFrame>(PLAYER_DATA_FILE_NAME);

        if (saveData != null)
        {
            Debug.Log($"找到存档: 场景={saveData.sceneName}, 位置={saveData.position}");
            StartCoroutine(LoadSavedGameWithFade(saveData));
        }
        else
        {
            Debug.LogWarning("没有找到存档文件，无法继续游戏");
            // 可以在这里添加UI反馈，比如显示提示信息
        }
    }

    private IEnumerator LoadSavedGameWithFade(PlayerActionFrame saveData)
    {
        // 让本脚本在场景切换过程中不被销毁，否则协程会中断导致无法淡入
        DontDestroyOnLoad(gameObject);

    // 创建淡化画布（同时创建一个独立的 Runner 来承载淡入协程）
    fadeCanvasGroup = CreateFadeCanvas();

        // 淡出（黑屏）
        yield return StartCoroutine(FadeToBlack(fadeDuration));

        bool sceneHandled = false;

        // 注册回调：场景完全加载后再定位玩家并开始淡入
        SceneManager.sceneLoaded += OnSceneLoaded;

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            try
            {
                Debug.Log($"场景加载完成回调: {scene.name}");

                // 使用 PlayerActionRecorder 来加载玩家状态（优先）
                PlayerActionRecorder playerRecorder = FindObjectOfType<PlayerActionRecorder>();
                if (playerRecorder != null)
                {
                    playerRecorder.Load();
                }
                else
                {
                    // 如果找不到 PlayerActionRecorder，使用备用方法设置玩家位置
                    SetPlayerPositionFromSave(saveData);
                }

                // 开始淡入：使用持久化的 Runner 启动协程，避免 ContinueGameButton 已被销毁
                if (runner != null)
                {
                    runner.StartFadeIn(fadeCanvasGroup, fadeDuration);
                }
                else
                {
                    Debug.LogError("SceneTransitionRunner 丢失，无法开始淡入");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"继续游戏定位阶段发生异常: {ex}");
            }
            finally
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                sceneHandled = true;
            }
        }

        // 加载保存的场景
        SceneManager.LoadScene(saveData.sceneName);

        // 等待回调处理完成（避免协程提前结束）
        yield return new WaitUntil(() => sceneHandled);

        // 过渡逻辑已交给 Runner，当前按钮对象可安全销毁
        if (this != null)
        {
            Destroy(gameObject);
        }
    }

    private void SetPlayerPositionFromSave(PlayerActionFrame saveData)
    {
        // 查找场景中的玩家
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 设置位置和旋转
            player.transform.position = saveData.position;
            player.transform.rotation = saveData.rotation;

            // 重置 Rigidbody2D 速度
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // 重新启用玩家控制
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.InhibitInput = false;
            }

            Debug.Log($"玩家已传送至: {saveData.position}");
        }
        else
        {
            Debug.LogWarning("场景中找不到玩家!");
        }
    }

    private CanvasGroup CreateFadeCanvas()
    {
        // 创建 Canvas
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // 确保在最上层

        // 添加 CanvasScaler 确保适配不同分辨率
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // 添加 GraphicRaycaster
        canvasObj.AddComponent<GraphicRaycaster>();

        // 创建淡化背景 Image
        GameObject fadeImageObj = new GameObject("FadeImage");
        fadeImageObj.transform.SetParent(canvasObj.transform, false);

        Image fadeImage = fadeImageObj.AddComponent<Image>();
        fadeImage.color = Color.black; // 设置为黑色

        RectTransform rectTransform = fadeImageObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

    CanvasGroup canvasGroup = canvasObj.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f; // 初始透明
        canvasGroup.blocksRaycasts = true; // 防止点击穿透

    // 在画布上挂一个 Runner 用来跨场景承载淡入协程
    runner = canvasObj.AddComponent<SceneTransitionRunner>();

        DontDestroyOnLoad(canvasObj);

        Debug.Log("淡化 Canvas 创建成功");

        return canvasGroup;
    }

    private IEnumerator FadeToBlack(float duration)
    {
        Debug.Log("淡出开始");
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            fadeCanvasGroup.alpha = alpha;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f; // 确保完全黑屏
        Debug.Log("淡出完成");
    }

    private IEnumerator FadeInFromBlack(float duration)
    {
        Debug.Log("淡入开始");
        float elapsed = 0f;

        if (fadeCanvasGroup == null)
        {
            Debug.LogError("fadeCanvasGroup 为 null");
            yield break;
        }

        // 确保开始时是完全黑屏
        fadeCanvasGroup.alpha = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsed / duration));
            fadeCanvasGroup.alpha = alpha;
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f; // 完全透明
        Debug.Log("淡入完成");

        // 销毁淡化画布
        if (fadeCanvasGroup != null)
        {
            Destroy(fadeCanvasGroup.gameObject);
        }
    }

    // 使用 Runner 启动的淡入流程，已改为在 SceneTransitionRunner 中实现

    /// <summary>
    /// 检查是否存在存档文件
    /// </summary>
    public bool HasSaveFile()
    {
        var saveData = SaveSystemTutorial.SaveSystem.LoadFromJson<PlayerActionFrame>(PLAYER_DATA_FILE_NAME);
        return saveData != null;
    }

}

// 跨场景协程承载者：专门负责跨场景的淡入动画，避免依赖按钮对象的生命周期
public class SceneTransitionRunner : MonoBehaviour
{
    public void StartFadeIn(CanvasGroup cg, float duration)
    {
        if (cg == null)
        {
            Debug.LogError("SceneTransitionRunner.StartFadeIn: CanvasGroup 为 null");
            return;
        }
        StartCoroutine(FadeInCo(cg, duration));
    }

    private IEnumerator FadeInCo(CanvasGroup cg, float duration)
    {
        float elapsed = 0f;
        cg.alpha = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsed / duration));
            cg.alpha = alpha;
            yield return null;
        }

        cg.alpha = 0f;
        Debug.Log("淡入完成 (Runner)");

        // 动画结束后销毁画布
        if (cg != null)
        {
            Destroy(cg.gameObject);
        }
    }
}
