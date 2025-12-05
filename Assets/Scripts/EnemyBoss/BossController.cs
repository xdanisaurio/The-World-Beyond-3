using Unity.VisualScripting;
using UnityEngine;


public class BossController : MonoBehaviour
{
    //Collider Del boss.
    private Collider _coliderBoss;

    //Nuevo
    [Header("Referencias externas")]
    //UI vida
    public UnityEngine.UI.Image healthBar;
    //Sistema de oleadas 
    public WaveManager waveManager;

    
    public BaseBossState previousState;//Nuevo



    //Del scripts del rayoLazer.
    [SerializeField]private BossLaserLine laserLine;

    //Variable de animacione.
    private Animator _animBoss;



    //Variables de EstadoAttackBasic
    [SerializeField]private float attackCooldown = 5f;
    [SerializeField]private float attackTimer = 0f;



    [Header("Deteccion")]
    public Transform player;
    public float detectionRadius = 5f;



    [Header("Ataque cuerpo a cuerpo")]
    public GameObject attackObject;



    [Header("Ataque a distancia")]
    public GameObject distanceProjectile;
    public Transform shootPivot;
    [SerializeField] private float _distanceCooldown = 2f;
    [SerializeField] private float _timer;
    


    private MachineStates _machineStates;




    //Estados
    public IdleBossState idleState;
    public BossAttackBasic attackBasicState;
    public BossAttackDistance attackDistanceState;

    public BossWaveState waveState;//Estado nuevo



    //Encapsulamientos de las variables attackBasic.
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public float AttackTimer { get => attackTimer; set => attackTimer = value; }
    public float DistanceCooldown { get => _distanceCooldown; set => _distanceCooldown = value; }
    


    //Encapsulamientos de las variables attackDistance. 
    public float Timer { get => _timer; set => _timer = value; }

    

    //Encapsulamiento de la mecanicaDeEstados.
    public MachineStates MachineStates { get => _machineStates; set => _machineStates = value; }
    
    

    //Encapsulamiento del rayolazer.
    public BossLaserLine LaserLine { get => laserLine; set => laserLine = value; }
    

    
    //Encapsulamiento de Animator.
    public Animator AnimBoss { get => _animBoss; set => _animBoss = value; }
    public Collider ColiderBoss { get => _coliderBoss; set => _coliderBoss = value; }//EncasulamientoColider.


    //Nuevo - control de oleadas
    private bool wave70 = false;
    private bool wave50 = false;



    private void Start()
    {
        _machineStates = GetComponent<MachineStates>();
        _animBoss = GetComponent<Animator>();
        _coliderBoss = GetComponent<Collider>(); 

        idleState = new IdleBossState(this);
        attackBasicState = new BossAttackBasic(this);
        attackDistanceState = new BossAttackDistance(this);

        waveState = new BossWaveState(this, waveManager);//Nuevo


        //Estado inicial del juego.
        _machineStates.SetState(idleState);

        //Transiciones de los estados.
        _machineStates.AddTransition(idleState, new StateTransition(attackBasicState, () => PlayerInRage()));
        _machineStates.AddTransition(attackBasicState, new StateTransition(attackDistanceState, () => !PlayerInRage()));
        _machineStates.AddTransition(attackDistanceState, new StateTransition(attackBasicState, () => PlayerInRage()));

    }


    //Nuevo
    private void Update()
    {
        LookAtPlayer();
        CheckWaveTriggers();
    }



    void CheckWaveTriggers()
    {
        float hp = healthBar.fillAmount;

        if (!wave70 && hp <= 0.70f)
        {
            wave70 = true;
            TriggerWaveState();
        }

        if (!wave50 && hp <= 0.30f)
        {
            wave50 = true;
            TriggerWaveState();
        }
    }



    void TriggerWaveState()
    {
        previousState = _machineStates.currentState as BaseBossState;
        _machineStates.SetState(waveState);
    }
    //





    public void AnimationEvent_BasicAttack()
    {
        if (_machineStates.currentState == attackBasicState)
        {
            attackBasicState.DoBasicAttack();
        }
    }



    public void AnimationEvent_ShootProjectile()
    {
        if (_machineStates.currentState == attackDistanceState)
        {
            attackDistanceState.DoShootProjectile();
        }
    }

    
    public void AnimationEvent_WaveFinished()
    {
        waveState.AnimationEvent_WaveFinished();
    }


    public bool PlayerInRage()
    {
        if (player == null) return false;

        float distance = Vector3.Distance(player.position, transform.position);
        return distance <= detectionRadius;
    }


    private void LookAtPlayer()
    {
        if (player == null) return;
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }
    }



    //GIZMOS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

    }


}
