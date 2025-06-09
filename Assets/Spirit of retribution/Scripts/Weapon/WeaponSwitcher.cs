using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using WeaponScript;
using System;

namespace WeaponSwitcherScript
{ 
    public class WeaponSwitcher : MonoBehaviour
    {
        [Header("For NPC")]
        public GameObject weaponOnStart;

        [Header("Weapon Holders")]
        public GameObject rightWeaponHolder;
        public GameObject backHolder;

        private GameObject targetHolder;

        [Header("Animation")]
        public Animator animator;


        private GameObject _currentWeapon;
        private GameObject _weaponInBack;

        private const float animationDelay = 0.5f;

        private bool _isWaiting;

        private string replaceWeaponTriggerName;
        public event Action onWeaponChanged;

        private void Start()
        {
            targetHolder = rightWeaponHolder;

            replaceWeaponTriggerName = "TakeWeapon";

            if(!weaponOnStart)
            return;

            AddWeaponOnStart();

        }

        private void AddWeaponOnStart()
        {
            _currentWeapon = Instantiate(weaponOnStart);
            _currentWeapon.transform.SetParent(targetHolder.transform);
            _currentWeapon.transform.localPosition = Vector3.zero;
            _currentWeapon.transform.localRotation = Quaternion.identity;

        }

        public void SetWeaponType(GameObject weapon)
        {
            var currentWeaponType = weapon.GetComponent<Weapon>().weaponType;
            animator.SetInteger("WeaponType", (int)currentWeaponType);
        }

        public void TakeNewWeaponInHand(GameObject weapon)
        {
            animator.SetTrigger("TakeWeapon");
            _currentWeapon = weapon;
            weapon.SetActive(false);
            StartCoroutine(WaitAndEquipPickedUp());
            
        }

        private IEnumerator WaitAndEquipPickedUp()
        {
            yield return new WaitForSeconds(animationDelay);

            _currentWeapon.SetActive(true);

            _currentWeapon.transform.SetParent(targetHolder.transform);
            _currentWeapon.transform.localPosition = Vector3.zero;
            _currentWeapon.transform.localRotation = Quaternion.identity;
            SetWeaponType(_currentWeapon);
            onWeaponChanged?.Invoke();

        }

        public void AddWeaponBack(GameObject weapon)
        {
            weapon.SetActive(false);
            _weaponInBack = weapon;

            _weaponInBack.transform.SetParent(backHolder.transform);
            _weaponInBack.transform.localPosition = Vector3.zero;
            _weaponInBack.transform.localRotation = Quaternion.identity;


        }

        public void PutWeaponBack()
        {
            _isWaiting = true;
            animator.SetTrigger(replaceWeaponTriggerName);
            StartCoroutine(WaitAndPutBack());

        }

        private IEnumerator WaitAndPutBack()
        {
            yield return new WaitForSeconds(animationDelay);
            
            _currentWeapon.SetActive(false);

            _currentWeapon.transform.SetParent(backHolder.transform);
            _currentWeapon.transform.localPosition = Vector3.zero;
            _currentWeapon.transform.localRotation = Quaternion.identity;
            _weaponInBack = _currentWeapon;
            _currentWeapon = null;
            _isWaiting = false;
            onWeaponChanged?.Invoke();
        }



        public void TakeWeaponFromBack()
        {
            _isWaiting = true;
            animator.SetTrigger(replaceWeaponTriggerName);
            StartCoroutine(WaitAndTakeFromBack());
            onWeaponChanged?.Invoke();
        }

        private IEnumerator WaitAndTakeFromBack()
        {
            yield return new WaitForSeconds(animationDelay);
            
            _weaponInBack.SetActive(true);
            
            _weaponInBack.transform.SetParent(targetHolder.transform);
            _weaponInBack.transform.localPosition = Vector3.zero;
            _weaponInBack.transform.localRotation = Quaternion.identity;
            _currentWeapon = _weaponInBack;
            _weaponInBack = null;
            _isWaiting = false;
            SetWeaponType(_currentWeapon);
            onWeaponChanged?.Invoke();
        }


        public void ToggleWeapons()
        {
            
            _isWaiting = true;
            animator.SetTrigger(replaceWeaponTriggerName);
            StartCoroutine(WaitAndToggle());

        }

        private IEnumerator WaitAndToggle()
        {
            yield return new WaitForSeconds(animationDelay);
            
            _currentWeapon.SetActive(false);
            _weaponInBack.SetActive(false);
            
            _weaponInBack.transform.SetParent(targetHolder.transform);
            _weaponInBack.transform.localPosition = Vector3.zero;
            _weaponInBack.transform.localRotation = Quaternion.identity;

            _currentWeapon.transform.SetParent(backHolder.transform);
            _currentWeapon.transform.localPosition = Vector3.zero;
            _currentWeapon.transform.localRotation = Quaternion.identity;

            GameObject tempHolder = _currentWeapon;
            _currentWeapon = _weaponInBack;
            _weaponInBack = tempHolder;

            _currentWeapon.SetActive(true);
            _weaponInBack.SetActive(false);

            _isWaiting = false;
            SetWeaponType(_currentWeapon);
            onWeaponChanged?.Invoke();
        }


        public GameObject GetCurrentWeapon() => _currentWeapon;
        public GameObject GetWeaponInBack() => _weaponInBack;
        public bool IsWaiting() => _isWaiting;


    }


}