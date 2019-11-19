using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pentRot : MonoBehaviour
{
    private float rotSpeed=60;
    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = 60;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,Time.deltaTime*rotSpeed);
    }
}
