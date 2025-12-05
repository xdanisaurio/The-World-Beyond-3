using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Componentes")]
    public NavMeshAgent Agent { get => _agent; set => _agent = value; }
    private NavMeshAgent _agent;

    [SerializeField] private Animator _animBasic;

    private MachineStates _machineStates;
    private EnemyPatrol _patrol;
    private EnemyChase _chase;
    private EnemyAttack _attack;

    [Header("Movimiento y Rotacion")]
    public float RotationSpeed = 5f;

    [Header("Deteccion del jugador")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float alertRadius = 3f;
    [SerializeField] private Transform _target;
    private Vector3 _lastKnwonTargetPosition;

    public Transform Target { get => _target; set => _target = value; }
    public Vector3 LastKnwonTargetPosition { get => _lastKnwonTargetPosition; set => _lastKnwonTargetPosition = value; }

    [Header("Movimiento y Patrullaje")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] List<Transform> _pivotes;
    [SerializeField] float minWaitTime = 1f;
    [SerializeField] float maxWaitTime = 3f;
    private float idleTime;
    private float CurrentIdleTime;
    protected bool isWaiting = false;

    [Header("Ataque")]
    [SerializeField] private Transform attackPivot;
    [SerializeField] private GameObject attackObject;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float _attackActiveTime = 0.5f;
    private float currentAttackCooldown = 0f;
    private bool isAttackOnCooldown = false;

    [Header("Partícula del Ataque")]
    [SerializeField] private ParticleSystem attackParticlePrefab; // Prefab de la partícula
    private ParticleSystem currentAttackParticle;                 // Instancia activa
    [SerializeField] private float attackParticleDuration = 1f;   // 🔥 Tiempo ajustable desde el inspector

    public List<Transform> Pivotes { get => _pivotes; set => _pivotes = value; }
    public float AlertRadius { get => alertRadius; set => alertRadius = value; }
    public EnemyAttack Attack { get => _attack; set => _attack = value; }
    public EnemyChase Chase { get => _chase; set => _chase = value; }
    public MachineStates MachineStates { get => _machineStates; set => _machineStates = value; }
    public EnemyPatrol Patrol { get => _patrol; set => _patrol = value; }
    public float AttackActiveTime { get => _attackActiveTime; set => _attackActiveTime = value; }
    public Animator AnimBasic { get => _animBasic; set => _animBasic = value; }

    private void Start()
    {
        _machineStates = GetComponent<MachineStates>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = moveSpeed;
        AnimBasic = GetComponent<Animator>();

        _patrol = new EnemyPatrol(this);
        _chase = new EnemyChase(this);
        _attack = new EnemyAttack(this);

        idleTime = Random.Range(minWaitTime, maxWaitTime);
        CurrentIdleTime = 0;

        _machineStates.SetState(Patrol);

        _machineStates.AddTransition(_patrol, new StateTransition(_chase, () => IsPlayerDetected() && !IsPlayerAlertRadius()));
        _machineStates.AddTransition(_patrol, new StateTransition(_attack, () => IsPlayerAlertRadius()));
        _machineStates.AddTransition(_chase, new StateTransition(_attack, () => IsPlayerAlertRadius()));
        _machineStates.AddTransition(_attack, new StateTransition(_chase, () => !IsPlayerAlertRadius() && IsPlayerDetected()));
        _machineStates.AddTransition(_chase, new StateTransition(_patrol, () => !IsPlayerDetected()));
        _machineStates.AddTransition(_attack, new StateTransition(_patrol, () => !IsPlayerDetected()));
    }

    private void Update()
    {
        if (_machineStates.currentState == _patrol)
            CurrentIdleTime += Time.deltaTime;
        else
            CurrentIdleTime = 0f;

        if (isAttackOnCooldown)
        {
            currentAttackCooldown -= Time.deltaTime;
            if (currentAttackCooldown <= 0)
                isAttackOnCooldown = false;
        }
    }

    public bool IsPlayerDetected()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        if (hits.Length > 0)
        {
            Target = hits[0].transform;
            return true;
        }
        Target = null;
        return false;
    }

    public bool IsPlayerAlertRadius()
    {
        if (Target == null) return false;
        return Vector3.Distance(transform.position, Target.position) <= alertRadius;
    }

    public void SaveTargetPositionAttack()
    {
        if (Target != null)
            _lastKnwonTargetPosition = Target.position;
    }

    public void SetAttackObject(bool active)
    {
        if (attackObject && attackPivot)
        {
            attackObject.transform.SetPositionAndRotation(attackPivot.position, attackPivot.rotation);
            attackObject.SetActive(active);
        }
    }

    public bool IsAttackOnCooldown() => isAttackOnCooldown;

    public void StarAttackCooldown()
    {
        isAttackOnCooldown = true;
        currentAttackCooldown = attackCooldown;
    }

    // SE EJECUTA EN EL FRAME EXACTO DEL GOLPE
    public void AttackFrameEvent()
    {
        // Activar objeto de ataque
        if (attackObject != null && attackPivot != null)
        {
            Vector3 dir = (_lastKnwonTargetPosition - attackPivot.position).normalized;

            attackObject.transform.position = attackPivot.position;
            attackObject.transform.rotation = Quaternion.LookRotation(dir);
            attackObject.SetActive(true);
        }

        // INSTANCIAR PARTÍCULA (NO SE DESACTIVA CON EL ATAQUE)
        if (attackParticlePrefab != null && attackPivot != null)
        {
            currentAttackParticle = Instantiate(
                attackParticlePrefab,
                attackPivot.position,
                attackPivot.rotation
            );

            currentAttackParticle.Play();

            // La destruimos después del tiempo configurado (independiente del ataque)
            Destroy(currentAttackParticle.gameObject, attackParticleDuration);
        }

        Invoke(nameof(DisableAttackObject), AttackActiveTime);
        DetectHit();
    }

    // AHORA SOLO DESACTIVA EL OBJETO — NO TOCA LA PARTÍCULA
    private void DisableAttackObject()
    {
        SetAttackObject(false);
    }

    private void DetectHit()
    {
        if (attackObject == null) return;

        float detectionRadius = 1.0f;

        Collider[] hits = Physics.OverlapSphere(attackObject.transform.position,
            detectionRadius, playerLayer);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                HealthSystem targetHealth = hit.GetComponent<HealthSystem>();
                if (targetHealth != null)
                {
                    IAffectHealth damageComp = attackObject.GetComponent<IAffectHealth>();
                    if (damageComp != null)
                    {
                        targetHealth.SetDamage(damageComp);
                        Debug.Log("Jugador golpeado");
                    }
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }
}
