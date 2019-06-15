using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EnemyBasicController : MonoBehaviour
{

    [SerializeField] bool canMove = true;
    [SerializeField] bool isMele  = true;
    [Range(0, 5f)] [SerializeField] float enemySpeed = 2.5f;
    [SerializeField] GameObject arrowPrefab;
    
    Transform arrowPosition;
    GameObject player;
    SpriteRenderer spriteRenderer;
    BoxCollider2D collider;
    Animator animator;
    
    AudioClip arrowClip;
    AudioHelper audioHelper;
    
    bool canGoForward  = false;
    bool playerInRange = false;
    bool isAttacking   = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        if (!isMele)
        {
            arrowPosition = gameObject.transform.GetChild(0);
            arrowClip = Resources.Load<AudioClip>("Sounds/arrowHit");
            audioHelper  = GetComponent<AudioHelper>(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        FlipSprite();
        UpdateStatus();
        Move();
    }

    void FlipSprite()
    {
        if (isAttacking)
        {
            //Do not flip the enemy 
            return;
        }
        
        if (player.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    
    void Move()
    {
        if (canGoForward)
        {
            transform.position += Vector3.right * enemySpeed * (spriteRenderer.flipX ? 1 : -1) * Time.deltaTime;
        }

        if (playerInRange && !isAttacking)
        {
            InitAttack();
        }
        
        animator.SetBool("Walking", canGoForward);
    }

    void UpdateStatus()
    {
        if (isAttacking)
        {
            //If attacking, the enemy should stay quiet
            canGoForward  = false;
            playerInRange = false;
            return;
        }
        
        if (CheckForPlayerCollision())
        {
            playerInRange = true;
            canGoForward = false;
        }
        //If he can attack the play he will not move
        else
        {
            canGoForward = !CheckForWorldCollision();
        }
        
        if (!canMove)
        {
            canGoForward = false; //By default, he can't move
        }
    }

    bool CheckForPlayerCollision()
    {
        float range = collider.size.x;
        Vector3 direction = spriteRenderer.flipX ? Vector2.right : Vector2.left;
        if (isMele)
        {
            return CheckForCollision(collider.transform.position, direction, range, (1 << 11));
        }
        else
        {
            direction = player.transform.position - transform.position;
            range = direction.magnitude;
            RaycastHit2D hit = Physics2D.Raycast(collider.transform.position, direction, range, (1 << 11) | (1 << 9) | (1 << 12));
            Debug.DrawRay(collider.transform.position, direction);
            return hit.collider != null && hit.collider.tag == "Player";
        }
    }
    
    bool CheckForWorldCollision()
    {
        Vector3 pos = collider.transform.position;
        if (CheckForCollision(pos, spriteRenderer.flipX ? Vector2.right : Vector2.left, collider.size.x,
            (1 << 9) | (1 << 12)))
        {
            return true;
        } else
        {
            pos += new Vector3(collider.size.x / 4 * (spriteRenderer.flipX ? 1 : -1), 0, 0);
            return !(CheckForCollision(pos, Vector3.down, collider.size.y,
                (1 << 9) | (1 << 12)));
        }
    }

    bool CheckForCollision(Vector3 position, Vector3 direction, float size, LayerMask mask)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction, size, mask);
        return hit.collider != null;
    }

    void InitAttack()
    {
        isAttacking = true;
        animator.SetBool("Attacking", isAttacking);
    }

    public void Attack()
    {
        if (CheckForPlayerCollision() && player != null && isMele)
        {
            player.GetComponent<PlayerHealthController>().KillPlayer(false);
        } else if (!isMele && arrowPosition != null)
        {
            var dir = player.transform.position - arrowPosition.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Instantiate(arrowPrefab, arrowPosition.position, rotation);
            if (audioHelper != null)
            {
                audioHelper.PlaySound(arrowClip);
            }
        }
    }
     
    public void StopAttacking()
    {
        isAttacking = false;
        animator.SetBool("Attacking", isAttacking);
    }
}
