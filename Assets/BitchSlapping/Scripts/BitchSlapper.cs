using System;
using UnityEngine;

namespace SGJ
{
    public class BitchSlapper : MonoBehaviour
    {
        private Rigidbody target;
        [SerializeField] private float slapForce = 5000f;
        
        private void OnTriggerEnter(Collider other)
        {
            var penguinController = other.gameObject.GetComponent<PenguinController>();
            
            if (penguinController)
            {
                target = penguinController.gameObject.GetComponent<Rigidbody>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var penguinController = other.gameObject.GetComponent<PenguinController>();
            
            if (penguinController)
            {
                target = null;
            }
        }

        public void OnSlapButtonPressed()
        {
            if (target)
            {
                target.AddForce(transform.forward * slapForce);    
            }
        }
    }
}