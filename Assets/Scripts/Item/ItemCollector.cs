using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private int items = 0;

    // ��������������
    public void CollectItem(GameObject item)
    {
        Destroy(item);
        items++;
        Debug.Log($"Items collected: {items}");
    }
}
