using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoverScript;
using CharStats;
using HealthScript;
using CharacterStateInterface;
using PatrolStateScript;
using PerceptionScript;
using AttackStateScript;
using CoverStateScript;

namespace AIController
{
    public class CharacterAIController : MonoBehaviour
    {
        private ICharacterState _currentState;
        public Transform[] patrolPoints;

        public LayerMask characterMask;
        public LayerMask coverMask;
        public float detectionRadius = 5f;
        public float sightAngle = 120f;
        public LayerMask obstructionMask;
        private PerceptionSystem _perception;
        private int _currentPatrolIndex;
        private UnityEngine.AI.NavMeshAgent _agent;
        private GameObject _currentEnemy;
        private bool _isInCover;
        private Rigidbody _rb;
        private Animator _animator;
        private Vector3 _initialLocation;

        

        private void Start() 
        {
            if(!gameObject.GetComponent<Rigidbody>())
            return;
            
            _rb = gameObject.GetComponent<Rigidbody>();


            SetPatrol();

            _perception = new PerceptionSystem(gameObject.transform, detectionRadius, sightAngle, characterMask, coverMask, obstructionMask);

            if(!gameObject.GetComponent<Animator>())
            return;

            _animator = gameObject.GetComponent<Animator>();

            if(!gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>())
            return;

            _agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            _initialLocation = gameObject.transform.position;

        }

        private void Update() 
        {
            if (!_agent)
                return;

            _currentState?.Execute();

  
            float actualSpeed = _agent.velocity.magnitude;

            if (actualSpeed < 0.1f)
                actualSpeed = 0f;

            float currentSpeed = _animator.GetFloat("Speed");
            float smoothedSpeed = Mathf.Lerp(currentSpeed, actualSpeed, Time.deltaTime * 10f);
            _animator.SetFloat("Speed", smoothedSpeed);

            if (_isInCover)
            {
                float distanceFromStartToEnemy = Vector3.Distance(_initialLocation, _currentEnemy.transform.position);
                if(distanceFromStartToEnemy >= 50f)
                {
                    SetPatrol();
                }

                float distanceToEnemy = Vector3.Distance(gameObject.transform.position, _currentEnemy.transform.position);
                if(distanceToEnemy >= 15f)
                {
                    _isInCover = false;
                    //.GetComponent<Cover>().LeaveCover();
                }
                return;
            }

            if (_currentEnemy)
            {
                var detectedCover = _perception.DetectCover();
                if (!detectedCover)
                    return;

                SetState(new CoverState(this, detectedCover, _currentEnemy));
                detectedCover.GetComponent<Cover>().TakeCover();
                _isInCover = true;
                return;
            }

            var detectedEnemy = _perception.DetectEnemy();
            if (!detectedEnemy)
                return;

            _currentEnemy = detectedEnemy;
            SetState(new AttackState(this, _currentEnemy));
        }

        public float GetSpeed()
        {
                float speed = _agent.velocity.magnitude;  
                if (speed < 0.1f) speed = 0f;
                return speed;
        }

        public void SetState(ICharacterState newState)
        {
            _currentState = newState;
        }

        public UnityEngine.AI.NavMeshAgent GetAgent()
        {
            return _agent;
        }

        public void SetPatrol()
        {
            SetState(new PatrolState(this ,patrolPoints));
            _currentEnemy = null;
        }

        public bool IsInCover
        {
            get { return _isInCover; }
            set {_isInCover = value;}
        }

    }
}

        /*
        [SerializeField]private float _sightRadius;
        [Range(0,360)]
        [SerializeField]private float _sightAngle;
        [SerializeField]private float _defaultAttackDistance;
        [SerializeField]private float _closeAttackDistance;
        [SerializeField]private float _walkSpeed;
        [SerializeField]private float _runSpeed;
        [SerializeField]private float _slowRunSpeed;
        [SerializeField]private Transform []_patrolPoints;
        [SerializeField]private float _delayBetweenPoints;
        [SerializeField]private LayerMask _characterMask;
        [SerializeField]private LayerMask _obstructionMask;
        [SerializeField]private LayerMask _coverMask;

        private CharacterPerception _aiPerception;
        private UnityEngine.AI.NavMeshAgent _agent;
        private int _currentPatrolPointID;
        private GameObject _enemy;
        private Cover _cover;
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
            _walkSpeed = 2.5f;
            _slowRunSpeed = 3.5f;
            _runSpeed = 5.5f;

            if(_patrolPoints.Length > 0)
            StartCoroutine(PatrolingLoop());
            

            if(!gameObject.GetComponent<CharacterPerception>())
            return;

            _aiPerception = gameObject.GetComponent<CharacterPerception>();
            _aiPerception.OnNoiseHeard += HearNoise;
            


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
                _agent.speed = _walkSpeed;
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
                _agent.speed = _runSpeed;
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
                    _cover.TakeCover();
                    float distanceBetweenEnemy = Vector3.Distance(_enemy.transform.position, _cover.transform.position);
                    if(distanceBetweenEnemy > _defaultAttackDistance)
                    {
                        _cover.LeaveCover();
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

                if (Vector3.Angle(transform.forward, directionToTarget) < _sightAngle / 2 && IsEnemy(target))
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

        private bool IsEnemy(GameObject character)
        {
            if (character == gameObject) return false;

            bool hasEnemyStats = character.TryGetComponent<CharacterStats>(out var enemyStats);
            bool hasOwnerStats = gameObject.TryGetComponent<CharacterStats>(out var ownerStats);

            if (hasEnemyStats && hasOwnerStats && enemyStats.GetSide != ownerStats.GetSide)
            {
                bool hasHealth = character.TryGetComponent<Health>(out var health);
                bool isAlive = !hasHealth || !health.isDead();
                return isAlive;
            }

            return false;

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
                        _cover = targetCover;
                        
                        break;
                    }
                }
            }
        }


        private void HearNoise(Vector3 position)
        {
            Debug.Log("I hear noise");
        }
    */