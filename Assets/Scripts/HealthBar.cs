using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthFill;
    private Unit unit;
    private Enemy enemy;
    private float health;

    void Start()
    {
        unit = GetComponentInParent<Unit>();

        if (unit != null)
        {
            health = unit.GetHealthUnit();
        }
        else
        {
            enemy = GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                health = enemy.GetHealthEnemy();
            }
            else
            {
                //////Debug.LogError("Le parent n'a pas de composant Unit ou Enemy.");
                return;
            }
        }

        // Initialiser le Slider
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    private void UpdateHealth(float value)
    {
        // Convertir la valeur du slider (0-10000) en une valeur entre 0 et 1
        float fillValue = Mathf.Clamp01(value / healthSlider.maxValue);
        healthFill.fillAmount = fillValue;
    }
    void Update()
    {
        if (unit != null)
        {
            health = unit.GetHealthUnit();
        }
        else if (enemy != null)
        {
            health = enemy.GetHealthEnemy();
        }

        healthSlider.value = health;
        UpdateHealth(health);
    }
}
