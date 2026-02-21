using UnityEngine;
using System.Collections;

public class MovementPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 5f;
    public float joystickRadius = 100f;

    [Header("Room Bounds")]
    [SerializeField] private RoomBounds roomBounds;
    [SerializeField] private float wallPadding = 0.5f;

    [Header("Combat")]
    public float knockbackForce = 6f;
    public float knockbackDuration = 0.12f;

    [Header("Dodge Roll")]
    [SerializeField] private float rollDistance = 3f;
    [SerializeField] private float rollDuration = 0.2f;
    [SerializeField] private float rollCooldown = 0.5f;

    private Rigidbody2D rb;

    private Vector2 touchStartPos;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;
    private bool isTouching;

    private bool isKnockedBack;
    private float knockbackTimer;
    private Vector2 knockbackVelocity;

    private bool isRolling;
    private bool canRoll = true;
    private int defaultLayer;
    private int rollingLayer = -1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultLayer = gameObject.layer;
        rollingLayer = LayerMask.NameToLayer("RollingPlayer");
        if (rollingLayer < 0)
            Debug.LogWarning("MovementPlayer: 'RollingPlayer' layer not found. Add it in Edit > Project Settings > Tags and Layers.");
    }

    void Update()
    {
        HandleInput();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryRoll();
        }
#endif
    }

    void FixedUpdate()
    {
        if (isKnockedBack)
        {
            rb.linearVelocity = knockbackVelocity;
            knockbackTimer -= Time.fixedDeltaTime;

            if (knockbackTimer <= 0f)
                isKnockedBack = false;

            return;
        }

        if (isRolling)
            return;

        rb.linearVelocity = moveInput * maxSpeed;
    }

    void HandleInput()
    {
#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif

        if (moveInput.sqrMagnitude > 0.01f)
            lastMoveDirection = moveInput.normalized;
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 0)
        {
            isTouching = false;
            moveInput = Vector2.zero;
            return;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            isTouching = true;
            touchStartPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            Vector2 delta = touch.position - touchStartPos;
            moveInput = Vector2.ClampMagnitude(delta / joystickRadius, 1f);
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isTouching = false;
            moveInput = Vector2.zero;
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isTouching = true;
            touchStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && isTouching)
        {
            Vector2 delta = (Vector2)Input.mousePosition - touchStartPos;
            moveInput = Vector2.ClampMagnitude(delta / joystickRadius, 1f);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isTouching = false;
            moveInput = Vector2.zero;
        }
    }

    void TryRoll()
    {
        if (!canRoll || isRolling || isKnockedBack)
            return;

        if (lastMoveDirection == Vector2.zero)
            return;

        StartCoroutine(DodgeRoll(lastMoveDirection));
    }

    IEnumerator DodgeRoll(Vector2 direction)
    {
        isRolling = true;
        canRoll = false;

        if (rollingLayer >= 0)
            gameObject.layer = rollingLayer;

        float elapsed = 0f;
        float speed = rollDistance / rollDuration;

        while (elapsed < rollDuration)
        {
            rb.linearVelocity = direction * speed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        if (rollingLayer >= 0)
            gameObject.layer = defaultLayer;
        isRolling = false;

        yield return new WaitForSeconds(rollCooldown);
        canRoll = true;
    }

    public void ApplyKnockback(Transform enemy)
    {
        if (isRolling) return;

        Vector2 direction = (transform.position - enemy.position).normalized;

        knockbackVelocity = direction * knockbackForce;
        knockbackTimer = knockbackDuration;
        isKnockedBack = true;
    }

    void LateUpdate()
    {
        if (roomBounds == null) return;

        float halfWidth = roomBounds.width / 2f;
        float halfHeight = roomBounds.height / 2f;

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(
            pos.x,
            roomBounds.transform.position.x - halfWidth + wallPadding,
            roomBounds.transform.position.x + halfWidth - wallPadding
        );

        pos.y = Mathf.Clamp(
            pos.y,
            roomBounds.transform.position.y - halfHeight + wallPadding,
            roomBounds.transform.position.y + halfHeight - wallPadding
        );

        transform.position = pos;
    }
}
