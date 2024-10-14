using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI nextWaveTimeIn;
    [SerializeField] private TextMeshProUGUI surviveInText;
    private bool isPrepareToSpawn;
    private void Start()
    {
        nextWaveTimeIn.gameObject.SetActive(true);
        isPrepareToSpawn = true;
        EnemyWaveManager.instance.OnNumberWaveChange += EnemyWaveManager_OnNumberWaveChange;
        EnemyWaveManager.instance.OnPrepareToSpawn += EnemyWaveManager_OnPrepareToSpawn;
    }

    private void EnemyWaveManager_OnPrepareToSpawn(object sender, bool isPrepareToSpawn)
    {
        this.isPrepareToSpawn = isPrepareToSpawn;
        nextWaveTimeIn.gameObject.SetActive(isPrepareToSpawn);
    }

    private void Update()
    {
        if(isPrepareToSpawn) nextWaveTimeIn.text = "Next Wave In: " + ((int)EnemyWaveManager.instance.timePrepareToSpawn).ToString();
        surviveInText.text = "Survive In: " + ((int)EnemyWaveManager.instance.timeWave).ToString();
    }

    private void EnemyWaveManager_OnNumberWaveChange(object sender, int wave)
    {
        waveText.text = "Wave: " + wave.ToString();
    }
}
