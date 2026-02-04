using UnityEngine;

public class Car_Interaction : Interactable
{
    private Car_Controller car;
    [SerializeField] Transform playerView;

    private float defaultPlayerScale;

    [Header("Exit details")]
    [SerializeField] private float exitCheckRadius;
    [SerializeField] private Transform[] exitPoints;
    [SerializeField] private LayerMask whatToIngoreForExit;

    private void Awake()
    {
        car = GetComponent<Car_Controller>();
    }

    public override void Interact()
    {
        base.Interact();

        GetIntoTheCar();
        //Debug.Log("Enter Car");
    }

    private void GetIntoTheCar()
    {
        ControlsManager.instance.SwitchToCarControl();
        car.carHealthController.UpdateCarHealthUI();

        car.ActivatedCar(true);

        defaultPlayerScale = player.transform.localScale.x;

        player.transform.localScale = new Vector3(.05f, .05f, .05f);
        player.transform.parent = car.transform;
        player.transform.localPosition = Vector3.up / 2;

        ThirdPersonCameraController.instance.ChangeCameraTarget(transform, 20);
    }

    public void GetOutOfTheCar()
    {
        if (!car.activateCar)
            return;

        car.ActivatedCar(false);

        if (player != null)
        {
            player.transform.parent = null;
            player.transform.position = GetExitPoint();
            player.transform.transform.localScale = new Vector3(defaultPlayerScale, defaultPlayerScale, defaultPlayerScale);
        }

        ControlsManager.instance.SwitchToCharacterControls();
        ThirdPersonCameraController.instance.ChangeCameraTarget(playerView, 10);
    }

    private Vector3 GetExitPoint()
    {
        for (int i = 0; i < exitPoints.Length; i++)
        {
            if (IsExitClear(exitPoints[i].position))
            {
                return exitPoints[i].position;
            }
        }

        return exitPoints[0].position;
    }

    private bool IsExitClear(Vector3 point)
    {
        Collider[] colliders = Physics.OverlapSphere(point, exitCheckRadius, ~whatToIngoreForExit);

        return colliders.Length == 0;
    }


    private void OnDrawGizmos()
    {
        if (exitPoints.Length > 0)
        {
            foreach (var point in exitPoints)
            {
                Gizmos.DrawWireSphere(point.position, exitCheckRadius);
            }
        }
    }
}
