using UnityEngine;

public class Effect : MonoBehaviour
{
    private float duration;
    private float timer;
    public void Setup(Vector3 scale, EffectData effectData)
    {
        transform.localScale = scale;
        duration = effectData.duration;
        timer = 0f;
    }
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > duration) gameObject.SetActive(false);
    }
}
