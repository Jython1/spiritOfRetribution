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
using CombatStateScript;
using CoverStateScript;
using CharacterAttackScript;
using ReturnStateScript;



namespace AIController
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(Health))]
    public class CharacterAIController : MonoBehaviour
    {


        private ICharacterState _currentState;

        [Header("Perception")]
        public Transform[] patrolPoints;
        public LayerMask characterMask;
        public LayerMask coverMask;
        public LayerMask obstructionMask;
        public float detectionRadius = 5f;
        public float sightAngle = 120f;

        [Header("Combat Behavior")]
        public float maxDistanceFromCover;
        public float maxDistanceFromStart;

        private PerceptionSystem _perception;
        private UnityEngine.AI.NavMeshAgent _agent;
        private GameObject _currentEnemy;
        private GameObject _detectedCover;
        private bool _isInCover;

        private Rigidbody _rb;
        private Animator _animator;
        private Transform _initialLocation;
        private Health _healthComp;

        private CharacterAttack _characterAttack;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            _healthComp = GetComponent<Health>();

            _initialLocation = transform;
            _perception = new PerceptionSystem(transform, detectionRadius, sightAngle, characterMask, coverMask, obstructionMask);

            _healthComp.onDeath += Death;

            SetPatrol();


        }

        private void Update()
        {
            if (_agent == null || _healthComp.isDead())
                return;

            //_agent.isStopped = false;

            _currentState?.Execute();

            HandleCharacterAttack();
            HandleAnimation();
            HandleCoverState();
            HandleEnemyDetection();
            CheckIfReturn();
        }

        private void HandleAnimation()
        {
            float actualSpeed = _agent.velocity.magnitude;
            if (actualSpeed < 0.1f) actualSpeed = 0f;

            float currentSpeed = _animator.GetFloat("Speed");
            float smoothedSpeed = Mathf.Lerp(currentSpeed, actualSpeed, Time.deltaTime * 10f);
            _animator.SetFloat("Speed", smoothedSpeed);
        }

        private void HandleCoverState()
        {
            if (!_isInCover || !_currentEnemy) return;

            float distance = Vector3.Distance(transform.position, _currentEnemy.transform.position);
            if (distance >= maxDistanceFromCover)
            {
                _isInCover = false;
                _detectedCover?.GetComponent<Cover>()?.LeaveCover();
                //SetState(new AttackState(this, _currentEnemy));
            }
        }

        private void HandleCharacterAttack()
        {
            if (!_characterAttack)
                return;

            _characterAttack?.Execute();
        }

        private void HandleEnemyDetection()
        {
            if (_currentEnemy)
            {
                _detectedCover = _perception.DetectCover();
                if (_detectedCover)
                {
                    _detectedCover.GetComponent<Cover>()?.TakeCover();
                    _isInCover = true;
                    SetState(new CoverState(this, _detectedCover, _currentEnemy));
                }
                return;
            }

            var detectedEnemy = _perception.DetectEnemy();
            if (detectedEnemy)
            {
                _currentEnemy = detectedEnemy;
                SetState(new CombatState(this, _currentEnemy));
                _characterAttack = GetComponent<CharacterAttack>();
                _characterAttack?.Initialize(_currentEnemy);
            }
        }

        public void CheckIfReturn()
        {
            if (!_currentEnemy) return;

            float distance = Vector3.Distance(_initialLocation.position, _currentEnemy.transform.position);
            if (distance >= maxDistanceFromStart)
            {
                SetState(new ReturningState(this, _initialLocation));
            }
        }

        public void SetState(ICharacterState newState)
        {
            _currentState = newState;
        }

        public UnityEngine.AI.NavMeshAgent GetAgent() => _agent;

        public void SetPatrol()
        {
            SetState(new PatrolState(this, patrolPoints));
            _currentEnemy = null;
        }

        public bool IsInCover
        {
            get => _isInCover;
            set => _isInCover = value;
        }

        private void Death()
        {
            _agent.isStopped = true;
            _animator.SetTrigger("Death");
            RemoveTargets();
            EnemyManager.Instance?.UnregisterAttacker(transform);
            StartCoroutine(RemoveCharacter());
        }

        public void RemoveTargets()
        {
            _currentEnemy = null;
            _isInCover = false;
        }

        private IEnumerator RemoveCharacter()
        {
            yield return new WaitForSeconds(10);
            Destroy(gameObject);
        }

        public CharacterAttack GetCharacterAttack() => _characterAttack;
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