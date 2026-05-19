using UnityEngine;
using UnityEngine.UIElements; // 确保引入这个

// 这个大括号包裹的是整个文件
public class ScentIndicatorController : MonoBehaviour
{
    public static ScentIndicatorController Instance;

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

        _MainCamera = Camera.main;

        if (_MainCamera == null)
        {
            Debug.LogError("找不到主摄像机！请确保场景中有一个标签为 MainCamera 的摄像机。");
        }
    }
    // ==========================================
    // 1. 变量声明区域（全部移到这里面来！）
    // ==========================================

    [Header("UI 设置")]
    [SerializeField] private string indicatorName = "ScentIndicator";
    [SerializeField] private Sprite arrowSprite;

    [Header("外观")]
    [SerializeField] private Color arrowColor = Color.white;
    [SerializeField][Range(0, 1)] private float minAlpha = 0.3f;
    [SerializeField][Range(0, 1)] private float maxAlpha = 1.0f;

    [Header("指示器参数")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private Vector2 detectionSize = new Vector2(50, 50);
    [SerializeField][Range(0.1f, 2f)] private float minScale = 0.5f;
    [SerializeField][Range(0.1f, 2f)] private float maxScale = 1.2f;

    // 私有变量
    private UIDocument _uiDocument;
    private VisualElement _indicator;
    private bool _isInitialized = false;
    private Camera _MainCamera;

    // 位置和目标缓存
    private Vector3 _playerPos;
    private Vector3 _targetPos;
    private Transform _playerTransform;


    // ==========================================
    // 2. 核心逻辑
    // ==========================================

    void Start()
    {
        FindPlayerAndTarget();
        SetupUI();
    }

    /*void Update()
    {
        if (_playerTransform != null)
        {
            _playerPos = _playerTransform.position;
        }

        if (_isInitialized && _indicator != null)
        {
            Vector3 direction = _targetPos - _playerPos;
            UpdateIndicator(direction);
        }
    }*/

    private void FindPlayerAndTarget()
    {
        // 这里的 "Player" 请替换为你实际的玩家 Tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _playerTransform = playerObj.transform;
            _playerPos = _playerTransform.position;
        }

        // 这里简单假设有一个 Treasure 物体，或者你需要从其他脚本获取
        // 为了演示，我们先随便找一个带有 Treasure 标签的物体
        GameObject treasureObj = GameObject.FindGameObjectWithTag("Treasure");
        if (treasureObj != null)
        {
            _targetPos = treasureObj.transform.position;
        }
    }

    private void SetupUI()
    {
        _uiDocument = GetComponent<UIDocument>();
        if (_uiDocument == null)
        {
            Debug.LogError("ScentIndicatorController: 缺少 UIDocument 组件！");
            return;
        }

        _indicator = _uiDocument.rootVisualElement.Q<VisualElement>(indicatorName);
        if (_indicator == null)
        {
            Debug.LogError($"ScentIndicatorController: 找不到名为 '{indicatorName}' 的 UI 元素！");
            return;
        }

        // 设置背景图 (如果需要)
        if (arrowSprite != null)
        {
            _indicator.style.backgroundImage = new StyleBackground(arrowSprite);
        }
        else
        {
            Debug.LogWarning("ScentIndicatorController: Arrow Sprite 未赋值，使用默认背景色。");
            _indicator.style.backgroundColor = new StyleColor(arrowColor);
        }

        // 设置初始尺寸
        _indicator.style.width = detectionSize.x;
        _indicator.style.height = detectionSize.y;

        // 初始状态隐藏
        _indicator.style.display = DisplayStyle.None;

        _isInitialized = true;
    }

    /* public void UpdateIndicator(Vector3 targetWorldPosition)
     {
         Vector2 screenPos = _MainCamera.WorldToScreenPoint(targetWorldPosition);
         if (_indicator.parent != null)
         {
             _indicator.style.left = screenPos.x - _indicator.parent.layout.width / 2; // 居中偏移
             _indicator.style.top = screenPos.y - _indicator.parent.layout.height / 2;
         }

         if (!_isInitialized || _indicator == null) return;

         // 不再使用内部查找的目标，而是使用传入的参数
         _targetPos = targetWorldPosition;
         float distance = Vector3.Distance(_playerPos, _targetPos);

         // 如果距离太远，隐藏
         if (distance > detectionRange)
         {
             _indicator.style.display = DisplayStyle.None;
             return;
         }

         // 如果在范围内，显示
         _indicator.style.display = DisplayStyle.Flex;

         // 1. 计算动态效果的比例因子 (越近越大、越不透明)
         float t = Mathf.Clamp01(1.0f - (distance / detectionRange));

         // 2. 计算并设置透明度
         float targetAlpha = Mathf.Lerp(minAlpha, maxAlpha, t);
         _indicator.style.opacity = targetAlpha;

         // 3. 计算并设置缩放
         float scale = Mathf.Lerp(minScale, maxScale, t);
         _indicator.style.scale = new Scale(new Vector3(scale, scale, 1));

         // 4. 【关键修改】计算箭头指向目标的2D方向
         Vector2 direction2D = new Vector2(_targetPos.x - _playerPos.x, _targetPos.y - _playerPos.y);

         // 5. 计算旋转角度
         // Mathf.Atan2(y, x) 返回的是与x轴正方向（右方）的夹角，弧度制
         // 乘以 Mathf.Rad2Deg 转为角度
         // UI Toolkit 中，0度指向右，正角度顺时针旋转
         float angle = Mathf.Atan2(direction2D.y, direction2D.x) * Mathf.Rad2Deg;

         // 6. 应用旋转
         _indicator.style.transformOrigin = new TransformOrigin(new Length(50, LengthUnit.Percent), new Length(50, LengthUnit.Percent));
         _indicator.style.rotate = new Rotate(new Angle(angle));
     }*/

    public void UpdateIndicator(Vector3 targetWorldPosition)
    {
        // 【已移除】有问题的 screenPos 和 left/top 设置代码
        // Vector2 screenPos = _MainCamera.WorldToScreenPoint(targetWorldPosition);
        // if (_indicator.parent != null) { ... }

        if (!_isInitialized || _indicator == null) return;

        // 不再依赖内部查找的目标，完全使用传入的参数
        _targetPos = targetWorldPosition;
        float distance = Vector3.Distance(_playerPos, _targetPos);

        // 如果距离太远，隐藏
        if (distance > detectionRange)
        {
            _indicator.style.display = DisplayStyle.None;
            return;
        }

        // 如果在范围内，显示
        _indicator.style.display = DisplayStyle.Flex;

        // 1. 计算动态效果的比例因子 (越近越大、越不透明)
        float t = Mathf.Clamp01(1.0f - (distance / detectionRange));

        // 2. 计算并设置透明度
        float targetAlpha = Mathf.Lerp(minAlpha, maxAlpha, t);
        _indicator.style.opacity = targetAlpha;

        // 3. 计算并设置缩放
        float scale = Mathf.Lerp(minScale, maxScale, t);
        _indicator.style.scale = new Scale(new Vector3(scale, scale, 1));

        // 4. 计算箭头指向目标的2D方向
        Vector2 direction2D = new Vector2(_targetPos.x - _playerPos.x, _targetPos.y - _playerPos.y);

        // 5. 计算旋转角度
        float angle = Mathf.Atan2(direction2D.y, direction2D.x) * Mathf.Rad2Deg;

        // 6. 应用旋转
        _indicator.style.transformOrigin = new TransformOrigin(new Length(50, LengthUnit.Percent), new Length(50, LengthUnit.Percent));
        _indicator.style.rotate = new Rotate(new Angle(angle));
    }
    public void ShowIndicator()
    {
        if (_isInitialized && _indicator != null)
        {
            _indicator.style.display = DisplayStyle.Flex;
        }
    }

    public void HideIndicator()
    {
        if (_isInitialized && _indicator != null)
        {
            _indicator.style.display = DisplayStyle.None;
        }
    }
}