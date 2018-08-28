using UnityEngine;
using System.Collections;
using LuaInterface;
using System;
using System.IO;
using UnityEngine.EventSystems;

public class GameInitialize : MonoBehaviour
{
    public void Awake()
    {
        FXGame.Managers.LuaManager.Instance.TestStart();
    }
}
