using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExampleClass : MonoBehaviour {

    void Start()
    {
        Debug.Log(Path.Combine("/First/Path/To", "Some/File/At/foo.txt"));
    }
}
