using UnityEngine;

public class ItemMagnet : MonoBehaviour
{
    public float attractDistance = 2f; 
    public float attractSpeed = 5f;    
    private Transform player;
    private bool attract = false;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }
    void Update()
    {
        if (player != null && attract)
        {
            // ����������ƶ�
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                attractSpeed * Time.deltaTime
            );

            // ����ܽ�ʱ������
            if (Vector2.Distance(transform.position, player.position) < 0.2f)
            {
                player.GetComponent<ItemCollector>()?.CollectItem(gameObject);
                attract = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ��ҽ���������Χ
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            attract = true;
        }
    }
    public void ResetItem()
    {
        transform.position = initialPosition;
        attract = false;
        player = null;
    }
}