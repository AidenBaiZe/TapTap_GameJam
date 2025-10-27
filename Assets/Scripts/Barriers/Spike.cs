using UnityEngine;

public class Spike : MonoBehaviour
{
    private PlayerStatus playerStatus;
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
            playerStatus = other.GetComponent<PlayerStatus>();


            playerInArea = true;

            playerStatus.Health -= 101;


        }
    }
   
    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    // ����뿪�Ķ����Ƿ������
    //    if (other.CompareTag("Player") && playerInArea)
    //    {

    //        // �ָ�ԭʼ��
    //        playerMovement.InverseAD = false;


    //        playerInArea = false;
    //        playerMovement = null;
    //    }
    //}
}
