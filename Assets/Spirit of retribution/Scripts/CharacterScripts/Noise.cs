using UnityEngine;
using PerceptionScript;
using AIController;

namespace NoiseCauser
{
    public class Noise : MonoBehaviour
    {


        public void MakeNoise(float noiseRadius)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, noiseRadius);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<CharacterAIController>(out CharacterAIController aiController))
                {
                    aiController.GetPerception().ReceiveNoise(transform.position);
                    Debug.Log(transform.position);
                }
            }

        }

        
    }

}
