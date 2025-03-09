using UnityEngine;
using System.Collections;
public class CharacterShoot : MonoBehaviour
{

    public delegate void ShootAction();
    public static event ShootAction OnShoot;

    public void Shooting()
    {
        OnShoot();
    }
}
