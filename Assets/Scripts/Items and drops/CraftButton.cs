using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���b��X�����~���s�W���}��
/// �Ω�Ĳ�o�X���Ӫ��~���\��
/// </summary>
public class CraftButton : MonoBehaviour
{
    //���~���
    public ItemData itemData;

    /// <summary>
    /// Ĳ�o�X��
    /// </summary>
    public void Craft()
    {
        Debug.Log($"CraftButton: try crafting {itemData.itemEnumName}");
        CraftManager.Instance.TryCraft(itemData.itemEnumName);
    }
}

