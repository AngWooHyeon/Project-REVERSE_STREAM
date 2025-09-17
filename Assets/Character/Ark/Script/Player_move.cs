using System.Xml.Serialization;
using UnityEngine;
using System.Collections;


public class Player_move : MonoBehaviour
{

    [Header("�̵� & �׼�")]
    [SerializeField] public float speed = 3f;               // �ȱ� �ӵ�
    [SerializeField] private float jumpForce = 7f;           // ���� ��

    [Header("���� �̵� ���")]
    [SerializeField] private float attackLockExtraDrag = 8f;

    private bool attackLocked = false;
    private float defaultDrag;

    // �ܺο��� �ٶ󺸴� ���� �ʿ��ϴ� ���� Getter �߰�
    public int FacingDir => facingDir;    

    // ===== ���� �÷��� =====
    [SerializeField] public int jumpCount = 0;
    private int facingDir = 1;
    public static float speedUP = 1;
    private const int maxJumps = 2;
    private float moveInput;
    public float dashDuration = 0.25f;
    public float dashSpeed = 3f;
    public float dashCooldown = 5.0f; // ��Ÿ�� ����
    private Vector2 dashInputDir;
    public bool isjump = false;
    
    public DashSkill Dash => dash;

    // �����ȡ��� �ִ� �⺻ �̵��ӵ� (��: ����, ���� ����Ʈ)
    [SerializeField] private float statMoveSpeed = 3f;

    // ���/�������� �ִ� ���ʽ� (��: �Ź�, ���� ��)
    private float equipmentSpeedBonus = 0f;

    // ����(����) ��Ƽ�ö��̾�
    private float buffSpeedMultiplier = 1f;

    // �����(����) ��Ƽ�ö��̾�
    private float debuffSpeedMultiplier = 1f;

    public float finalSpeed = 0f;

    public float CurrentMoveSpeed => CalculateCurrentSpeed();

    // ����� ���� �ڷ�ƾ ���۷���
    private Coroutine _speedRecoverRoutine;
    private Coroutine _animationSlowRoutine;
    private Coroutine _stepRoutine;

