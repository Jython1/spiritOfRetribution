using UnityEngine;
using System.ComponentModel;
using UnityEngine.AI;
using CharacterStateInterface;
using CombatStateScript;
using CharacterAttackScript;
using CoverStateScript;
using ReturnStateScript;
using PatrolStateScript;
using System.Collections.Generic;
using PerceptionScript;
using CoverScript;
using CharacterAnimatorScript;
using CharStats;

namespace AIController
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterAIController : MonoBehaviour
    {
        private ICharacterState _currentState;
        private NavMeshAgent _agent;
        private CharacterAttack _characterAttack;
        private PerceptionSystem _perception;
        private CharacterAnimator _animController;
        private bool _isCrouching;
        private bool _isInCover = false;

        [Header("AI Settings")]
        private GameObject _currentEnemy;
        private GameObject _coverObject;
        private Transform _returnPoint;
        private const float CoverLeaveDistance = 15f;
        private const float CoverReachDistance = 0.5f;
        [SerializeField] private Transform[] _patrolPoints;
        [SerializeField] private float _detectionRadius;
        [SerializeField] private float _sightAngle;
        [SerializeField] private LayerMask _characterMask;
        [SerializeField] private LayerMask _coverMask;
        [SerializeField] private LayerMask _obstructionMask;

        public bool canTakeCover;
        public enum CharacterStatus
        {
            [InspectorName("Всегда атакует")]
            AlwaysAttack,

            [InspectorName("Атакует только во время боя")]
            AttackWhenBattle,

            [InspectorName("Игнорирует бой")]
            IgnoreBattle
        }

        public CharacterStatus characterStatus;


        private void Awake()
        {
            
            _agent = GetComponent<NavMeshAgent>();
            _characterAttack = GetComponent<CharacterAttack>();
            _perception = new PerceptionSystem(gameObject.transform, _detectionRadius, _sightAngle,
            _characterMask, _coverMask, _obstructionMask,
                            gameObject.GetComponent<CharacterStats>().GetCharacterSide());
            _animController = GetComponent<CharacterAnimator>();
        }

        void Start()
        {
            if (_patrolPoints.Length == 0)
                return;

            SetPatrolState();

        }

        private void Update()
        {
            StateSelector();
            _currentState?.Execute();
        }

        

    private void StateSelector()
    {
        if (characterStatus == CharacterStatus.IgnoreBattle)
            return;

        var seenEnemy = _perception.SeeEnemy();
        

        if (seenEnemy && !_currentEnemy)
        {
            _currentEnemy = seenEnemy;
        }


        if (!_currentEnemy && !seenEnemy)
        {
            LeaveCover();
            return;
        }

        if (!_currentEnemy)
            return;

        if (!canTakeCover)
        {
            if (seenEnemy)
                SetCombatState(_currentEnemy);
            return;
        }

        float distanceToEnemy = Vector3.Distance(transform.position, _currentEnemy.transform.position);
        var seenCover = _perception.SeeCover();

        if (!_coverObject)
        {
            // Priority: Take cover if available, otherwise engage in combat
            if (seenCover)
            {
                SetCoverState(seenCover, _currentEnemy);
                _coverObject = seenCover;
            }
            else if (seenEnemy)
            {
                SetCombatState(_currentEnemy);
            }
        }
        else
        {
            // Leave cover if enemy is too far away
            if (distanceToEnemy >= CoverLeaveDistance)
            {
                LeaveCover();
                
                if (seenEnemy)
                    SetCombatState(_currentEnemy);
            }
        }
    }

    private void LeaveCover()
    {
        if (_coverObject)
        {
            _coverObject.GetComponent<Cover>()?.LeaveCover();
            _coverObject = null;
            _isInCover = false;
        }
    }
        public void SetCombatState(GameObject enemy)
        {
            _currentEnemy = enemy;
            _characterAttack.Initialize(enemy);
            _currentState = new CombatState(this, enemy);
            _perception.OnNoiseHeared -= MoveToNoisePosition;
        }

        public void SetCoverState(GameObject cover, GameObject enemy)
        {
            _coverObject = cover;
            _currentEnemy = enemy;
            _isInCover = true;
            _characterAttack.Initialize(enemy);
            _currentState = new CoverState(this, cover, enemy);
        }

        public void SetReturnState()
        {
            _currentState = new ReturningState(this, _returnPoint);
            _perception.OnNoiseHeared += MoveToNoisePosition;
        }

        public void SetPatrolState()
        {
            _currentState = new PatrolState(this, _patrolPoints);
            _currentEnemy = null;
            _isInCover = false;
            _perception.OnNoiseHeared += MoveToNoisePosition;
        }

        public void RemoveTargets()
        {
            _currentEnemy = null;
            _coverObject = null;
            _isInCover = false;
        }

        public void Crouch()
        {
            _animController.Crouch();
            _isCrouching = true;
        }

        public void UnChrouch()
        {
            _animController.UnCrouch();
            _isCrouching = false;
        }

        private void MoveToNoisePosition()
        {

        }

        public NavMeshAgent GetAgent() => _agent;
        public PerceptionSystem GetPerception() => _perception;
        public CharacterAttack GetCharacterAttack() => _characterAttack;
        public GameObject GetCurrentEnemy() => _currentEnemy;
        public bool IsInCover() => _isInCover;
        public bool IsCrouching() => _isCrouching;
    }
}
