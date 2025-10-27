using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float Health;
    public GameObject NewPlayer;
    
    [Header("影子重叠死亡判定")]
    [Tooltip("与影子距离小于此值时触发死亡（单位：Unity单位）")]
    public float shadowDeathDistance = 0.3f;
    
    void Start()
    {
        Health = 100;
        isDead = false;
    }

    private bool isDead = false; // 防止重复触发死亡
    
    void Update()
    {
        // 检查是否是影子对象
        var recorder = GetComponent<PlayerActionRecorder>();
        if (recorder == null)
        {
            // 这是影子对象，不处理死亡逻辑
            return;
        }
        
        // 使用overlap检测与影子是否重叠
        CheckShadowOverlap();
        
        if (Health <= 0 && !isDead) {
            isDead = true;
            PlayerDie();
        }
    }
    
    // 检测与影子是否重叠
    private void CheckShadowOverlap()
    {
        // 获取所有ShadowReplayer对象
        ShadowReplayer[] allShadows = FindObjectsOfType<ShadowReplayer>();
        
        foreach (var shadow in allShadows)
        {
            // 检查距离
            float distance = Vector2.Distance(transform.position, shadow.transform.position);
            
            // 如果距离小于设定值，触发死亡
            // 注意：只在影子真正开始回放后才检测，避免初始瞬移导致的误判
            if (distance < shadowDeathDistance && !isDead)
            {
                Debug.Log("Player与影子重叠，触发死亡！距离: " + distance);
                Health = 0;
                return;
            }
        }
    }
    public void PlayerDie()
    {
        var items = Object.FindObjectsByType<ItemMagnet>(FindObjectsSortMode.None);

        foreach (var item in items)
        {
            item.ResetItem();
        }
        //PlayerRespawn();
        Health = 100;
        StartCoroutine(PlayerDying());
        Debug.Log("Dead");
        
    }
    public void PlayerRespawn()
    {
        var recorder = gameObject.GetComponent<PlayerActionRecorder>();
        if (recorder != null)
        {
            recorder.Load(); 
        }
        else
        {
            Debug.LogWarning("Î´ÕÒµ½ PlayerActionRecorder ");
        }
        Health = 100;
        isDead = false; // 重置死亡标志，允许再次死亡
        gameObject.GetComponent<PlayerTimeScale>().StopTimeScale();
        
        // æ¢å¤ADé®æ§å¶ï¼é²æ­¢å¤æ´»åä¸ç´å¤±æ§
        gameObject.GetComponent<PlayerMovement>().SetADOutOfControl(false, false);


    }

    private IEnumerator PlayerDying(float i=3)
    {
        GameObject saveobject = GameObject.FindGameObjectWithTag("Save");
        TextMeshProUGUI Timingtext=saveobject.GetComponent<SaveMono>().GameObjecttext.GetComponent<TextMeshProUGUI>();

        Timingtext.text = "5";
        gameObject.GetComponent<PlayerMovement>().InhibitInput = true;


        while (i >0)
        {
            
                Timingtext.text = ((int)i).ToString();
            
                gameObject.transform.position = new Vector2(100, 58852f);
            i-=Time.deltaTime;
            yield return null;

        }


        Timingtext.text = "";
        PlayerRespawn();
        gameObject.GetComponent<PlayerMovement>().InhibitInput = false;
    }

}
