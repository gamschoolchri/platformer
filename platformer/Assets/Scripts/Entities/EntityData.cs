using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/Entity Data")]
public class EntityData : ScriptableObject
{

    [Header("기본 정보")]
    [SerializeField] private string entityName;
    [SerializeField] private int health;
    [SerializeField] private int energy;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 size;

    [SerializeField] private List<Buff> buffs;
    [SerializeField] private List<DeBuff> debuffs;

    [SerializeField] private HurtboxData hurtboxData;

    public string EntityName => entityName;
    public int Health => health;
    public int Energy => energy;
    public float Speed => speed;
    public Vector2 Size => size;
    public IReadOnlyList<Buff> Buffs => buffs;
    public IReadOnlyList<DeBuff> Debuffs => debuffs;
    public HurtboxData HurtboxData => hurtboxData;

    [Header("스킬 목록")]
    [SerializeReference, SubclassSelector]
    private List<Skill> skills = new();

    public IReadOnlyList<Skill> Skills => skills;
}