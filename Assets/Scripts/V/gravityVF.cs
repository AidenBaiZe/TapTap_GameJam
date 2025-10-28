using UnityEngine;
using UnityEngine.VFX;

public class gravityVF : MonoBehaviour
{
    [SerializeField] private VisualEffect visualEffect;
    
    void Start()
    {
        // 如果没有手动指定VisualEffect，尝试从当前物体获取
        if (visualEffect == null)
        {
            visualEffect = GetComponent<VisualEffect>();
        }
        
        // 设置VisualEffect属性
        SetVisualEffectProperties();
    }
    
    private void SetVisualEffectProperties()
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
        
        // 获取父物体的scale
        Vector3 scale = parentTransform.localScale;
        
        // 获取父物体的position
        Vector3 position = parentTransform.position;
        
        // 计算属性值
        float xValue = scale.x * 10f;
        float lifetimeValue = Mathf.Sqrt(scale.y *2f/ 60f);
        float bottomValue = position.y - scale.y*0.5f;
        float bottom1Value = bottomValue - scale.y * 0.1f;
        
        // 设置VisualEffect的Properties
        visualEffect.SetFloat("positionx", parentTransform.position.x);
        visualEffect.SetFloat("x", xValue);
        visualEffect.SetFloat("lifetime", lifetimeValue);
        visualEffect.SetFloat("Bottom", bottomValue);
        visualEffect.SetFloat("Bottom1", bottom1Value);
        
        Debug.Log($"设置重力效果 - x: {xValue}, lifetime: {lifetimeValue}, Bottom: {bottomValue}, Bottom1: {bottom1Value}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
