using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathBarUI : MonoBehaviour
{
    [SerializeField] private Transform Bar;
    [SerializeField] private Transform objHealth;
    [SerializeField] private Transform separator;
    [SerializeField] private Transform startSeparator;
    [SerializeField] private bool isPlayer;
    private Player player;
    private Enemy enemy;

    private float sizeY, sizeZ;
    public void Awake()
    {
        sizeY = Bar.localScale.y;
        sizeZ = Bar.localScale.z;
    }
    public void Start()
    {
        if (isPlayer)
        {
            player = objHealth.GetComponent<Player>();
            player.OnPlayerDamage += Player_OnPlayerDamage;
        }
        else
        {
            enemy = objHealth.GetComponent<Enemy>();
            enemy.OnEnemyDamage += Enemy_OnEnemyDamage;
        }
        SetUpHealthUI();
        UpdateVisual();
    }

    private void Enemy_OnEnemyDamage(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void Player_OnPlayerDamage(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (isPlayer)
        {
            float healthNormalize = player.GetHeathNormalize();
            if (healthNormalize < 1)
            {
                Bar.localScale = new Vector3(player.GetHeathNormalize(), sizeY, sizeZ);
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            float healthNormalize = enemy.GetHeathNormalize();
            if (healthNormalize < 1)
            {
                Bar.localScale = new Vector3(enemy.GetHeathNormalize(), sizeY, sizeZ);
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
    private void UpdateSeparator(int numberSeparator, float distanceEachSeparator)
    {
        for (int i = 1; i < numberSeparator; i++)
        {
            Instantiate(separator, startSeparator.transform.position + new Vector3(distanceEachSeparator * i * transform.localScale.x, 0, 0), Quaternion.identity, startSeparator);
        }
    }
    private void SetUpHealthUI()
    {
        int amountHealthPart = 5;
        int numberSeparator = 0;
        if (isPlayer)
        {
            numberSeparator = Mathf.FloorToInt(player.GetHeathMax() / amountHealthPart);
        }
        else
        {
            //numberSeparator = Mathf.FloorToInt(enemy.GetHeathMax() / amountHealthPart);
        }
        float distanceEachSeparator = Bar.Find("Bar").transform.localScale.x / numberSeparator;
        UpdateSeparator(numberSeparator, distanceEachSeparator);
    }
}
