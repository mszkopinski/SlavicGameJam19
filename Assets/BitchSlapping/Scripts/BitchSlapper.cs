using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SGJ
{
    public class BitchSlapper : MonoBehaviour
    {
        private Rigidbody target;
        [SerializeField] private float slapForce = 5000f;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] SlapSounds;
        
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
                int currentFatLevel = GetComponent<PenguinController>().CurrentFatLevel;
                float targetMass = target.GetComponent<Rigidbody>().mass;
                target.AddForce(transform.forward * slapForce *  targetMass * (currentFatLevel * 0.7f) );
                audioSource.PlayOneShot(SlapSounds[Random.Range(0, SlapSounds.Length)]);
            }
        }
    }
}