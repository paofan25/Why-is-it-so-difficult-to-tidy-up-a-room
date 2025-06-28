using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMapData",menuName = "创建游戏数据/地图数据")]
public class MapData : ScriptableObject
{
    [Header("棋盘边长")]
    public int sideLength;
    [Header("最大回合数")]
    public int maxRound;
    [Header("每个棋盘数据(确保数量是边长的平方)")]
    public List<MapDataPoint> mapDatas;
}
[Serializable]
public class MapDataPoint
{
    public int x;
    public int y;
    public DataType type;
}
public enum DataType
{
    Desk,
    Bed,
}