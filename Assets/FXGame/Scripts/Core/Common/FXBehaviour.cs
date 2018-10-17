using UnityEngine;
using System.Collections;
/// <summary>
/// 需要继承自 MonoBehaviour 的类建议继续此类  
/// 为防止在Awake 或 Start的时候m_transform = transform 等 大量此写法在频繁使用时增加初始化的时间
/// 当只有在需要使用transform 等搜索属性时缓存,不会在初始化缓存 减少Instantiate 或 AddComponent 等的负担
/// </summary>
public class FXBehaviour : MonoBehaviour
{
    private Transform m_cacheTransform;
    public UnityEngine.Transform CacheTransform
    {
        get
        {
            if (null == m_cacheTransform) m_cacheTransform = gameObject.GetComponent<Transform>();
            return m_cacheTransform;
        }
    }


    private Animator m_cacheAnimator;
    public UnityEngine.Animator CacheAnimator
    {
        get
        {
            if (null == m_cacheAnimator) m_cacheAnimator = GetComponent<Animator>();
            return m_cacheAnimator;
        }
    }

    private Collider m_cacheCollider;
    public UnityEngine.Collider CacheCollider
    {
        get
        {

            if (null == m_cacheCollider) m_cacheCollider = GetComponent<Collider>();
            return m_cacheCollider;
        }
    }


    private UnityEngine.UI.Image m_cacheImage;
    public UnityEngine.UI.Image CacheImage
    {
        get
        {

            if (null == m_cacheImage) m_cacheImage = GetComponent<UnityEngine.UI.Image>();
            return m_cacheImage;
        }
    }


    private UnityEngine.SpriteRenderer m_spriteRenderer;
    public UnityEngine.SpriteRenderer CacheSpriteRenderer
    {
        get
        {

            if (null == m_spriteRenderer) m_spriteRenderer = GetComponent<UnityEngine.SpriteRenderer>();
            return m_spriteRenderer;
        }
    }
}
