using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Entity))]
public class Enemy : MonoBehaviour
{

    private EnemyDataSO so;
    public EnemyDataSO EnemyDataSO => so;
    private EnemyData data;
    public ref EnemyData EnemyData => ref data;
    private Entity ent;

    [SerializeField] private bool isHurtboxActive = false;
    public bool IsHurtboxActive => isHurtboxActive;



    void Awake()
    {
        ent = GetComponent<Entity>();

        so = ent.EntityDataSO as EnemyDataSO;
        so.NullCheck(nameof(Enemy));
        Setup(); //soon delete

    }


    private void Setup()
    {

    }

}