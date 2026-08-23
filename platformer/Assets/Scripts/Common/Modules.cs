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
    public virtual void Setup(GameObject caster, GameObject parent, int facing)
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

    public override void Setup(GameObject caster, GameObject parent, int facing)
    {
        base.Setup(caster, parent, facing);
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

    public override void Setup(GameObject caster, GameObject parent, int facing)
    {
        base.Setup(caster, parent, facing);
        if (this.caster == null) return;
        if (effect == null)
        {
            if (prefab.NullCheck(nameof(EffectModule))) return;
            effect = UnityEngine.Object.Instantiate(prefab, this.parent.transform);
        }

        effect.Setup(this.caster, this.parent, effectData, facing);
    }
}

[Serializable]
public abstract class Condition
{
    protected Character caster;
    public virtual Condition Clone()
    {
        return (Condition)MemberwiseClone();
    }
    public virtual void Setup(Character caster)
    {
        this.caster = caster;
    }
    public abstract bool CanExecute();
    public virtual void OnExecute() { }

}
public class CooldownCondition : Condition
{
    private bool cannotExecute;

    public override bool CanExecute() => !cannotExecute;
    public override void OnExecute()
    {
        // CoroutineManager.Instance.Toggle(value => cannotExecute = value, 0f, caster.)
    }
}
