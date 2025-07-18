using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private float speed = 40f;
    private Vector3 lastMoveDirection;
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;
        
        if (Input.GetKey(KeyCode.D)) moveX = 1;
        
        if (Input.GetKey(KeyCode.A)) moveX = -1;
        
        if (Input.GetKey(KeyCode.W)) moveY = 1;
        
        if (Input.GetKey(KeyCode.S)) moveY = -1;
        
        bool isIdle = moveX == 0 && moveY==0;
        if (isIdle)
        {
            // TODO: Idle animation
        }
        else
        {
            float distance = speed * Time.deltaTime;
            Vector3 moveDirection = new Vector3(moveX, moveY, 0).normalized;

            if (TryMove(moveDirection, distance))
            {
                // TODO: Walking animation
            }
            else
            {
                // TODO: Idle animation
            }
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
            lastMoveDirection = direction;
            transform.position += direction * distance;
            return true;
        }

        return false;
    }

    private bool CanMove(Vector3 direction, float distance)
    {
        return Physics2D.Raycast(transform.position, direction, distance).collider == null;
    }
}
