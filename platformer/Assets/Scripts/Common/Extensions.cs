using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static GameObject FindClosestTarget(this Transform transform, float radius, string targetTag = null, LayerMask layerMask = default)
    {
        int mask = (layerMask.value == 0) ? ~0 : layerMask.value;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, mask);

        GameObject closestTarget = null;
        float minDistance = float.MaxValue;
        bool hasTag = !string.IsNullOrEmpty(targetTag);

        foreach (var col in hitColliders)
        {
            if (col.transform == transform) continue;
            if (!hasTag || col.CompareTag(targetTag))
            {
                float distance = (col.transform.position - transform.position).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTarget = col.gameObject;
                }
            }
        }

        return closestTarget;
    }
    public static int Facing(this GameObject target) => target.TryGetComponent(out SpriteRenderer sr) && sr.flipX ? -1 : 1;

    public static bool NullCheck<T>(this T target, string name)
    {
        if (target is Object unityObj)
        {
            if (unityObj == null)
            {
                Debug.LogError($"[{name}] {typeof(T).Name} (UnityObject) is null or destroyed");
                return true;
            }
        }
        else if (EqualityComparer<T>.Default.Equals(target, default))
        {
            Debug.LogError($"[{name}] {typeof(T).Name} is null or default");
            return true;
        }
        return false;
    }


    public static void DrawGizmo(this VectorRange range, Vector2 position, Color color)
    {
        Color originalColor = Gizmos.color;
        Gizmos.color = color;
        Gizmos.DrawWireCube(position + range.Center, range.Size);
        Gizmos.color = originalColor;
    }
}

