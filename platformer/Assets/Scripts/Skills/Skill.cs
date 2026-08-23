using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
public class Skill : MonoBehaviour
{
    [SerializeField] private SkillDataSO data;
    public SkillDataSO SkillDataSO => data;
    private readonly List<SkillModule> modules = new();

    public void Setup(GameObject caster, int facing)
    {
        transform.position = caster.transform.position + new Vector3(data.Offset.x * facing, data.Offset.y, 0);
        CoroutineManager.Instance.Toggle(value => gameObject.SetActive(value), 0f, data.Duration);
        foreach (SkillModule module in data.Modules)
        {
            SkillModule moduleCopy = module.Clone();
            modules.Add(moduleCopy);
            moduleCopy.Setup(caster, gameObject, facing);
        }
    }
    private void FixedUpdate()
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