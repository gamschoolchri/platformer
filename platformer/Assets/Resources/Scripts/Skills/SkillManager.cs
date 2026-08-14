using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;
    private Dictionary<string, List<Skill>> pool = new();
    private Transform skillContainer;
    private static GameObject manager;

    public static SkillManager Instance => instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        manager = new("(System) SkillManager");
        instance = manager.AddComponent<SkillManager>();

        DontDestroyOnLoad(manager);

        GameObject container = new("SkillContainer");
        container.transform.SetParent(manager.transform);
        instance.skillContainer = container.transform;

    }

    public void SpawnSkill(string skillName, GameObject parent, int facing)
    {
        if (!pool.ContainsKey(skillName))
            pool[skillName] = new List<Skill>();

        Skill obj = pool[skillName].Find(x => !x.gameObject.activeInHierarchy);

        if (obj == null)
        {
            Skill prefab = Resources.Load<Skill>($"Skills/Prefabs/{skillName}");

            if (prefab == null)
            {
                Debug.LogError($"[SkillManager] '{skillName}' 프리팹을 찾을 수 없습니다. 경로를 확인하세요.");
                return;
            }

            obj = Instantiate(prefab);
            pool[skillName].Add(obj);
        }
        if (obj.movementType == MovementType.Attatched)
        {
            obj.transform.SetParent(parent.transform);
            obj.transform.localPosition = Vector3.zero;
        }
        else
        {
            obj.transform.SetParent(skillContainer);
            obj.transform.position = parent.transform.position;
        }
        obj.gameObject.SetActive(true);
        obj.Setup(facing);

    }

    public void ResetStagePools()
    {
        foreach (var poolList in pool.Values)
        {
            foreach (var obj in poolList)
            {
                if (obj != null) Destroy(obj.gameObject);
            }
        }
        pool.Clear();
        Resources.UnloadUnusedAssets();
    }
}