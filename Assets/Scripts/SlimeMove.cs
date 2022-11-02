using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SlimeMove : MonoBehaviour
{
    private bool _detectedPlayer;
    private GameObject _playerRef;

    [SerializeField] private NavMeshAgent _navMeshAgent;

    [SerializeField] private List<Vector3> _patrolPoints;
    private int _patrolIndex = -1;

    /*Stats may differ between slimes*/
    private float _speed;
    private float _walkRange; //how far around they will patrol
    private float _detectRange;

    public void Initialize(float speed = 1f, float walkRange = 5f, float detectRange = 5f, List<Vector3> patrolPoints = null)
    {

        _speed = speed;
        _walkRange = walkRange;
        //more of a formallity since we set the collider radious 
        _detectRange = detectRange;

        if (patrolPoints == null)
        {
            GeneratePatrol();
        }

        //collision only possible if player was intended to have rigid body
        //_sphereCollider.radius = _detectRange;
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    #region EventFunctions
    void Awake()
    {
        Initialize();
        Patrol();
    }

    void Start()
    {
        _playerRef = GameObject.Find("Player");
    }
    void Update()
    {
        if (_detectedPlayer)
        {
            MoveTowardPlayer();
        }
        else if (PathNeedsUpdate())
        {
            Patrol();
        }

        if (!_detectedPlayer && Vector3.Distance(transform.position, _playerRef.transform.position) < _detectRange)
        {
            _detectedPlayer = true;
        }
        else if (_detectedPlayer && Vector3.Distance(transform.position, _playerRef.transform.position) > _detectRange)
        {
            _detectedPlayer = false;
        }

    }
    #endregion

    #region pathfinding
    protected virtual void GeneratePatrol()
    {
        int points = Random.Range(2, 6);
        for (int i = 0; i < points; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * _speed;

            randomDirection += transform.position;

            NavMeshHit hit;

            Vector3 finalPosition = Vector3.zero;

            if (NavMesh.SamplePosition(randomDirection, out hit, _speed, 1))
            {
                finalPosition = hit.position;
            }

            _patrolPoints.Add(finalPosition);
        }
    }

    /// <summary>
    /// Returns true if the nmAgent is close to destination and not already finding a new path
    /// </summary>
    /// <returns></returns>
    protected virtual bool PathNeedsUpdate()
    {
        return !(_navMeshAgent.pathPending) && (_navMeshAgent.remainingDistance < 0.5f);
    }

    /// <summary>
    /// Updates destination to simulate patrol movement
    /// </summary>
    /// <remarks>Will return the slimes to normal patrol if they have broken LOS with player</remarks>
    protected virtual void Patrol()
    {
        _patrolIndex++;
        _patrolIndex %= _patrolPoints.Count;
        _navMeshAgent.destination = _patrolPoints[_patrolIndex];
    }

    /// <summary>
    /// Update player as destination
    /// </summary>
    protected virtual void MoveTowardPlayer()
    {
        _navMeshAgent.SetDestination(_playerRef.transform.position);
    }
    #endregion
}