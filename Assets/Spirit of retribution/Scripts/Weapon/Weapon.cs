using UnityEngine;


public abstract class Weapon : MonoBehaviour
{
    public float damage;
    public float fireRate;
    private Rigidbody _rb;
    private bool _isEquiped;

    public abstract void Shoot();

    private void Start() 
    {
        if(!gameObject.GetComponent<Rigidbody>())
        return;

        _rb = gameObject.GetComponent<Rigidbody>();

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if(!col.gameObject.GetComponent<WeaponController>())
            return;

            var weaponController = col.gameObject.GetComponent<WeaponController>();
            weaponController.EquipWeapon(gameObject);
            
        }
    }

    public void SetEquiped()
    {
        foreach(Collider c in GetComponents<Collider>()) 
            c.enabled = false;

        _rb.isKinematic = true;
       _isEquiped = true;
    }

    public void SetUnequiped()
    {
        foreach(Collider c in GetComponents<Collider>())
            c.enabled = true;

        _rb.isKinematic = false;
       _isEquiped = false;
    }

}
