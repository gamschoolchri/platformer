using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hitbox : MonoBehaviour
{
    private BoxCollider2D col;
    private HitData hitData;
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
        hitData = data.HitData;
        CoroutineManager.Instance.ActiveToggle(gameObject, 0f, data.Duration);
    }

    private void OTriggerEnter2D(Collider2D collision)
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