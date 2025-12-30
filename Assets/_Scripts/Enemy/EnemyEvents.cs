using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    // Animation Event
    public void EnabledTrigger()
    {
        enemy.isTrigger = true;
    }
}
