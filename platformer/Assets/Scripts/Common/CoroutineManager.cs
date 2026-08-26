using System;
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
                instance = FindAnyObjectByType<CoroutineManager>();

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
        if (!timeDict.TryGetValue(seconds, out WaitForSeconds wait))
        {
            timeDict.Add(seconds, wait = new(seconds));
        }

        return wait;
    }

    public void Clear()
    {
        timeDict.Clear();
    }




    public void Toggle(Action<bool> Toggle, float startup, float duration)
    {
        StartCoroutine(ToggleIEnumerator(Toggle, startup, duration));
    }
    private IEnumerator ToggleIEnumerator(Action<bool> Toggle, float startup, float duration)
    {
        yield return WaitForSeconds(startup);
        Toggle.Invoke(true);
        yield return WaitForSeconds(duration);
        Toggle.Invoke(false);
    }
}