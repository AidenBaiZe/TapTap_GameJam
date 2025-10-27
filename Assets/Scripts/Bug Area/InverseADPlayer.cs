using UnityEngine;

public class InverseADPlayer : MonoBehaviour
{
    [Header("Arrow Prefabs")]
    [SerializeField] public GameObject LeftArrow;
    [SerializeField] public GameObject RightArrow;
    
    [Header("Position Settings")]
    public Vector3 L; // 左位置
    public Vector3 R; // 右位置
    
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float delayTime = 0.2f; // 箭头生成后的停顿时间
    
    [Header("Inverse Control")]
    [SerializeField] public bool inverse = false; // 反转控制变量
    
    private GameObject currentLeftArrow;
    private GameObject currentRightArrow;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool created = false;
    [SerializeField] private bool isADOutOfControl = false; // 区分是AD失控系统还是左右互换系统
    private GameObject player;
    
    void Start()
    {
        created=false;
        isADOutOfControl=false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // 检查inverse变量是否发生变化
        
        if (created)
        {
            MoveArrows();
        }
    }
    
    /// <summary>
    /// 开始反转特效
    /// </summary>
    public void StartInverseEffect()
    {
        // 如果已经在移动，不重复启动
        if (isMoving)
        {
            return;
        }
        
        if(!created){
        // 生成箭头
        StartCoroutine(GenerateArrowsWithDelay(inverse));
        }
        
    }
    
    /// <summary>
    /// 设置inverse值并开始特效
    /// </summary>
    /// <param name="newInverse">新的inverse值</param>
    public void SetInverseAndStart(bool newInverse)
    {
        inverse = newInverse;
        StartInverseEffect();
    }
    
    /// <summary>
    /// 生成箭头并延迟后开始移动
    /// </summary>
    /// <param name="inverse">是否反转</param>
    /// <returns></returns>
    private System.Collections.IEnumerator GenerateArrowsWithDelay(bool inverse)
    {
        // 先生成箭头
        GenerateArrows(inverse);
        
        // 停顿指定时间
        yield return new WaitForSeconds(delayTime);
        Debug.Log("delayTime"+delayTime);
        if(created){
            isMoving = true;
        }
    }
    
