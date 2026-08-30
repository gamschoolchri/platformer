using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Entity : MonoBehaviour
{
    [SerializeField] private EntityDataSO so;
    public EntityDataSO EntityDataSO => so;
    private EntityData data;
    public ref EntityData EntityData => ref data;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D col;

    private Vector2 floorCheckSize;

    [SerializeField] private bool isOnFloor = false;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isAction = false;
    public bool IsOnFloor => isOnFloor;
    public bool IsMoving => isMoving;
    public bool IsAction => isAction;



    void Awake()
    {
        so.NullCheck(nameof(Entity));
        data = new(so);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        floorCheckSize = new(col.bounds.size.x, 0.1f);
    }
    void OnEnable()
    {
        Setup();
    }
    public void Setup()
    {
        data.Setup();
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.x != 0)
        {
            sr.flipX = Mathf.Sign(rb.linearVelocity.x) == -1.0f;
        }
        isMoving = rb.linearVelocity.sqrMagnitude > 0.01f;
        isOnFloor = CheckOnFloor();
    }

    public void Move(Vector2 direction)
    {
        rb.linearVelocity = direction * data.Speed;
    }

    private bool CheckOnFloor()
    {
        if (sr == null || sr.sprite.NullCheck(nameof(Entity))) return false;
        return Physics2D.OverlapBox(transform.position, floorCheckSize, 0f, LayerMask.GetMask("Floor")) != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.identity;

        if (col == null) col = GetComponent<Collider2D>();

        Vector2 size = (col != null)
            ? new Vector2(col.bounds.size.x, 0.1f)
            : Vector2.zero;

        Gizmos.DrawWireCube(transform.position, size);
    }
}