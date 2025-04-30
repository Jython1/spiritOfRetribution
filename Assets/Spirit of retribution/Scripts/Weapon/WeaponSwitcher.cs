using UnityEngine;
using System.Collections.Generic;

public class WeaponSwitcher : MonoBehaviour
{
    private GameObject _currentWeapon;
    private GameObject _backHolder;
    private GameObject _beltHolder;
    private List<GameObject> _modulesHolder;



    public void switchWeapons()
    {
        GameObject tempHolder;
        tempHolder = _currentWeapon;
        _currentWeapon = _backHolder;
        _backHolder = tempHolder;


    }

    public void SetCurrentWeapon(GameObject weapon)
    {
        
        _currentWeapon = weapon;
        _currentWeapon.transform.SetParent(gameObject.transform);
        _currentWeapon.transform.localPosition = Vector3.zero;
        _currentWeapon.transform.localRotation = Quaternion.identity;

    }


}
