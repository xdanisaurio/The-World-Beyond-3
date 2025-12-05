using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{

    [SerializeField] private GameObject _attackPivot;
    [SerializeField] private GameObject _attackCollider;
    [SerializeField] private float _attackDuration = 1.5f;
    [SerializeField] private float _rechargeTime = 2f;
    private NavMeshAgent agent;


    private MachineStates machineStates;
    private IdleState idle;
    private WalkState walk;
    private PlayerAttack attack;
    public Animator anim;
    private Rigidbody _rb;
    private Transform _player;
    private InteractSystem interact;
    private float _rechargeCount = 0f;

    // ------------------ SONIDO DE ATAQUE ------------------
    [SerializeField] private AudioClip attackSound;
    private AudioSource audioSourceAtaque;

    // ------------------ SONIDO DE CAMINAR -----------------
    [SerializeField] private AudioClip walkSound;
    private AudioSource audioSourceCaminar;



    public Rigidbody Rb { get => _rb; set => _rb = value; }
    public Transform Player { get => _player; set => _player = value; }
    public GameObject AttackPivot { get => _attackPivot; set => _attackPivot = value; }
    public GameObject AttackCollider { get => _attackCollider; set => _attackCollider = value; }
    public float AttackDuration { get => _attackDuration; set => _attackDuration = value; }
    public float RechargeTime { get => _rechargeTime; set => _rechargeTime = value; }
    public float RechargeCount { get => _rechargeCount; set => _rechargeCount = value; }


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        machineStates = GetComponent<MachineStates>();
        interact = GetComponent<InteractSystem>();
        _rb = GetComponent<Rigidbody>();
        _player = transform;
        anim = GetComponent<Animator>();

        // AUDIO DEL ATAQUE — se usa el AudioSource que ya tiene el personaje
        audioSourceAtaque = GetComponent<AudioSource>();

        // AUDIO DE CAMINAR — creamos un AudioSource exclusivo
        audioSourceCaminar = gameObject.AddComponent<AudioSource>();
        audioSourceCaminar.loop = true;
        audioSourceCaminar.playOnAwake = false;
        audioSourceCaminar.spatialBlend = 0.5f; // sonido semi-3D


        idle = new IdleState(this);
        walk = new WalkState(this);
        attack = new PlayerAttack(this);


        //Tranciciones entre estados.
        machineStates.SetState(idle);
        machineStates.AddTransition(idle, new StateTransition(walk, () => Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f));
        machineStates.AddTransition(walk, new StateTransition(idle, () => Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1f && Mathf.Abs(Input.GetAxis("Vertical")) < 0.1f));
        machineStates.AddTransition(idle, new StateTransition(attack, () => Input.GetMouseButtonDown(0)));
        machineStates.AddTransition(walk, new StateTransition(attack, () => Input.GetMouseButtonDown(0)));


        //Volver de nuevo al estado apropiado después del ataque.
        machineStates.AddTransition(attack, new StateTransition(walk, () => attack.CanExitState()
        && (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)));
        machineStates.AddTransition(attack, new StateTransition(idle, () => attack.CanExitState() && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1f
        && Mathf.Abs(Input.GetAxis("Vertical")) < 0.1f));


    }


    private void Update()
    {
        // Actualizar animación
        bool isWalking = machineStates.currentState == walk;
        anim.SetBool("IsWalking", isWalking);

        // -------- SONIDO DE CAMINAR --------
        if (isWalking)
        {
            if (!audioSourceCaminar.isPlaying && walkSound != null)
            {
                audioSourceCaminar.clip = walkSound;
                audioSourceCaminar.Play();
            }
        }
        else
        {
            if (audioSourceCaminar.isPlaying)
            {
                audioSourceCaminar.Stop();
            }
        }
        // -----------------------------------

        if (Input.GetKeyDown(KeyCode.E))
        {
            interact.Interact();
        }

    }


    public void StartAttackCoroutine()
    {
        anim.SetTrigger("Attack");//Activar animacion de ataque.

        // SONIDO DEL ATAQUE
        if (attackSound != null && audioSourceAtaque != null)
            audioSourceAtaque.PlayOneShot(attackSound);

        StartCoroutine(DesactivateAttackAfterTime(_attackDuration));
    }


    private System.Collections.IEnumerator DesactivateAttackAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (_attackCollider != null)
        {
            _attackCollider.gameObject.SetActive(false);
        }
    }

}

