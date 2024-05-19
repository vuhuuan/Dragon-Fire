using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed = 10;
    [SerializeField] float coolDownExist = 4;
    private float _typeSpeed;
    private int currentType = 1;

    private float coolDownExistCounter;

    private bool hit;
    private float direction;

    private Animator anim;

    private BoxCollider2D boxCollider;
    private float _damage;

    void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        coolDownExistCounter = coolDownExist;
        _typeSpeed = speed;
        SetType(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (hit) { return;  }
        transform.Translate(_typeSpeed * Time.deltaTime * direction, 0, 0);

        if (coolDownExistCounter <= 0)
        {
            Deactivate();
        } else
        {
            coolDownExistCounter -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            hit = true;
            boxCollider.enabled = false;
            transform.position = new Vector2(transform.position.x + transform.localScale.x/4.6f, transform.position.y + transform.localScale.y / 4.6f);
            anim.SetTrigger("explode");
        }

        if (other.tag == "Enemy")
        {
            if (other.GetComponent<Health>())
            {
                Debug.Log(_damage);
                other.GetComponent<Health>().TakeDamage(_damage);
            }
        }
    }

    // to call everytime we shot
    public void SetDirection(float _direction, int type)
    {
        coolDownExistCounter = coolDownExist;

        direction = _direction;
        gameObject.SetActive(true);
        SetType(type);

        if (currentType >= 3)
        {
            anim.SetBool("max", true);
        }
        else
        {
            anim.SetBool("max", false);
        }

        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;

        if (_direction != Mathf.Sign(localScaleX))
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void DeactivateAsType(int type)
    {
        if (type == currentType)
        {
            SetType(1);
            gameObject.SetActive(false);
        }
    }
    public void SetType(int type)
    {
        switch (type) 
        {
            case 1:
                currentType = 1;
                _damage = 1;
                transform.localScale = 0.6f * Vector3.one;
                _typeSpeed = speed;
                break;

            case 2:
                currentType = 2;
                _damage = 2;
                transform.localScale = 1.2f * Vector3.one;
                _typeSpeed = 1.2f * speed;
                break;

            case 3:
                currentType = 3;
                _damage = 4;
                transform.localScale = 1.5f * Vector3.one;
                _typeSpeed = 1.5f * speed;
                break;
        }   
    }
}
