using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ETools
{
    [MenuItem("persistant/open11")]
    public static void Open()

    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
}
