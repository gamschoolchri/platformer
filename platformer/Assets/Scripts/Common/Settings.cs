using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Scriptable Objects/Settings")]
public class Settings : ScriptableObject
{
    private static Settings instance;
    public static Settings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<Settings>("Common/Data/Settings");
                instance.NullCheck("Settings");
            }
            return instance;
        }
    }

    [Header("프리팹 설정")]
    public Hitbox defaultHitbox;
    public Hurtbox defaultHurtbox;
    public GameObject player;
    public GameObject damageTextPrefab;

    [Header("레이어 설정")]
    public LayerMask enemyLayer;
    public LayerMask playerLayer;

}