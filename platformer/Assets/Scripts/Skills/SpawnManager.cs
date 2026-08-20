using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager instance;
    private Dictionary<string, List<Skill>> skillPool = new();
    private List<Hitbox> hitboxPool = new();
    private Transform Container;
    private static readonly GameObject manager;

    public static SpawnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindAnyObjectByType<SpawnManager>();

                if (instance == null)
                {
                    GameObject manager = new("(System) SpawnManager");
                    instance = manager.AddComponent<SpawnManager>();
                    DontDestroyOnLoad(manager);

                    GameObject container = new("Container");
                    container.transform.SetParent(manager.transform);
                    instance.Container = container.transform;
                }
                else
                {
                    instance.ResetOnReload();
                }
            }
            return instance;
        }
    }
    private void ResetOnReload()
    {
        skillPool ??= new Dictionary<string, List<Skill>>();
        skillPool.Clear();

        Transform containerTransform = transform.Find("Container");
        if (containerTransform != null)
        {
            foreach (Transform child in containerTransform)
            {
                if (child != null) Destroy(child.gameObject);
            }
            Container = containerTransform;
        }
        else
        {
            GameObject container = new("Container");
            container.transform.SetParent(transform);
            Container = container.transform;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticVariables()
    {
        instance = null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var trigger = Instance;
    }

    public void SpawnSkill(GameObject caster, string skillID, int facing)
    {
        if (!skillPool.ContainsKey(skillID))
            skillPool[skillID] = new List<Skill>();

        Skill skill = skillPool[skillID].Find(x => !x.gameObject.activeInHierarchy);

        if (skill == null)
        {
            Skill prefab = Resources.Load<Skill>($"Skills/Prefabs/{skillID}");

            if (prefab.NullCheck($"{GetType().Name},{skillID}")) return;

            skill = Instantiate(prefab, Container);
            skillPool[skillID].Add(skill);
        }
        skill.Setup(caster, facing);

    }
    public void SpawnHitbox(GameObject caster, HitboxData data, int facing)
    {
        Hitbox hitbox = hitboxPool.Find(x => !x.gameObject.activeInHierarchy);
        if (hitbox == null)
        {
            hitbox = Instantiate(Settings.Instance.defaultHitbox);
            hitboxPool.Add(hitbox);
        }
        hitbox.Setup(caster, data, facing);
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
        Debug.Log($"[{GetType().Name}] Pool Clear Finished");
    }
}