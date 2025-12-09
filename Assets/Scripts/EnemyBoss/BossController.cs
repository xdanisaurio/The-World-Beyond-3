using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // Collider del boss
    private Collider _coliderBoss;

    [Header("Referencias externas")]
    public UnityEngine.UI.Image healthBar;
    public WaveManager waveManager;
    public BaseBossState previousState;

    [SerializeField] private BossLaserLine laserLine;
    private Animator _animBoss;

    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private float attackTimer = 0f;

    [Header("Detección")]
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

    // Estados
    public IdleBossState idleState;
    public BossAttackBasic attackBasicState;
    public BossAttackDistance attackDistanceState;
    public BossWaveState waveState;

    // ------------------------------
    // --- NUEVO ---
    // Evita que el boss cambie de estado hasta terminar la animación de ataque
    [Header("Control de Ataque")]
    public bool ataqueEnCurso = false;
    // ------------------------------

    // Encapsulamientos
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public float AttackTimer { get => attackTimer; set => attackTimer = value; }
    public float DistanceCooldown { get => _distanceCooldown; set => _distanceCooldown = value; }
    public float Timer { get => _timer; set => _timer = value; }
    public MachineStates MachineStates { get => _machineStates; set => _machineStates = value; }
    public BossLaserLine LaserLine { get => laserLine; set => laserLine = value; }
    public Animator AnimBoss { get => _animBoss; set => _animBoss = value; }
    public Collider ColiderBoss { get => _coliderBoss; set => _coliderBoss = value; }

    // Control de oleadas
    private bool wave70 = false;
    private bool wave50 = false;

    // ---------------------------
    // FASE ÚNICA DE PROYECTILES
    // ---------------------------
    [Header("Fase única de proyectiles")]
    public int projectilesInPhase = 2;
    public float projectileSpawnRadius = 10f;
    public float indicatorTime = 1f;
    private bool phaseActive = false;
    // ---------------------------

    private void Start()
    {
        _machineStates = GetComponent<MachineStates>();
        _animBoss = GetComponent<Animator>();
        _coliderBoss = GetComponent<Collider>();

        idleState = new IdleBossState(this);
        attackBasicState = new BossAttackBasic(this);
        attackDistanceState = new BossAttackDistance(this);
        waveState = new BossWaveState(this, waveManager);

        _machineStates.SetState(idleState);

        // --- CAMBIO ---
        // Ahora las transiciones verifican ataqueEnCurso
        _machineStates.AddTransition(idleState,
            new StateTransition(attackBasicState, () => PlayerInRage() && !ataqueEnCurso));

        _machineStates.AddTransition(attackBasicState,
            new StateTransition(attackDistanceState, () => !PlayerInRage() && !ataqueEnCurso));

        _machineStates.AddTransition(attackDistanceState,
            new StateTransition(attackBasicState, () => PlayerInRage() && !ataqueEnCurso));
        // --- CAMBIO ---
    }

    private void Update()
    {
        LookAtPlayer();
        CheckWaveTriggers();
        CheckPhaseActivation();
    }

    private void CheckWaveTriggers()
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

    private void TriggerWaveState()
    {
        previousState = _machineStates.currentState as BaseBossState;
        _machineStates.SetState(waveState);
    }

    private void CheckPhaseActivation()
    {
        if (!phaseActive && healthBar.fillAmount <= 0.3f)
        {
            phaseActive = true;
        }
    }

    public void AnimationEvent_BasicAttack()
    {
        if (_machineStates.currentState == attackBasicState)
            attackBasicState.DoBasicAttack();
    }

    // ---------------------------
    // --- NUEVO ---
    // Este evento viene al final de la animación de ataque
    // Se pone desde el Animator
    public void AnimationEvent_AtaqueTermino()
    {
        ataqueEnCurso = false;
    }
    // ---------------------------

    // ---------------------------
    // ATAQUE A DISTANCIA CON FASE ÚNICA
    // ---------------------------
    public void AnimationEvent_ShootProjectile()
    {
        if (_machineStates.currentState == attackDistanceState)
        {
            if (phaseActive)
                ShootPhaseProjectiles();
            else
                attackDistanceState.DoShootProjectile();
        }
    }

    private void ShootPhaseProjectiles()
    {
        if (distanceProjectile == null || player == null) return;

        for (int i = 0; i < projectilesInPhase; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * projectileSpawnRadius;
            Vector3 targetPos = player.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            GameObject proj = Instantiate(distanceProjectile, shootPivot.position, Quaternion.identity);
            ProjectileBoss pb = proj.GetComponent<ProjectileBoss>();
            if (pb != null)
            {
                pb.indicatorDelay = indicatorTime;
                pb.SetTarget(targetPos);
            }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
