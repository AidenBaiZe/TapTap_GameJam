using UnityEngine;

public class HeightTrackMono : MonoBehaviour
{
    [Header("��Ծ�߶�׷��")]
    [SerializeField] private bool trackJumpHeight = true;

    private float jumpStartHeight;
    private float currentJumpHeight;
    private float maxJumpHeight;
    private bool isTrackingJump = false;

    // ����Ծ��ʼʱ����
    public void StartJumpTracking()
    {
        if (trackJumpHeight)
        {
            jumpStartHeight = transform.position.y;
            currentJumpHeight = 0f;
            maxJumpHeight = 0f;
            isTrackingJump = true;
        }
    }

    // �� FixedUpdate �и��¸߶�
    private void FixedUpdate()
    {
        UpdateJumpHeight();
    }
    void UpdateJumpHeight()
    {
        if (isTrackingJump && !gameObject.GetComponent<PlayerMovement>().isGrounded)
        {
            currentJumpHeight = transform.position.y - jumpStartHeight;
            if (currentJumpHeight > maxJumpHeight)
            {
                maxJumpHeight = currentJumpHeight;
                Debug.Log("������Ծ���߶�maxJumpHeight" + maxJumpHeight);
            }
        }
    }

    // �����ʱ����
    public void EndJumpTracking()
    {
        if (isTrackingJump)
        {
            isTrackingJump = false;
            Debug.Log("������Ծ���߶�: " + maxJumpHeight.ToString("F2"));

            // ����׷��
            currentJumpHeight = 0f;
            maxJumpHeight = 0f;
        }
    }

    // Ȼ�����ʵ��ĵط�������Щ������
    // - �� Jump() �����е��� StartJumpTracking()
    // - �� FixedUpdate �е��� UpdateJumpHeight()
    // - �� CheckGrounded() �м�⵽���ʱ���� EndJumpTracking()
}
