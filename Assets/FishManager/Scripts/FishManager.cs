﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace SGJ
{
    public class FishManager : MonoBehaviour
    {
        [SerializeField] private GameObject IslandRoot;
        [SerializeField] private GameObject WorldRoot;

        private Bounds IslandBounds = new Bounds(Vector3.zero, Vector3.zero);
        private Bounds WorldBounds = new Bounds(Vector3.zero, Vector3.zero);

        [SerializeField] private float MinSpawnDelay = 0.5f;
        [SerializeField] private float MaxSpawnDelay = 1.0f;
        [SerializeField] private GameObject[] FishPrefabs;

        [SerializeField] private GameObject[] OrcasPrefabs;
        
        [SerializeField] private float MinOrcasSpawnDelay = 10.5f;
        [SerializeField] private float MaxOrcasSpawnDelay = 15.0f;

        void OnEnable()
        {
            WorldBounds = WorldRoot.CalculateBounds();
            IslandBounds = IslandRoot.CalculateBounds();
            StartCoroutine(SpawnFish());
            StartCoroutine(SpawnOrca());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private Vector3 GetRandomDestination()
        {
            float minX = IslandRoot.transform.position.x + IslandBounds.extents.x;
            float minZ = IslandRoot.transform.position.z + IslandBounds.extents.z;

            Vector3 newVec = new Vector3(Random.Range (minX, -minX),
                IslandRoot.transform.position.y + 1,
                Random.Range (minZ, -minZ));
            return newVec;
        }

        private Vector3 GetRandomSource()
        {
            float minX = WorldRoot.transform.position.x - WorldBounds.extents.x;
            float maxX = IslandRoot.transform.position.x - IslandBounds.extents.x;
            float x = Random.Range(minX, maxX) * (Random.Range(0f, 1f) > 0.5 ? 1f : -1f);
            
            float minZ = WorldRoot.transform.position.z - WorldBounds.extents.z;
            float maxZ = IslandRoot.transform.position.z - IslandBounds.extents.z;
            float z = Random.Range(minZ, maxZ) * (Random.Range(0f, 1f) > 0.5 ? 1 : -1);

            Vector3 newVec = new Vector3(x, WorldRoot.transform.position.y, z);
            return newVec;
        }

        private IEnumerator SpawnFish()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(MinSpawnDelay, MaxSpawnDelay));
                if (FishPrefabs.Length > 0)
                {
                    var fish = Instantiate(FishPrefabs[Random.Range(0, FishPrefabs.Length)], GetRandomSource(), Quaternion.identity);
                    var rigid = fish.GetComponent<Rigidbody>();
                    
                    ShootAt(rigid, GetRandomDestination());                    
                }
            }
        }

        private IEnumerator SpawnOrca()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(MinSpawnDelay, MaxSpawnDelay));
                if (OrcasPrefabs.Length > 0)
                {
                    var orca = Instantiate(OrcasPrefabs[Random.Range(0, OrcasPrefabs.Length)], GetRandomSource(), Quaternion.identity);
                    var rigid = orca.GetComponent<Rigidbody>();
                    
                    ShootAt(rigid, GetRandomDestination());
                }
            }
        }

        private void ShootAt(Rigidbody projectile, Vector3 target)
        {
            WorldBounds = WorldRoot.CalculateBounds();
            IslandBounds = IslandRoot.CalculateBounds();
            
            projectile.transform.LookAt(target);
                    
            float angle = Random.Range(30f,70f) * Mathf.Deg2Rad;
 
            Vector3 planarTarget = new Vector3(target.x, 0, target.z);
            Vector3 planarPostion = new Vector3(projectile.transform.position.x, 0, projectile.transform.position.z);
 
            float distance = Vector3.Distance(planarTarget, planarPostion);
            float yOffset = projectile.transform.position.y - target.y;
            float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * Physics.gravity.magnitude * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
            Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));
            float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (target.x > projectile.transform.position.x ? 1 : -1);
            Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
                    
            projectile.AddForce(finalVelocity * projectile.mass, ForceMode.Impulse);
        }


    }
}


