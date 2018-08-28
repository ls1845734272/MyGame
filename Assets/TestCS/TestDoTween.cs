using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestDoTween : MonoBehaviour {

    int number = 10;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //private void FunctionOne()
    //{
    //    // 创建一个 Tweener 是自身坐标 一秒内 移动到  坐标 Vector3(5, 5, 5) 位置
    //    Tween tween = DOTween.To(() => transform.localPosition, r => transform.localPosition = r, new Vector3(1280, 720, 0), 10);

    //    // 创建一个 Tweener 对象， 另 number的值在 5 秒内变化到 100
    //    Tween t = DOTween.To(() => number, x => number = x, 100, 5);
    //    // 给执行 t 变化时，每帧回调一次 UpdateTween 方法
    //    t.OnUpdate(() => UpdateTween(number));
    //}
    //private void UpdateTween(int num)
    //{
    //    Debug.Log(num);      // 变化过程中， 每帧回调该方法
    //}
}
