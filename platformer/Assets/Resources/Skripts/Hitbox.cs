using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hitbox : MonoBehaviour
{
    private BoxCollider2D col;
    private HitData hitData;
    private float duration;
    private float timer;
    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }
    public void Setup(Vector3 scale, HitboxData hitboxData)
    {
        transform.localScale = scale;
        col.size = Vector2.one;
        duration = hitboxData.duration;
        hitData = hitboxData.hitData;
        timer = 0f;
    }
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > duration) gameObject.SetActive(false);
    }
    private void TriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {
            other.GetComponent<enemy>().Onhit(hitData);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // 콜라이더의 월드 좌표 중심점 구하기
        Vector3 center = transform.TransformPoint(col.offset);
        // 콜라이더의 크기 구하기
        Vector3 size = col.size;

        // 회전값까지 고려해서 속이 빈 상자(기즈모) 그리기
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(col.offset, col.size);
    }
}
