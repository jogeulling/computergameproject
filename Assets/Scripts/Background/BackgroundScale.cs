using UnityEngine;

public class BackgroundScale : MonoBehaviour
{
    public SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.sr = GetComponent<SpriteRenderer>();
        float width = this.sr.sprite.bounds.size.x;
        float height = this.sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
