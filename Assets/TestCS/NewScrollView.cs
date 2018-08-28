using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public enum SCEOLL_TYPE
{
    HOEIZONTAL,
    VERTICAL,
    BOTH
}

public class NewScrollView : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler {

    public GameObject container;
    public float RELOCATE_DURATION = 0.2f;//超出范围，还原的时间
    public float INERTANCE_SPEED = 0.96f;//惯性
    public float RESISTANCE_SPEED = 0.65f;//阻力
    public float LIMIT_VALUE = 1.0f;
    public bool _isInertanceFinish;

    protected GameObject _tweenMaker;
    public SCEOLL_TYPE direction;
    public bool dragable;
    public bool inertanceEnable;//惯性

    private Vector2 lastMovePoint;
    private Vector2 minOffset;
    private Vector2 maxOffset;
    protected Vector2 scrollDistance;

    private bool _isDraging;

    //scroll和container的size的大小
    protected float _rectWidth;
    protected float _rectHeight;
    protected float _containerWidth;
    protected float _containerHeight;
    protected Vector2 _containerLocalPosition;

    public NewScrollView()
    {
        direction = SCEOLL_TYPE.VERTICAL;
        lastMovePoint = Vector2.zero;
        dragable = true;
        scrollDistance = Vector2.zero;
        maxOffset = Vector2.zero;
        minOffset = Vector2.zero;
    }

    void Awake()
    {
        _tweenMaker = new GameObject();
        _tweenMaker.name = "_tweenMaker";
        _tweenMaker.transform.SetParent(this.transform);
        _tweenMaker.transform.localPosition = Vector3.zero;

        updateLimitOffset();
    }
    // Use this for initialization
    void Start () {
        Debug.Log("Start Open");
        Vector2 kk = new Vector2(429, 437);
        setContainerSize(kk);
        setContentOffsetToTop();
    }
	
	// Update is called once per frame
	void Update () {
        if (dragable == false) return;
        if (_isDraging) return;
        if (inertanceEnable)
        {
            Vector2 offset = getContentOffset() + scrollDistance;
            if (validateOffset(ref offset)) //超出范围，返回true
            {
                if (Mathf.Abs(scrollDistance.x) >= LIMIT_VALUE || Mathf.Abs(scrollDistance.y) >= LIMIT_VALUE)
                {
                    scrollDistance *= RESISTANCE_SPEED;//阻力
                    setContentOffsetWithoutCheck(getContentOffset() + scrollDistance);

                    _isInertanceFinish = false;

                }
                else
                {
                    if (!_isInertanceFinish)
                    {
                        _isInertanceFinish = true;
                        scrollDistance = Vector2.zero;
                        relocateContainerWithoutCheck(offset);
                    }
                }
            }
            else
            {
                if (Mathf.Abs(scrollDistance.x) >= LIMIT_VALUE || Mathf.Abs(scrollDistance.y) >= LIMIT_VALUE)
                {
                    scrollDistance *= INERTANCE_SPEED;
                    setContentOffsetWithoutCheck(getContentOffset() + scrollDistance);

                    _isInertanceFinish = false;

                }
                else
                {
                    if (!_isInertanceFinish)
                    {
                        _isInertanceFinish = true;

                        scrollDistance = Vector2.zero;
                    }
                }
            }

        }
    }

    /// <summary>
    /// 设置scroll和container的大小
    /// </summary>
    /// <param name="size"></param>
    void setContainerSize(Vector2 size)
    {
        Vector2 curSize = this.GetComponent<RectTransform>().rect.size;
        int width = Mathf.Max((int)curSize.x, (int)size.x);
        int height = Mathf.Max((int)curSize.y, (int)size.y);
        _containerWidth = width;
        _containerHeight = height;

        _rectWidth = this.GetComponent<RectTransform>().rect.width;
        _rectHeight = this.GetComponent<RectTransform>().rect.height;

        if (container != null)
            container.GetComponent<RectTransform>().sizeDelta = new Vector2(_containerWidth, _containerHeight);

        updateLimitOffset();

    }


