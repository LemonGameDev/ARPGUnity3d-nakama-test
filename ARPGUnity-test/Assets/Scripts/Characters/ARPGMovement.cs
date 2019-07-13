using System;
using System.Collections;
using Characters;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ARPGMovement : CharacterMovement, ISync<Vector3>
{
    private NavMeshAgent _navMeshAgent;
    private Coroutine _movementCoroutine;

    public float rotationSpeed = 20;
    [SerializeField] private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("Walking");
    private Rigidbody rb;
    private RotateToClick _rotateToClick;
    private Coroutine rotationCouroutine;
    
    

    private void Awake()
    {
        OnSendData = delegate { };
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        _rotateToClick = new RotateToClick(gameObject, rb);
    }

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (_rotateToClick != null)
        {
            _rotateToClick.Tick();
        }
    }

    public override void MovePlayer(Vector3 pos)
    {
        _animator.SetBool(Walking, true);
        if (rotationCouroutine != null)
        {
            StopCoroutine(rotationCouroutine);
            rotationCouroutine = null;
        }
        rotationCouroutine = StartCoroutine(_rotateToClick.SmoothRotation(pos, rotationSpeed));

        StartCoroutine(CheckTargetReached(pos));
        _navMeshAgent.SetDestination(pos);
        OnSendData(pos);
    }

    public override void MovePlayer(Vector2 axys)
    {
        throw new NotImplementedException();
    }

    private IEnumerator CheckTargetReached(Vector3 pos)
    {
        //var dist = _navMeshAgent.remainingDistance;
        while (true)
        {
            var pos1 = (transform.position - pos).magnitude;
            //Debug.Log("pos reached -> " + pos1);
            if (Math.Abs(pos1) < 0.1f)
            {
//                Debug.Log("TARGET REACHED!!!!!!");
                _animator.SetBool(Walking, false);
                yield break;
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator RotatePlayer(Vector3 pos)
    {
        float dotProd = 0;
        while (dotProd < 0.9)
        {
            var dirFromAtoB = (pos - transform.position).normalized;
            dotProd = Vector3.Dot(transform.forward, dirFromAtoB);

            if (dotProd < 0.9)
            {
                Vector3 dir = pos - transform.position;
                dir.y = 0; // keep the direction strictly horizontal
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
            }
            else
            {
                yield break;
            }

            yield return new WaitForSeconds(.001f);
        }
    }
    

    public override void ChangeMoveState(bool canMove)
    {
        CanMove = canMove;
        _navMeshAgent.isStopped = !CanMove;
    }

    IEnumerator MovePlayerToPoint(Vector3 pos)
    {
        yield return null;
    }

    public Action<Vector3> OnSendData { get; set; } = delegate(Vector3 vector3) {  };
    public void MoveFromServer(Vector3 pos)
    {
        MovePlayer(pos);
    }
}