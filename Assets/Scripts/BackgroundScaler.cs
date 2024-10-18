using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    [SerializeField] SpriteRenderer _background;

    void Start()
    {
        ScaleBackground();
    }

    void ScaleBackground()
    {
        if (_background == null) {
            _background = GetComponent<SpriteRenderer>();
        }

        float width = _background.sprite.bounds.size.x;
        float height = _background.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        float scale = Mathf.Max(worldScreenWidth / width, worldScreenHeight / height);

        transform.localScale = new Vector3(scale, scale, 1);
    }
}
