using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    private Enemy enemy;
    private Enemy_Melee enemyMelee;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        enemyMelee = GetComponentInParent<Enemy_Melee>();
    }

    // Animation Event
    public void EnabledTrigger()
    {
        enemy.isTrigger = true;
    }

    public void DisableTrailRenderer()
    {
        enemy.DisabledTrailRenderer();
    }

    public void EnableTrailRenderer()
    {
        enemy.EnabledTrailRenderer();
    }

    public void EnemyMeleeAttackCheck()
    {
        enemy.audioManager.PlaySFX(enemyMelee.meleeSFX.swoosh, true);
    }
}
