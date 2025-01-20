using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