    private void GenerateArrows(bool inverse)
    {
        created=true;
        
        Debug.Log("GenerateArrows"+inverse);
        if (inverse)
        {
            Debug.Log("inverse"+inverse);
                currentLeftArrow =GameObject.Instantiate(LeftArrow);//, (transform.position + new Vector3(player.transform.localScale.x*L.x,L.y,L.z)), Quaternion.identity, transform);
                currentRightArrow = GameObject.Instantiate(RightArrow);//, (transform.position + new Vector3(player.transform.localScale.x*R.x,R.y,R.z)), Quaternion.identity, transform);


            currentLeftArrow.transform.parent = transform;
        currentRightArrow.transform.parent = transform;
        currentLeftArrow.transform.position =  player.transform.position+new Vector3(L.x,L.y,L.z);
        currentRightArrow.transform.position=player.transform.position+ new Vector3(R.x,R.y,R.z);
        
        }
        else
        {
            Debug.Log("inverse"+inverse);
            Debug.Log("transform.position + L"+(transform.position + L));
            Debug.Log("transform.position + R"+(transform.position + R));

            currentRightArrow = GameObject.Instantiate(RightArrow);//, transform.position + new Vector3(player.transform.localScale.x*L.x,L.y,L.z), Quaternion.identity, transform);
            currentLeftArrow = GameObject.Instantiate(LeftArrow);//, transform.position + new Vector3(player.transform.localScale.x*R.x,R.y,R.z), Quaternion.identity, transform);
            currentLeftArrow.transform.parent = transform;
        currentRightArrow.transform.parent = transform;
            

  currentRightArrow.transform.position =  player.transform.position+new Vector3(L.x,L.y,L.z);
        currentLeftArrow.transform.position=player.transform.position+ new Vector3(R.x,R.y,R.z);

            // currentRightArrow = Instantiate(RightArrow, player.transform.position + new Vector3(L.x,L.y,L.z), Quaternion.identity, transform);
            // currentLeftArrow = Instantiate(LeftArrow, player.transform.position + new Vector3(R.x,R.y,R.z), Quaternion.identity, transform);
        }
    }
    
    
    private void MoveArrows()
    {
        Vector3 leftTarget, rightTarget;
       
        if (inverse)
        {
            leftTarget = player.transform.position + new Vector3(R.x,R.y,R.z);
            rightTarget = player.transform.position + new Vector3(L.x,L.y,L.z);
            Debug.Log("leftTarget"+leftTarget);
            Debug.Log("rightTarget"+rightTarget);
        }
        else
        {
            rightTarget = player.transform.position + new Vector3(R.x,R.y,R.z);
            leftTarget = player.transform.position + new Vector3(L.x,L.y,L.z);
        }
        
        if(isMoving){         
            // 计算左箭头的相对位置
            Vector3 leftRelativePos = currentLeftArrow.transform.position - player.transform.position;
            Vector3 leftDirection = (leftTarget - player.transform.position) - leftRelativePos;
            Vector3 newLeftRelativePos = leftRelativePos + leftDirection.normalized*leftDirection.magnitude * moveSpeed * Time.deltaTime;
            currentLeftArrow.transform.position = newLeftRelativePos + player.transform.position;

            // 计算右箭头的相对位置
            Vector3 rightRelativePos = currentRightArrow.transform.position - player.transform.position;
            Vector3 rightDirection = (rightTarget - player.transform.position) - rightRelativePos;
            Vector3 newRightRelativePos = rightRelativePos + rightDirection.normalized * rightDirection.magnitude * moveSpeed * Time.deltaTime;
            currentRightArrow.transform.position = newRightRelativePos + player.transform.position;
            
             Debug.Log("currentRightArrow.transform.positioncurrentRightArrow.transform.positioncurrentRightArrow.transform.positioncurrentRightArrow.transform.positioncurrentRightArrow.transform.positioncurrentRightArrow.transform.position"+currentRightArrow.transform.position+inverse+currentRightArrow.name);
             Debug.Log("currentLeftArrow.transform.positioncurrentLeftArrow.transform.positioncurrentLeftArrow.transform.positioncurrentLeftArrow.transform.positioncurrentLeftArrow.transform.positioncurrent"+currentLeftArrow.transform.position+inverse+currentLeftArrow.name);
            Debug.Log("transform.position"+transform.position);
        }
        if (currentLeftArrow != null && currentRightArrow != null)
        {
            bool leftReached = Vector3.Distance(currentLeftArrow.transform.position, leftTarget) < 0.5f;
            bool rightReached = Vector3.Distance(currentRightArrow.transform.position, rightTarget) < 0.5f;
            
            if (leftReached && rightReached)
            {
                Debug.Log("currentRightArrow.transform.position"+currentRightArrow.transform.position);
                Debug.Log("rightTarget"+rightTarget);
                Debug.Log("到达目标位置inverse"+inverse);
                ClearArrows();
                isMoving = false;
                Debug.Log("isMoving"+isMoving);
            }
        }
    }
    
    
    
    private void ClearArrows()
    {
        created=false;
        
        if (currentLeftArrow != null)
        {
            Destroy(currentLeftArrow);
            currentLeftArrow = null;
        }
        
        if (currentRightArrow != null)
        {
            Destroy(currentRightArrow);
            currentRightArrow = null;
        }
    }
    
    /// <summary>
    /// 停止当前特效
    /// </summary>
    public void StopEffect()
    {
        isMoving = false;
        isADOutOfControl = false;
        ClearArrows();
    }
    
