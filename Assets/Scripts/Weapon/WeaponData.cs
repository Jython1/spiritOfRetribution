using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{

    [Header("Info")]
    string name;

    [Header("Shooting")]
    public int maxAmmo;
    public int currentAmmo;
    public float delayBetweenShots;
    public bool isInfinityAmmo = false;
    
}
