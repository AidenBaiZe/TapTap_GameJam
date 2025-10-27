using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 让影子基于 PlayerActionRecorder 的历史数据进行“延迟回放”。
/// - 不依赖单例到影子自身，不参与存档/输入/物理
/// - 若找不到记录，回退为直接跟随主角 Transform（可选）
/// 用法：挂到影子物体上，影子不要使用 Player 标签、不要挂玩家逻辑脚本。
/// </summary>
[DisallowMultipleComponent]
public class ShadowReplayer : MonoBehaviour
{
    [Header("回放设置")]
    [Tooltip("影子相对玩家的时间延迟（秒）")] public float timeDelay = 1.0f;
    [Tooltip("是否只在与记录相同的场景时才回放")] public bool requireSameScene = true;
    [Tooltip("是否复制旋转")] public bool copyRotation = true;
    [Tooltip("找不到记录时是否直接跟随玩家位置")] public bool fallbackFollowPlayer = true;
    [Tooltip("跟随时的偏移（仅回退跟随模式下生效）")] public Vector3 followOffset = Vector3.zero;

    private Rigidbody2D rb;
    private Transform player;
    private bool hasStarted = false; // 标记影子是否已开始回放
    private float startDelayTimer = 0f; // 启动延迟计时器

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // 影子不参与物理，避免干扰玩法；若需要保留刚体，设置为 kinematic
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        
        // 禁用影子上的PlayerStatus组件，防止影子触发死亡
        var playerStatus = GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.enabled = false;
        }
        
        // 确保影子有碰撞器且设置为Trigger，用于检测与player的重叠
        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        
        // 初始化延迟计时器
        startDelayTimer = timeDelay;
    }

    private void Start()
    {
        CachePlayer();
    }

    private void CachePlayer()
    {
        var playerGo = GameObject.FindGameObjectWithTag("Player");
        if (playerGo != null) player = playerGo.transform;
    }

    private void Update()
    {
        // 如果还没有开始，等待延迟时间
        if (!hasStarted)
        {
            startDelayTimer -= Time.deltaTime;
            if (startDelayTimer <= 0)
            {
                hasStarted = true;
            }
            else
            {
                // 等待期间不移动影子，避免触发初始死亡
                return;
            }
        }
        
        // 优先使用玩家动作记录器的历史帧
        var recorder = PlayerActionRecorder.Instance;
        if (recorder != null)
        {
            var frame = recorder.GetActionFrame(timeDelay);
            if (frame != null && (!requireSameScene || frame.sceneName == SceneManager.GetActiveScene().name))
            {
                ApplyFrame(frame);
                return;
            }
        }

        // 回退：直接跟随玩家（但现在不应该执行到这里，因为没有历史记录时会等待）
        if (fallbackFollowPlayer)
        {
            if (player == null) CachePlayer();
            if (player != null)
            {
                // 使用偏移，避免完全重叠
                Vector3 safeOffset = followOffset.magnitude > 0.1f ? followOffset : new Vector3(0, 0, 0);
                MoveTo(player.position + safeOffset, copyRotation ? player.rotation : transform.rotation);
            }
        }
    }

    private void ApplyFrame(PlayerActionFrame frame)
    {
        MoveTo(frame.position, copyRotation ? frame.rotation : transform.rotation);
    }

    private void MoveTo(Vector3 position, Quaternion rotation)
    {
        if (rb != null)
        {
            rb.MovePosition(position);
            if (copyRotation) rb.SetRotation(rotation);
        }
        else
        {
            transform.position = position;
            if (copyRotation) transform.rotation = rotation;
        }
    }
}