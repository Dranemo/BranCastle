using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDuration = 0.2f;
    private EdgeCollider2D attackCollider;
    private Vector2[] colliderPoints;

    void Start()
    {
        attackCollider = GetComponent<EdgeCollider2D>();
        attackCollider.enabled = false;

        // Create points for a 1/5 arc
        colliderPoints = new Vector2[20];
        for (int i = 0; i < colliderPoints.Length; i++)
        {
            float angle = i * Mathf.PI / 5f / (colliderPoints.Length - 1); // Divide by 5 for a 1/5 arc
            colliderPoints[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        attackCollider.points = colliderPoints;
    }

    void Update()
    {
        // Attack
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        attackCollider.enabled = false;
    }

    // Draw the collider in the scene view
    void OnDrawGizmos()
    {
        if (attackCollider && attackCollider.enabled)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < colliderPoints.Length - 1; i++)
            {
                Gizmos.DrawLine(transform.TransformPoint(colliderPoints[i]), transform.TransformPoint(colliderPoints[i + 1]));
            }
        }
    }
}
