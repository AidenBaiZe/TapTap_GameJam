using UnityEngine;
using UnityEngine.VFX;

public class HorizentalForce : MonoBehaviour
{
    [SerializeField] private VisualEffect visualEffect;
    
    void Start()
    {
        // 如果没有手动指定VisualEffect，尝试从当前物体获取
        if (visualEffect == null)
        {
            visualEffect = GetComponent<VisualEffect>();
        }
        
        // 获取父物体的边界信息
        SetBoundsToVisualEffect();
    }
    
    private void SetBoundsToVisualEffect()
    {
        if (visualEffect == null)
        {
            Debug.LogWarning("VisualEffect组件未找到！");
            return;
        }
        
        // 获取父物体
        Transform parentTransform = transform.parent;
        if (parentTransform == null)
        {
            Debug.LogWarning("没有找到父物体！");
            return;
        }
        
        // 获取父物体的Transform信息
        Vector3 position = parentTransform.position;
        Vector3 scale = parentTransform.localScale;
        
        // 获取父物体的UnomalyForceArea组件
        UnomalyForceArea forceArea = parentTransform.GetComponent<UnomalyForceArea>();
        Vector3 bottomBound, topBound;
        
        if (forceArea != null)
        {
            // 获取Force值并赋给VisualEffect的direction属性
            visualEffect.SetVector3("direction", forceArea.force);
            Debug.Log($"设置方向 - direction: {forceArea.force}");
            
            // 根据force的x和y值决定不同的设置
            if (forceArea.force.y == 0 && forceArea.force.x != 0)
            {
                // force.y为0而x不为0时
                visualEffect.SetVector3("Scale", new Vector3(10f, 0.5f, 1f));
                if (forceArea.force.x < 0)
                {
                    // force.y为负数时
                    bottomBound = new Vector3(position.x + scale.x * 0.5f, position.y - scale.y * 0.5f, position.z);
                    topBound = new Vector3(position.x + scale.x * 0.5f, position.y + scale.y * 0.5f, position.z);
                }
                else
                {
                    // force.y为正数时
                    bottomBound = new Vector3(position.x - scale.x * 0.5f, position.y - scale.y * 0.5f, position.z);
                    topBound = new Vector3(position.x - scale.x * 0.5f, position.y + scale.y * 0.5f, position.z);
                }
                Debug.Log("设置垂直力场模式 - Scale: (10, 0.5, 1)");
            }
            else if (forceArea.force.x == 0 && forceArea.force.y != 0)
            {
                // force.x为0而y不为0时
                visualEffect.SetVector3("Scale", new Vector3(0.5f, 10f, 1f));
                if (forceArea.force.y < 0)
                {
                    // force.y为负数时
                    bottomBound = new Vector3(position.x - scale.x * 0.5f, position.y + scale.y * 0.5f, position.z);
                    topBound = new Vector3(position.x + scale.x * 0.5f, position.y + scale.y * 0.5f, position.z);
                }
                else
                {
                    // force.y为正数时
                    bottomBound = new Vector3(position.x - scale.x * 0.5f, position.y - scale.y * 0.5f, position.z);
                    topBound = new Vector3(position.x + scale.x * 0.5f, position.y - scale.y * 0.5f, position.z);
                }
                Debug.Log("设置默认力场模式 - Scale: (1, 1, 1)");
                
            }
            else
            {
                // 其他情况使用默认设置
                visualEffect.SetVector3("Scale", new Vector3(0.5f, 10f, 1f));
                if (forceArea.force.y < 0)
                {
                    // force.y为负数时
                    bottomBound = new Vector3(position.x - scale.x * 0.5f, position.y + scale.y * 0.5f, position.z);
                    topBound = new Vector3(position.x + scale.x * 0.5f, position.y + scale.y * 0.5f, position.z);
                }
                else
                {
                    // force.y为正数时
                    bottomBound = new Vector3(position.x - scale.x * 0.5f, position.y - scale.y * 0.5f, position.z);
                    topBound = new Vector3(position.x + scale.x * 0.5f, position.y - scale.y * 0.5f, position.z);
                }
                Debug.Log("设置默认力场模式 - Scale: (1, 1, 1)");
                
            }
        }
        else
        {
            Debug.LogWarning("父物体没有UnomalyForceArea组件！");
            // 默认情况
            bottomBound = new Vector3(position.x - scale.x * 0.5f, position.y + scale.y * 0.5f, position.z);
            topBound = new Vector3(position.x + scale.x * 0.5f, position.y + scale.y * 0.5f, position.z);
        }
        
        // 根据force类型计算time值
        float timeValue;
        if (forceArea != null)
        {
            if (forceArea.force.y == 0 && forceArea.force.x != 0)
            {
                // 水平力场：使用scale.x计算
                timeValue = scale.x * 1.5f / 80f;
            }
            else
            {
                // 垂直力场或其他：使用scale.y计算
                timeValue = scale.y * 1.5f / 80f;
            }
        }
        else
        {
            // 默认情况
            timeValue = scale.y * 1.5f / 80f;
        }
        Vector3 direction =  forceArea.force.normalized;
        visualEffect.SetVector3("direction", direction);
        // 将值赋给VisualEffect的Properties
        visualEffect.SetVector3("Top", topBound);
        visualEffect.SetVector3("Bottom", bottomBound);
        visualEffect.SetFloat("Time", timeValue);
        
        Debug.Log($"设置边界 - Top: {topBound}, Bottom: {bottomBound}");
        Debug.Log($"设置时间 - time: {timeValue}");
    }
}
