using System.Collections.Generic;
using UnityEngine;
public class CharacterManager : MonoBehaviour
{

    private static CharacterManager instance;

    private readonly List<Character> characterPool = new();

    public static CharacterManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<CharacterManager>();

                if (instance == null)
                {
                    GameObject manager = new("(System) CharacterManager");
                    instance = manager.AddComponent<CharacterManager>();
                    DontDestroyOnLoad(manager);
                }
            }
            return instance;
        }
    }

    void Update()
    {
        UpdateDuration();
    }
    public void RegisterCharacter(Character character)
    {
        if (character.NullCheck(nameof(CharacterManager))) return;
        characterPool.Add(character);
    }
    public void UnRegisterCharacter(Character character)
    {
        if (character.NullCheck(nameof(CharacterManager))) return;
        characterPool.Remove(character);
    }

    private void UpdateDuration()
    {
        foreach (Character character in characterPool)
        {

        }
    }
    public void ProcessStatus(CharacterData characterData, DefenderHitData DefenderHitData)
    {

    }

    public void Clear()
    {
        foreach (var character in characterPool)
        {
            if (character != null && !character.CompareTag("Player")) Destroy(character.gameObject);
        }
        characterPool.Clear();
        Resources.UnloadUnusedAssets();
        Debug.Log($"[{nameof(CharacterManager)}] Pool Clear Finished");
    }
}