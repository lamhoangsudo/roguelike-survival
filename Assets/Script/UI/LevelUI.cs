using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    private void Start()
    {
        LevelSystem.instance.OnLevelChanged += Instance_OnLevelChanged;
    }

    private void Instance_OnLevelChanged(object sender, int level)
    {
        levelText.text = "Level: " + level.ToString();
    }
}
