using UnityEngine;
using Newtonsoft.Json;
using System;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour
{
    [SerializeField] private EntityData data;
    public EntityData EntityData => data;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Hurtbox hurtbox;
    public Vector2 floorCheckSize = new(0.5f, 0.1f);
    public Vector2 floorCheckOffset = new(0f, 0f);

    [SerializeField] private int facing = 1;
    [SerializeField] private bool isHurtboxActive = false;
    [SerializeField] private bool isOnFloor = false;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isAction = false;

    public int Facing => facing;
    public bool IsHurtboxActive => isHurtboxActive;
    public bool IsOnFloor => isOnFloor;
    public bool IsMoving => isMoving;
    public bool IsAction => isAction;



    void Awake()
    {
        data.NullCheck(GetType().Name);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Setup();
    }


    public void Setup()
    {
        if (hurtbox == null) hurtbox = Instantiate(Settings.Instance.defaultHurtbox, transform);

        hurtbox.Setup(data.HurtboxData, facing);



        SetHurtboxActive(true);
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.x != 0)
        {
            facing = (int)Mathf.Sign(rb.linearVelocity.x);
            sr.flipX = facing == -1;
        }
        isMoving = rb.linearVelocity.sqrMagnitude > 0.01f;
        isOnFloor = Physics2D.OverlapBox(transform.position + (Vector3)floorCheckOffset, floorCheckSize, 0f, LayerMask.GetMask("Floor")) != null;
    }





    public void SetHurtboxActive(bool isActive)
    {
        hurtbox.gameObject.SetActive(isActive);
        isHurtboxActive = isActive;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(floorCheckOffset, floorCheckSize);
    }
}