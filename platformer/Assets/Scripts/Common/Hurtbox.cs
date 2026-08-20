using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hurtbox : MonoBehaviour
{
    private BoxCollider2D col;
    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }
    public void Setup(HurtboxData hurtboxData, int facing)
    {
        transform.localScale = hurtboxData.Size;
        transform.localPosition = new(hurtboxData.Offset.x * facing, hurtboxData.Offset.y, 0);
        col.size = Vector2.one;
    }


    public void OnHit(HitData hitdata)
    {
        return;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(col.offset, col.size);
    }
}
