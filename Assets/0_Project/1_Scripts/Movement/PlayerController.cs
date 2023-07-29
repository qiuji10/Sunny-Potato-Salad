using System.Collections;
using System;
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
    [Header("Movement")]
    public bool cantMove;
    public float moveSpeed = 5.0f;
    public Transform moveCoord;

    [Header("Swipe Settings")]
    public float swipeDuration;
    private float _firstTapTime;

    private Vector3 _firstTouchPos;

    private DirectionStates _direction;
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
        SwipeDetection();
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

    private void SwipeDetection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _firstTapTime = Time.time;
            _firstTouchPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 swipeDelta = Input.mousePosition - _firstTouchPos;

            if (swipeDelta != Vector2.zero && Time.time - _firstTapTime <= swipeDuration)
            {
                SwipeDirection(swipeDelta);
            }

        }
    }

    private void SwipeDirection(Vector2 delta)
    {
        float xAbs = Mathf.Abs(delta.x);
        float yAbs = Mathf.Abs(delta.y);

        //Horizontal Swipe
        if (xAbs > yAbs)
        {
            //do left or right
            if (delta.x > 0)
            {
                Debug.Log("Right");
                _direction = DirectionStates.right;
            }

            else if (delta.x < 0)
            {
                Debug.Log("Left");
                _direction = DirectionStates.left;
            }
        }
        //Vertical swipe
        else
        {
            //up or down
            if (delta.y > 0)
            {
                _direction = DirectionStates.forward;
            }

            else if (delta.y < 0)
            {
                _direction = DirectionStates.backward;
            }
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
