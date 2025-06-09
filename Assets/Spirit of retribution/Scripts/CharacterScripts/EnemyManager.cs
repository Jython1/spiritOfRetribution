using UnityEngine;
using System.Collections.Generic;

public class EnemyManager: MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private List<Transform> activeAttackers = new List<Transform>();
    public float minDistanceBetweenEnemies = 10f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterAttacker(Transform enemy)
    {
        if (!activeAttackers.Contains(enemy))
            activeAttackers.Add(enemy);
    }

    public void UnregisterAttacker(Transform enemy)
    {
        if (activeAttackers.Contains(enemy))
            activeAttackers.Remove(enemy);
    }

    public Vector3 GetAvoidanceOffset(Transform enemy, Vector3 targetPosition)
    {
        Vector3 offset = Vector3.zero;

        foreach (var other in activeAttackers)
        {
            if (other == enemy) continue;

            float distance = Vector3.Distance(enemy.position, other.position);
            if (distance < minDistanceBetweenEnemies)
            {
                Vector3 directionAway = (enemy.position - other.position).normalized;
                float repulsionStrength = 1 - (distance / minDistanceBetweenEnemies);
                offset += directionAway * repulsionStrength;
            }
        }

        return offset;
    }
}
