using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayer;
    private Transform player;
    private float speed = 10f;

    private void Start()
    {
        player = FindObjectOfType<PlayerCharacter>().gameObject.transform;
    }
    
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float distance = speed * Time.deltaTime;
        Vector3 moveDirection = (player.position - transform.position).normalized;

        if (TryMove(moveDirection, distance))
        {
            // TODO: Walking animation
        }
    }

    private bool TryMove(Vector3 baseDirection, float distance)
    {
        Vector3 direction = baseDirection;
        bool canMove = CanMove(direction, distance);
        if (!canMove)
        {
            // Hit something. Can't move diagonally
            // Test if can move horizontally
            direction = new Vector3(baseDirection.x, 0, 0).normalized;
            canMove = direction.x != 0 && CanMove(direction, distance);

            if (!canMove)
            {
                // Can't move horizontally
                // Test if can move vertically
                direction = new Vector3(0, baseDirection.y, 0).normalized;
                canMove = direction.y != 0 && CanMove(direction, distance);
            }
        }

        if (canMove)
        {
            // Can move vertically
            transform.position += direction * distance;
            return true;
        }
        

        return false;
    }

    private bool CanMove(Vector3 direction, float distance)
    {
        Collider2D collider = Physics2D.Raycast(transform.position, direction, distance, collisionLayer).collider;
        return collider == null;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
