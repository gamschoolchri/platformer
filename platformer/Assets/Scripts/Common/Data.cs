using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillDataSO", menuName = "Scriptable Objects/Skill Data")]
public class SkillDataSO : ScriptableObject
{
    [Header("기본 정보")]
    [SerializeField] private string skillName;
    [SerializeField] private float cooldown;
    [SerializeField] private float duration;
    [SerializeField] private int useEnergy;
    [SerializeField] private Vector2 offset;

    public string SkillName => skillName;
    public float Cooldown => cooldown;
    public float Duration => duration;
    public int UseEnergy => useEnergy;
    public Vector2 Offset => offset;

    [Header("모듈 목록")]
    [SerializeReference, SubclassSelector]
    private List<SkillModule> modules = new();

    public IReadOnlyList<SkillModule> Modules => modules;
}

[CreateAssetMenu(fileName = "NewEntityDataSO", menuName = "Scriptable Objects/Entity Data")]
public class EntityDataSO : ScriptableObject
{

    [Header("기본 정보")]
    [SerializeField] private string entityName;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private Vector2 defaultSize;

    public string EntityName => entityName;
    public Vector2 DefaultSize => defaultSize;
    public float DefaultSpeed => defaultSpeed;


}

[CreateAssetMenu(fileName = "NewCharacterDataSO", menuName = "Scriptable Objects/Character Data")]
public class CharacterDataSO : EntityDataSO
{
    [Header("세부 정보")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int maxEnergy;
    [SerializeField] private HurtboxData hurtboxData;
    [SerializeField] private List<Buff> buffs;
    [SerializeField] private List<DeBuff> debuffs;

    [Header("스킬 목록")]
    [SerializeField] private List<SkillPack> skillPacks;

    public int MaxHealth => maxHealth;
    public int MaxEnergy => maxEnergy;
    public HurtboxData HurtboxData => hurtboxData;
    public IReadOnlyList<Buff> Buffs => buffs;
    public IReadOnlyList<DeBuff> Debuffs => debuffs;
    public IReadOnlyList<SkillPack> SkillPacks => skillPacks;

}

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Scriptable Objects/Enemy Data")]
public class EnemyDataSO : CharacterDataSO
{
    [SerializeField] private MovementType type;
    public MovementType Type => type;
}