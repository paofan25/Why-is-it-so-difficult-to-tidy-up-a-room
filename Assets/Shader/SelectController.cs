using System.Collections.Generic;
using UnityEngine;
 
public class SelectController : MonoBehaviour { // 单击选中控制
    private List<GameObject> targets; // 选中的游戏对象
    private List<GameObject> loseFocus; // 失焦的游戏对象
    private RaycastHit hit; // 碰撞信息
 
    private void Awake() {
        targets = new List<GameObject>();
        loseFocus = new List<GameObject>();
    }
 
    private void Update() {
        if (Input.GetMouseButtonUp(0)) {
            GameObject hitObj = GetHitObj();
            if (hitObj == null) { // 未选中任何物体, 已描边的全部取消描边
                targets.ForEach(obj => loseFocus.Add(obj));
                targets.Clear();
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
                if (targets.Contains(hitObj)) { // Ctrl重复选中, 取消描边
                    loseFocus.Add(hitObj);
                    targets.Remove(hitObj);
                } else { // Ctrl追加描边
                    targets.Add(hitObj);
                }
            } else { // 单选描边
                targets.ForEach(obj => loseFocus.Add(obj));
                targets.Clear();
                targets.Add(hitObj);
                loseFocus.Remove(hitObj);
            }
            DrawOutline();
        }
    }
 
    private void DrawOutline() { // 绘制描边
        targets.ForEach(obj => {
            if (obj.GetComponent<OutlineEffect>() == null) {
                obj.AddComponent<OutlineEffect>();
            } else {
                obj.GetComponent<OutlineEffect>().enabled = true;
            }
        });
        loseFocus.ForEach(obj => {
            if (obj.GetComponent<OutlineEffect>() != null) {
                obj.GetComponent<OutlineEffect>().enabled = false;
            }
        });
        loseFocus.Clear();
    }
 
    private GameObject GetHitObj() { // 获取屏幕射线碰撞的物体
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            return hit.transform.gameObject;
        }
        return null;
    }
}