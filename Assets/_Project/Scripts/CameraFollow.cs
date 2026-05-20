using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // 跟随的目标（拖入Player）
    [SerializeField] private float smoothing = 5f; // 跟随平滑度，值越大跟随越紧
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // 摄像机偏移，Z轴通常为负值

    void LateUpdate()
    {
        if (target != null)
        {
            // 计算目标位置
            Vector3 targetPosition = target.position + offset;
            // 平滑地移动摄像机到目标位置
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        }
    }
}