    // ===== ���ο��� ����� ���� =====
    private PlayerAnimationSync sync;
    private Rigidbody2D rb;
    public DashSkill dash;
    private Ground ground;
    private PlayerHealth health;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sync = GetComponentInParent<PlayerAnimationSync>();
        dash = GetComponent<DashSkill>();
        ground = GetComponentInParent<Ground>();
        health = GetComponentInParent<PlayerHealth>();
        defaultDrag = rb.linearDamping;
    }
    private void Update()
    {
        //Debug.Log(rb.linearVelocity);
        HandleMovementAndJump();
    }

    private void FixedUpdate()
    {
        // ���� ��� ���¸� x�ӵ� ���� 0(�̲����� ����)
        if (attackLocked)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        if (!dash.IsDashing && !health.isknockback)
        {
            finalSpeed = CalculateCurrentSpeed();
            rb.linearVelocity = new Vector2(moveInput * finalSpeed, rb.linearVelocity.y);
        }
    }

    private void HandleMovementAndJump()
    {
        // �̵� ���� �Է� (�¿� �̵���)
        moveInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1f;

        // �뽬 ���� �Է� (8����)
        dashInputDir = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow)) dashInputDir.y += 1f;
        if (Input.GetKey(KeyCode.DownArrow)) dashInputDir.y -= 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) dashInputDir.x -= 1f;
        if (Input.GetKey(KeyCode.RightArrow)) dashInputDir.x += 1f;

        dashInputDir = dashInputDir.normalized;

        if (Input.GetKeyDown(KeyCode.D))
        {
            //dash.TryDash(dashInputDir, rb, animController);
            dash.TryDash(dashInputDir, rb);
            //StartCoroutine(IgnoreCollider(0.2f));
        }

        // ĳ���� �ٶ󺸴� ���� ������Ʈ
        if (moveInput != 0f)
        {
            facingDir = (int)Mathf.Sign(moveInput);
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * facingDir;
            transform.localScale = scale;
        }

        // �ִϸ��̼� ó��
        sync.AirSpeedY(rb.linearVelocity.y);
        sync.IsWalking(moveInput != 0f);

        // ����
        //if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        //{
        //    sync.Jump();
        //    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        //    ground.isGroundedNow = false;
        //    isjump = true;
        //    jumpCount++;
        //    StartCoroutine(Jump());
        //    StartCoroutine(JumpMooshi(0.5f));
        //}
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Space) && ground.floatGround)
            {
                //StartCoroutine(IgnoreCollider(0.5f));
            }
            else
            {
                Debug.Log("kkkkkk");
                sync.Jump();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                ground.isGroundedNow = false;
                isjump = true;
                jumpCount++;
                StartCoroutine(Jump());
                //StartCoroutine(IgnoreCollider(0.4f));
            }

        }
    }
    public bool IsInvincible()
    {
        return dash.IsInvincible;
    }

    public void ResetJumpCount()
    {
        jumpCount = 0;
        //Debug.Log("Player_move: ���� ī��Ʈ �ʱ�ȭ");
    }
    private float CalculateCurrentSpeed()
    {
        float basePlusEquip = speed + statMoveSpeed + equipmentSpeedBonus;
        return basePlusEquip * buffSpeedMultiplier * debuffSpeedMultiplier * speedUP;
    }

    /// <summary>
    /// ���� ��� ���� �ִϸ��̼��� ���� ������ �ӵ��� ������ �����մϴ�.
    /// </summary>
    public void AttackSpeedDownDuringAnimation(float slowRatio = 0.1f)
    {
        // �̹� ���� ���� �ڷ�ƾ�� ������ �ߴ�
        if (_animationSlowRoutine != null)
            StopCoroutine(_animationSlowRoutine);

        // �ӵ� ������ ����
        debuffSpeedMultiplier = slowRatio;

        // ���� ��� ���� �ִϸ��̼� State�� �ؽ� ����
        AnimatorStateInfo stateInfo = sync.CurrentStateInfo;
        int currentStateHash = stateInfo.shortNameHash;

        // �ش� State�� ���� ������ ���� �ڷ�ƾ ����
        _animationSlowRoutine = StartCoroutine(RecoverAfterAnimationEnd(currentStateHash));
    }

    public void SetAttackLock(bool locked)
    {
        attackLocked = locked;
        if (locked)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            rb.linearDamping = attackLockExtraDrag;   // �ٴڿ��� �̲����� ����
        }
        else
        {
            rb.linearDamping = defaultDrag;
        }
    }

    public void StepForward(float distance, float duration, AnimationCurve curve = null)
    {
        if (_stepRoutine != null) StopCoroutine(_stepRoutine);
        _stepRoutine = StartCoroutine(StepRoutine(distance, duration, curve));
    }

    private IEnumerator StepRoutine(float distance, float duration, AnimationCurve curve)
    {
        // �ٶ󺸴� ���� �ݿ�
        float signedDistance = distance * Mathf.Sign(FacingDir);
        Vector2 start = rb.position;
        Vector2 target = start + new Vector2(signedDistance, 0f);

        float t = 0f;
        while (t < duration)
        {
            t += Time.fixedDeltaTime;
            float lerp = duration > 0f ? Mathf.Clamp01(t / duration) : 1f;
            float eased = (curve != null) ? curve.Evaluate(lerp) : lerp;
            Vector2 next = Vector2.Lerp(start, target, eased);
            rb.MovePosition(next);
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(target);
        _stepRoutine = null;
    }

    private IEnumerator RecoverAfterAnimationEnd(int stateHash)
    {
        // ù ������ �ǳʶٱ�
        yield return null;

        // ���� State�� ������ �ʾ����� ��� ���
        while (sync.CurrentStateInfo.shortNameHash == stateHash
               && sync.CurrentStateInfo.normalizedTime < 1f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        // �ִϸ��̼� ���� ������ �ӵ� ����
        debuffSpeedMultiplier = 1f;
        _animationSlowRoutine = null;
    }

    private IEnumerator Jump()
    {
        yield return null;
        isjump = false;
    }

    private IEnumerator IgnoreCollider(float ignoretime)
    {
        CircleCollider2D parentCollider = GetComponentInParent<CircleCollider2D>();
        parentCollider.enabled = false;
        yield return new WaitForSeconds(ignoretime);
        parentCollider.enabled = true;
    }
}



