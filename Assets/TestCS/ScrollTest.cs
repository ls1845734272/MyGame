//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class ScrollTest : MonoBehaviour,IBeginDragHandler,IEndDragHandler {

//    private ScrollRect scroll;
//    private bool isDraged;
//    float[] pageArr;
//    float targetPagePos;
//    // Use this for initialization
//    void Start () {
//        scroll = GetComponent<ScrollRect>();

//    }

//	// Update is called once per frame
//	void Update () {
//		if (!isDraged)
//        {
//            float finalPos = Mathf.Lerp(scroll.horizontalNormalizedPosition, targetPagePos, 0.1f);
//            scroll.horizontalNormalizedPosition = finalPos;
//        }
//	}

//    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
//    {
//        isDraged = true;
//    }

//    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
//    {
//        float posX = scroll.horizontalNormalizedPosition;
//        float posY = scroll.verticalNormalizedPosition;
//        int index = 0;
//        float minOffset = 1;
//        foreach (float item in pageArr)
//        {
//            float offset = Mathf.Abs(item - posX);
//            if (offset < minOffset)
//            {
//                index = Array.IndexOf(pageArr, item);
//                minOffset = offset;
//            }
//        }
//        targetPagePos = pageArr[index];
//        isDraged = false;
//    }
//}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class ScrollTest : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{

    private ScrollRect rect;                        //滑动组件    
    private float targethorizontal = 0;             //滑动的起始坐标    
    private bool isDrag = false;                    //是否拖拽结束    
    private List<float> posList = new List<float>();            //求出每页的临界角，页索引从0开始    
    private int currentPageIndex = -1;
    public Action<int> OnPageChanged;
    public RectTransform content;
    private bool stopMove = true;
    public float smooting = 4;      //滑动速度    
    public float sensitivity = 0;
    private float startTime;

    private float startDragHorizontal;
    public Transform toggleList;

    void Start()
    {
        Debug.Log("开始执行了这里");
        rect = transform.GetComponent<ScrollRect>();
        var _rectWidth = GetComponent<RectTransform>();
        var tempWidth = ((float)content.transform.childCount * _rectWidth.rect.width);
        content.sizeDelta = new Vector2(tempWidth, _rectWidth.rect.height);
        //未显示的长度  
        float horizontalLength = content.rect.width - _rectWidth.rect.width;
        for (int i = 0; i < rect.content.transform.childCount; i++)
        {
            posList.Add(_rectWidth.rect.width * i / horizontalLength);
        }
        //int index = 0;

    }

    void Update()
    {
        if (!isDrag && !stopMove)
        {
            startTime += Time.deltaTime;
            float t = startTime * smooting;
            rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, t);
            if (t >= 1)
                stopMove = true;
        }
        //Debug.Log(rect.horizontalNormalizedPosition);

    }

    public void pageTo(int index)
    {
        if (index >= 0 && index < posList.Count)
        {
            rect.horizontalNormalizedPosition = posList[index];
            SetPageIndex(index);
            GetIndex(index);
        }
    }
    private void SetPageIndex(int index)
    {
        if (currentPageIndex != index)
        {
            currentPageIndex = index;

            if (OnPageChanged != null)
            {
                OnPageChanged(index);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        //开始拖动  
        startDragHorizontal = rect.horizontalNormalizedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float posX = rect.horizontalNormalizedPosition;
        posX += ((posX - startDragHorizontal) * sensitivity);
        posX = posX < 1 ? posX : 1;
        posX = posX > 0 ? posX : 0;
        int index = 0;

        float offset = Mathf.Abs(posList[index] - posX);  


        for (int i = 1; i < posList.Count; i++)
        {
            float temp = Mathf.Abs(posList[i] - posX);
            //Debug.Log("temp " + temp);  
            //Debug.Log("i" + i);  
            if (temp < offset)
            {
                index = i;
                offset = temp;
            }
            //Debug.Log("index " + index);  
        }
        SetPageIndex(index);
        GetIndex(index);
        targethorizontal = posList[index]; //设置当前坐标，更新函数进行插值    
        isDrag = false;
        startTime = 0;
        stopMove = false;

    }

    public void GetIndex(int index)
    {
        //var toogle = toggleList.GetChild(index).GetComponent<Toggle>();
        //toogle.isOn = true;
    }
}