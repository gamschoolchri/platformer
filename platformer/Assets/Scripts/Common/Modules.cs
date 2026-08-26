using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SkillModule
{
    protected GameObject caster;
    protected GameObject parent;

    public virtual SkillModule Clone()
    {
        return (SkillModule)MemberwiseClone();
    }
    public virtual void Setup(GameObject caster, GameObject parent)
    {
        if (caster.NullCheck(nameof(SkillModule))) return;
        this.caster = caster;
        this.parent = parent;
    }
}

[Serializable]
public abstract class MoveModule : SkillModule
{
    protected Vector2 velocity;
    public Vector2 Velocity => velocity;
    public virtual void FixedUpdate()
    {
        caster.transform.Translate(velocity * Time.fixedDeltaTime);
    }
}

[Serializable]
public class HomingModule : MoveModule
{
    private float detectRadius = 1f;
    private float speed = 1f;
    private LayerMask targetLayerMask = default;
    private string targetTag = null;

    private GameObject target;

    public float DetectRadius => detectRadius;
    public float Speed => speed;
    public LayerMask TargetLayerMask => targetLayerMask;
    public string TargetTag => targetTag;
    public GameObject Target => target;

    public override void Setup(GameObject caster, GameObject parent)
    {
        base.Setup(caster, parent);
        if (this.caster == null) return;

        target = this.caster.transform.FindClosestTarget(detectRadius, targetTag, targetLayerMask);

        if (target != null)
        {
            velocity = (target.transform.position - this.caster.transform.position).normalized * speed;
        }
    }

    public override void FixedUpdate()
    {
        if (caster == null) return;

        if (target == null)
        {
            target = caster.transform.FindClosestTarget(detectRadius, targetTag, targetLayerMask);
            if (target == null) return;
        }

        velocity = (target.transform.position - caster.transform.position).normalized * speed;
        base.FixedUpdate();
    }
}

[Serializable]
public class EffectModule : SkillModule
{
    [SerializeField] private Effect prefab;
    [SerializeField] private EffectData effectData;

    public Effect Prefab => prefab;
    public EffectData EffectData => effectData;
    [SerializeField, HideInInspector] private Effect effect;
    public Effect Effect => effect;

    public override SkillModule Clone()
    {
        EffectModule clone = (EffectModule)base.Clone();
        clone.effectData = clone.effectData.Clone();
        return clone;
    }
    public override void Setup(GameObject caster, GameObject parent)
    {
        base.Setup(caster, parent);
        if (this.caster == null) return;
        if (effect == null)
        {
            if (prefab.NullCheck(nameof(EffectModule))) return;
            effect = UnityEngine.Object.Instantiate(prefab, this.parent.transform);
        }

        effect.Setup(this.caster, this.parent, effectData);
    }
}

[Serializable]
public abstract class SkillCondition
{
    protected GameObject caster;
    protected Skill skill;
    public virtual SkillCondition Clone()
    {
        return (SkillCondition)MemberwiseClone();
    }
    public virtual void Setup(GameObject caster, Skill skill)
    {
        this.caster = caster;
        this.skill = skill;
    }
    public abstract bool CanExecute();
    public virtual void OnExecute() { }

}
public class CooldownCondition : SkillCondition
{
    private bool isOnCooldown;

    public override bool CanExecute() => !isOnCooldown;
    public override void OnExecute()
    {
        CoroutineManager.Instance.Toggle(value => isOnCooldown = value, 0f, skill.SkillData.Cooldown);
    }
}
public class DistanceCondition : SkillCondition
{
    [SerializeField] private Range distance;
    public Range Distance { get => distance; set => distance = value; }
    private Range sqrDistance;
    private Enemy casterEnemy;
    // public override void Setup(GameObject caster, Skill skill)
    // {
    //     base.Setup(caster,skill);
    //     casterEnemy=caster.get
    // }
    public override bool CanExecute() { return false; }
}