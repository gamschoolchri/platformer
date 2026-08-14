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
    public float damage;
    public float knockback;
    public Buff[] buffs;
    public DeBuff[] debuffs;

}


[System.Serializable]
public struct Animation
{
    public string animationName;
    public Sprite[] sprites;
}

[System.Serializable]
public struct HitboxData
{
    public float startup;
    public float duration;
    public Vector2 size;
    public Vector2 offset;
    public HitData hitData;
}

[System.Serializable]
public struct HurtboxData
{
    public Vector2 size;
    public Vector2 offset;
}

[System.Serializable]
public struct EffectData
{
    public string prefabPath;
    public float startup;
    public float duration;
    public Vector2 size;
    public Vector2 offset;
}


[System.Serializable]
public struct EntityData
{
    public string entityName;
    public Vector2 size;
    public int health;
    public int energy;
    public float moveSpeed;
    public Buff[] buffs;
    public DeBuff[] debuffs;

    public HurtboxData hurtboxData;


}

[System.Serializable]
public struct SkillData
{
    public string skillName;
    public string animationTrigger;
    public float coolDown;
    public float duration;
    public int useEnergy;

    public MovementType movementType;
    public HitboxData hitboxData;
    public EffectData effectData;
}