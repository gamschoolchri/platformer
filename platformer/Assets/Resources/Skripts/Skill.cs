using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;
public class Skill : MonoBehaviour
{
    public string fileName = "Skill_001";

    private MovementType movementType;
    private SkillData skillData;

    private Hitbox hitboxPrefab;
    private Hitbox hitbox;
    private bool isHitboxActive;

    private Effect effectPrefab;
    private Effect effect;
    private bool isEffectActive;

    private float timer;

    void Awake()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Skills/Data/{fileName}");
        Debug.Log($"Skills/Data/{fileName}");
        if (jsonFile != null)
        {
            skillData = JsonConvert.DeserializeObject<SkillData>(jsonFile.text);
            movementType = skillData.movementType;
        }
        else Debug.LogError("파일이 존재하지 않습니다.");

        hitboxPrefab = Resources.Load<Hitbox>("Common/Prefabs/Hitbox");
        effectPrefab = Resources.Load<Effect>(skillData.effectData.prefabPath);
        if (hitboxPrefab == null || effectPrefab == null)
        {
            Debug.LogError($"[에러] 경로가 틀렸거나 파일이 없습니다: {hitboxPrefab == null} {effectPrefab == null}");
            return;
        }
    }

    public void Setup(int facing)
    {

        if (hitbox == null) hitbox = Instantiate(hitboxPrefab);

        hitbox.Setup(new Vector3(skillData.hitboxData.size.x, skillData.hitboxData.size.y, 1f), skillData.hitboxData);
        hitbox.transform.SetParent(transform);
        hitbox.transform.localPosition = new(skillData.hitboxData.offset.x * facing, skillData.hitboxData.offset.y, 0);

        if (effect == null) effect = Instantiate(effectPrefab);

        Vector3 effectSize = new(skillData.effectData.size.x, skillData.effectData.size.y, 1f);
        if (movementType != MovementType.Static) effectSize.x *= facing;
        effect.Setup(effectSize, skillData.effectData);
        effect.transform.SetParent(transform);
        effect.transform.localPosition = new(skillData.effectData.offset.x * facing, skillData.effectData.offset.y, 0);

        hitbox.gameObject.SetActive(false);
        isHitboxActive = false;
        effect.gameObject.SetActive(false);
        isEffectActive = false;
        timer = 0f;
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (!isHitboxActive && timer > skillData.hitboxData.startup)
        {
            hitbox.gameObject.SetActive(true);
            isHitboxActive = true;
        }
        if (!isEffectActive && timer > skillData.effectData.startup)
        {
            effect.gameObject.SetActive(true);
            isEffectActive = true;
        }
        if (timer > skillData.duration) gameObject.SetActive(false);

    }
    public MovementType MovementType
    {
        get { return movementType; }
    }
}
