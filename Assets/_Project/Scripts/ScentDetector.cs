using UnityEngine;

public class ScentDetector : MonoBehaviour
{
    [Header("目标设置")]
    [SerializeField] private Transform treasureTarget; // 拖入你的宝藏物体
    [SerializeField] private float maxScentDistance = 20f;

    [Header("摄像机")]
    [SerializeField] private Camera targetCamera; // 建议拖拽赋值，比 Camera.main 更可靠
    private Transform _playerTransform;

    void Start()
    {
        _playerTransform = this.transform; // 玩家就是脚本挂载的对象
       /* if (targetCamera == null)
        {
            targetCamera = Camera.main; // 备用方案
            Debug.LogWarning("ScentDetector: Camera not assigned, using Camera.main.");
        }*/
    }

    void Update()
    {
        // 1. 基础检查
        if (treasureTarget == null || ScentIndicatorController.Instance == null)
        {
            ScentIndicatorController.Instance?.HideIndicator();
            return;
        }

        // 2. 计算距离和方向
        Vector3 directionToTarget = treasureTarget.position - _playerTransform.position;
        float distance = directionToTarget.magnitude;

        // 3. 判断是否在探测范围内
        if (distance < maxScentDistance)
        {
            // 4. 在范围内：通知 UI 控制器“显示并更新”箭头
            ScentIndicatorController.Instance.ShowIndicator();
            if (targetCamera != null)
            {
                ScentIndicatorController.Instance.UpdateIndicator(treasureTarget.position);
            }
        }
        else
        {
            // 5. 超出范围：通知 UI 控制器“隐藏”箭头
            ScentIndicatorController.Instance.HideIndicator();
        }
    }

    // 可选：在Scene视图绘制探测范围，便于调试
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxScentDistance);
    }
}