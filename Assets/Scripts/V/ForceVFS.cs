using UnityEngine;
using UnityEngine.VFX;

public class ForceVFS : MonoBehaviour
{
    [Header("组件引用")]
    [SerializeField] private VisualEffect visualEffect;
    
    private UnomalyForceArea parentForceArea;
    
    void Start()
    {
        // 获取父物体的UnomalyForceArea组件
        parentForceArea = transform.parent.GetComponent<UnomalyForceArea>();
        
        // 自动获取VisualEffect组件
        if (visualEffect == null)
            visualEffect = GetComponent<VisualEffect>();
            
        // 检查组件是否存在
        if (parentForceArea == null)
        {
            Debug.LogError($"ForceVFS: 父物体 {transform.parent.name} 上没有找到 UnomalyForceArea 组件！");
        }
        
        if (visualEffect == null)
        {
            Debug.LogError($"ForceVFS: 当前物体 {gameObject.name} 上没有找到 VisualEffect 组件！");
        }
    }

    void Update()
    {
        // 如果组件都存在，更新VisualEffect的direction属性
        if (parentForceArea != null && visualEffect != null)
        {
            Vector3 force = parentForceArea.force;
            Vector3 direction = force.normalized; // 获取力的方向（单位向量）
            
            // 设置VisualEffect的direction属性
            visualEffect.SetVector3("direction", direction);
        }
    }
}
