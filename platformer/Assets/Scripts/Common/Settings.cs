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
    [SerializeField] private Hitbox defaultHitbox;
    [SerializeField] private Hurtbox defaultHurtbox;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject damageTextPrefab;
    public Hitbox DefaultHitbox => defaultHitbox;
    public Hurtbox DefaultHurtbox => defaultHurtbox;
    public GameObject Player => player;
    public GameObject DamageTextPrefab => damageTextPrefab;

    [Header("레이어 설정")]
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask playerLayer;
    public LayerMask CharacterLayer => characterLayer;
    public LayerMask EnemyLayer => enemyLayer;
    public LayerMask PlayerLayer => playerLayer;


}