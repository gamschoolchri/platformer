using UnityEngine;

public class Effect : MonoBehaviour
{
    private EffectData effectData;
    private int facing;
    public void Setup(EffectData data, int facing)
    {
        this.facing = facing;
        effectData = data;
        transform.localScale = (Vector3)effectData.Size;
        CoroutineManager.Instance.ActiveToggle(gameObject, effectData.Startup, effectData.Duration);
        MakeHitbox(0);
    }
    public void MakeHitbox(int index)
    {
        SkillManager.Instance.SpawnHitbox(gameObject, effectData.Hitboxes[index], facing);
    }
}