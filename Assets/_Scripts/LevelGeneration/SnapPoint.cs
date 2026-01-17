using UnityEngine;

public enum SnapPointType { Enter, Exit, Spawn }


public class SnapPoint : MonoBehaviour
{
    public SnapPointType snapPointType;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnValidate()
    {
        gameObject.name = "SnapPoint - " + snapPointType.ToString();
    }
}
