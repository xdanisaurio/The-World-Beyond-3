using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyInterController : MonoBehaviour
{
    //Instanciar el boomerang aqui.
    [SerializeField] private GameObject boomerangPrefab;
    public GameObject BoomerangPrefab { get => boomerangPrefab; set => boomerangPrefab = value; }

    public float _alertRadius = 2f;
    [SerializeField] private Transform boomerangSpawnPoint;

    private MachineStates machineStates;
    private EnemyInterPatrol patrol;
    private EnemyInterChase _chase;
    private EnemyInterAttack _attack;
    private Animator _animator;

    [SerializeField] bool _animationFinished;

    public NavMeshAgent Agent { get => _agent; set => _agent = value; }
    private NavMeshAgent _agent;

    public Transform Target { get => _target; set => _target = value; }
    [SerializeField] Transform _target;

    public List<Transform> Pivotes { get => _pivotes; set => _pivotes = value; }
    public float AlertRadius { get => _alertRadius; set => _alertRadius = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public EnemyInterChase Chase { get => _chase; set => _chase = value; }
    public Transform BoomerangSpawnPoint { get => boomerangSpawnPoint; set => boomerangSpawnPoint = value; }
    public bool AnimationFinished1 { get => _animationFinished; set => _animationFinished = value; }

    [SerializeField] List<Transform> _pivotes;

    [SerializeField] LayerMask playerLayer;
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float minWaitTime = 1f;
    [SerializeField] float maxWaitTime = 3f;
    protected bool isWaiting = false;

    private float idleTime;
    private float CurrentIdleTime;

    float _distanceToTarget;

    // 🔥 NUEVO: Referencia al sistema de salud para detectar muerte
    private EnemyHealth health;


    private void Start()
    {
        machineStates = GetComponent<MachineStates>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = moveSpeed;
        _animator = GetComponent<Animator>();

        // Obtener referencia al sistema de vida
        health = GetComponent<EnemyHealth>();

        patrol = new EnemyInterPatrol(this);
        _chase = new EnemyInterChase(this);
        _attack = new EnemyInterAttack(this);

        //Implementación nueva.
        idleTime = Random.Range(minWaitTime, maxWaitTime);
        CurrentIdleTime = 0;

        machineStates.SetState(patrol);

        machineStates.AddTransition(patrol, new StateTransition(_chase, () => IsPlayerDetected() && _distanceToTarget >= _alertRadius));
        machineStates.AddTransition(patrol, new StateTransition(_attack, () => IsPlayerDetected() && _distanceToTarget < _alertRadius));

        machineStates.AddTransition(_chase, new StateTransition(_attack, () => IsPlayerDetected() && _distanceToTarget < _alertRadius));
        machineStates.AddTransition(_chase, new StateTransition(patrol, () => !IsPlayerDetected()));

        machineStates.AddTransition(_attack, new StateTransition(_chase, () => IsPlayerDetected() && _distanceToTarget >= _alertRadius && _animationFinished));
        machineStates.AddTransition(_attack, new StateTransition(patrol, () => !IsPlayerDetected() && _animationFinished));
    }


    private void Update()
    {
        // SI EL ENEMIGO MUERE → DETENER TODO (rotación + movimiento + IA)
        if (health != null && health.IsDead)
        {
            if (_agent != null)
                _agent.enabled = false; // Evita movimiento por NavMesh

            return; // Evita toda la lógica de rotación y detección
        }

        // Comportamiento normal si NO está muerto
        if (machineStates.currentState == patrol)
        {
            CurrentIdleTime += Time.deltaTime;
        }
        else
        {
            CurrentIdleTime = 0;
        }

        if (_target != null)
        {
            Vector3 direction = _target.position - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            _distanceToTarget = Vector3.Distance(transform.position, _target.position);
        }
    }


    public bool IsPlayerDetected()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        _target = playerInRange.Length > 0 ? playerInRange[0].transform : null;
        return _target;
    }

    public void LaunchBoomerangFromAnimation()
    {
        Debug.Log("Estado actual: " + machineStates.currentState);
        if (machineStates.currentState == _attack)
        {
            _attack.LaunchBoomerangFromAnimation();
        }
    }

    public void AnimationFinished()
    {
        _animationFinished = true;
    }

    public void AnimationStarted()
    {
        _animationFinished = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AlertRadius);
    }
}
