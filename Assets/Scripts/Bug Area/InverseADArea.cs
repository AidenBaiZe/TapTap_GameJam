using UnityEngine;
using System.Collections;

public class InverseADArea : MonoBehaviour
{
    [Header("Inverse AD Player")]
    [SerializeField] private InverseADPlayer inverseADPlayer;
    
    [SerializeField] private float waitTime = 0.2f;
    private PlayerMovement playerMovement;
    private bool playerInArea;
    void Start()
    {
        // 如果没有手动指定InverseADPlayer，尝试自动查找
        if (inverseADPlayer == null)
        {
            inverseADPlayer = GameObject.FindGameObjectWithTag("CanvasPlayer").GetComponent<InverseADPlayer>();// FindObjectOfType<InverseADPlayer>();

            if (inverseADPlayer == null)
            {
                Debug.LogError("InverseADPlayer not found! Please assign it in the inspector.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查进入的对象是否是玩家
        if (other.CompareTag("Player"))
        {
            playerInArea = true;
            
            // 设置inverse为true，实现正向移动（LeftArrow在左边，RightArrow在右边）
            if (inverseADPlayer != null)
            {
                inverseADPlayer.SetInverseAndStart(true);
            }
            
            // 同时设置PlayerMovement的InverseAD
            StartCoroutine(InverseAD());
        }
    }

    // 当玩家碰撞体离开区域时调用
    private void OnTriggerExit2D(Collider2D other)
    {
        // 检查离开的对象是否是玩家
        if (other.CompareTag("Player"))
        {
            playerInArea = false;
            
            // 设置inverse为false，实现逆向移动（RightArrow在左边，LeftArrow在右边）
            if (inverseADPlayer != null)
            {
                inverseADPlayer.SetInverseAndStart(false);
            }
            
            // 同时重置PlayerMovement的InverseAD
            StartCoroutine(ResetAD());
        }
    }
    
    private IEnumerator InverseAD()
    {
        float time = 0;
        while(time < waitTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerMovement.InverseAD = true;
    }
    
    private IEnumerator ResetAD()
    {
        float time = 0;
        while(time < waitTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerMovement.InverseAD = false;
    }
}