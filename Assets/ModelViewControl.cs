using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//脚本挂在Modelshow GameObject下
public class ModelViewControl : MonoBehaviour {
    private bool isClick = false;
    private Vector3 startPos;       //点下开始位置
    private Vector3 endPos;         //点下终点位置

    //回调间距
    float interval = 0.01f;
    float clickBeginTime = 0.0f;
    //模型引用
    private Transform model;    //模型根节点
    void Start () {
        model = transform;
      
    }
    // Update is called once per frame
    void Update () {
#if UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown(0))
#elif UNITY_ANDROID
        if(Input.touchCount > 0 && !isClick) 
#endif
        {
            isClick = true;
#if UNITY_STANDALONE_WIN
            startPos = Input.mousePosition;
#elif UNITY_ANDROID
            startPos = Input.touches[0].position;
#endif
            clickBeginTime = Time.time;
        }
#if UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonUp(0))
#elif UNITY_ANDROID
        if (Input.touchCount == 0 && isClick)
#endif
        {
            isClick = false;
        }
        if (isClick && (Time.time - clickBeginTime) > interval)
        {
#if UNITY_STANDALONE_WIN
            endPos = Input.mousePosition;
#elif UNITY_ANDROID
            endPos = Input.touches[0].position;
#endif
            if ((endPos - startPos).magnitude < 5)
            {
                return;
            }
            if(Mathf.Abs(endPos.x - startPos.x) < 5)
            {
                endPos.x = startPos.x;
            }
            if (Mathf.Abs(endPos.y - startPos.y) < 5)
            {
                endPos.y = startPos.y;
            }
            RotateModel(startPos,endPos);
            startPos = endPos;
        }
    }
    void RotateModel(Vector3 startPos , Vector3 endPos)
    {
        Vector3 direction = endPos - startPos;
        Vector3 world_axis = Vector3.Cross(direction, Vector3.forward);
        model.Rotate(world_axis.normalized, direction.magnitude * 0.3f, Space.World);
    }
}
