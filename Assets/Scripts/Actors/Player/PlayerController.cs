using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputController))]
public class PlayerController : HealthBase
{
    private InputController inp;
    private PlayerManager mgr;

    [Header("Animations")]
    [SerializeField] private Transform[] eyes;
    [SerializeField] private Transform hand;
    [SerializeField] private float handStart;
    [SerializeField] private float handEnd;
    [SerializeField] private float handTime;
    [SerializeField] private float deathTime = 3;
    [SerializeField] private ParticleSystem deathParticles;

    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float secondJumpForce;
    private int wallJumps;
    private float jumpEnd;

    [Header("Combat")]
    [SerializeField] private GameObject bulletPrefab;
    private int firedBullets;
    [SerializeField] private float collisionLaunchForce;

    [Header("Colliders")]
    [SerializeField] private float colliderHeight;
    [SerializeField] private float colliderWidth;
    private bool bottom, left, right;
    [SerializeField] private PhysicsMaterial2D slippery;
    [SerializeField] private float wallJumpGrav;
    private float grav;

    [Header("Interactions")]
    private DirectInteraction currInteraction;
    [HideInInspector] public int totalInteractions = 0;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip hurtClip;

    private Rigidbody2D rb;
    private AudioSource otherAudio;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, -colliderHeight / 2), new Vector2(colliderWidth, colliderHeight));
        Gizmos.DrawWireCube(transform.position + new Vector3(-0.625f - colliderHeight / 2, 0.5f), new Vector2(colliderHeight, colliderWidth));
        Gizmos.DrawWireCube(transform.position + new Vector3(0.625f + colliderHeight / 2, 0.5f), new Vector2(colliderHeight, colliderWidth));
    }

    protected override void Awake()
    {
        inp = GetComponent<InputController>();
        rb = GetComponent<Rigidbody2D>();
        grav = rb.gravityScale;
        otherAudio = transform.GetChild(0).GetComponent<AudioSource>();
    }

    private void Start()
    {
        mgr = PlayerManager.Instance;
        RefillHealth();
        CameraController.instance.SetFollowTarget(transform);
    }

    public void RefillHealth()
    {
        currHealth = (int)mgr.upgrades[UpgradeType.health];
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == Constants.GroundLayer)
        {
            rb.gravityScale = (left && inp.left || right && inp.right) && rb.velocity.y < 0 && wallJumps > 0 ? wallJumpGrav : grav;
            
            rb.sharedMaterial = slippery;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case Constants.InteractiveLayer:
                currInteraction = collision.GetComponentInParent<DirectInteraction>();
                if (currInteraction.CanInteract())
                    LevelUI.Instance.SetInteractTarget(collision.transform);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case Constants.InteractiveLayer:
                currInteraction = null;
                LevelUI.Instance.SetInteractTarget(null);
                break;
        }
    }

    void Update()
    {
        CheckColls();

        if (!GameController.Instance.movementPaused)
        {
            UpdateMovement();
            UpdateInput();
        }

        LevelUI.Instance.SetHandCount((int)mgr.upgrades[UpgradeType.hand] - totalInteractions);
        LevelUI.Instance.SetSpitballCount((int)mgr.upgrades[UpgradeType.spitballCount] - firedBullets);
        LevelUI.Instance.SetHealthCount(currHealth);
    }
    private void CheckColls()
    {
        bottom = Physics2D.OverlapBox((Vector2)transform.position + new Vector2(0, -colliderHeight / 2),
                                      new Vector2(colliderWidth, colliderHeight),
                                      0, 1 << Constants.GroundLayer) != null;

        if (bottom)
            wallJumps = mgr.upgrades[UpgradeType.jumpCount].unlocked ? (int)mgr.upgrades[UpgradeType.jumpCount] : 0;

        left = Physics2D.OverlapBox((Vector2)transform.position + new Vector2(-0.625f - colliderHeight / 2, 0.5f),
                                      new Vector2(colliderHeight, colliderWidth),
                                      0, 1 << Constants.GroundLayer) != null;

        right = Physics2D.OverlapBox((Vector2)transform.position + new Vector2(0.625f + colliderHeight / 2, 0.5f),
                                      new Vector2(colliderHeight, colliderWidth),
                                      0, 1 << Constants.GroundLayer) != null;
    }

    private void UpdateMovement()
    {
        float x = 0;
        if (inp.left) x -= 1;
        if (inp.right) x += 1;

        //if (x != 0 && !movementAudio.isPlaying)
        //    movementAudio.Play();

        rb.velocity = new Vector2(x * mgr.upgrades[UpgradeType.moveSpeed], rb.velocity.y);

        if (inp.jump.down) 
        {
            if (!bottom)
            {
                if (wallJumps > 0 && (left && inp.left || right && inp.right))
                    wallJumps--;
                else
                    return;
            }

            otherAudio.clip = jumpClip;
            otherAudio.Play();
            rb.velocity = new Vector2();
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpEnd = Time.time + mgr.upgrades[UpgradeType.jumpLength];
        }
        else if (inp.jump && Time.time < jumpEnd)
        {
            rb.AddForce(new Vector2(0, secondJumpForce) * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    private void UpdateInput()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (Transform t in eyes) 
        {
            Vector2 eyeDir = (Vector2)t.position - mousePos;
            t.localEulerAngles = new Vector3(0, 0, -Vector2.SignedAngle(eyeDir, Vector2.right));
        }

        if (GameController.Instance.paused)
            return;

        if (inp.interact.down && currInteraction)
        {
            if (currInteraction.requiresHand)
            {
                if (mgr.upgrades[UpgradeType.hand].unlocked &&
                    totalInteractions < mgr.upgrades[UpgradeType.hand])
                {
                    if (currInteraction.Interact(transform))
                    {
                        totalInteractions++;
                        StartCoroutine(HandAnim());
                    }
                }
            }
            else
            {
                currInteraction.Interact(transform);
                StartCoroutine(HandAnim());
            }
        }

        if (inp.fire.down && mgr.upgrades[UpgradeType.spitballCount].unlocked)
        {
            if (firedBullets < mgr.upgrades[UpgradeType.spitballCount])
            {
                Fire(mousePos - ((Vector2)transform.position + new Vector2(0, 0.5f)));
                firedBullets++;
            }
        }
    }

    private void Fire(Vector2 target)
    {
        otherAudio.clip = shootClip;
        otherAudio.Play();
        GameObject obj = Instantiate(bulletPrefab);
        obj.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        obj.GetComponent<Bullet>().Fire(target);
    }

    private IEnumerator HandAnim()
    {
        hand.gameObject.SetActive(true);

        float side = currInteraction.transform.position.x < transform.position.x ? 1 : -1;

        float startTime = Time.time;
        float t = 0;
        while (t < 1)
        {
            t = (Time.time - startTime) / handTime;

            hand.localEulerAngles = new Vector3(0, 0, side * (handStart + (handEnd - handStart) * t));

            yield return new WaitForEndOfFrame();
        }

        hand.gameObject.SetActive(false);
    }

    protected override bool OnHit(int damage)
    {
        if (GameController.Instance.movementPaused)
            return false;

        otherAudio.clip = hurtClip;
        otherAudio.Play();
        rb.AddForce(Vector2.up * collisionLaunchForce, ForceMode2D.Impulse);

        return base.OnHit(damage);
    }

    protected override void Death()
    {
        foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
            s.gameObject.SetActive(false);
        Invoke("EndLevel", deathTime);
        GameController.Instance.SetMovement(true);
        rb.velocity = new Vector2();
        deathParticles.Play();
    }

    private void EndLevel()
    {
        GameController.Instance.EndLevel();
    }
}
