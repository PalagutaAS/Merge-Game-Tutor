using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerUI;
    [SerializeField] private Animator _anim;
    [SerializeField] private float _timer = 8f;
    [SerializeField] private float _warning = 5f;

    private bool _isStart = false;
    private bool _isShowTimerWarning = false;
    private bool _isOverTime = false;
    private float _time = 0;

    public static UnityEvent IsOver = new UnityEvent();

    private void Awake()
    {
        if (_timerUI == null)
        {
            _timerUI = GetComponent<TextMeshProUGUI>();
        }       
        if (_anim == null)
        {
            _anim = GetComponent<Animator>();
        }
        _timerUI.enabled = false;
        _anim.enabled = false;
        Limiter.StandsInZoneLimit.AddListener(StartTimer);
        Limiter.OutInZoneLimit.AddListener(CloseTimer);
    }

    private void Update()
    {
        if (!_isStart) return;

        _time -= Time.deltaTime;
        if (_time <= _warning && !_isShowTimerWarning) 
        {
            ShowWarning();
        }
        if (_time <= 0 && !_isOverTime)
        {
            EndTimer();
        }
        
    }

    private void EndTimer() 
    {
        IsOver.Invoke();
        _timerUI.text = "0:00";
        _isOverTime = true;
        _time = 0;
        Invoke(nameof(UnshowWarning), 1.5f);
    }

    private void CloseTimer()
    {
        _isStart = false;
        UnshowWarning();
    }

    private void ResetTimer()
    {
        _isStart = false;
        _isShowTimerWarning = false;
        _isOverTime = false;
        _time = 0;
    }

    private void ShowWarning()
    {
        _timerUI.text = ConvertTimeToString(_time);
        _timerUI.enabled = true;
        _anim.enabled = true;
        _isShowTimerWarning = true;
    }

    private void StartTimer()
    {
        _isStart = true;
        _time = _timer;
    }

    private void UnshowWarning()
    {
        _timerUI.enabled = false;
        _anim.enabled = false;
        ResetTimer();
    }

    private void FixedUpdate()
    {
        if (_isShowTimerWarning && !_isOverTime)
        {
            _timerUI.text = ConvertTimeToString(_time);
        }
    }

    private string ConvertTimeToString(float time)
    {
        string sec = $"{(int)time}";
        int mSec = (int)((time * 100) % 100);
        string ms = (mSec < 10) ? "0" + $"{mSec}" : $"{mSec}";
        return $"{sec}:{ms}";
    }
}
