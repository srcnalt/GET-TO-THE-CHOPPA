using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nicholas : Character
{
    public void Release()
    {
        transform.SetParent(null);
    }
}
