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
    public void Setup(GameObject caster, HitboxData data, int facing)
    {
        transform.position = caster.transform.position + new Vector3(data.Offset.x * facing, data.Offset.y, 0);
        transform.localScale = data.Size;
        col.size = Vector2.one;
        duration = data.Duration;
        hitData = data.HitData;
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
