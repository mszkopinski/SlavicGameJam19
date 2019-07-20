using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
    public class Snower : MonoBehaviour
    {
        [SerializeField] private GameObject IslandRoot;
        private Bounds IslandBounds;
        
        [SerializeField] private float MinSpawnDelay = 0.5f;
        [SerializeField] private float MaxSpawnDelay = 1.0f;
        [SerializeField] private GameObject[] SnowPilePrefabs;

        void Awake()
        {
            IslandBounds = IslandRoot.CalculateBounds();
        }
        
        void OnEnable()
        {
            IslandBounds = IslandRoot.CalculateBounds();
            StartCoroutine(SpawnSnowPile());
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

        IEnumerator SpawnSnowPile()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(MinSpawnDelay, MaxSpawnDelay));
                
                var snowPile = Instantiate(SnowPilePrefabs[Random.Range(0, SnowPilePrefabs.Length)], GetRandomDestination(), Quaternion.identity);
            }
        }
        
        private void OnDisable()
        {
            StopAllCoroutines();
        }



    }
    

}
