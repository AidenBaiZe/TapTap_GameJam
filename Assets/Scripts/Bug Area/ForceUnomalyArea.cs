using UnityEngine;

public class UnomalyForceArea : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] public Vector2 force ; // Ҫʩ�ӵ���
    [SerializeField] private bool restoreOnExit = true; // �뿪ʱ�Ƿ�ָ�ԭ��

    private Vector2 originalForce; // �洢ԭʼ��
    private PlayerMovement playerMovement; // ����ƶ��������
    private bool playerInArea = false; // ����Ƿ���������

    // ��������ײ����봥����ʱ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ������Ķ����Ƿ������
        if (other.CompareTag("Player"))
        {
            // ��ȡPlayerMovement���
            playerMovement = other.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerInArea = true;

                // ����ԭʼ����ֵ
                //originalForce = playerMovement.Force;

                // Ӧ���µ���
                playerMovement.Force += force;

                Debug.Log($"���������������� {originalForce} ��Ϊ {force}");
            }
        }
    }

    // ��������ײ���뿪������ʱ����
    private void OnTriggerExit2D(Collider2D other)
    {
        // ����뿪�Ķ����Ƿ������
        if (other.CompareTag("Player") && playerInArea)
        {
            if (restoreOnExit && playerMovement != null)
            {
                // �ָ�ԭʼ��
                playerMovement.Force -= force;
                Debug.Log($"�뿪�����������ָ�Ϊ {originalForce}");
            }

            playerInArea = false;
            playerMovement = null;
        }
    }

    // ��Inspector�п��ӻ�����������
    private void OnDrawGizmos()
    {
        // ������������Ŀ��ӻ���ɫ
        Gizmos.color = new Color(0f, 0.8f, 1f, 0.3f); // ��͸����ɫ
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
                Gizmos.color = new Color(0f, 0.8f, 1f, 0.8f);
                Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
            }
            else if (collider is CircleCollider2D)
            {
                CircleCollider2D circleCollider = (CircleCollider2D)collider;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawSphere(circleCollider.offset, circleCollider.radius);

                // ���Ʊ߿�
                Gizmos.color = new Color(0f, 0.8f, 1f, 0.8f);
                Gizmos.DrawWireSphere(circleCollider.offset, circleCollider.radius);
            }
        }

        // ����������ָʾ��
        DrawForceDirectionIndicator();
    }

    // ����������ָʾ��
    private void DrawForceDirectionIndicator()
    {
        Vector3 areaCenter = transform.position;

        // ��׼������������ʾ����
        Vector3 forceDirection = force.normalized;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(areaCenter, forceDirection * 2f);

        // ���Ƽ�ͷ
        Vector3 arrowTip = areaCenter + forceDirection * 2f;
        Vector3 perpendicular = new Vector3(-forceDirection.y, forceDirection.x, 0) * 0.3f;
        Gizmos.DrawLine(arrowTip, arrowTip - forceDirection * 0.5f + perpendicular);
        Gizmos.DrawLine(arrowTip, arrowTip - forceDirection * 0.5f - perpendicular);

        // ��ʾ���Ĵ�С
#if UNITY_EDITOR
        UnityEditor.Handles.Label(areaCenter + forceDirection * 1.2f, $"Force: {force.magnitude:F1}");
#endif
    }
}