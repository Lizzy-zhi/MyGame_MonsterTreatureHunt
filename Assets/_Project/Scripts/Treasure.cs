using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound; // 可选：收集音效
    [SerializeField] private int scoreValue = 1;    // 每个宝藏的分数

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 检测是否是玩家触碰
        if (other.CompareTag("Player"))
        {
            // 2. 通知 GameManager 增加分数
            GameManager.Instance.AddScore(scoreValue);

            // 3. 播放音效（如果有）
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            // 4. 销毁宝藏物体（被捡走了）
            Destroy(gameObject);
        }
    }
}
