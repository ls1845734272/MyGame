using UnityEngine;
using System.Collections;

public class UICameraFog : MonoBehaviour
{
    private bool revertFogState = false;
    void OnPreRender()
    {
        revertFogState = RenderSettings.fog;
        RenderSettings.fog = false;
    }
    void OnPostRender()
    {
        RenderSettings.fog = revertFogState;
    }
}
