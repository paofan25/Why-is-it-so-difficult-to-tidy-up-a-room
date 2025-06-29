using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameRoundFSM : MonoBehaviour
{
    public FSM fsm;
    public GameRoundBlackboard blackBoard;
    private void Awake()
    {
        fsm = new FSM(blackBoard);
        fsm.AddState(StateType.Wait, new WaitState(fsm));
        fsm.AddState(StateType.PlayerRound, new PlayerRoundState(fsm));
        fsm.AddState(StateType.ItemRound, new ItemRoundState(fsm));
        fsm.SwitchState(StateType.Wait);
    }

    void Update()
    {
        fsm.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(fsm.curState);
            for (int i = 0; i < blackBoard.items.Count; i++)
            {
                Debug.Log($"{blackBoard.items[i].name} {blackBoard.items[i].itemData.x} {blackBoard.items[i].itemData.y} {blackBoard.items[i].itemData.spawnX} {blackBoard.items[i].itemData.spawnY}");
            }
        }
    }
}
public class WaitState : IState
{
    private FSM fsm;
    private GameRoundBlackboard blackboard;
    public WaitState(FSM fsm)
    {
        this.fsm = fsm;
        this.blackboard = fsm.blackboard as GameRoundBlackboard;
    }
    public void OnEnter()
    {
        //�ȴ��غϼ���
        for (int i = 0; i < blackboard.items.Count; i++)
        {
            blackboard.items[i].itemData.SetWaitRound(blackboard.items[i].itemData.GetWaitRound() - 1);
        }
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        if (blackboard.currentItem == null && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.gameObject.GetComponent<ItemMono>())
                {
                    MusicMgr.Instance.PlaySound("Sounds/SFX_In_Click_A");
                    blackboard.currentItem = hit.collider.gameObject.GetComponent<ItemMono>();
                    blackboard.player.GetComponent<NavMeshAgent>().SetDestination(hit.collider.gameObject.transform.position);
                    //fsm.SwitchState(StateType.PlayerRound);
                }
            }
        }
        if(blackboard.currentItem != null && Vector3.Distance(blackboard.currentItem.transform.position, blackboard.player.transform.position) < 0.8f)
        {
            MusicMgr.Instance.PlaySound("Sounds/SFX_In_OBJ_A");
            blackboard.currentItem.gameObject.SetActive(false);
            fsm.SwitchState(StateType.PlayerRound);
        }
    }
}
public class PlayerRoundState : IState
{
    private FSM fsm;
    private GameRoundBlackboard blackboard;
    public PlayerRoundState(FSM fsm)
    {
        this.fsm = fsm;
        this.blackboard = fsm.blackboard as GameRoundBlackboard;
    }
    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        blackboard.player.x = blackboard.goToMapData.mapData.x;
        blackboard.player.y = blackboard.goToMapData.mapData.y;
        //Debug.Log($"goToMapdata {blackboard.goToMapData.mapData.x} {blackboard.goToMapData.mapData.y}");
        blackboard.currentItem.itemData.spawnX = blackboard.goToMapData.mapData.x;
        blackboard.currentItem.itemData.spawnY = blackboard.goToMapData.mapData.y;
        blackboard.currentItem.itemData.SetAlive();
        blackboard.currentItem.itemData.SetWaitRound(2);
        blackboard.currentItem.itemData.CheckIsRight();
        //����
        blackboard.currentItem = null;
        blackboard.goToMapData = null;
        bool isOver = true;
        for (int i = 0; i < blackboard.items.Count; i++)
        {
            if (!blackboard.items[i].itemData.isRight)
            {
                isOver = false;
                break;
            }
        }
        if (isOver)
        {
            //ʤ��
            MusicMgr.Instance.PlaySound("Sounds/SFX_In_Win");
            Debug.Log("ʤ��");
        }
    }

    public void OnUpdate()
    {
        if (blackboard.goToMapData == null && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.gameObject.GetComponent<MapDataMono>() && hit.collider.gameObject.GetComponent<MapDataMono>().mapData.type == blackboard.currentItem.itemData.type)
                {
                    MusicMgr.Instance.PlaySound("Sounds/SFX_In_Click_B");
                    blackboard.goToMapData = hit.collider.gameObject.GetComponent<MapDataMono>();
                    blackboard.player.GetComponent<NavMeshAgent>().SetDestination(blackboard.goToMapData.gotoPos.transform.position);
                    //fsm.SwitchState(StateType.PlayerRound);
                }
            }
        }
        if (blackboard.goToMapData != null && Vector3.Distance(blackboard.goToMapData.gotoPos.transform.position, blackboard.player.transform.position) < 0.5f)
        {
            //�ƶ�����
            blackboard.currentItem.transform.position = blackboard.goToMapData.putPos.transform.position;
            MusicMgr.Instance.PlaySound("Sounds/SFX_In_OBJ_B");
            blackboard.currentItem.gameObject.SetActive(true);
            fsm.SwitchState(StateType.ItemRound);
        }
    }
}
public class ItemRoundState : IState
{
    private FSM fsm;
    private GameRoundBlackboard blackboard;
    public ItemRoundState(FSM fsm)
    {
        this.fsm = fsm;
        this.blackboard = fsm.blackboard as GameRoundBlackboard;
    }
    public void OnEnter()
    {
        for (int i = 0; i < blackboard.items.Count; i++)
        {
            if(blackboard.items[i].itemData.IsAlive && blackboard.items[i].itemData.GetWaitRound()<=0)
            {
                blackboard.items[i].itemData.AIMove(blackboard.player.x, blackboard.player.y);
                blackboard.items[i].Move(blackboard.player.x, blackboard.player.y);
            }
        }
    }

    public void OnExit()
    {
        bool isOver = true;
        for (int i = 0; i < blackboard.items.Count; i++)
        {
            if (!blackboard.items[i].itemData.isRight)
            {
                isOver = false;
                break;
            }
        }
        if (isOver)
        {
            //ʤ��
            Debug.Log("ʤ��");
            MusicMgr.Instance.PlaySound("Sounds/SFX_In_Win");
        }
    }

    public void OnUpdate()
    {
        bool Over = true;
        for (int i = 0; i < blackboard.items.Count; i++)
        {
            if (blackboard.items[i].isDone || !blackboard.items[i].itemData.IsAlive)
            {
                Over = true;
            }
            else
            {
                Debug.Log($"{blackboard.items[i].name} {blackboard.items[i].isDone} {blackboard.items[i].itemData.IsAlive}");
                Over = false;
            }
        }
        if (Over)
        {
            fsm.SwitchState(StateType.Wait);
        }
    }
}
[Serializable]
public class GameRoundBlackboard : BlackBoard
{
    public RoundState currentState;
    public PLayerMono player;
    public ItemMono currentItem;
    public MapDataMono goToMapData;
    public List<ItemMono> items;
}
