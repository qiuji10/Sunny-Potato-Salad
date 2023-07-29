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


public class PlayerController : MonoBehaviour
{
    private DirectionStates _direction;

    public bool cantMove;
    public float moveSpeed = 5.0f;
    public Transform moveCoord;

    private Vector3 _nextPosition;
    private Vector3 _prevPosition;


    public void Start()
    {
        moveCoord.parent = null;

        _direction = DirectionStates.forward;
    }

    public void Update()
    {
        if (cantMove) return;
        ChangeDirection();
        ConstantMovement();
    }

    private void ConstantMovement()
    {
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
        Ground ground = ChunkManager.GetGround(transform.position);
        yield return new WaitForSeconds(duration);
        ground.SetGroundState(GroundState.Digged);

        cantMove = false;
    }

    private IEnumerator StunDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        cantMove = false;
    }
    
    private IEnumerator MovementSlowed(float duration)
    {
        yield return new WaitForSeconds(duration);
        moveSpeed = 5.0f;
    }

    public void KnockBack(float stunTime)
    {
        cantMove = true;
        moveSpeed = 5.0f / 2f;
        moveCoord.position = _prevPosition;
        StartCoroutine(StunDuration(0.25f));
        StartCoroutine(MovementSlowed(stunTime));
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("TreasureChest"))
        {
            cantMove = true;
            StartCoroutine(Digging(0.5f));
        }

        if (other.CompareTag("Tree"))
        {
            KnockBack(0.25f);
        }

        if (other.CompareTag("Rock"))
        {
            KnockBack(1.0f);
        }
    }


}
