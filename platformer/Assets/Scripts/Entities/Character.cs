using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Entity))]
public class Character : MonoBehaviour
{

    private CharacterDataSO so;
    public CharacterDataSO CharacterDataSO => so;
    private CharacterData data;
    public ref CharacterData CharacterData => ref data;
    private Entity ent;

    private Hurtbox hurtbox;
    [SerializeField] private bool isHurtboxActive = false;
    public bool IsHurtboxActive => isHurtboxActive;



    void Awake()
    {
        ent = GetComponent<Entity>();

        so = ent.EntityDataSO as CharacterDataSO;
        so.NullCheck(nameof(Character));
        data = new(this, so);
        Setup(); //soon delete

    }
    void OnEnable()
    {
        CharacterManager.Instance.RegisterCharacter(this);
    }
    void OnDisable()
    {
        CharacterManager.Instance.UnRegisterCharacter(this);
    }

    private void Setup()
    {
        if (hurtbox == null) hurtbox = Instantiate(Settings.Instance.defaultHurtbox, transform);
        hurtbox.Setup(data.HurtboxData);
        SetHurtboxActive(true);
    }

    public void OnAttack(AttackerHitData attackerHitData) { }
    public void OnDamage(DefenderHitData defenderHitData) { }
    public void SetHurtboxActive(bool isActive)
    {
        hurtbox.gameObject.SetActive(isActive);
        isHurtboxActive = isActive;
    }
}