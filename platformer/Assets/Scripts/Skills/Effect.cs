using UnityEngine;

public class Effect : MonoBehaviour
{
    private EffectData data;
    private GameObject caster;
    private GameObject skillObject;
    public void Setup(GameObject caster, GameObject skillObject, EffectData data)
    {
        if (data.NullCheck(nameof(Effect))) return;
        Debug.Log(data.Duration);
        this.data = data;
        this.caster = caster;
        this.skillObject = skillObject;
        transform.localScale = (Vector3)this.data.Size;
        CoroutineManager.Instance.Toggle(value => gameObject.SetActive(value), this.data.Startup, this.data.Duration);
    }
    public void MakeHitbox(int index)
    {
        SpawnManager.Instance.SpawnHitbox(caster, skillObject, data.Hitboxes[index]);
    }
}