using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoamlyGravityArea : MonoBehaviour
{
    [Header("�����쳣����")]
    [SerializeField] private float abnormalGravity = -2f; // �쳣����ֵ

    private float originalGravity; // �洢ԭʼ����ֵ
    private PlayerMovement playerMovement; // ����ƶ��������
    [SerializeField] private Transform playerTransform; // ��ұ任���
    [SerializeField] private Quaternion originalPlayerRotation; // �洢���ԭʼ��ת

    // ��������ײ����봥����ʱ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ������Ķ����Ƿ������
        if (other.CompareTag("Player"))
        {
            // ��ȡPlayerMovement���
            playerMovement = other.GetComponent<PlayerMovement>();
            playerTransform = other.transform;

            if (playerMovement != null)
            {
                playerMovement.playerInUnomalyArea = true;

                originalPlayerRotation = Quaternion.Euler(0, 0, 0);              //playerTransform.rotation;              //ȷ��(0, 0, 0)Ϊ��ʼ��ת

                
                other.GetComponent<PlayerMovement>().FlipPlayerUpsideDown(0.1f, abnormalGravity);

                    
                
            }
        }
    }

    // ��������ײ���뿪������ʱ����
    private void OnTriggerExit2D(Collider2D other)
    {
        // ����뿪�Ķ����Ƿ������
        if (other.CompareTag("Player") )
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement.playerInUnomalyArea)
            {



                other.GetComponent<PlayerMovement>().RestorePlayerRotation(0.1f);




                playerMovement.playerInUnomalyArea = false;
                playerMovement = null;
            }

        }
    }

    private void OnDrawGizmos()
    {
        // ���������쳣����Ŀ��ӻ���ɫ
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // ��͸����ɫ
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null)
        {
            // ������ײ�����ͻ��Ʋ�ͬ����״
            if (collider is BoxCollider2D)
            {
                BoxCollider2D boxCollider = (BoxCollider2D)collider;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(boxCollider.offset, boxCollider.size);

                // ���Ʊ߿�
                Gizmos.color = new Color(1f, 0.5f, 0f, 0.8f);
                Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
            }
            else if (collider is CircleCollider2D)
            {
                CircleCollider2D circleCollider = (CircleCollider2D)collider;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawSphere(circleCollider.offset, circleCollider.radius);

                // ���Ʊ߿�
                Gizmos.color = new Color(1f, 0.5f, 0f, 0.8f);
                Gizmos.DrawWireSphere(circleCollider.offset, circleCollider.radius);
            }
        }

        // ������������ָʾ��
        DrawGravityDirectionIndicator();
    }

    // ������������ָʾ��
    private void DrawGravityDirectionIndicator()
    {
        Vector3 areaCenter = transform.position;
        Vector3 gravityDirection = (abnormalGravity >= 0) ? Vector3.down : Vector3.up;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(areaCenter, gravityDirection * 2f);

        // ���Ƽ�ͷ
        Vector3 arrowTip = areaCenter + gravityDirection * 2f;
        Vector3 perpendicular = new Vector3(-gravityDirection.y, gravityDirection.x, 0) * 0.3f;
        Gizmos.DrawLine(arrowTip, arrowTip - gravityDirection * 0.5f + perpendicular);
        Gizmos.DrawLine(arrowTip, arrowTip - gravityDirection * 0.5f - perpendicular);
    }
}