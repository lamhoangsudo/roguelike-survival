using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointText;
    private void Start()
    {
        PointSystem.instance.OnPlayerPointChange += PointSystem_OnPlayerPointChange;
    }

    private void PointSystem_OnPlayerPointChange(object sender, float point)
    {
        pointText.text = "Point: " + ((int)point).ToString();
    }
}
