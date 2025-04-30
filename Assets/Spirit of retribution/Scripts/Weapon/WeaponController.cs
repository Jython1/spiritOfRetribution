using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponSwitcher weaponSwitcher;


    public void EquipWeapon(GameObject weapon)
    {

        weapon.GetComponent<Weapon>().SetEquiped();
        weaponSwitcher.SetCurrentWeapon(weapon);

    }

}
