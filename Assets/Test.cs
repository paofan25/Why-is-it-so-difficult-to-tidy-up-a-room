using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.SymbolStore;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public MapData mapData;
    public ItemData itemData;
    public int currentRound;
    public List<Text> texts;
    public Text roundText;
    private int playerX;
    private int playerY;
    private bool isDone = false;

    [FormerlySerializedAs("maxCount")] public int maxcount;
    private void Awake(){
        maxcount = itemData.maxCount;
        //渲染初始化
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].text = "";
        }
#if UNITY_EDITOR
        for (int i = 0; i < itemData.itemDatas.Count; i++)
        {
            ItemDataPoint data = itemData.itemDatas[i];
            int sideLength = mapData.sideLength;
            if (data.x < 0 || data.x > sideLength || data.y < 0 || data.y > sideLength ||
                data.spawnX < 0 || data.spawnX > sideLength || data.spawnY < 0 || data.spawnY > sideLength)
            {
                Debug.LogError($"=====>第{i + 1}个棋子位置数据错误");
            }
            int currentPos = (data.spawnX - 1) * mapData.sideLength + data.spawnY;
            int id = (data.x - 1) * mapData.sideLength + data.y;
            texts[currentPos - 1].text += $"{id},";
            //MapDataPoint mapDataPoint = null;
            //for (int j = 0; j < mapData.mapDatas.Count; j++)
            //{
            //    if (mapData.mapDatas[i].x == data.x && mapData.mapDatas[i].y == data.y)
            //    {
            //        mapDataPoint = mapData.mapDatas[i];
            //        break;
            //    }
            //}
            //if(mapDataPoint != null && mapDataPoint.type == data.type)
            //{

            //}
            //else
            //{
            //    Debug.LogError("地图数据错误,可能类型不对齐！");
            //}
        }
#endif
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GoTo(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GoTo(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GoTo(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GoTo(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GoTo(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GoTo(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GoTo(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            GoTo(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GoTo(9);
        }
    }
    public void GoTo(int id)
    {
        if (isDone) return;
        ItemDataPoint curPoint = null;
        for (int i = 0; i < itemData.itemDatas.Count; i++)
        {
            if ((itemData.itemDatas[i].x - 1) * mapData.sideLength + itemData.itemDatas[i].y == id)
            {
                curPoint = itemData.itemDatas[i];
                break;
            }
        }
        if(curPoint == null)
        {
            Debug.LogError("=====>没有选择的该棋子!");
            return;
        }
        currentRound++;
        MoveTo(curPoint);
        Draw();
        if (CheckIsOver())
        {
            Debug.Log("游戏结束");
            return;
        }
        for (int i = 0; i < itemData.itemDatas.Count; i++)
        {
            itemData.itemDatas[i].SetWaitRound(itemData.itemDatas[i].GetWaitRound() - 1);
            if(itemData.itemDatas[i].GetWaitRound() <= 0 && itemData.itemDatas[i].IsAlive)
            {
                itemData.itemDatas[i].AIMove(playerX, playerY, mapData.sideLength) ;
            }
        }
        Draw();
        if (CheckIsOver())
        {
            Debug.Log("游戏结束");
            return;
        }
    }
    public void MoveTo(ItemDataPoint point)
    {
        point.spawnX = point.x;
        point.spawnY = point.y;
        playerX = point.x;
        playerY = point.y;
        point.SetAlive();
        point.SetWaitRound(3);
    }
    public bool CheckIsOver()
    {
        for (int i = 0; i < itemData.itemDatas.Count; i++)
        {
            if (itemData.itemDatas[i].x != itemData.itemDatas[i].spawnX ||
                itemData.itemDatas[i].y != itemData.itemDatas[i].spawnY)
            {
                return false;
            }
        }
        isDone = true;
        return true;
    }
    public void Draw()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].text = "";
        }
        for (int i = 0; i < itemData.itemDatas.Count; i++)
        {
            ItemDataPoint data = itemData.itemDatas[i];
            int sideLength = mapData.sideLength;
            int currentPos = (data.spawnX - 1) * mapData.sideLength + data.spawnY;
            int id = (data.x - 1) * mapData.sideLength + data.y;
            texts[currentPos - 1].text += $"{id},";
        }
        texts[(playerX - 1) * mapData.sideLength + playerY - 1].text += $"人,";
        roundText.text = currentRound.ToString();
    }
}
//[Serializable]
//public class Point
//{
//    public int id;
//    public int currentId;
//    private int stayRound = 0;
//    private bool isAlive = false;
//    public int StayRound
//    {
//        get { return stayRound; }
//        set { stayRound = value; }
//    }
//    public bool IsAlive
//    {
//        get { return isAlive; }
//        set { isAlive = value; }
//    }
//    public void AIMove(/*int playerId,*/int sideLength = 3)
//    {
//        int playerId = id;
//        //行列
//        int px = (playerId - 1) / sideLength + 1;
//        int py = (playerId - 1) % sideLength + 1;
//        int x = (currentId - 1) / sideLength + 1;
//        int y = (currentId - 1) % sideLength + 1;
//        //行差
//        int dx = x - px;
//        //列差
//        int dy = y - py;
//        int dxAbs = Math.Abs(dx);
//        int dyAbs = Math.Abs(dy);
//        int moveX = Math.Sign(dx);
//        int moveY = Math.Sign(dy);
//        if ((dxAbs == 1 && dyAbs == 0) || 
//            (dyAbs == 1 && dxAbs == 0))
//        {
//            Debug.Log($"{id} 禁锢");
//            return;
//        }
//        int tempid = currentId;
//        if (moveX == 0) moveX = 1;
//        int newx = x + moveX;
//        if (newx < 1 || newx > 3) newx = x - 1;
//        if (newx <1 || newx > 3)
//        {
//            if (moveY == 0) moveY = 1;
//            int newy = y + moveY;
//            if (newy < 1 || newy > 3) newy = y - 1;
//            if (newy <1 || newy > 3)
//            {
//                Debug.Log($"{id}不移动到");
//                return;
//            }
//            currentId = (x - 1) * 3 + newy;
//            Debug.Log($"{id}从{tempid}移动到{currentId}");
//            return;
//        }
//        currentId = (newx - 1) * 3 + y;
//        Debug.Log($"{id}从{tempid}移动到{currentId}");
//    }
//    public override string ToString()
//    {
//        return $"id:{id} currentid:{currentId} isAlive:{isAlive} stayRound{stayRound}";
//    }
//}