using UnityEngine;

public class AimLine : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteLine;
    void Awake()
    {
        if (!_spriteLine)
        {
            _spriteLine = GetComponent<SpriteRenderer>();
        }
        Timer.IsOver.AddListener(Unshow);
    }

    private void Unshow()
    {
        _spriteLine.enabled = false;
    }    
    private void Show()
    {
        _spriteLine.enabled = true;
    }
}
