using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    private Player player;

    private bool isDead = false;

    private void Start()
    {
        player = GetComponent<Player>();

        Debug.Log("Animator: " + player.anim);
    }

    public void PlayerAnimationDead()
    {
        isDead = true;
        player.anim.enabled = false;
        player.ragdoll.RagdollActive(true);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
