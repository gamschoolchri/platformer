using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager instance;
    private static GameObject manager;
    public static CoroutineManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindAnyObjectByType<CoroutineManager>();

                if (instance == null)
                {
                    manager = new("(System) CoroutineManager");
                    instance = manager.AddComponent<CoroutineManager>();

                    DontDestroyOnLoad(manager);
                }

            }
            return instance;
        }
    }

    private static readonly Dictionary<float, WaitForSeconds> timeDict = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var trigger = Instance;
    }

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!timeDict.TryGetValue(seconds, out var wait))
        {
            timeDict.Add(seconds, wait = new WaitForSeconds(seconds));
        }

        return wait;
    }

    public void Clear()
    {
        timeDict.Clear();
    }



    public void ActiveToggle(GameObject target, float startup, float duration)
    {
        if (target.NullCheck(GetType().Name)) return;
        StartCoroutine(ActiveToggleIEnumerator(target, startup, duration));
    }
    private IEnumerator ActiveToggleIEnumerator(GameObject target, float startup, float duration)
    {
        yield return WaitForSeconds(startup);
        target.SetActive(true);
        yield return WaitForSeconds(duration);
        if (target != null) target.SetActive(false);
    }
}