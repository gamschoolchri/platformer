using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Skill))]
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
    public void Setup(HitboxData hitboxData)
    {
        transform.localScale = hitboxData.size;
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

    void OTriggerEnter2D(Collider2D collision)
    {
        Hurtbox hurtbox = collision.GetComponent<Hurtbox>();
        if (hurtbox != null)
        {
            hurtbox.OnHit(hitData);
        }
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(col.offset, col.size);
    }
}
