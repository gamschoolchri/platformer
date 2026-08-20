using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SkillModule
{
    protected GameObject caster;

    public virtual SkillModule Clone()
    {
        return (SkillModule)MemberwiseClone();
    }
    public virtual void Setup(GameObject caster, int facing)
    {
        if (caster.NullCheck(GetType().Name)) return;
        this.caster = caster;
    }
}

[Serializable]
public abstract class MoveModule : SkillModule
{
    protected Vector2 velocity;
    public virtual void FixedUpdate()
    {
        caster.transform.Translate(velocity * Time.fixedDeltaTime);
    }
}

[Serializable]
public class HomingModule : MoveModule
{
    public float detectRadius = 1f;
    public float speed = 1f;
    public LayerMask targetLayerMask = default;
    public string targetTag = null;

    private GameObject target;

    public override void Setup(GameObject caster, int facing)
    {
        base.Setup(caster, facing);
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
    public Effect prefab;
    public EffectData effectData;
    [SerializeField, HideInInspector] private Effect effect;

    public override void Setup(GameObject caster, int facing)
    {
        base.Setup(caster, facing);
        if (this.caster == null) return;
        if (effect == null)
        {
            if (prefab.NullCheck(GetType().Name)) return;
            effect = UnityEngine.Object.Instantiate(prefab, this.caster.transform);
        }

        effect.Setup(effectData, facing);
    }
}