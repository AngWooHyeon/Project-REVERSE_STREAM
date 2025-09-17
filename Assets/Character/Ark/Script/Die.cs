using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : MonoBehaviour
{
    public bool isDead = false;
    private PlayerHealth health;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
    }

    public void die()
    {
        Debug.Log("���� �Լ�");
        isDead = true;

        // ü�� ���� �� UI ����
        if (health.hpUI != null)
        {
            health.hpUI.SetHP((int)PlayerHealth.currentHP, (int)PlayerHealth.maxHP);
        }

        // �� ���� �� �ٽ� �ε� (���� ó�� ���·�)
        SceneManager.LoadScene("Tutorial");
    }
}

