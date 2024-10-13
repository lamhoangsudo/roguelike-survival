using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance;
    [SerializeField] private CinemachineVirtualCamera cameraHandler;
    [SerializeField] private Camera minimap;
    [SerializeField] private float zoomAmount;
    [SerializeField] private float zoomSpeed;
    [SerializeField] float maxZoom = 30f;
    [SerializeField] float minZoom = 10f;
    private float orthographicSize;
    private float targetOrthographicSize;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        orthographicSize = cameraHandler.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }
    private void Update()
    {
        HandLerZoom();
    }
    private void HandLerZoom()
    {
        targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minZoom, maxZoom);
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
        cameraHandler.m_Lens.OrthographicSize = orthographicSize;
        minimap.orthographicSize = orthographicSize * 4f;
    }
}
