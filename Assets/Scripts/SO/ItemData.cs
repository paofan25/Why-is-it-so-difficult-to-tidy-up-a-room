using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemData", menuName = "创建游戏数据/物体数据")]
public class ItemData : ScriptableObject
{
    [Header("开局物体数据，spawnXY为出生位置，xy为目标位置")]
    public List<ItemDataPoint> itemDatas;
}
[Serializable]
public class ItemDataPoint
{
    public int x;
    public int y;
    public int spawnX;
    public int spawnY;
    public DataType type;
    private bool isAlive = false;
    public bool IsAlive => isAlive;
    private int waitRound = 0;
    public void SetAlive()
    {
        isAlive = true;
    }
    public int GetWaitRound()
    {
        return waitRound;
    }
    public void SetWaitRound(int round)
    {
        waitRound = round;
    }
    public void AIMove(int playerX,int playerY,int sideLength = 3)
    {
        //行差
        int dx = spawnX - x;
        //列差
        int dy = spawnY - y;
        int dxAbs = Math.Abs(dx);
        int dyAbs = Math.Abs(dy);
        int moveX = Math.Sign(dx);
        int moveY = Math.Sign(dy);
        if ((Math.Abs(spawnX - playerX) == 1 && Math.Abs(spawnY - playerY) == 0) ||
            (Math.Abs(spawnY - playerY) == 1 && Math.Abs(spawnX - playerX) == 0))
        {
            return;
        }
        if (moveX == 0) moveX = 1;
        int newx = x + moveX;
        if (newx < 1 || newx > 3) newx = x - 1;
        if (newx < 1 || newx > 3)
        {
            if (moveY == 0) moveY = 1;
            int newy = y + moveY;
            if (newy < 1 || newy > 3) newy = y - 1;
            if (newy < 1 || newy > 3)
            {
                return;
            }
            spawnY = newy;
            return;
        }
        spawnX = newx;
    }
}