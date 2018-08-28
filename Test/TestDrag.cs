//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class TestDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
//{
//    RectTransform imgrect;
//    Vector3 offset;

//    void Start()
//    {
//        //    Debug.Log("start");
//        imgrect = transform.gameObject.GetComponent<RectTransform>();
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        SetDraggedPosition(eventData);
//        //Vector3 globalMousePos;
//        //if (RectTransformUtility.ScreenPointToWorldPointInRectangle(imgRect, eventData.position, eventData.pressEventCamera, out globalMousePos))
//        //{
//        //    Debug.Log("当前的点的信息111");
//        //    Debug.Log(globalMousePos);
//        //    //offset = Input.mousePosition - globalMousePos;
//        //}

//        //Vector2 globalMousePos2;
//        //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imgRect, Input.mousePosition, eventData.pressEventCamera, out globalMousePos2))
//        //{
//        //    Debug.Log("当前的点的信息222");
//        //    Debug.Log(globalMousePos2);
//        //    //offset = Input.mousePosition - globalMousePos;
//        //}

//        //Vector3 globalMousePos;
//        //if (RectTransformUtility.ScreenPointToWorldPointInRectangle(imgRect, eventData.position, eventData.pressEventCamera, out globalMousePos))
//        //{
//        //    offset = Input.mousePosition + globalMousePos;
//        //}
//    }

//    // during dragging
//    public void OnDrag(PointerEventData eventData)
//    {
//        SetDraggedPosition(eventData);
//        //imgRect.anchoredPosition = offset + mouseUguiPos;
//        //Vector3 mouseUguiPos;
//        //bool isRect = RectTransformUtility.ScreenPointToWorldPointInRectangle(imgRect, eventData.position, eventData.enterEventCamera, out mouseUguiPos);
//        //if (isRect)
//        //{
//        //    imgRect.anchoredPosition = offset - mouseUguiPos;
//        //}
//    }

//    // end dragging
//    public void OnEndDrag(PointerEventData eventData)
//    {
//        SetDraggedPosition(eventData);
//    }

//    private void SetDraggedPosition(PointerEventData eventdata)
//    {
//        //var rt = gameobject.getcomponent<recttransform>();

//        // transform the screen point to world point int rectangle
//        Vector3 globalmousepos;
//        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(imgrect, eventdata.position, eventdata.enterEventCamera, out globalmousepos))
//        {
//            imgrect.position = globalmousepos;
//        }
//    }

//    //public Canvas canvas;//画布
//    //private RectTransform rectTransform;//坐标

//    //void Start()
//    //{
//    //    canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
//    //    rectTransform = canvas.transform as RectTransform; //也可以写成this.GetComponent<RectTransform>(),但是不建议；

//    //}

//    //void Update()
//    //{
//    //    if (Input.GetMouseButtonDown(0))
//    //    {
//    //        Vector2 pos;
//    //        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, canvas.worldCamera, out pos))
//    //        {
//    //            rectTransform.anchoredPosition = pos;
//    //            Debug.Log(pos);
//    //        }
//    //    }
//    //}
//}





using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestDrag : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    //Vector3 vec3;
    //Vector3 pos;

    //public void MoveObject()
    //{
    //    Vector3 off = Input.mousePosition - vec3;
    //    vec3 = Input.mousePosition;
    //    pos = pos + off;
    //    transform.GetComponent<RectTransform>().position = pos;
    //}
    //public void PointerDown()
    //{
    //    vec3 = Input.mousePosition;
    //    pos = transform.GetComponent<RectTransform>().position;
    //}
    RectTransform imgRect;
    Vector3 offset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 mouseUguiPos;
        bool isRect = RectTransformUtility.ScreenPointToWorldPointInRectangle(imgRect, eventData.position, eventData.enterEventCamera, out mouseUguiPos);
        if (isRect)
        {
            //Debug.Log("imgRect.localPosition:"+imgRect.localPosition);
            //Debug.Log("mouseUguiPos:"+mouseUguiPos );
            offset = imgRect.position - mouseUguiPos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetRecttrans(eventData);

    }

    void Start()
    {
        Debug.Log("start");
        imgRect = transform.gameObject.GetComponent<RectTransform>();
    }

    void SetRecttrans(PointerEventData eventData)
    {
        Vector3 mouseUguiPos;
        bool isRect = RectTransformUtility.ScreenPointToWorldPointInRectangle(imgRect, eventData.position, eventData.enterEventCamera, out mouseUguiPos);
        if (isRect)
        {
            //Debug.Log("imgRect.anchoredPosition2222:" + imgRect.anchoredPosition);
            //Debug.Log("mouseUguiPos2222:" + mouseUguiPos);
            imgRect.position = offset + mouseUguiPos;
        }
    }
}






























//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

//public class DragWindow : MonoBehaviour/*, IDragHandler, IBeginDragHandler*/
//{
//    Vector3 vec3;
//    Vector3 pos;

//    public void MoveObject()
//    {
//        Vector3 off = Input.mousePosition - vec3;
//        vec3 = Input.mousePosition;
//        pos = pos + off;
//        transform.GetComponent<RectTransform>().position = pos;
//    }
//    public void PointerDown()
//    {
//        vec3 = Input.mousePosition;
//        pos = transform.GetComponent<RectTransform>().position;
//    }
//    //RectTransform imgRect;
//    //Vector3 offset;

//    //public void OnBeginDrag(PointerEventData eventData)
//    //{
//    //    Vector3 mouseUguiPos;
//    //    bool isRect = RectTransformUtility.ScreenPointToWorldPointInRectangle(imgRect, eventData.position, eventData.enterEventCamera, out mouseUguiPos);
//    //    if (isRect)
//    //    {
//    //        offset = imgRect.localPosition - mouseUguiPos;
//    //    }
//    //}

//    //public void OnDrag(PointerEventData eventData)
//    //{
//    //    SetRecttrans(eventData);

//    //}

//    //void Start()
//    //{
//    //    imgRect = transform.gameObject.GetComponent<RectTransform>();
//    //}

//    //void SetRecttrans(PointerEventData eventData)
//    //{
//    //    Vector3 mouseUguiPos;
//    //    bool isRect = RectTransformUtility.ScreenPointToWorldPointInRectangle(imgRect, eventData.position, eventData.enterEventCamera, out mouseUguiPos);
//    //    if (isRect)
//    //    {
//    //        imgRect.anchoredPosition = offset + mouseUguiPos;
//    //    }
//    //}
//}
