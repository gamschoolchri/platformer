using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/Skill Data")]
public class SkillData : ScriptableObject
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