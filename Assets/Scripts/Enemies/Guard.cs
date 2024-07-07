using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Enemy
{
    // Start is called before the first frame update
    private void Awake()
    {
        base.Awake();

        speed = 1.5f;
        health = 100f;
        damage = 100f;
        bloodCount = 75;
    }
    void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        FollowPath();
    }
    public class MinimapIcon : MonoBehaviour
    {
        public Transform guardTransform; // R�f�rence au transform du garde
        public Vector3 offset = new Vector3(0, 2, 0); // D�calage pour positionner l'ic�ne au-dessus du garde

        void LateUpdate()
        {
            if (guardTransform != null)
            {
                // Positionner l'ic�ne au-dessus du garde avec un d�calage
                transform.position = guardTransform.position + offset;
            }
        }
    }
}   
