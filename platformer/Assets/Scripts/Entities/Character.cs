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
    [SerializeField] private bool OnGCD = false;
    public bool IsHurtboxActive => isHurtboxActive;



    void Awake()
    {
        ent = GetComponent<Entity>();

        so = ent.EntityDataSO as CharacterDataSO;
        so.NullCheck(nameof(Character));
        data = new(so);

    }
    void OnEnable()
    {
        CharacterManager.Instance.RegisterCharacter(this);
        Setup();
    }
    void OnDisable()
    {
        CharacterManager.Instance.UnRegisterCharacter(this);
    }

    public void Setup()
    {
        data.Setup(gameObject);
        if (hurtbox == null) hurtbox = Instantiate(Settings.Instance.DefaultHurtbox, transform);
        hurtbox.Setup(data.HurtboxData);
        SetHurtboxActive(true);
    }
    public void UseSkill(int? skillIndex = null)
    {
        if (OnGCD || data.SkillPacks == null) return;
        if (skillIndex == null)
        {
            foreach (SkillPack skillPack in data.SkillPacks)
            {
                if (skillPack.CanExecute())
                {
                    SpawnManager.Instance.SpawnSkill(gameObject, skillPack.Skill.name);
                    CoroutineManager.Instance.Toggle((value) => OnGCD = value, 0f, data.GCD);
                    return;
                }
            }
        }
        else
        {
            SkillPack skillPack = data.SkillPacks[skillIndex.Value];
            if (skillPack.CanExecute())
            {
                SpawnManager.Instance.SpawnSkill(gameObject, skillPack.Skill.name);
                CoroutineManager.Instance.Toggle((value) => OnGCD = value, 0f, data.GCD);
                return;
            }
        }
    }
    public void OnAttack(AttackerHitData attackerHitData) { }
    public void OnDamage(DefenderHitData defenderHitData) { }
    public void SetHurtboxActive(bool isActive)
    {
        hurtbox.gameObject.SetActive(isActive);
        isHurtboxActive = isActive;
    }
}