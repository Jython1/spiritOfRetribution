using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoverScript
{

    public class Cover : MonoBehaviour
    {
        private bool _isTaken;
        private bool _isWaiting;

        public bool IsTaken()
        {
            return _isTaken;
        }

        public void TakeCover()
        {
            _isTaken = true;
        }

        public void LeaveCover()
        {
            if(_isTaken)
            {
                _isWaiting = true;
                StartCoroutine(Wait(5f));

            }
        }

        IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _isTaken = false;
            _isWaiting = false;
        }

    }


}
