using UnityEngine;

public class Enemy_DropController : MonoBehaviour
{
    public GameObject missionObjectKey;

    public void GiveKey(GameObject newKey) => missionObjectKey = newKey;

    public void DropItem()
    {
        CreateItem(missionObjectKey);
    }

    private void CreateItem(GameObject go)
    {
        GameObject newItem = Instantiate(go, transform.position + Vector3.up, Quaternion.identity);
    }
}
