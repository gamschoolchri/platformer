using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hurtbox : MonoBehaviour
{
    private BoxCollider2D col;
    private Character casterCharacter;
    private GameObject caster;
    public Character OwnerCharacter => casterCharacter;
    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        caster = transform.parent.gameObject;
        casterCharacter = caster.GetComponent<Character>();
        col.isTrigger = true;
    }
    public void Setup(HurtboxData hurtboxData)
    {
        transform.localScale = hurtboxData.Size;
        transform.localPosition = new(hurtboxData.Offset.x * caster.Facing(), hurtboxData.Offset.y, 0);
        col.size = Vector2.one;
    }


    public void OnHit(DefenderHitData defenderHitData)
    {
        casterCharacter.OnDamage(defenderHitData);
        return;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(col.offset, col.size);
    }
}
