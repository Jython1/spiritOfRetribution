using UnityEngine;
using System;

namespace ShootingEvent
{
    public class CharacterShoot : MonoBehaviour
    {

        public static event Action OnShoot;

        public void Shooting()
        {
            OnShoot?.Invoke();
        }
    }


}
