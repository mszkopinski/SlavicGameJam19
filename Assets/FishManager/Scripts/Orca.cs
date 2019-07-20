using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
    public class Orca : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            GameObject otherGameObject = other.collider.gameObject;
            if (otherGameObject.GetComponent<IceCrack>() && otherGameObject.GetComponent<Rigidbody>() == null)
            {
                other.collider.gameObject.AddComponent<Rigidbody>();
            }
        }
    }

}

