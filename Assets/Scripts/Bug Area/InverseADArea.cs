using UnityEngine;

public class InverseADArea : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private bool playerInArea;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ������Ķ����Ƿ������
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>();

           
                playerInArea = true;

                playerMovement.InverseAD = true;

            
        }
    }

    // ��������ײ���뿪������ʱ����
    private void OnTriggerExit2D(Collider2D other)
    {
        // ����뿪�Ķ����Ƿ������
        if (other.CompareTag("Player") && playerInArea)
        {
            
                // �ָ�ԭʼ��
                playerMovement.InverseAD = false;
            

            playerInArea = false;
            playerMovement = null;
        }
    }
}
