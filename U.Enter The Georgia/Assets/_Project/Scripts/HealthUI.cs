using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private PlayerScript player;

    void Awake()
    {
        healthText = GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        if (healthText != null)
        {
            healthText.text = player.Health.ToString();
        }
    }
}
