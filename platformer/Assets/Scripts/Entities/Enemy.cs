using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    private static readonly int StateHash = Animator.StringToHash("State");
    private EnemyDataSO so;
    public EnemyDataSO EnemyDataSO => so;
    private EnemyData data;
    public ref EnemyData EnemyData => ref data;
    private Entity ent;
    private Character chara;
    private Animator anim;
    private State currentState;


    public float sqrDistanceToTarget()
    {
        if (data.NullCheck(nameof(Enemy))) return 0f;
        if (data.Target.NullCheck(nameof(Enemy))) return 0f;
        Vector2 targetPosition = data.Target.transform.position;
        Vector2 curPosition = transform.position;
        return (targetPosition - curPosition).sqrMagnitude;
    }





    void Awake()
    {
        ent = GetComponent<Entity>();
        chara = GetComponent<Character>();
        anim = GetComponent<Animator>();

        so = ent.EntityDataSO as EnemyDataSO;
        so.NullCheck(nameof(Enemy));
        data = new(so);

    }
    void OnEnable()
    {
        Setup();
    }

    public void Setup()
    {
        data.Setup(gameObject);
    }
    void FixedUpdate()
    {
        DetectLasting();
        Detect();
        chara.UseSkill();
    }
    private void Detect()
    {
        Vector2 position = (Vector2)transform.position + new Vector2(data.DetectRange.Center.x * gameObject.Facing(), data.DetectRange.Center.y);
        Collider2D[] detectedList = Physics2D.OverlapBoxAll(position, data.DetectRange.Size, 0f, Settings.Instance.CharacterLayer);
        foreach (Collider2D detectedCollider in detectedList)
        {
            if (data.IsMatch(detectedCollider.gameObject))
            {
                data.Target = detectedCollider.gameObject;
                Debug.Log("detected!!");
                return;
            }
        }
    }
    private void DetectLasting()
    {
        if (data.Target != null)
        {
            if (!data.DetectLastingRange.Contains(data.Target.transform.position - transform.position)) data.Target = null;
        }
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        anim.SetInteger(StateHash, (int)newState);
    }

}