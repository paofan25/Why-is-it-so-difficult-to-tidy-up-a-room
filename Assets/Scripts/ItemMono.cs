using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ItemMono : MonoBehaviour
{
    public ItemDataPoint itemData;
    public Vector3 targetPos;    public bool isDone = true;
    private bool needCheck;
    private void Update()
    {
        if (needCheck)
        {
            if (Vector3.Distance(transform.position, targetPos) < 1f)
            {
                isDone = true;
                needCheck = false;
            }
            else
            {
                isDone = false;
                if (itemData.IsAlive && itemData.GetWaitRound() <= 0)
                    Debug.Log(name + " " + Vector3.Distance(transform.position, targetPos) + " " + targetPos);
            }
            if (!itemData.IsAlive || itemData.GetWaitRound() > 0)
            {
                isDone = true;
                needCheck = false;
            }
        }
    }
    public void Move(int playerX, int playerY)
    {
        if ((Math.Abs(itemData.spawnX - playerX) == 1 && Math.Abs(itemData.spawnY - playerY) == 0) ||
            (Math.Abs(itemData.spawnY - playerY) == 1 && Math.Abs(itemData.spawnX - playerX) == 0) ||
            (itemData.spawnX == playerX && itemData.spawnY == playerY))
        {
            return;
        }
        isDone = false;
        if(itemData.IsAlive && itemData.GetWaitRound() <= 0)
        {
            targetPos = new Vector3((itemData.spawnY - 2) * 2, 0.4f, (2 - itemData.spawnX) * 2);
            Ray ray = new Ray(targetPos + Vector3.up * 5, Vector3.down);
            if(NavMesh.Raycast(targetPos + Vector3.up * 5, targetPos - Vector3.up * 5,out var navMeshHit, NavMesh.AllAreas))
            {
                targetPos = navMeshHit.position;
            }
            GetComponent<NavMeshAgent>().SetDestination(targetPos);
            Debug.Log($"name:{name} target:{targetPos} pos:{itemData.spawnX} {itemData.spawnY}");
        }
        needCheck = true;
    }
}
