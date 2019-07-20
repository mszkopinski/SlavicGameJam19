using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace SGJ
{
    public class SnowballCannon : MonoBehaviour
    {
        [SerializeField]
        private int ammo = 0;

        [SerializeField] private GameObject ProjectilePrefab;
        [SerializeField] private float ShootForce = 2000f;

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent(typeof(IThrowable)) is IThrowable throwable)
            {
                throwable.OnPicked(out var amount);
                ammo += amount;
            }
        }

        public void OnFirePress()
        {
            if (ammo <= 0) return;
            
            GameObject snowball = Instantiate(ProjectilePrefab, transform.position + new Vector3(0f,0.5f, 0f) + transform.forward, Quaternion.identity);
            snowball.GetComponent<Rigidbody>().AddForce(transform.forward * ShootForce);
            ammo--;
        }
    }
}
