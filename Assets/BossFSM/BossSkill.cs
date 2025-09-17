using UnityEngine;

[System.Serializable]
public class BossSkill
{
    public string skillName;
    public System.Action skillAction;
    public float cooldown;
    public float lastUsedTime;  // �� 'ĳ��Ʈ ���� ��'�� ����
    public float castTime;

    public bool IsReady()
    {
        return Time.time >= lastUsedTime + cooldown;
    }

    public void Use()
    {
        skillAction?.Invoke();
        // lastUsedTime �� ���⼭ �������� �ʴ´�!
    }
}
