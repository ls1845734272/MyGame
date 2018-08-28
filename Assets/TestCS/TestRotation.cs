using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    private Vector3 c_defDir = new Vector3(34.63f,-74.87f,-30.122f);
    public Vector3 m_Speed = new Vector3(2, 2, 2);
    private Vector3 m_TargetDir = Vector3.zero;
    private Transform m_Transform;
    private bool m_Starting = false;
    private bool m_Stoping = false;
    private Quaternion m_StopDir = Quaternion.identity;
    public float m_StopSpeed = 5f;



    private Vector3[] m_ConstanDir = new Vector3[] 
    {
        Vector3.up,
        new Vector3(45,50,0),
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back,
    };




    public Transform m_Child;
    void Start()
    {
        m_Transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Starting)
        {
            return;
        }



        if (m_Stoping)
        {
            m_Transform.localRotation = Quaternion.Lerp(m_Transform.localRotation, m_StopDir, Time.deltaTime * m_StopSpeed);
            m_Child.localRotation = Quaternion.Lerp(m_Child.localRotation,Quaternion.identity, Time.deltaTime * m_StopSpeed);


        }
        else
        {
            m_TargetDir += m_Speed * Time.deltaTime;
            m_Transform.localRotation = Quaternion.Euler(m_TargetDir);
        }
    }

    public void StartRotate()
    {
        m_Starting = true;
        m_Stoping = false;
        m_Child.localRotation = Quaternion.Euler(c_defDir);
    }


    public void StopRotate(int pointNum)
    {
        Debug.Log(pointNum);
        m_Stoping = true;
        m_StopDir = Quaternion.FromToRotation(m_ConstanDir[pointNum], Vector3.up);
    }



    private void OnGUI()
    {
        if (GUILayout.Button("start",GUILayout.Width(200),GUILayout.Height(200)))
        {
            StartRotate();
        }

        if (GUILayout.Button("stop", GUILayout.Width(200), GUILayout.Height(200)))
        {
            StopRotate(Random.Range(1, 6));
        }
    }
}
