using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    public float size = 1.0f;
    public float minSize = 0.0f;
    public float maxSize = 1.5f;
    public float speed = 20.0f;
    private GameManager GameManager;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private bool hit = false;
    

    private void Awake(){
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        GameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f); 
        this.transform.localScale = Vector3.one * this.size;

        _rigidbody.mass = this.size * 2;
    }

    public void SetTrajectory(Vector2 direction){
        _rigidbody.AddForce(direction * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet"){
            hit = true;
        }
        if (collision.gameObject.tag == "EnemyBullet"){
            GameManager.audioManager.Play("bonk");
        }
    }
    private void FixedUpdate(){
        if (hit){
            if((this.size - 0.5f) >= minSize){
                CreateSplit();
                CreateSplit();
            }
            
            GameManager.AsteroidDestroyed(this);
            Destroy(this.gameObject);
        }
    }

    private void Update() {
        if (Mathf.Abs(transform.position.y) > (GameManager.boundary.transform.localScale.y / 2) + 1.5f) {
            EmergencyWrapVertical();
        } 
        if (Mathf.Abs(transform.position.x) > (GameManager.boundary.transform.localScale.x / 2) + 1.5f) {
            EmergencyWrapHorizontal();
        } 
    }

    private void CreateSplit(){
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;
        half.SetTrajectory(-Random.insideUnitCircle.normalized * (this.speed));
        GameManager.IncrementAsteroidsAlive();
    }

    private void EmergencyWrapVertical(){
        transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
    }
    private void EmergencyWrapHorizontal(){
        transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
    }

}
