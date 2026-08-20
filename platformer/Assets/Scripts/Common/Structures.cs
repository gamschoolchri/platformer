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
public struct HitData
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
public struct HitboxData
{
    [SerializeField] private float duration;
    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 offset;
    [SerializeField] private HitData hitData;

    public readonly float Duration => duration;
    public readonly Vector2 Size => size;
    public readonly Vector2 Offset => offset;
    public readonly HitData HitData => hitData;
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

// [System.Serializable]
// public struct EntityData
// {
//     [SerializeField] private string entityName;
//     public readonly string EntityName => entityName;

//     [SerializeField] private Vector2 size;
//     public readonly Vector2 Size => size;

//     [SerializeField] private int health;
//     public readonly int Health => health;

//     [SerializeField] private int energy;
//     public readonly int Energy => energy;

//     [SerializeField] private float moveSpeed;
//     public readonly float MoveSpeed => moveSpeed;

//     [SerializeField] private Buff[] buffs;
//     public readonly Buff[] Buffs => buffs;

//     [SerializeField] private DeBuff[] debuffs;
//     public readonly DeBuff[] Debuffs => debuffs;

//     [SerializeField] private HurtboxData hurtboxData;
//     public readonly HurtboxData HurtboxData => hurtboxData;
// }

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