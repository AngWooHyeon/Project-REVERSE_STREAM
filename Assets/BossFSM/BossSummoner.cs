using UnityEngine;

public class BossSummoner : MonoBehaviour
{
    [Header("Boss ����")]
    [SerializeField] private GameObject bossPrefab;   // ��ȯ�� ���� ������
    [SerializeField] private Transform spawnPoint;    // ������ ��Ÿ�� ��ġ

    [Header("�÷��̾� ����")]
    [SerializeField] private string playerTag = "Player";
    private bool playerInRange = false;
    private bool bossSpawned = false;

    void Update()
    {
        if (playerInRange && !bossSpawned)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SpawnBoss();
            }
        }
    }

    private void SpawnBoss()
    {
        if (bossPrefab != null && spawnPoint != null)
        {
            Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
            bossSpawned = true; // �ߺ� ��ȯ ����
        }
        else
        {
            Debug.LogWarning("BossSummoner: bossPrefab �Ǵ� spawnPoint�� �������� ����");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
        }
    }
}
