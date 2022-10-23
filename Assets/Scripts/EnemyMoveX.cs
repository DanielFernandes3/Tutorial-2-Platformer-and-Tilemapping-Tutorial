using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveX : MonoBehaviour
{
    [SerializeField] float distanceToCover;
    [SerializeField] float speed;

    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    // Follows the target position like with a spring
    void Update()
    {
       Vector3 v = startingPosition;
       v.x += distanceToCover * Mathf.Sin(Time.time*speed);
       transform.position = v;
    }
}
