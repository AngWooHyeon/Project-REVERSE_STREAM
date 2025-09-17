using System.Collections;
using UnityEngine;

public class Boss1_Animation : MonoBehaviour 
{
    [Header("Animator")]
    public Animator ani;

    // ������ ����ϴ� "��� Ʈ����" ��� (Animator �Ķ���� �̸��� ��Ȯ�� ��ġ�ؾ� ��)
    [Header("All Trigger Names (Triggers only)")]
    [SerializeField] private string[] allTriggers = new string[] {
        "IsJump",
        "IsTop",
        "IsPrepareJump",
        "IsThrow",
        "IsCatch",
        "IsWheel",
        "WheelStart",
        "WheelEnd",
        "IsBackstep",
        "ISCHANGING",
        "Enthrow",
        "Dash",
        "Groggy",
        "GroggyEnd",
        "Nattack",
        "Nattackprepare"
    };

    private void Awake()
    {
        if (!ani) ani = GetComponent<Animator>();
    }

    // -------------- Ʈ���� ���� ���� --------------
    /// ��� Ʈ���� ����
    public void ResetAllTriggers()
    {
        if (!ani) return;
        for (int i = 0; i < allTriggers.Length; i++)
        {
            var t = allTriggers[i];
            if (!string.IsNullOrEmpty(t)) ani.ResetTrigger(t);
        }
    }

    /// ��� Ʈ���Ÿ� ����� ������ ���� ���� �� Set
    public void SetTriggerSafe(string trigger)
    {
        if (!ani || string.IsNullOrEmpty(trigger)) return;

        // �ٸ� ��� Ʈ���Ÿ� ���� ����
        for (int i = 0; i < allTriggers.Length; i++)
        {
            var t = allTriggers[i];
            if (!string.IsNullOrEmpty(t) && t != trigger)
                ani.ResetTrigger(t);
        }

        // ��� Ʈ���� ��ȭ
        ani.SetTrigger(trigger);
    }

    /// �� ������ �������� ����(Animator�� ���̸� ���� �ð� ����)
    public IEnumerator SetTriggerAndYield(string trigger)
    {
        SetTriggerSafe(trigger);
        yield return null; // 1������ ��� �� Ʈ���� �Һ� ��ȸ Ȯ��
    }
    // ---------------------------------------------

    // ===== ���⼭���� ���� �޼������ ��� ���� ���۷� ���� =====
    public void Jump()                 => SetTriggerSafe("IsJump");
    public void JumpAttack()           => SetTriggerSafe("IsTop");

    public void KeepGoing()            => ani.SetBool("KeepGoing", true);   // Bool�� �״�� ����
    public void KeepEnd()              => ani.SetBool("KeepGoing", false);

    public void PrepareJump()          => SetTriggerSafe("IsPrepareJump");
    public void ThrowBoomerang()       => SetTriggerSafe("IsThrow");
    public void CatchBoomerang()       => SetTriggerSafe("IsCatch");

    public void WheelPrepare()         => SetTriggerSafe("IsWheel");
    public void WheelStart()           => SetTriggerSafe("WheelStart");
    public void WheelEnd()             => SetTriggerSafe("WheelEnd");

    public void BackStep()             => SetTriggerSafe("IsBackstep");
    public void PhaseChange()          => SetTriggerSafe("ISCHANGING");

    public void EnThrow()              => SetTriggerSafe("Enthrow");
    public void Dash()                 => SetTriggerSafe("Dash");

    public void Groggy()               => SetTriggerSafe("Groggy");
    public void GroggyEnd()            => SetTriggerSafe("GroggyEnd");

    public void Nattack()              => SetTriggerSafe("Nattack");
    public void Nattackprepare()       => SetTriggerSafe("Nattackprepare");
}
