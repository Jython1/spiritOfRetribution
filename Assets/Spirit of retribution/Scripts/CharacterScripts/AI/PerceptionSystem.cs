using UnityEngine;
using CoverScript;

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
        

        public PerceptionSystem(Transform characterTransform, float detectionRadius, float sightAngle, 
        LayerMask characterMask, LayerMask coverMask, LayerMask obstructionMask)
        {
            _characterTransform = characterTransform;
            _detectionRadius = detectionRadius;
            _sightAngle = sightAngle;
            _characterMask = characterMask;
            _coverMask = coverMask;
            _obstructionMask = obstructionMask;
            
        }

        public GameObject DetectEnemy()
        {

            Collider[] rangeChecks = Physics.OverlapSphere(_characterTransform.position, _detectionRadius, _characterMask);

            if (rangeChecks.Length != 0)
            {
                GameObject target = rangeChecks[0].gameObject;
                Vector3 directionToTarget = (target.transform.position - _characterTransform.position).normalized;

                if (Vector3.Angle(_characterTransform.forward, directionToTarget) < _sightAngle / 2 /*&& IsEnemy(target)*/)
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

        public GameObject DetectCover()
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
        
    }
}
