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
    public float targetYPosition = -37f;
    public float moveToTargetDuration = 2f;

    [Header("摄像机拉近")]
    public float zoomInSize = 3f; // 拉近后的视野大小（更小的数值）
    public float zoomDuration = 1f; // 拉近持续时间

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
    private float moveStartTime;

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
            moveStartTime = Time.time;
        }
    }

    void MoveToTargetUpdate()
    {
        float elapsedTime = Time.time - moveStartTime;
        float moveProgress = Mathf.Clamp01(elapsedTime / moveToTargetDuration);

        // 移动相机到目标位置
        Vector3 targetPosition = new Vector3(fixedXPosition, targetYPosition, offset.z);
        transform.position = Vector3.Lerp(startPosition, targetPosition, moveProgress);

        // 同时拉近摄像机（视野变小）
        if (zoomDuration > 0)
        {
            float zoomProgress = Mathf.Clamp01(elapsedTime / zoomDuration);
            // 从originalSize缩小到zoomInSize
            cam.orthographicSize = Mathf.Lerp(originalSize, zoomInSize, zoomProgress);
        }

        if (moveProgress >= 1f)
        {
            isMovingToTarget = false;
            transform.position = targetPosition;
            cam.orthographicSize = zoomInSize; // 确保最终拉近
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