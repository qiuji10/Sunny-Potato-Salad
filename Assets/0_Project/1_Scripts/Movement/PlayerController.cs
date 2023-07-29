using System.Collections;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public enum DirectionStates
{
    forward,
    backward,
    left,
    right
}


public class PlayerController : MonoBehaviour
{
    public Animator animControl;

    [Header("Movement")]
    public bool cantMove;
    public float moveSpeed = 5.0f;
    private float neutralSpeed;
    private float buffSpeed;
    public Transform moveCoord;

    [Header("Swipe Settings")]
    public float swipeDuration;
    private float _firstTapTime;

    private Vector3 _firstTouchPos;

    private DirectionStates _direction;
    private Vector3 _nextPosition;
    private Vector3 _prevPosition;

    [Header("Ability Modify")]
    public GameObject shield;
    public float diggingLength = 1.25f;
    public float stunLength;

    private bool _ReceivedBuffSpeed;
    private Coroutine movingCoroutine;

    [SerializeField] private Timer timer;

    public void Start()
    {
        moveCoord.parent = null;

        neutralSpeed = buffSpeed = moveSpeed;
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

        animControl.SetFloat("FrontBack", moveCoord.position.x - transform.position.x);
        animControl.SetFloat("Sideway", moveCoord.position.z - transform.position.z);
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

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
                _direction = DirectionStates.right;
            }

            else if (delta.x < 0)
            {
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

    private IEnumerator Digging()
    {
        Ground ground = ChunkManager.GetGround(transform.position);
        yield return new WaitForSeconds(diggingLength);
        ground.SetGroundState(GroundState.Digged);
        TreasureChest_AbilityPoint();
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
        moveSpeed = _ReceivedBuffSpeed ? buffSpeed : neutralSpeed;
    }

    public void KnockBack(float stunTime)
    {
        cantMove = true;
        moveSpeed = 5.0f / 2f;
        moveCoord.position = _prevPosition;
        StartCoroutine(StunDuration(0.25f));
        StartCoroutine(MovementSlowed(stunTime));
    }

    public void TreasureChest_AbilityPoint()
    {
        int rand = Random.Range(0, 10);

        if (rand >= 0 && rand < 5)
        {
            Debug.Log("Speed Earn");

            if (movingCoroutine != null)
                StopCoroutine(movingCoroutine);

            _ReceivedBuffSpeed = true;
            buffSpeed *= 1.05f;
            moveSpeed = buffSpeed;
            movingCoroutine = StartCoroutine(MovementSpeed());
        }

        if (rand >= 5 && rand < 7)
        {
            shield.SetActive(true);
        }

        if (rand >= 7 && rand < 9)
        {
            Debug.Log("Dig Buff");
            diggingLength = 0.15f;
            StartCoroutine(DiggingSpeed());
        }

        if (rand == 9)
        {
            timer.IncreaseTime(15);
        }

    }

    private IEnumerator DiggingSpeed()
    {
        yield return new WaitForSeconds(15.0f);
        diggingLength = 1.25f;
    }

    private IEnumerator MovementSpeed()
    {
        yield return new WaitForSeconds(5.0f);
        buffSpeed = neutralSpeed;
        _ReceivedBuffSpeed = false;
        movingCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("TreasureChest"))
        {
            cantMove = true;
            other.gameObject.SetActive(false);
            StartCoroutine(Digging());
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
