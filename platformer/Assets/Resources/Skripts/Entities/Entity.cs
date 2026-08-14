using UnityEngine;
using Newtonsoft.Json;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour
{
    public string fileName = "Entity_001";
    private EntityData entityData;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Hurtbox hurtboxPrefab;
    private Hurtbox hurtbox;
    public Vector2 groundCheckSize = new(0.5f, 0.1f);
    public Vector2 groundCheckOffset = new(0f, 0f);

    [field: SerializeField] public int facing { get; private set; }
    [field: SerializeField] public bool isHurtboxActive { get; private set; }
    [field: SerializeField] public bool isGround { get; private set; }
    [field: SerializeField] public bool isMoving { get; private set; }
    [field: SerializeField] public bool isAction { get; private set; }


    public event Action OnDamage;
    public event Action OnDead;
    public event Action OnUseSkill;
    public event Action OnDetectEnemy;

    void Awake()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Entities/Data/{fileName}");
        if (jsonFile != null)
        {
            entityData = JsonConvert.DeserializeObject<EntityData>(jsonFile.text);
        }
        else Debug.LogError("파일이 존재하지 않습니다.");

        hurtboxPrefab = Resources.Load<Hurtbox>("Common/Prefabs/Hurtbox");
        if (hurtboxPrefab == null)
        {
            Debug.LogError($"[에러] 경로가 틀렸거나 파일이 없습니다: {hurtboxPrefab == null}");
        }

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Setup();
    }


    public void Setup()
    {
        Debug.Log(hurtboxPrefab == null);
        if (hurtbox == null) hurtbox = Instantiate(hurtboxPrefab);

        hurtbox.Setup(entityData.hurtboxData);
        hurtbox.transform.SetParent(transform);
        hurtbox.transform.localPosition = new(entityData.hurtboxData.offset.x * facing, entityData.hurtboxData.offset.y, 0);


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
        isGround = Physics2D.OverlapBox(transform.position + (Vector3)groundCheckOffset, groundCheckSize, 0f, LayerMask.GetMask("Ground")) != null;
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
        Gizmos.DrawWireCube(groundCheckOffset, groundCheckSize);
    }
}