    /// <summary>
    /// 检查是否正在播放特效
    /// </summary>
    public bool IsEffectPlaying()
    {
        return isMoving;
    }
    
    /// <summary>
    /// 开始AD键失控特效
    /// </summary>
    /// <param name="forceLeft">是否强制向左</param>
    public void StartADOutOfControlEffect(bool forceLeft)
    {
        // 如果已经有箭头在运行，先停止
        if (created)
        {
            StopEffect();
        }
        
        // 设置为AD失控模式
        isADOutOfControl = true;
        
        // 生成两个箭头
        GenerateArrowsForADOutOfControl();
        
        // 启动协程：0.2秒后只保留失控方向的箭头
        StartCoroutine(RetainOnlyOutOfControlArrow(forceLeft));
    }
    
    /// <summary>
    /// 停止AD键失控特效
    /// </summary>
    public void StopADOutOfControlEffect()
    {
        StopEffect();
    }
    
    /// <summary>
    /// 为AD失控生成箭头
    /// </summary>
    private void GenerateArrowsForADOutOfControl()
    {
        
         currentLeftArrow=GameObject.Instantiate(LeftArrow);
         currentRightArrow=GameObject.Instantiate(RightArrow);

        // currentLeftArrow = Instantiate(LeftArrow, player.transform.position + new Vector3(player.transform.localScale.x*L.x,L.y,L.z), Quaternion.identity, transform);
        // currentRightArrow = Instantiate(RightArrow, player.transform.position + new Vector3(player.transform.localScale.x*R.x,R.y,R.z), Quaternion.identity, transform);


        // currentLeftArrow = Instantiate(LeftArrow, transform.position + new Vector3(L.x,L.y,L.z), Quaternion.identity, transform);
        // currentRightArrow = Instantiate(RightArrow, transform.position + new Vector3(R.x,R.y,R.z), Quaternion.identity, transform);
        // currentLeftArrow = GameObject.Instantiate(LeftArrow);
        // currentRightArrow = GameObject.Instantiate(RightArrow);
        // currentLeftArrow.transform.position = transform.position + new Vector3(L.x,L.y,L.z);
        // currentRightArrow.transform.position = transform.position + new Vector3(R.x,R.y,R.z);
        // currentLeftArrow.transform.parent = transform;
        // currentRightArrow.transform.parent = transform;

        currentLeftArrow.transform.parent = transform;
        currentRightArrow.transform.parent = transform;
        currentLeftArrow.transform.position =  player.transform.position+new Vector3(L.x,L.y,L.z);
        currentRightArrow.transform.position=player.transform.position+ new Vector3(R.x,R.y,R.z);

        Debug.Log("currentLeftArrow.transform.position"+currentLeftArrow.transform.position);
        Debug.Log("currentRightArrow.transform.position"+currentRightArrow.transform.position);

        Debug.Log("AD失控：生成左右箭头");
    }
    
    /// <summary>
    /// 协程：0.2秒后只保留失控方向的箭头
    /// </summary>
    /// <param name="forceLeft">是否强制向左</param>
    /// <returns></returns>
    private System.Collections.IEnumerator RetainOnlyOutOfControlArrow(bool forceLeft)
    {
        // 等待0.2秒
        yield return new WaitForSeconds(0.2f);
        
        if (isADOutOfControl)
        {
           
                // 强制向左，保留左箭头，销毁右箭头
            //     if (currentRightArrow != null)
            //     {
            //         Destroy(currentRightArrow);
            //         currentRightArrow = null;
            //     }
            //     Debug.Log("AD失控：保留左箭头，销毁右箭头");
            // }
            // else
            // {
                // 强制向右，保留右箭头，销毁左箭头
                if (currentLeftArrow != null)
                {
                    Destroy(currentLeftArrow);
                    currentLeftArrow = null;
                }
                Debug.Log("AD失控：保留右箭头，销毁左箭头");
            
        }
    }
}