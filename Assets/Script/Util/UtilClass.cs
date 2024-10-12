using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilClass
{
    public static Vector3 GetRamdomVector()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
    }
}
