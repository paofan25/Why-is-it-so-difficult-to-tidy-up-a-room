using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private int sideLength = 3;
    private int currentRound;
    public List<Point> points;
    public List<Text> texts;
    public bool randomSpawn;
    public Text roundText;
    private int playerId;
    private bool isDone = false;
    private void Awake()
    {
        if (randomSpawn)
        {
            Debug.Log("随机生成还没做");
        }
        //渲染初始化
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].text = "";
        }
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].id > sideLength * sideLength ||
                points[i].id <= 0 ||
                points[i].currentId > sideLength * sideLength ||
                points[i].currentId <= 0 ||
                points[i].StayRound != 0 ||
                points[i].IsAlive == true)
            {
                Debug.LogError($"=====>第{i + 1}个棋子数据错误");
            }
            else
            {
                texts[points[i].currentId - 1].text += $"{points[i].id},";
            }
        }
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < points.Count; i++)
            {
                Debug.Log(points[i]);
            }
        }
    }
    public void GoTo(int id)
    {
        if (isDone) return;
        Point curPoint = null;
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].id == id)
            {
                curPoint = points[i];
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
        for (int i = 0; i < points.Count; i++)
        {
            points[i].StayRound--;
            if(points[i].StayRound <= 0 && points[i].IsAlive)
            {
                points[i].AIMove(/*playerId*/);
            }
        }
        Draw();
        if (CheckIsOver())
        {
            Debug.Log("游戏结束");
            return;
        }
    }
    public void MoveTo(Point point)
    {
        point.currentId = point.id;
        playerId = point.id;
        point.IsAlive = true;
        point.StayRound = 3;
    }
    public bool CheckIsOver()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].id != points[i].currentId)
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
        for (int i = 0; i < points.Count; i++)
        {
            texts[points[i].currentId - 1].text += $"{points[i].id},";
        }
        texts[playerId - 1].text += $"人,";
        roundText.text = currentRound.ToString();
    }
}
[Serializable]
public class Point
{
    public int id;
    public int currentId;
    private int stayRound = 0;
    private bool isAlive = false;
    public int StayRound
    {
        get { return stayRound; }
        set { stayRound = value; }
    }
    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }
    public void AIMove(/*int playerId,*/int sideLength = 3)
    {
        int playerId = id;
        //行列
        int px = (playerId - 1) / sideLength + 1;
        int py = (playerId - 1) % sideLength + 1;
        int x = (currentId - 1) / sideLength + 1;
        int y = (currentId - 1) % sideLength + 1;
        //行差
        int dx = x - px;
        //列差
        int dy = y - py;
        int dxAbs = Math.Abs(dx);
        int dyAbs = Math.Abs(dy);
        int moveX = Math.Sign(dx);
        int moveY = Math.Sign(dy);
        if ((dxAbs == 1 && dyAbs == 0) || 
            (dyAbs == 1 && dxAbs == 0))
        {
            Debug.Log($"{id} 禁锢");
            return;
        }
        int tempid = currentId;
        if (moveX == 0) moveX = 1;
        int newx = x + moveX;
        if (newx < 1 || newx > 3) newx = x - 1;
        if (newx <1 || newx > 3)
        {
            if (moveY == 0) moveY = 1;
            int newy = y + moveY;
            if (newy < 1 || newy > 3) newy = y - 1;
            if (newy <1 || newy > 3)
            {
                Debug.Log($"{id}不移动到");
                return;
            }
            currentId = (x - 1) * 3 + newy;
            Debug.Log($"{id}从{tempid}移动到{currentId}");
            return;
        }
        currentId = (newx - 1) * 3 + y;
        Debug.Log($"{id}从{tempid}移动到{currentId}");
    }
    public override string ToString()
    {
        return $"id:{id} currentid:{currentId} isAlive:{isAlive} stayRound{stayRound}";
    }
}
public enum pointState
{
    wait,
}