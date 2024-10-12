using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallBack : MonoBehaviour
{
    [SerializeField] private List<Transform> vfxLoading;
    private void Awake()
    {
        Instantiate(vfxLoading[Random.Range(0, vfxLoading.Count)], Vector3.zero, Quaternion.identity);
    }
    private void Update()
    {
        LoadSceneManager.LoaderCallBack();
    }
}
