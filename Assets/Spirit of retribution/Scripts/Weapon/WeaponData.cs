using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{

    //[Header("Info")]
    //public string name;

    [Header("Shooting")]
    public int maxAmmo;
    public float delayBetweenShots;
    public bool isInfinityAmmo = false;

    public float distance;
    
}
