using System;
using UnityEngine;
using CoverScript;
using CharStats;
using NoiseCauser;

namespace PerceptionScript
{
    public class PerceptionSystem
    {
        private Transform _characterTransform;
        private LayerMask _characterMask;
        private LayerMask _coverMask;
        private LayerMask _obstructionMask;
        private float _detectionRadius;
        private float _sightAngle;
        private Vector3 _noisePosition;
        private CharacterSide _currentSide;
        public event Action OnNoiseHeared;


        public PerceptionSystem(Transform characterTransform, float detectionRadius, float sightAngle,
        LayerMask characterMask, LayerMask coverMask, LayerMask obstructionMask, CharacterSide currentSide)
        {
            _characterTransform = characterTransform;
            _detectionRadius = detectionRadius;
            _sightAngle = sightAngle;
            _characterMask = characterMask;
            _coverMask = coverMask;
            _obstructionMask = obstructionMask;
            _currentSide = currentSide;

        }

        public GameObject SeeEnemy()
        {

            Collider[] rangeChecks = Physics.OverlapSphere(_characterTransform.position, _detectionRadius, _characterMask);

            if (rangeChecks.Length != 0)
            {
                GameObject target = rangeChecks[0].gameObject;
                Vector3 directionToTarget = (target.transform.position - _characterTransform.position).normalized;

                if (Vector3.Angle(_characterTransform.forward, directionToTarget) < _sightAngle / 2 && IsEnemy(target))
                {
                    float distanceToTarget = Vector3.Distance(_characterTransform.position, target.transform.position);

                    if (!Physics.Raycast(_characterTransform.position, directionToTarget, distanceToTarget, _obstructionMask))
                        return target;

                    else
                        return null;
                }

            }
            return null;
        }

        private bool IsEnemy(GameObject targetObject)
        {
            if (targetObject.TryGetComponent<CharacterStats>(out var targetStats))
            {
                CharacterSide targetSide = targetStats.GetCharacterSide();

                return FactionRelations.IsEnemy(_currentSide, targetSide);
            }

            return false;

        }

        public GameObject SeeCover()
        {
            Collider[] hits = Physics.OverlapSphere(_characterTransform.position, _detectionRadius / 2f, _coverMask);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<Cover>(out Cover cover))
                {

                    if (!cover.IsTaken())
                    {
                        return hit.gameObject;
                    }
                }
            }
            return null;

        }

        public void ReceiveNoise(Vector3 noisePosition)
        {
            _noisePosition = noisePosition;
            OnNoiseHeared?.Invoke();
        }

        public void ClearNoisePosition()
        {
            _noisePosition = Vector3.zero;
        }

        public Vector3 GetNoisePosition => _noisePosition;

    }
    
}
