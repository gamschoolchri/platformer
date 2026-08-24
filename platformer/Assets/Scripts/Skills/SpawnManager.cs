using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager instance;
    private readonly Dictionary<string, List<Skill>> skillPool = new();
    private readonly List<Hitbox> hitboxPool = new();
    private Transform Container;

    public static SpawnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SpawnManager>();

                if (instance == null)
                {
                    GameObject manager = new("(System) SpawnManager");
                    instance = manager.AddComponent<SpawnManager>();
                    DontDestroyOnLoad(manager);

                    GameObject container = new("Container");
                    container.transform.SetParent(manager.transform);
                    instance.Container = container.transform;
                }
            }
            return instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticVariables()
    {
        if (instance != null)
            instance.Clear();
        instance = null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var trigger = Instance;
    }

    public void SpawnSkill(GameObject caster, string skillID)
    {
        if (!skillPool.ContainsKey(skillID))
            skillPool[skillID] = new List<Skill>();

        Skill skill = skillPool[skillID].Find(x => !x.gameObject.activeInHierarchy);

        if (skill == null)
        {
            Skill prefab = Resources.Load<Skill>($"Skills/Prefabs/{skillID}");

            if (prefab.NullCheck($"{nameof(SpawnManager)},{skillID}")) return;

            skill = Instantiate(prefab, Container);
            skillPool[skillID].Add(skill);
        }
        skill.Setup(caster);

    }
    public void SpawnHitbox(GameObject caster, GameObject skillObject, HitboxData data)
    {
        Hitbox hitbox = hitboxPool.Find(x => !x.gameObject.activeInHierarchy);
        if (hitbox == null)
        {
            hitbox = Instantiate(Settings.Instance.defaultHitbox, Container);
            hitboxPool.Add(hitbox);
        }
        hitbox.Setup(caster, skillObject, data);
    }

    public void Clear()
    {
        foreach (var skillList in skillPool.Values)
        {
            foreach (var skill in skillList)
            {
                if (skill != null) Destroy(skill.gameObject);
            }
        }
        skillPool.Clear();
        foreach (var hitbox in hitboxPool)
        {
            if (hitbox != null) Destroy(hitbox.gameObject);
        }
        hitboxPool.Clear();
        Resources.UnloadUnusedAssets();
        Debug.Log($"[{nameof(SpawnManager)}] Pool Clear Finished");
    }
}