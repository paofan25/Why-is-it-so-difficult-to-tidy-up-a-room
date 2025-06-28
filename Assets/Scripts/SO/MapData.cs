using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMapData",menuName = "������Ϸ����/��ͼ����")]
public class MapData : ScriptableObject
{
    [Header("���̱߳�")]
    public int sideLength;
    [Header("���غ���")]
    public int maxRound;
    [Header("ÿ����������(ȷ�������Ǳ߳���ƽ��)")]
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