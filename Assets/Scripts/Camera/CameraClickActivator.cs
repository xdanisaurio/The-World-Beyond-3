using UnityEngine;


public class CameraClickActivator : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject playerObject;
    private CharacterController player;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }

        if (playerObject != null)
        {
            player = playerObject.GetComponent<CharacterController>();
        }
        else
        {
            Debug.Log("");
        }

    }



    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrigerPlayerAttack();
        }
    }



    private void TrigerPlayerAttack()
    {
        if (player == null)
        {
            return;
        }

        if (player.RechargeCount > 0f)
        {
            return;
        }

        player.AttackCollider.SetActive(true);
        player.StartAttackCoroutine();
        player.RechargeCount = player.RechargeTime;
    }
}
