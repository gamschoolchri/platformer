using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Character))]
public class Enemy : MonoBehaviour
{

    private EnemyDataSO so;
    public EnemyDataSO EnemyDataSO => so;
    private EnemyData data;
    public ref EnemyData EnemyData => ref data;
    private Entity ent;
    private Character chara;
    private bool isDetecting;


    // public float sqrDistanceToTarget()
    // {
    //     if (!data.NullCheck(nameof(Enemy)))
    //     {
    //         Vector2 targetPosition = data.Target.transform.position;
    //         Vector2 curPosition = transform.position;
    //         return (targetPosition - curPosition).sqrMagnitude;
    //     }
    //     return 0f;
    // }





    void Awake()
    {
        ent = GetComponent<Entity>();
        chara = GetComponent<Character>();

        so = ent.EntityDataSO as EnemyDataSO;
        so.NullCheck(nameof(Enemy));
        data = new(so);
        Setup(); //soon delete

    }


    private void Setup()
    {

    }
    void FixedUpdate()
    {
        chara.UseSkill();
    }

}