using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewCharacterDataSO", menuName = "Scriptable Objects/Character Data")]
public class CharacterDataSO : EntityDataSO
{
    [Header("세부 정보")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int maxEnergy;
    [SerializeField] private HurtboxData hurtboxData;
    [SerializeField] private List<Buff> buffs;
    [SerializeField] private List<Debuff> debuffs;

    [Header("스킬 목록")]
    [SerializeField] private float globalCoolDown;
    [SerializeField] private List<SkillPack> skillPacks;

    public int MaxHealth => maxHealth;
    public int MaxEnergy => maxEnergy;
    public HurtboxData HurtboxData => hurtboxData;
    public IReadOnlyList<Buff> Buffs => buffs;
    public IReadOnlyList<Debuff> Debuffs => debuffs;
    public float GCD => globalCoolDown;
    public IReadOnlyList<SkillPack> SkillPacks => skillPacks;


}