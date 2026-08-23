using UnityEngine;

public class Effect : MonoBehaviour
{
    private EffectData data;
    private GameObject caster;
    private GameObject skillObject;
    private int facing;
    public void Setup(GameObject caster, GameObject skillObject, EffectData data, int facing)
    {
        if (data.NullCheck(nameof(Effect))) return;
        this.facing = facing;
        this.data = data;
        this.caster = caster;
        this.skillObject = skillObject;
        transform.localScale = (Vector3)this.data.Size;
        CoroutineManager.Instance.Toggle(v => gameObject.SetActive(v), this.data.Startup, this.data.Duration);
    }
    public void MakeHitbox(int index)
    {
        SpawnManager.Instance.SpawnHitbox(caster, skillObject, data.Hitboxes[index], facing);
    }
}