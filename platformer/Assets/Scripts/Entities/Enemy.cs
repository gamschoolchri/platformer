using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
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
    private Collider2D col;
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
        col = GetComponent<Collider2D>();

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
        TargetUpdate();
        chara.UseSkill();
    }
    private void TargetUpdate()
    {
        if (data.Target != null)
        {
            Vector2 targetLocalPosition = data.Target.transform.position - transform.position;
            if (!data.TargetSwitchRange.Contains(targetLocalPosition)) TargetDetect(data.TargetSwitchRange);
            if (!data.TargetMaintainRange.Contains(targetLocalPosition)) data.Target = null;
        }
        if (data.Target == null) TargetDetect(data.TargetDetectRange);
    }
    private void TargetDetect(VectorRange range)
    {
        Vector2 position = (Vector2)transform.position + new Vector2(range.Center.x * gameObject.Facing(), range.Center.y);
        Collider2D[] detectedList = Physics2D.OverlapBoxAll(position, range.Size * 0.5f, 0f, Settings.Instance.CharacterLayer);
        foreach (Collider2D detectedCollider in detectedList)
        {
            if (detectedCollider != col && data.IsMatch(detectedCollider.gameObject))
            {
                data.Target = detectedCollider.gameObject;
                return;
            }
        }
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        anim.SetInteger(StateHash, (int)newState);
    }
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            data.TargetDetectRange.DrawGizmo(transform.position, Color.darkRed);
            data.TargetSwitchRange.DrawGizmo(transform.position, Color.softRed);
            data.TargetMaintainRange.DrawGizmo(transform.position, Color.indianRed);
        }
        else
        {
            ent = GetComponent<Entity>();
            so = (EnemyDataSO)ent.EntityDataSO;
            so.TargetDetectRange.DrawGizmo(transform.position, Color.darkRed);
            so.TargetSwitchRange.DrawGizmo(transform.position, Color.softRed);
            so.TargetMaintainRange.DrawGizmo(transform.position, Color.indianRed);
        }

    }
}