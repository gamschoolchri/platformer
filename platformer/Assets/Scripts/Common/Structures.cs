using System.Collections.Generic;
using UnityEngine;

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
    Attatched, Projectile, Static
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
    [SerializeField] private string prefabPath;
    [SerializeField] private float startup;
    [SerializeField] private float duration;
    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 offset;
    [SerializeField] private List<HitboxData> hitboxes;

    public readonly string PrefabPath => prefabPath;
    public readonly float Startup => startup;
    public readonly float Duration => duration;
    public readonly Vector2 Size => size;
    public readonly Vector2 Offset => offset;
    public readonly IReadOnlyList<HitboxData> Hitboxes => hitboxes;
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

    public int MaxHealth { readonly get => maxHealth; set => maxHealth = value; }
    public int Health { readonly get => health; set => health = Mathf.Clamp(value, 0, maxHealth); }
    public int MaxEnergy { readonly get => maxEnergy; set => maxEnergy = value; }
    public int Energy { readonly get => energy; set => energy = Mathf.Clamp(value, 0, maxEnergy); }
    public List<Buff> Buffs { readonly get => buffs; set => buffs = value; }
    public List<DeBuff> Debuffs { readonly get => debuffs; set => debuffs = value; }

    public readonly HurtboxData HurtboxData => hurtboxData;

    public CharacterData(CharacterDataSO so)
    {
        maxHealth = so.MaxHealth;
        health = so.MaxHealth;
        maxEnergy = so.MaxEnergy;
        energy = so.MaxEnergy;

        buffs = so.Buffs != null ? new(so.Buffs) : new();
        debuffs = so.Debuffs != null ? new(so.Debuffs) : new();
        hurtboxData = so.HurtboxData;
    }
}

[System.Serializable]
public struct EnemyData
{
    [SerializeField] private List<Skill> skills;

    public List<Skill> Skills { readonly get => skills; set => skills = value; }


    public EnemyData(EnemyDataSO so)
    {
        skills = so.Skills != null ? new(so.Skills) : new();
    }
}


[System.Serializable]
public struct SkillPack
{
    [Header("스킬 정보")]
    [SerializeField] private Skill skill;

    [Header("발동 정책 및 우선순위")]
    [SerializeField] private int priority;
    [SerializeField] private bool canInterrupt;

    [Header("엔티티 전용 발동 조건 모듈들")]
    [SerializeReference, SubclassSelector]
    private List<Condition> conditions;

    public readonly Skill Skill => skill;
    public readonly int Priority => priority;
    public readonly bool CanInterrupt => canInterrupt;
    public readonly IReadOnlyList<Condition> Conditions => conditions;

    public readonly bool CanExecute(Character caster)
    {
        if (skill.NullCheck(nameof(SkillPack))) return false;
        if (conditions == null) return true;

        foreach (Condition condition in conditions)
        {
            if (!condition.CanExecute()) return false;
        }
        return true;
    }
}
// [System.Serializable]
// public struct SkillData
// {
//     public string skillName;
//     public string animationTrigger;
//     public float coolDown;
//     public float duration;
//     public int useEnergy;

//     public MovementType movementType;
//     public HitboxData hitboxData;
//     public EffectData effectData;
// }