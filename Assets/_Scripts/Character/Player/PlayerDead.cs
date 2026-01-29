using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        player?.ragdoll.RagdollActive(false);
    }

    public void PlayerAnimationDead()
    {
        player.anim.enabled = false;
        player.ragdoll.RagdollActive(true);
        player.GetComponent<CharacterController>().enabled = false;

        GameManager.instance.GameOver();
    }
}
