using UnityEngine;

public static class Extensions
{
    public static GameObject FindClosestTarget(this Transform transform, float radius, string targetTag = null, LayerMask layerMask = default)
    {
        int mask = (layerMask.value == 0) ? ~0 : layerMask.value;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, mask);

        GameObject closestTarget = null;
        float minDistance = float.MaxValue;
        bool hasTag = !string.IsNullOrEmpty(targetTag);

        foreach (var col in hitColliders)
        {
            if (!hasTag || col.CompareTag(targetTag))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTarget = col.gameObject;
                }
            }
        }

        return closestTarget;
    }
}