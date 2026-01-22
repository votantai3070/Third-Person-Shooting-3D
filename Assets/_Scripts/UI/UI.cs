using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    public UI_Ingame ingameUI { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ingameUI = GetComponentInChildren<UI_Ingame>();
    }
}
