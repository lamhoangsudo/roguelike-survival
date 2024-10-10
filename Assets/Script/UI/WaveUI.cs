using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    private void Start()
    {
        EnemyWaveManager.instance.OnNumberWaveChange += EnemyWaveManager_OnNumberWaveChange;
    }

    private void EnemyWaveManager_OnNumberWaveChange(object sender, int wave)
    {
        waveText.text = "Wave: " + wave.ToString();
    }
}
