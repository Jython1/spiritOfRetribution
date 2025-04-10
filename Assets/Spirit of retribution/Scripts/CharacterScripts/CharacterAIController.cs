using System;
using CoverScript;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAIController : MonoBehaviour
{
    [SerializeField]private float _sightRadius;
    [Range(0,360)]
    [SerializeField]private float _sightAngle;
    [SerializeField]private float _defaultAttackDistance;
    [SerializeField]private float _closeAttackDistance;
    [SerializeField]private Transform []_patrolPoints;
    [SerializeField]private float _delayBetweenPoints;
    [SerializeField]private LayerMask _characterMask;
    [SerializeField]private LayerMask _obstructionMask;
    [SerializeField]private LayerMask _coverMask;

    private UnityEngine.AI.NavMeshAgent _agent;

    private int _currentPatrolPointID;

    private GameObject _enemy;
    private GameObject _cover;

    private bool _isDoingQuest;
    private Vector3 _initialPoint;



    private void Start()
    {


        if(!gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>() && !_isDoingQuest)
        return;

        _initialPoint = gameObject.transform.position;
        _currentPatrolPointID = 0;
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        StartCoroutine(EnemyDetectionLoop());

        if(_patrolPoints.Length > 0)
        StartCoroutine(PatrolingLoop());
    }

    private IEnumerator EnemyDetectionLoop()
    {
        if(_enemy == null)
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);

            while (true)
            {
                yield return wait;
                TryFindEnemy();
            }

        }

    }

    IEnumerator PatrolingLoop()
    {
        while(true)
        {
            GoToPosition(_patrolPoints[_currentPatrolPointID], 0f);

            
            while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
            {
                yield return null;
            }

            yield return new WaitForSeconds(_delayBetweenPoints);

            _currentPatrolPointID = (_currentPatrolPointID + 1) % _patrolPoints.Length;
        }
    }

    IEnumerator CoverDetectionLoop()
    {
        while(true)
        {
            if(_cover == null)
            {
                TryFindCover();
            }
            yield return new WaitForSeconds(0.1f);

        }
    }



    void Update()
    {
        if(_enemy != null)
        {

            if(!_cover)
            {

                LookAtPosition(_enemy.transform);
                GoToPosition(_enemy.transform, _defaultAttackDistance);
                float distanceBetween = Vector3.Distance(transform.position, _enemy.transform.position);
                StartCoroutine(CoverDetectionLoop());
                if(distanceBetween > _sightRadius)
                {
                    _enemy = null;
                    StartCoroutine(PatrolingLoop());
                }

            }

            else if(_cover)
            {
                LookAtPosition(_enemy.transform);
                GoToPosition(_cover.transform, 0f);
                float distanceBetweenEnemy = Vector3.Distance(_enemy.transform.position, _cover.transform.position);
                if(distanceBetweenEnemy > _defaultAttackDistance)
                {
                    _cover.GetComponent<Cover>().LeaveCover();
                    _cover = null;
                }

                
            }

            if(Vector3.Distance(_enemy.transform.position, _initialPoint) >= 100)
            {
                _enemy = null;
                StartCoroutine(PatrolingLoop());

            }
        }

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


    private void TryFindEnemy()
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

    private void TryFindCover()
    {
        Collider[] coverChecks = Physics.OverlapSphere(transform.position, (_sightRadius / 2f), _coverMask);

        foreach (Collider collider in coverChecks)
        {
            if (collider.TryGetComponent<Cover>(out Cover targetCover))
            {
                if (!targetCover.IsTaken())
                {
                    _cover = collider.gameObject;
                    
                    break;
                }
            }
        }
    }
}