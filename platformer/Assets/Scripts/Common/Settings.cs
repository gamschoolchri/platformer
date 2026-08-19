using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Scriptable Objects/Settings")]
public class Settings : ScriptableObject
{
    public static Settings Instance { get; private set; }

    [Header("프리팹 설정")]
    public Hitbox defaultHitbox;
    public GameObject damageTextPrefab;

    [Header("레이어 설정")]
    public LayerMask enemyLayer;
    public LayerMask playerLayer;

    private void OnEnable()
    {
        Instance = this;
    }
}