using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Boss1_Animation))]
[RequireComponent(typeof(Boss1_Coroutine))]
public class Boss1_BackDash : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] public Transform player;  // �ν����Ϳ��� �־ �ǰ�, ��� �ڵ� Ž��
    public Rigidbody2D rb { get; private set; }
    public Boss1_Coroutine coroutine { get; private set; }
    public Boss1_Animation ani { get; private set; }

    // MonoBehaviour�� �ƴ� ���ɼ� ������ GetComponent ����
    // public Boss1_Phase1 phase1;

    [Header("Params")]
    public float dashSpeed = 0.1f;

    // �ܺο��� �б� ��, �ݵ�� ����ǵ��� ������Ƽ�� ����
    public Transform Player
    {
        get
        {
            if (!player) TryResolvePlayer();
            return player;
        }
    }

    void Reset()
    {
        // �����Ϳ��� ������Ʈ �߰��� �� �ڵ� �ἱ
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Boss1_Animation>();
        coroutine = GetComponent<Boss1_Coroutine>();
        if (!player) TryResolvePlayer();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Boss1_Animation>();
        coroutine = GetComponent<Boss1_Coroutine>();

        // GetComponent<Boss1_Phase1>()�� ����(������Ʈ�� �ƴ� �� ����)
        // phase1�� FSM �� �Ŵ������� ���� ���Թ޾ƾ� ��.

        if (!player) TryResolvePlayer();
    }

    void OnEnable()
    {
        // ��Ȱ����Ȱ�� ��ȯ �ÿ��� ����
        if (!player) TryResolvePlayer();
    }

    void Start()
    {
        // �ٸ� ������Ʈ�� Awake���� �ڷ�ƾ�� �����ص� Start �������� ��κ� �غ��
        if (!player)
        {
            Debug.LogError("[Boss1_BackDash] Player ������ ã�� ���߽��ϴ�. Tag=Player �Ǵ� ���� �Ҵ��� Ȯ���ϼ���.", this);
        }
    }

    void TryResolvePlayer()
    {
        // 1����: Tag
        var go = GameObject.FindWithTag("Player");
        if (go) { player = go.transform; return; }

        // 2����: ������ �÷��̾� ��Ʈ�ѷ�/�ｺ ���� ��ǥ ������Ʈ�� Ž�� (�ִٸ� Ÿ�� �ٲ��ּ���)
        var pc = FindObjectOfType<PlayerHealth>(includeInactive: true);
        if (pc) { player = pc.transform; return; }
    }

    // �ܺ�(�ڷ�ƾ ����)���� �����ϰ� ȣ���� �� �ִ� ��ƿ
    public bool TryGetPlayer(out Transform t)
    {
        t = Player;
        return t != null;
    }
}
