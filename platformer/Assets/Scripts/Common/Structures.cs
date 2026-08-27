using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Range
{
    [SerializeField] private float start;
    [SerializeField] private float end;
    private float min;
    private float max;

    public Range(float start, float end)
    {
        this.start = start;
        this.end = end;
        min = Mathf.Min(start, end);
        max = Mathf.Max(start, end);
    }
    public float Start { readonly get => start; set => start = value; }
    public float End { readonly get => end; set => end = value; }
    public float Min { readonly get => min; set => min = value; }
    public float Max { readonly get => max; set => max = value; }

    public readonly bool Contains(float value)
    {
        return value >= min && value <= max;
    }

    public readonly bool ContainsExclusive(float value)
    {
        return value > min && value < max;
    }

    public readonly float Clamp(float value) => Mathf.Clamp(value, min, max);
}

[System.Serializable]
public enum Buff
{

}

[System.Serializable]
public enum DeBuff
{

}

[System.Serializable]
public enum MovementType
{
    Static, Walker, Runner, Flying
}

[System.Serializable]
public struct DefenderHitData
{
    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    [SerializeField] private List<Buff> buffs;
    [SerializeField] private List<DeBuff> debuffs;

    public readonly float Damage => damage;
    public readonly float Knockback => knockback;
    public readonly IReadOnlyList<Buff> Buffs => buffs;
    public readonly IReadOnlyList<DeBuff> Debuffs => debuffs;
}

[System.Serializable]
public struct AttackerHitData
{

}

[System.Serializable]
public struct HitboxData
{
    [SerializeField] private float duration;
    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 offset;
    [SerializeField] private DefenderHitData defenderHitData;
    [SerializeField] private AttackerHitData attackerHitData;

    public readonly float Duration => duration;
    public readonly Vector2 Size => size;
    public readonly Vector2 Offset => offset;
    public readonly DefenderHitData DefenderHitData => defenderHitData;
    public readonly AttackerHitData AttackerHitData => attackerHitData;
}

[System.Serializable]
public struct HurtboxData
{
    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 offset;

    public readonly Vector2 Size => size;
    public readonly Vector2 Offset => offset;
}

[System.Serializable]
public struct EffectData
{
    [SerializeField] private float startup;
    [SerializeField] private float duration;
    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 offset;
    [SerializeField] private List<HitboxData> hitboxes;
    public float Startup { readonly get => startup; set => startup = value; }
    public float Duration { readonly get => duration; set => duration = value; }
    public Vector2 Size { readonly get => size; set => size = value; }
    public Vector2 Offset { readonly get => offset; set => offset = value; }
    public List<HitboxData> Hitboxes { readonly get => hitboxes; set => hitboxes = value; }

    public readonly EffectData Clone()
    {
        EffectData clone = this;
        clone.Hitboxes = new(Hitboxes);
        return clone;
    }
}

[System.Serializable]
public struct EntityData
{
    [SerializeField] private string name;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 defaultSize;
    [SerializeField] private Vector2 size;

    public string Name { readonly get => name; set => name = value; }
    public readonly float DefaultSpeed => defaultSpeed;
    public float Speed { readonly get => speed; set => speed = value; }
    public readonly Vector2 DefaultSize => defaultSize;
    public Vector2 Size { readonly get => size; set => size = value; }

    public EntityData(EntityDataSO so)
    {
        name = so.EntityName;
        defaultSpeed = so.DefaultSpeed;
        speed = so.DefaultSpeed;
        defaultSize = so.DefaultSize;
        size = so.DefaultSize;
    }
}

