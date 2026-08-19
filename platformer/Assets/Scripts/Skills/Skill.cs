using UnityEngine;
using Newtonsoft.Json;
public class Skill : MonoBehaviour
{
    [SerializeField] private SkillData data;
    public SkillData skillData => data;

    public void Setup(GameObject caster, int facing)
    {
        transform.position = caster.transform.position + new Vector3(data.Offset.x * facing, data.Offset.y, 0);
        CoroutineManager.Instance.ActiveToggle(gameObject, 0f, data.Duration);
        foreach (SkillModule module in data.Modules)
        {
            Debug.Log(module.GetType().Name);
            module.Setup(gameObject, facing);
        }
    }
    private void FixedUpdate()
    {
        foreach (SkillModule module in data.Modules)
        {
            if (module is MoveModule moveModule)
            {
                moveModule.FixedUpdate();
            }
        }
    }
}