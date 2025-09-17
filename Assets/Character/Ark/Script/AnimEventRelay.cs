using UnityEngine;

public class AnimEventRelay : MonoBehaviour
{
    [Tooltip("�� ������Ʈ�� �̺�Ʈ �ҽ��� ���� (Body�� üũ)")]
    [SerializeField] private bool isEventSource = true;

    private IAttackAnimEvents receiver;

    void Awake()
    {
        // �θ� Ʈ���� �ִ� ������(�θ� ��ũ��Ʈ) ã��
        receiver = GetComponentInParent<IAttackAnimEvents>();
        if (receiver == null)
            Debug.LogWarning("[AnimEventRelay] �������� IAttackAnimEvents ã�� ����.", this);
    }

    // === �ִϸ��̼� �̺�Ʈ���� ���� ȣ���� �޼���� ===
    // Attack 1/2 Ŭ���� Ÿ�� �����ӿ��� ȣ�� (int ����: 1 or 2)
    public void AE_Step(int index)
    {
        if (!isEventSource) return;
        receiver?.OnStep(index);
    }

    // ����/��ų �� �����ӿ��� ȣ��
    public void AE_AttackEnd()
    {
        if (!isEventSource) return;
        receiver?.OnAttackEnd();
    }

    // ��ų ���� �����ӿ��� ȣ��(�ʿ��� ����)
    public void AE_SkillStart()
    {
        if (!isEventSource) return;
        receiver?.OnSkillStart();
    }

    // ��ų ���� �����ӿ��� ȣ��
    public void AE_SkillEnd()
    {
        if (!isEventSource) return;
        receiver?.OnSkillEnd();
    }
}
