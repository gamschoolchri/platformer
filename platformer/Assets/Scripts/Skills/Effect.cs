using UnityEngine;

public class Effect : MonoBehaviour
{
    private EffectData data;
    private int facing;
    public void Setup(EffectData data, int facing)
    {
        if (data.NullCheck(GetType().Name)) return;
        this.facing = facing;
        this.data = data;
        transform.localScale = (Vector3)this.data.Size;
        CoroutineManager.Instance.ActiveToggle(gameObject, this.data.Startup, this.data.Duration);
    }
    public void MakeHitbox(int index)
    {
        SpawnManager.Instance.SpawnHitbox(gameObject, data.Hitboxes[index], facing);
    }
}