using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Animator))]

public class EntityAnimator : MonoBehaviour
{
    private static readonly int UseSkillHash = Animator.StringToHash("useSkill");
    private static readonly int DeadHash = Animator.StringToHash("dead");
    private static readonly int DamageHash = Animator.StringToHash("damage");
    private Entity ent;
    private Animator am;
    void Awake()
    {
        am = GetComponent<Animator>();
        ent = GetComponent<Entity>();
    }
    void OnEnable()
    {
        ent.OnDamage += () => am.SetTrigger(DamageHash);
        ent.OnDead += () => am.SetTrigger(DeadHash);
        ent.OnUseSkill += () => am.SetTrigger(UseSkillHash);
    }
    void OnDisable()
    {
        ent.OnDamage -= () => am.SetTrigger(DamageHash);
        ent.OnDead -= () => am.SetTrigger(DeadHash);
        ent.OnUseSkill -= () => am.SetTrigger(UseSkillHash);
    }
}
