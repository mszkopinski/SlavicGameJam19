using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class CameraTargetGroup : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup CinemachineTargetGroup;
    private List<GameObject> players = new List<GameObject>();

    public void OnPlayerJoined(object value)
    {
        GameObject player = (GameObject) value;
        if (!players.Contains(player))
        {
            players.Add(player);
            if (players.Count == 1)
            {
                AddTarget(player);
            }
            else
            {
                UpdateTargets();
            }
        }
    }

    private void AddTarget(GameObject gameObject)
    {
        var newTargets = CinemachineTargetGroup.m_Targets.ToList();
        newTargets.Add(new CinemachineTargetGroup.Target()
        {
            radius = 3f,
            target = gameObject.transform,
            weight = 1
        });

        CinemachineTargetGroup.m_Targets = newTargets.ToArray();
    }

    private void UpdateTargets()
    {
        CinemachineTargetGroup.Target[] targets = new CinemachineTargetGroup.Target[players.Count];

        var indexer = 0;
        for (indexer = 0; indexer < players.Count; indexer++)
        {
            targets[indexer] = new CinemachineTargetGroup.Target()
                {radius = 3f, target = players[indexer].transform, weight = 1};
        }

        CinemachineTargetGroup.m_Targets = targets;
    }
}