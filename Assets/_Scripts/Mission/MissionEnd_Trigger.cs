using UnityEngine;

public class MissionEnd_Trigger : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player: " + other.gameObject.name + " entered mission end trigger.");
        Debug.Log("Player: " + player);

        if (other.gameObject != player)
            return;

        if (MissionManager.instance.MissionCompleted())
            Debug.Log("Level completed");
    }
}