[System.Serializable]
public struct CharacterData
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energy;
    [SerializeField] private List<Buff> buffs;
    [SerializeField] private List<DeBuff> debuffs;
    [SerializeField] private HurtboxData hurtboxData;
    [SerializeField] private float globalCoolDown;
    [SerializeField] private List<SkillPack> skillPacks;

    public int MaxHealth { readonly get => maxHealth; set => maxHealth = value; }
    public int Health { readonly get => health; set => health = Mathf.Clamp(value, 0, maxHealth); }
    public int MaxEnergy { readonly get => maxEnergy; set => maxEnergy = value; }
    public int Energy { readonly get => energy; set => energy = Mathf.Clamp(value, 0, maxEnergy); }
    public List<Buff> Buffs { readonly get => buffs; set => buffs = value; }
    public List<DeBuff> Debuffs { readonly get => debuffs; set => debuffs = value; }
    public float GCD { readonly get => globalCoolDown; set => globalCoolDown = value; }
    public List<SkillPack> SkillPacks { readonly get => skillPacks; set => skillPacks = value; }


    public readonly HurtboxData HurtboxData => hurtboxData;




    public CharacterData(GameObject caster, CharacterDataSO so)
    {
        maxHealth = so.MaxHealth;
        health = so.MaxHealth;
        maxEnergy = so.MaxEnergy;
        energy = so.MaxEnergy;

        buffs = (so.Buffs != null) ? new(so.Buffs) : new();
        debuffs = (so.Debuffs != null) ? new(so.Debuffs) : new();
        hurtboxData = so.HurtboxData;
        globalCoolDown = so.GCD;
        skillPacks = new();
        if (so.SkillPacks != null)
        {
            foreach (SkillPack skillPack in so.SkillPacks)
            {
                SkillPack skillPackCopy = skillPack.Clone();
                skillPackCopy.Setup(caster);
                skillPacks.Add(skillPackCopy);
            }
        }
    }
}

[System.Serializable]
public struct EnemyData
{
    [SerializeField] private List<EnemyModule> modules;
    public List<EnemyModule> Modules { readonly get => modules; set => modules = value; }

    public EnemyData(EnemyDataSO so)
    {
        modules = new();

        if (so.Modules != null)
        {
            foreach (EnemyModule module in so.Modules)
            {
                EnemyModule moduleCopy = module.Clone();
                modules.Add(moduleCopy);
            }
        }
    }
    public readonly void Setup(GameObject caster)
    {
        if (modules != null)
        {
            foreach (EnemyModule module in modules)
            {
                module.Setup(caster);
            }
        }

    }
}


[System.Serializable]
public struct SkillPack
{
    [Header("스킬 정보")]
    [SerializeField] private Skill skill;

    [Header("발동 정책")]
    [SerializeField] private bool canInterrupt;

    [Header("엔티티 전용 발동 조건 모듈들")]
    [SerializeReference, SubclassSelector]
    private List<SkillCondition> conditions;

    public Skill Skill { readonly get => skill; set => skill = value; }

    public bool CanInterrupt { readonly get => canInterrupt; set => canInterrupt = value; }

    public List<SkillCondition> Conditions { readonly get => conditions; set => conditions = value; }



    public readonly SkillPack Clone()
    {
        SkillPack clone = this;
        clone.Conditions = (Conditions != null) ? Conditions.Select(item => item.Clone()).ToList() : new();
        return clone;
    }

    public readonly void Setup(GameObject caster)
    {
        if (conditions != null)
        {
            foreach (SkillCondition condition in conditions)
            {
                condition.Setup(caster, skill);
            }
        }
    }

    public readonly bool CanExecute()
    {
        if (skill.NullCheck(nameof(SkillPack))) return false;
        if (conditions == null) return true;

        foreach (SkillCondition condition in conditions)
        {
            if (!condition.CanExecute()) return false;
        }
        return true;
    }
}
[System.Serializable]
public struct SkillData
{
    [Header("기본 정보")]
    [SerializeField] private string name;
    [SerializeField] private float cooldown;
    [SerializeField] private float duration;
    [SerializeField] private int useEnergy;
    [SerializeField] private Vector2 offset;

    [Header("기본 정보")]
    [SerializeReference, SubclassSelector]
    private List<SkillModule> modules;

    public string Name { readonly get => name; set => name = value; }
    public float Cooldown { readonly get => cooldown; set => cooldown = value; }
    public float Duration { readonly get => duration; set => duration = value; }
    public int UseEnergy { readonly get => useEnergy; set => useEnergy = value; }
    public Vector2 Offset { readonly get => offset; set => offset = value; }
    public List<SkillModule> Modules { readonly get => modules; set => modules = value; }

    public SkillData(SkillDataSO so)
    {
        name = so.SkillName;
        cooldown = so.Cooldown;
        duration = so.Duration;
        useEnergy = so.UseEnergy;
        offset = so.Offset;
        modules = new();
        if (so.Modules != null)
        {
            foreach (SkillModule module in so.Modules)
            {
                SkillModule moduleCopy = module.Clone();
                modules.Add(moduleCopy);
            }
        }

    }
    public readonly void Setup(GameObject caster, GameObject parent)
    {
        if (modules != null)
        {
            foreach (SkillModule module in modules)
            {
                module.Setup(caster, parent);
            }
        }

    }
}