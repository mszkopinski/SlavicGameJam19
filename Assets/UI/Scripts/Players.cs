using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ.UI
{
    public class Players : MonoBehaviour
    {
        [SerializeField] private GameObject Container;
        [SerializeField] private GameObject PlayerPrefab;

        public void OnPlayerJoined(object value)
        {
            GameObject source = (GameObject) value;
            GameObject player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity, Container.transform);
            player.GetComponent<GUIPlayerInfo>().Source = source;
        }
    }
    

}