    /// <summary>
    /// 移动范围
    /// </summary>
    void updateLimitOffset()
    {
        Vector2 _rect = new Vector2(_rectWidth, _rectHeight);
        Vector2 _container = new Vector2(_containerWidth, _containerHeight);
        maxOffset.x = 0;
        minOffset.x = _rect.x - _container.x;

        maxOffset.y = 0;
        minOffset.y = _rect.y - _container.y;

        if (direction == SCEOLL_TYPE.HOEIZONTAL)
        {
            minOffset.y = 0;
        }else if(direction == SCEOLL_TYPE.VERTICAL)
        {
            minOffset.x = 0;
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isDraging) return;

        Vector2 point = transform.InverseTransformPoint(eventData.position);

        if(dragable)
        {
            lastMovePoint = point;

            scrollDistance = Vector2.zero;
            _isDraging = true;
            LeanTween.cancel(_tweenMaker);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 point = transform.InverseTransformPoint(eventData.position);

        if (dragable)
        {
            scrollDistance = point - lastMovePoint;
            lastMovePoint = point;
            switch (direction)
            {
                case SCEOLL_TYPE.HOEIZONTAL:
                    scrollDistance.y = 0;
                    break;
                case SCEOLL_TYPE.VERTICAL:
                    scrollDistance.x = 0;
                    break;
                default:
                    break;
            }

            Vector2 vec = getContentOffset() + scrollDistance;
            Debug.Log(vec);
            if (validateOffset(ref vec))
            {
                if (direction == SCEOLL_TYPE.VERTICAL)
                {
                    scrollDistance.y *= 0.2f;
                }
                else if (direction == SCEOLL_TYPE.HOEIZONTAL)
                {
                    scrollDistance.x *= 0.2f;
                }
            }
            setContentOffsetWithoutCheck(getContentOffset() + scrollDistance);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragable)
        {
            _isDraging = false;
            Vector2 offset = getContentOffset();
            if (validateOffset(ref offset))
            {
                //这里是超出范围的回调
                scrollDistance = Vector2.zero;
                relocateContainerWithoutCheck(offset);
            }
        }
    }


    public Vector2 getContentOffset()
    {
        return _containerLocalPosition;
    }


    /// <summary>
    /// 判断当前点是否超出范围   超出返回true
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    protected bool validateOffset(ref Vector2 point)
    {
        float x = point.x;
        float y = point.y;
        x = Math.Min(x, maxOffset.x);
        x = Math.Max(x, minOffset.x);
        y = Math.Min(y, maxOffset.y);
        y = Math.Max(y, minOffset.y);

        if (point.x != x || point.y != y)
        {
            point.x = x;
            point.y = y;
            return true;
        }

        point.x = x;
        point.y = y;
        return false;
    }


    protected void setContentOffsetWithoutCheck(Vector2 offset)
    {
        _containerLocalPosition = offset;
        if (container != null)
            container.transform.localPosition = offset;
    }

    //迁移container 外部的检查
    public void  relocateContainerWithoutCheck(Vector2 offset)
    {
        //RELOCATE_DURATION
        setContentOffsetEaseInWithoutCheck(offset, RELOCATE_DURATION);
    }
    public void setContentOffsetEaseInWithoutCheck(Vector2 offset,float duration)
    {
        LeanTween.cancel(_tweenMaker);
        LeanTween.value(_tweenMaker, _containerLocalPosition, offset, duration)
            .setEase(LeanTweenType.easeInQuad)
            .setOnUpdate((Vector2 val) => {
                _containerLocalPosition = val;
                if (container != null)
                    container.transform.localPosition = val;
            })
            .setOnComplete(HelloWorld);
    }
    public void HelloWorld()
    {
        Debug.Log("Hello World");
    }
    public void setContentOffsetToTop()
    {
        if (direction == SCEOLL_TYPE.VERTICAL)
        {
            Vector2 point = new Vector2(0, -(_containerHeight - _rectHeight));
            setContentOffset(point);
        }
    }
    public void setContentOffset(Vector2 offset)
    {
        validateOffset(ref offset);

        _containerLocalPosition = offset;
        if (container != null)
            container.transform.localPosition = offset;
    }
}

