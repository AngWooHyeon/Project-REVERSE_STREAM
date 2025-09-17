using UnityEngine;

public class InventoryOpener : MonoBehaviour
{
    [SerializeField] private InventoryBackPanelAdapter adapter;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (adapter == null) return;

            // UINavigator ��å�� ���� ����/�ݱ�
            if (adapter.IsOpen)
            {
                adapter.Hide();
            }
            else
            {
                UINavigator.Instance?.CloseAllOverlaysExcept(adapter);
                adapter.Show(); // ���ο��� inv.OpenInventory() ȣ��
            }
        }
    }
}
