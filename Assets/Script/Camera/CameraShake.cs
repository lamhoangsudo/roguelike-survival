using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private float timer;
    private float timerMax;
    private float startIntencity;
    private CinemachineBasicMultiChannelPerlin channelPerlin;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = transform.GetComponent<CinemachineVirtualCamera>();
        channelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (timer < timerMax)
        {
            timer += Time.deltaTime;
            float amplitude = Mathf.Lerp(startIntencity, 0f, timer / timerMax);
            channelPerlin.m_AmplitudeGain = amplitude;
        }
    }
    public void setShake(float intencity, float timeMax)
    {
        this.timerMax = timeMax;
        startIntencity = intencity;
        timer = 0;
    }
}
