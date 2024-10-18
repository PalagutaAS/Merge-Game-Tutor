using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Limiter : MonoBehaviour
{
    private bool _hasLimitCrossed = false;
    private List<int> _collisionStay = new();

    public static UnityEvent StandsInZoneLimit = new UnityEvent();
    public static UnityEvent OutInZoneLimit = new UnityEvent();

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ball ball))
        {
            _collisionStay.Remove(ball.gameObject.GetInstanceID());
            if (_hasLimitCrossed)
            {
                _hasLimitCrossed = (_collisionStay.Count != 0);
                if (!_hasLimitCrossed) {
                    OutInZoneLimit.Invoke();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ball ball))
        {
            _collisionStay.Add(ball.gameObject.GetInstanceID());
            if (!_hasLimitCrossed)
            {
                StandsInZoneLimit.Invoke();
                _hasLimitCrossed = (_collisionStay.Count != 0);
            }
        }
    }
}
