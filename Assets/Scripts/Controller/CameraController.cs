using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public GameObject battles;

    [Header("跟随设置")]
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("限制设置")]
    public float minX = 0f;
    public float maxX = 0f;

    [Header("目标位置设置")]
    public float targetYPosition = -40f;
    public float moveToTargetDuration = 2f;
    public float GetTargetYPosition()
    {
        return targetYPosition;
    }

    private Camera cam;
    private float originalSize;
    private float fixedXPosition;
    private bool isMovingToTarget = false;
    private Vector3 startPosition;
    private bool isMoving = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        cam = GetComponent<Camera>();
        originalSize = cam.orthographicSize;
        fixedXPosition = transform.position.x;
        battles.SetActive(false);
    }

    void LateUpdate()
    {
        if (isMovingToTarget)
        {
            MoveToTargetUpdate();
        }
        else if (player != null && !isMoving)
        {
            FollowPlayerVerticalOnly();
        }
    }

    void FollowPlayerVerticalOnly()
    {
        float targetY = player.position.y + offset.y;
        Vector3 desiredPosition = new Vector3(fixedXPosition, targetY, offset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public void StartMoveToTarget()
    {
        if (!isMovingToTarget)
        {
            isMovingToTarget = true;
            isMoving = true;
            startPosition = transform.position;
            
        }
    }

    void MoveToTargetUpdate()
    {
        float progress = Mathf.Clamp01(Time.deltaTime / moveToTargetDuration);
        Vector3 targetPosition = new Vector3(fixedXPosition, targetYPosition, offset.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, progress);

        if (Mathf.Abs(transform.position.y - targetYPosition) < 0.1f)
        {
            isMovingToTarget = false;
            transform.position = targetPosition;
            Debug.Log("相机已到达Y=-25");
        }
        PlayerBattleController.Instance.enabled = true;
        battles.SetActive(true);
    }

    public void SetFixedXPosition(float xPosition)
    {
        fixedXPosition = xPosition;
        Vector3 newPosition = new Vector3(fixedXPosition, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }
}