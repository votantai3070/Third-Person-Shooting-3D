using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    private Enemy enemy;
    private Enemy_Melee enemyMelee;
    private Enemy_Zombie_Attack[] enemyZombieAttack;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        enemyMelee = GetComponentInParent<Enemy_Melee>();
        enemyZombieAttack = GetComponentsInChildren<Enemy_Zombie_Attack>(true);
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
        enemyMelee.EnableColliderEnemyZombie(true);
        enemy.audioManager.PlaySFX(enemyMelee.meleeSFX.swoosh, true);

        foreach (var enemyScript in enemyZombieAttack)
        {
            enemyScript.OnAttackAnimationEnd();
        }
    }
}
