using UniRx;
using UnityEngine;

/// <summary>
/// �t�d��ӭI�]������
/// </summary>
public class PackageUIController : MonoBehaviour
{
    public GameObject ItemUI;
    public GameObject WeaponUI;
    public GameObject CollectionUI;
    public GameObject BackMask;

    /// <summary>
    /// ���}�D�㤶��
    /// </summary>
    public void Item()
    {
        //�ҥΫ��wUI
        if (ItemUI == null)
        {
            Debug.LogError("Backpack UI is not assigned!");
            return;
        }


        ItemUI.SetActive(true); // �����I�]��ܪ��A
        
        //��sitem��T
        EventManager.TriggerEvent(NameOfEvent.UpdateItem);

        BackMask.SetActive(true);

        // ��I�]�}�ҮɡA�Ѱ���w�ƹ�
        Debug.Log(ItemUI.activeSelf);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //PlayerManager.Instance.playerStatus = PlayerStatus.ui; // ��s���a���A�� UI �Ҧ��A�ڥu�O�q�A�i��|�o�˼g�A�n���n�H�K�A.jpg

        // ��I�]�����ɡA���s��w�ƹ�
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //PlayerManager.Instance.playerStatus = PlayerStatus.move; // ��_�첾�ʼҦ��A�ڥu�O�q�A�i��|�o�˼g�A�n���n�H�K�A.jpg

    }

    /// <summary>
    /// ���W�D�㤶��
    /// </summary>
    public void CloseUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // �����I�]��ܪ��A
        BackMask.SetActive(false);
        ItemUI.SetActive(false);
        //WeaponUI.SetActive(false);
        //CollectionUI.SetActive(false);
        //Debug.Log("test2");
    }

    #region event
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnEnable()
    {
        // ���U��  �ƥ󪺭q�\
        disposables.Add(EventManager.StartListening(
            NameOfEvent.OpenItemPage,
            () => Item()
        ));

        disposables.Add(EventManager.StartListening(
            NameOfEvent.CloseUI,
            () => CloseUI()
        ));
    }

    private void OnDisable()
    {
        disposables.Clear(); // �۰ʨ����Ҧ��ƥ�q�\
    }
    #endregion
}
