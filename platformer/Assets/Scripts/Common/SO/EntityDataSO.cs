using UnityEngine;
[CreateAssetMenu(fileName = "NewEntityDataSO", menuName = "Scriptable Objects/Entity Data")]
public class EntityDataSO : ScriptableObject
{

    [Header("기본 정보")]
    [SerializeField] private string entityName;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private Vector2 defaultSize;

    public string EntityName => entityName;
    public Vector2 DefaultSize => defaultSize;
    public float DefaultSpeed => defaultSpeed;


}