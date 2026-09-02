using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
[Serializable]
public class CooldownCondition : SkillCondition
{
    [SerializeField] private bool isOnCooldown;

    public override bool CanExecute() => !isOnCooldown;
    public override void OnExecute()
    {
        CoroutineManager.Instance.Toggle(value => isOnCooldown = value, 0f, skill.SkillData.Cooldown);
    }
}
[Serializable]
public class DistanceCondition : SkillCondition
{
    [SerializeField] private VectorRange distance;
    public VectorRange Distance { get => distance; set => distance = value; }
    private Enemy casterEnemy;
    public override void Setup(GameObject caster, Skill skill)
    {
        base.Setup(caster, skill);
        casterEnemy = caster.GetComponent<Enemy>();
    }
    public override bool CanExecute() => distance.Contains(casterEnemy.EnemyData.Target.transform.position - caster.transform.position);
}
[Serializable]
public class EnergyCondition : SkillCondition
{
    private int useEnergy;
    private Character casterCharacter;
    public int UseEnergy { get => useEnergy; set => useEnergy = value; }
    public override void Setup(GameObject caster, Skill skill)
    {
        base.Setup(caster, skill);
        casterCharacter = caster.GetComponent<Character>();
        useEnergy = skill.SkillData.UseEnergy;
    }
    public override bool CanExecute() => casterCharacter.CharacterData.Energy >= useEnergy;
}
[Serializable]
public abstract class EnemyModule
{
    public virtual void Setup(GameObject caster)
    {

    }
    public virtual EnemyModule Clone()
    {
        return (EnemyModule)MemberwiseClone();
    }
}

[Serializable]
public abstract class DetectCondition
{
    protected GameObject caster;
    public abstract bool IsMatch(GameObject target);
    public virtual DetectCondition Clone()
    {
        return (DetectCondition)MemberwiseClone();
    }
    public virtual void Setup(GameObject caster)
    {
        this.caster = caster;
    }
}
[Serializable]
public class TagCondition : DetectCondition
{
    [SerializeField] private string defaultTargetTag;
    private string targetTag;
    public string DefaultTargetTag => defaultTargetTag;
    public string TargetTag { get => targetTag; set { targetTag = value; targetTagHandle = TagHandle.GetExistingTag(targetTag); } }
    private TagHandle targetTagHandle;
    public override void Setup(GameObject caster)
    {
        base.Setup(caster);
        TargetTag = defaultTargetTag;
    }
    public override bool IsMatch(GameObject target)
    {
        return target.CompareTag(targetTagHandle);
    }
}