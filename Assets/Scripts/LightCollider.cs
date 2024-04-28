using UnityEngine.Rendering.Universal;
using UnityEngine;

[RequireComponent(typeof(Light2D))]
[RequireComponent(typeof(PolygonCollider2D))]
public class LightCollider : MonoBehaviour
{
    private Light2D light2D;
    private PolygonCollider2D polygonCollider2D;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        // Cr�ez un nouveau tableau de Vector2
        Vector2[] points = new Vector2[light2D.shapePath.Length];

        // Copiez les coordonn�es x et y de chaque point de shapePath dans le nouveau tableau
        for (int i = 0; i < light2D.shapePath.Length; i++)
        {
            points[i] = new Vector2(light2D.shapePath[i].x, light2D.shapePath[i].y);
        }

        // Ajustez les points du PolygonCollider pour qu'ils correspondent � la forme de la lumi�re
        polygonCollider2D.points = points;
    }

}
