using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
public class Skill : MonoBehaviour
{
    [SerializeField] private SkillDataSO so;

    private SkillData data;
    public SkillData SkillData => data;
    public SkillDataSO SkillDataSO => so;
    private readonly List<SkillModule> modules = new();

    void Awake()
    {
        data = new(so);
    }
    public void Setup(GameObject caster)
    {
        transform.position = caster.transform.position + new Vector3(data.Offset.x * caster.Facing(), data.Offset.y, 0);
        CoroutineManager.Instance.Toggle(value => gameObject.SetActive(value), 0f, data.Duration);
        data.Setup(caster, gameObject);
    }
    void FixedUpdate()
    {
        foreach (SkillModule module in modules)
        {
            if (module is MoveModule moveModule)
            {
                moveModule.FixedUpdate();
            }
        }
    }
}