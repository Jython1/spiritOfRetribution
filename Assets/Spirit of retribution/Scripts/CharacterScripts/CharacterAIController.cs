using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAIController : MonoBehaviour
{
    [SerializeField]private float _sightRadius;
    [Range(0,360)]
    [SerializeField]private float _sightAngle;

    private UnityEngine.AI.NavMeshAgent _agent;

    private int _currentPatrolPointID;

    private GameObject _enemy;

    private bool _isDoingQuest;
    private Coroutine _patrolCoroutine;

    [SerializeField]private Transform []_patrolPoints;
    [SerializeField]private float _delayBetweenPoints;
    [SerializeField]private LayerMask _characterMask;
    [SerializeField]private LayerMask _obstructionMask;

    private void Start()
    {
        if(!gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>() && !_isDoingQuest)
        return;


        _currentPatrolPointID = 0;
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        StartCoroutine(FOVRoutine());

        if(_patrolPoints.Length > 0)
        Patrol();
    }

    private IEnumerator FOVRoutine()
    {
        if(_enemy == null)
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);

            while (true)
            {
                yield return wait;
                EnemyDetection();
                
            }

        }

    }

    IEnumerator WaitForOtherPoint()
    {

        yield return new WaitForSeconds(_delayBetweenPoints);
        _currentPatrolPointID = (_currentPatrolPointID + 1) % _patrolPoints.Length;
        Patrol();
    }

    void Update()
    {
        if(_enemy != null)
        {
            if (_patrolCoroutine != null)
            {
                StopCoroutine(_patrolCoroutine);
                _patrolCoroutine = null;
            }

            LookAtPosition(_enemy.transform);
            GoToPosition(_enemy.transform, 3f);
            float distanceBetween = Vector3.Distance(transform.position, _enemy.transform.position);
            if(distanceBetween > _sightRadius)
            {
                _enemy = null;
                Patrol();
            }

        }

    }

    private void Patrol()
    {
        GoToPosition(_patrolPoints[_currentPatrolPointID], 0f);

        if (_patrolCoroutine != null)
        StopCoroutine(_patrolCoroutine);

        _patrolCoroutine = StartCoroutine(WaitForOtherPoint());
    }


    public void GoToPosition(Transform position, float stoppingDistance)
    {
        _agent.stoppingDistance = stoppingDistance;
        _agent.destination = position.position;
    }




    public void LookAtPosition(Transform target)
    {
            Vector3 direction = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            float rotationSpeed = 5f;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }


    private void EnemyDetection()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _sightRadius, _characterMask);

        if (rangeChecks.Length != 0)
        {
            GameObject target = rangeChecks[0].gameObject;
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _sightAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
                    _enemy = target;
                else
                    _enemy = null;
            }
            else
                _enemy = null;
        }
    }
}
