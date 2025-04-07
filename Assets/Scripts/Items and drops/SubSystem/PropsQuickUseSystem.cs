using TMPro;
using UniRx;
using UnityEngine;

/// <summary>
/// �D��ֱ��ϥΨt��
/// �t�d�N�X�譱���D��ֱ��ϥ�
/// </summary>
public class PropsQuickUseSystem : MonoSingleton<PropsQuickUseSystem>
{
    //�ֱ����
    public ItemName[] quickProps = new ItemName[4] { ItemName.None, ItemName.None, ItemName.None, ItemName.None};
    public TextMeshProUGUI[] probsText = new TextMeshProUGUI[4];

    [Header("�ֱ��I��")]
    private bool isQuickUseMode = false;
    private Vector2 startMousePos;
    [SerializeField]private int selectedIndex = -1; // ��e�襤���D�����
    public float cancelDistance = 100f; // ���ʶW�L�o�ӶZ���N�������

    [Header("����S��")]
    public GameObject quickPropsUI;

    public Color selectedColor = Color.yellow; // �襤�ɪ��r���C��
    public Color normalColor = Color.white; // ���襤�ɪ��r���C��

    private int previousSelectedIndex = -2; // �l�ܤW�@���襤������

    private void Start()
    {
        quickPropsUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartQuickUseMode();
        }

        if (isQuickUseMode)
        {
            UpdateSelection();
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            EndQuickUseMode();
        }
    }

    /// <summary>
    /// �]�m�ֱ����
    /// </summary>
    /// <param name="index"></param>
    /// <param name="itemName"></param>
    private void SetQuickProp(int index, ItemName itemName)
    {
        quickProps[index] = itemName;
    }

    private void StartQuickUseMode()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isQuickUseMode = true;
        startMousePos = Input.mousePosition; // �O�����U F �ɪ��ƹ���m
        selectedIndex = -1; // ���m���

        quickPropsUI.SetActive(true);
        UpdateTextInfo();
    }

    private void UpdateSelection()
    {
        Vector2 currentMousePos = Input.mousePosition;
        Vector2 direction = currentMousePos - startMousePos;
        float distance = direction.magnitude;

        if (distance > cancelDistance)
        {
            selectedIndex = -1; // �W�X�d��A�������
            UpdateTextEffects(); // ��s�r��S��
            previousSelectedIndex = selectedIndex; // ��s�W�@���襤������
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360; // �T�O���׬O 0~360

        if (distance > 30f) // �ƹ��ݭn���@�w���ʶq�~Ĳ�o���
        {
            if (angle >= 45 && angle < 135)
                Select(0); // �V�W
            else if ( angle >= 135 && angle < 225)
                Select(1); // �V��
            else if (angle >= 225 && angle < 315)
                Select(2); // �V�U
            else
                Select(3); // �V�k
        }

        // �ˬd�襤�����ެO�_����
        if (selectedIndex != previousSelectedIndex)
        {
            UpdateTextEffects(); // ��s�r��S��
            previousSelectedIndex = selectedIndex; // ��s�W�@���襤������
        }
    }

    private void UpdateTextEffects()
    {
        // ���m�Ҧ��r���C�⬰���`�C��
        for (int i = 0; i < probsText.Length; i++)
        {
            if (probsText[i] != null)
            {
                probsText[i].color = normalColor;
            }
        }

        // �襤����ܹ����r���C��
        if (selectedIndex >= 0 && selectedIndex < probsText.Length && probsText[selectedIndex] != null)
        {
            probsText[selectedIndex].color = selectedColor;
        }
    }

    private void Select(int index)
    {
        selectedIndex = index;
        
    }

    private void EndQuickUseMode()
    {
        isQuickUseMode = false;

        if (selectedIndex >= 0 && selectedIndex < quickProps.Length)
        {
            UseItem(quickProps[selectedIndex]); // �ϥο襤���D��
            UpdateTextInfo();
        }

        selectedIndex = -1; // ���m���

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        quickPropsUI.SetActive(false);
    }

    private void UseItem(ItemName item)
    {
        if (item == ItemName.None) return;
        Debug.Log($"�ϥιD��: {item}");
        // �b�o�̹�@�ϥιD�㪺�޿�

        ItemManager.Instance.ItemChoosed(item);
        ItemManager.Instance.ItemTrigger();
    }

    /// <summary>
    /// ��s�ֱ���餤����r
    /// </summary>
    private void UpdateTextInfo()
    {
        for(int i = 0; i < 4; i++)
        {
            //�w�]
            probsText[i].text = "[]";
            if (quickProps[i] == ItemName.None) {
                continue;
            }

            ItemData itemData = ItemManager.Instance.GetItemData(quickProps[i]);

            //�ˬd����ƬO�_�s�b
            if (itemData != null)
            {
                ItemName itemName = itemData.itemEnumName;
                bool isExist = InventoryManager.Instance.SearchItem(itemName);

                //�ˬd�Ӫ��~�b�I�]���O�_�s�b
                if (isExist == true) {
                    string info = itemData.itemName;
                    probsText[i].text = info;
                }
                else
                {
                    quickProps[i] = ItemName.None;
                }
            }
        }
    }

    #region event
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnEnable()
    {
        // ���U��  �ƥ󪺭q�\
        disposables.Add(EventManager.StartListening<int, ItemName>(
            NameOfEvent.SetQuickProp,
            (index, itemName) => SetQuickProp(index, itemName)
        ));
    }

    private void OnDisable()
    {
        disposables.Clear(); // �۰ʨ����Ҧ��ƥ�q�\
    }
    #endregion
}
