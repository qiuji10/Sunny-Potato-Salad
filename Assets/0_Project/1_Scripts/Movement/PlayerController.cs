using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionStates
{
    forward,
    backward,
    left,
    right
}

public enum GeneralStates
{
    move,
    dig,
    bounce
}

public class PlayerController : MonoBehaviour
{
    private DirectionStates _direction;

    public float moveSpeed = 5.0f;
    public Transform moveCoord;

    private Vector3 _nextPosition;
    private Vector3 _prevPosition;

    private bool _cantMove;

    public void Start()
    {
        moveCoord.parent = null;

        _direction = DirectionStates.forward;
    }

    public void Update()
    {

        ChangeDirection();
        ConstantMovement();
    }

    private void ConstantMovement()
    {
        if (_cantMove) return;

        transform.position = Vector3.MoveTowards(transform.position, moveCoord.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, moveCoord.position) >= 0.0001f) return;

        switch (_direction)
        {
            case DirectionStates.forward:
                _nextPosition = new Vector3(-2f, 0f, 0f);
                break;
            case DirectionStates.backward:
                _nextPosition = new Vector3(2f, 0f, 0f);
                break;
            case DirectionStates.left:
                _nextPosition = new Vector3(0f, 0f, -2f);
                break;
            case DirectionStates.right:
                _nextPosition = new Vector3(0f, 0f, 2f);
                break;
        }

        _prevPosition = moveCoord.position;
        moveCoord.position += _nextPosition;
    }

    private void ChangeDirection()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _direction = DirectionStates.forward;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _direction = DirectionStates.backward;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _direction = DirectionStates.left;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _direction = DirectionStates.right;
        }
    }

    private IEnumerator Digging(float duration)
    {
        yield return new WaitForSeconds(duration);

    }

    private IEnumerator StunDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        _cantMove = false;
    }

    private void KnockBackDirection()
    {
        moveCoord.position = _prevPosition;
    }

    private void OnTriggerEnter(Collider other)
    {

        //if (other.CompareTag("Treasure"))
        //{
            
        //}

        if (other.CompareTag("Obstacle"))
        {
            _cantMove = true;
            KnockBackDirection();
            StartCoroutine(StunDuration(0.25f));
        }
    }


}
