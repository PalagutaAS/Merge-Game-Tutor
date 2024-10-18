using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public enum Evolution
{
    Pluto,      //0
    Moon,       //1
    Mercury,    //2
    Mars,       //3
    Venus,      //4
    Earth,      //5
    Neptune,    //6
    Uran,       //7
    Saturn,     //8
    Jupiter,    //9
}
public class Ball : MonoBehaviour
{
    [SerializeField] private Evolution _evolution;
    [SerializeField] private ParticleSystem _particle;

    private Vector3 _previousPosition = Vector3.zero;
    private Rigidbody2D _rigid;
    private Collider2D _collider;
    private List<int> _collisionEnterWithOther = new();

    public static UnityEvent<Ball, Ball> OnMerge = new UnityEvent<Ball, Ball>();

    #region PROP
    public bool IsColliderEnabled { get => _collider.enabled; }
    public Evolution Evolution { get => _evolution; }
    public Rigidbody2D Rigidbody { get => _rigid; }
    #endregion

    #region PUBLIC STATIC

    public static Ball Instant(Ball prefabBall, Vector3 position, Transform parent)
    {
        GameObject createdBall = Instantiate(prefabBall.gameObject, position, Quaternion.Euler(new(0, 0, Random.Range(0f, 360f))), parent);
        return createdBall.GetComponent<Ball>();
    }

    public static Ball InstantiateDeactivated(Ball prefabBall, Vector3 position, Transform parent)
    {
        Ball ball = Instant(prefabBall, position, parent);
        ball.Deactivate();
        return ball;
    }

    #endregion

    private void Update()
    {
        if (IsColliderEnabled)
        {
            Vector3 velocity = (transform.position - _previousPosition) / Time.deltaTime;
            _previousPosition = transform.position;
        }
    }

    private void Awake()
    {
        if (_rigid == null)
        {
            _rigid = gameObject.GetComponent<Rigidbody2D>();
        }
        if (_collider == null)
        {
            _collider = gameObject.GetComponent<Collider2D>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsColliderEnabled && !_collisionEnterWithOther.Contains(collision.gameObject.GetInstanceID()) && collision.gameObject.TryGetComponent(out Ball ball))
        {
            if (_evolution == ball._evolution && ball.IsColliderEnabled)
            {
                StartMerge();
                ball.StartMerge();
                OnMerge.Invoke(this, ball);
                ball.Destroy();
                Destroy();
            } else {
                _collisionEnterWithOther.Add(collision.gameObject.GetInstanceID());
            }
        }
    }


    #region PUBLIC
    public void StartMerge() 
    {
        _collider.enabled = false;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void Activate() 
    {
        _collider.enabled = true;
        _rigid.isKinematic = false;
        int angle = Random.Range(500, 800);
        angle *= (Random.Range(0f, 1f) > 0.5f) ? -1 : 1;
        _rigid.AddTorque(angle);
        if (_particle)
        {
            _particle.Play();
        }
        
    }

    public void Deactivate() 
    {
        _rigid.isKinematic = true;
        _collider.enabled = false;
    }
    #endregion
}
