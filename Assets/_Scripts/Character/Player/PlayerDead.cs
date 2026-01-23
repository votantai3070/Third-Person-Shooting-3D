using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void PlayerAnimationDead()
    {
        player.anim.enabled = false;
    }
}
