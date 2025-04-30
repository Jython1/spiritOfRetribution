using UnityEngine;

namespace NoiseCauser
{
    public class Noise : MonoBehaviour
    {

        [SerializeField] private float _noiseRadius = 5f;
        [SerializeField] private LayerMask _listenerMask;
/*
        public void MakeNoise()
        {
            Collider[] listeners = Physics.OverlapSphere(transform.position, _noiseRadius, _listenerMask);
            

            foreach (Collider listener in listeners)
            {
                if (listener.TryGetComponent<CharacterPerception>(out var perception))
                {
                    perception.ReceiveNoise(transform.position);
                }
            }


        }*/
    }

}
