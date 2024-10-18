using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Ball[] balls;
    [SerializeField] Transform _parentForBalls;
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] float _heightSpawn;
    [SerializeField] float _horizontalLimit = 4f; 
    [SerializeField] float _forceThorw = 0f;
    [SerializeField] float _delayBeforeSpawn = 0.5f;

    private Dictionary<Evolution, Ball> ballsPrefab = new Dictionary<Evolution, Ball>();
    private Ball _createdBall;
    private bool _enabled = true;

    private void Awake()
    {
        if (!_parentForBalls)
        {
            throw new NullReferenceException("Parent for Balls is Required!");
        }
        Timer.IsOver.AddListener(StopSpawnAndThorw);
        Ball.OnMerge.AddListener(MergingAndCreate);
        CreateDictionary();
    }

    private void Start()
    {
        SetSpawnPosition();
        CreateBallForThrow();
    }

    private void CreateBallForThrow()
    {
        _createdBall = Ball.InstantiateDeactivated(GetRandomBall(), _spawnPoint.transform.position, _parentForBalls);
    }

    private void Update()
    {
        if (!_enabled) return;

        SetSpawnPosition();
        if (_createdBall)
        {
            _createdBall.transform.position = GetSpawnPosition();
        }

        if (Input.GetMouseButtonDown(0) && _createdBall) 
        {
            ThorwBall();
            Invoke(nameof(CreateBallForThrow), _delayBeforeSpawn);
        }
    }

    private void SetSpawnPosition() 
    {
        Vector3 newPosition = _spawnPoint.transform.position + Vector3.right * Input.GetAxis("Mouse X") * 0.5f;
        newPosition.x = Mathf.Clamp(newPosition.x, -_horizontalLimit, _horizontalLimit);
        newPosition.y = _heightSpawn;
        _spawnPoint.transform.position = newPosition;
    }
    
    private Vector3 GetSpawnPosition() 
    {
        return _spawnPoint.transform.position;
    }

    private void ThorwBall() 
    {
        _createdBall.Activate();
        _createdBall.Rigidbody.AddForce(Vector2.down * _forceThorw * (1 + (int)_createdBall.Evolution), ForceMode2D.Impulse);
        _createdBall = null;
    }

    private void CreateDictionary()
    {
        foreach (Ball item in balls)
        {
            ballsPrefab.Add(item.Evolution, item);
        }
    }

    private Ball GetBall(Evolution evolution) 
    {
        if (ballsPrefab.TryGetValue(evolution, out Ball prefabBall))
        {
            return prefabBall;
        }
        return GetBall(Evolution.Pluto);
    }

    private void MergingAndCreate(Ball ball1, Ball ball2)
    {
        Evolution currentEvolution = ball1.Evolution;
        Vector3 createPosition = (ball1.transform.position + ball2.transform.position) / 2;
        Vector2 averageVelocity = (ball1.Rigidbody.velocity + ball2.Rigidbody.velocity) / 4;
        Ball createdBall = Ball.Instant(GetBall(++currentEvolution), createPosition, _parentForBalls);
        createdBall.Rigidbody.velocity = averageVelocity;
    }

    public void StopSpawnAndThorw() 
    {
        CancelInvoke(nameof(CreateBallForThrow));
        if (_createdBall)
        {
            Destroy(_createdBall.gameObject);
            _createdBall = null;
        }
        _enabled = false;
    }
    private Ball GetRandomBall()
    {
        int count = Enum.GetValues(typeof(Evolution)).Length;
        return GetBall((Evolution)Random.Range(1, count - (count - 4)));
    }
}
