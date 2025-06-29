using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameRoundMgr : MonoBehaviour// : SingletonAutoMono<GameRoundMgr>
{
    public RoundState currentState;
    public PLayerMono player;
    private void Awake()
    {
        currentState = RoundState.Wait;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentState == RoundState.Wait)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.gameObject.GetComponent<ItemMono>())
                {
                    Debug.Log("ÓÐitemMONO");
                }
                else
                {
                    Debug.Log("Ã»ÓÐitemMONO");
                }
            }
        }
    }
    public Vector3 GetDestination(Vector3 pos)
    {
        if (NavMesh.Raycast(pos + Vector3.up * 5, Vector3.down * 10, out var navHit, NavMesh.AllAreas))
        {
            return navHit.position;
        }
        return Vector3.zero;
    }
    //IEnumerator GoToGrid()
    //{

    //}
}
public enum RoundState
{
    PlayerRound,
    PlayerRoundWait,
    ItemRound,
    Wait,
}
