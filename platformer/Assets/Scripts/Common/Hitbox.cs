using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hitbox : MonoBehaviour
{
    private BoxCollider2D col;
    private GameObject caster;
    private DefenderHitData defenderHitData;
    private AttackerHitData attackerHitData;
    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }
    public void Setup(GameObject caster, GameObject skillObject, HitboxData data, int facing)
    {
        this.caster = caster;
        transform.position = skillObject.transform.position + new Vector3(data.Offset.x * facing, data.Offset.y, 0);
        transform.localScale = data.Size;
        col.size = Vector2.one;
        defenderHitData = data.DefenderHitData;
        attackerHitData = data.AttackerHitData;
        CoroutineManager.Instance.Toggle(value => gameObject.SetActive(value), 0f, data.Duration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Hurtbox hurtbox))
        {
            if (caster.TryGetComponent(out Character casterCharacter)) casterCharacter.OnAttack(attackerHitData);
            hurtbox.OnHit(defenderHitData);
        }
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(col.offset, col.size);
    }
}