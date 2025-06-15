using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEditor;
#if UNITY_EDITOR
using static UnityEditor.Progress; // ������ ���� �ڵ�
#endif


namespace Controller
{
    public class Machine : MonoBehaviour
    {
        private ThirdPersonCamera cameraScript;
        private BlockController machineController;

        //�� ��ġ
        public GameObject handPos;

        //����
        [Header("items")]
        public GameObject pt_thread; //��Ʈ��

        public GameObject breakGlass; //�� ����
        public GameObject breakPlastic; //�� �ö�ƽ
        public GameObject breakCan; //�� ĵ

        public GameObject moltenGlass; //���� ����
        public GameObject moltenPlastic; //���� �ö�ƽ
        public GameObject aluminum;//�˷�̴�

        public GameObject compressedPaper; //��������

        //������ UI
        [Header("machine")]
        public GameObject loading_ui;//���ۺ��� �ε� UI
        public GameObject finish_ui; //��� �ϼ� UI
        public TextMeshProUGUI countText; //��Ʈ ���� Ȯ�� UI

        //�м�� UI
        [Header("breaker")]
        public GameObject B_loading_ui;//���ۺ��� �ε� UI
        public GameObject B_finish_ui; //��� �ϼ� UI
        public TextMeshProUGUI breaker_countText_glass; //�м�� �ܷ� UI
        public TextMeshProUGUI breaker_countText_plastic; //�м�� �ܷ� UI
        public TextMeshProUGUI breaker_countText_can; //�м�� �ܷ� UI

        //�뱤�� UI
        [Header("Blast Furnace")]
        public GameObject BF_loading_ui;//���ۺ��� �ε� UI
        public GameObject BF_finish_ui;//��� �ϼ� UI
        public TextMeshProUGUI BF_countText_glass; //�뱤�� �ܷ� UI
        public TextMeshProUGUI BF_countText_plastic; //�뱤�� �ܷ� UI
        public TextMeshProUGUI BF_countText_can; //�뱤�� �ܷ� UI

        //����� UI
        [Header("Compressor")]
        public GameObject C_loading_ui;//���ۺ��� �ε� UI
        public GameObject C_finish_ui; //��� �ϼ� UI
        public TextMeshProUGUI C_countText; //��Ʈ ���� Ȯ�� UI

        //��� ������
        private int machine = 10;
        private int breaker = 45;
        private int blastFurnace = 15;
        private int compressor = 10;

        //������ ��� ����
        private int pt_deleteCount = 0; //��Ʈ ���� Ȯ��
        private bool ptthread = false; //��Ʈ�� �ϼ� ����

        //�м�� ��� ����
        private int glass_deleteCount = 0; //���� ���� Ȯ��
        private int plastic_deleteCount = 0;//�ö�ƽ ���� Ȯ��
        private int can_deleteCount = 0; //ĵ ���� Ȯ��
        private bool glass_break = false; //�� ���� �ϼ� ����
        private bool plastic_break = false; //�� �ö�ƽ �ϼ� ����
        private bool can_break = false; //�� ĵ �ϼ� ����

        //�뱤�� ��� ����
        private int b_glass_deleteCount = 0; //�� ���� ���� Ȯ��
        private int b_plastic_deleteCount = 0;//�� �ö�ƽ ���� Ȯ��
        private int b_can_deleteCount = 0; //�� ĵ ���� Ȯ��
        private bool glass_molten = false; //���� ���� �ϼ� ����
        private bool plastic_molten = false; //���� �ö�ƽ �ϼ� ����
        private bool can_molten = false; //�˷�̴� �ϼ� ����

        //����� ��� ����
        private int paper_deleteCount = 0; //���� ���� Ȯ��
        private bool compressed_paper = false; //�������� �ϼ� ����

        // �������������ͼ��͡�����������������
        // 1. ������ ����
        //������
        public int GetMachineDurability() => machine; 
        public void SetMachineDurability(int value) { machine = value; UpdateText(); } 
        //�м��
        public int GetBreakerDurability() => breaker;
        public void SetBreakerDurability(int value) { breaker = value; UpdateText(); }
        //�뱤��
        public int GetBlastFurnaceDurability() => blastFurnace;
        public void SetBlastFurnaceDurability(int value) { blastFurnace = value; UpdateText(); }
        //�����
        public int GetCompressorDurability() => compressor;
        public void SetCompressorDurability(int value) { compressor = value; UpdateText(); }

        // 2. ������ ����
        //��Ʈ ����
        public int GetPtDeleteCount() => pt_deleteCount; 
        public void SetPtDeleteCount(int value) { pt_deleteCount = value; UpdateText(); }
        //�ϼ� ����
        public bool IsPtThreadCompleted() => ptthread;
        public void SetPtThreadCompletion(bool value) { ptthread = value; UpdateText(); }

        // 3. �м�� ��� ����
        //���� ����
        public int GetGlassDeleteCount() => glass_deleteCount;
        public void SetGlassDeleteCount(int value) { glass_deleteCount = value; UpdateText(); }
        //�ö�ƽ ����
        public int GetPlasticDeleteCount() => plastic_deleteCount;
        public void SetPlasticDeleteCount(int value) { plastic_deleteCount = value; UpdateText(); }
        //ĵ ����
        public int GetCanDeleteCount() => can_deleteCount;
        public void SetCanDeleteCount(int value) { can_deleteCount = value; UpdateText(); }
        //�������� �ϼ� ����
        public bool IsGlassBreakCompleted() => glass_break;
        public void SetGlassBreakCompletion(bool value) { glass_break = value; UpdateText(); }
        //�ö�ƽ ���� �ϼ� ����
        public bool IsPlasticBreakCompleted() => plastic_break;
        public void SetPlasticBreakCompletion(bool value) { plastic_break = value; UpdateText(); }
        //ĵ ���� �ϼ� ����
        public bool IsCanBreakCompleted() => can_break;
        public void SetCanBreakCompletion(bool value) { can_break = value; UpdateText(); }

        // 4. �뱤�� ��� ����
        //�������� ����
        public int GetBGlassDeleteCount() => b_glass_deleteCount;
        public void SetBGlassDeleteCount(int value) { b_glass_deleteCount = value; UpdateText(); }
        //�ö�ƽ ���� ����
        public int GetBPlasticDeleteCount() => b_plastic_deleteCount;
        public void SetBPlasticDeleteCount(int value) { b_plastic_deleteCount = value; UpdateText(); }
        //ĵ ���� ����
        public int GetBCanDeleteCount() => b_can_deleteCount;
        public void SetBCanDeleteCount(int value) { b_can_deleteCount = value; UpdateText(); }
        //���� ���� �ϼ� ����
        public bool IsGlassMoltenCompleted() => glass_molten;
        public void SetGlassMoltenCompletion(bool value) { glass_molten = value; UpdateText(); }
        //���� �ö�ƽ �ϼ� ����
        public bool IsPlasticMoltenCompleted() => plastic_molten;
        public void SetPlasticMoltenCompletion(bool value) { plastic_molten = value; UpdateText(); }
        //�˷�̴� �ϼ� ����
        public bool IsCanMoltenCompleted() => can_molten;
        public void SetCanMoltenCompletion(bool value) { can_molten = value; UpdateText(); }

        // 5. ����� ��� ����
        //���� ����
        public int GetPaperDeleteCount() => paper_deleteCount;
        public void SetPaperDeleteCount(int value) { paper_deleteCount = value; UpdateText(); }
        //���� ���� �ϼ� ����
        public bool IsCompressedPaperCompleted() => compressed_paper;
        public void SetCompressedPaperCompletion(bool value) { compressed_paper = value; UpdateText(); }

        //��������������������������������������������


        private void Start()
        {
            cameraScript = GetComponent<ThirdPersonCamera>();            

            //BlockController�� ����ϱ� ����
            GameObject obj = GameObject.Find("UIController");
            if (obj != null)
                machineController = obj.GetComponent<BlockController>();

            UpdateFinish();
            UpdateText();
        }

        private void UpdateFinish()
        {
            if(ptthread)
            {
                finish_ui.SetActive(true);
            }

            if (glass_break || plastic_break || can_break)
            {
                B_finish_ui.SetActive(true);
            }

            if(aluminum || moltenGlass || moltenPlastic)
            {
                BF_finish_ui.SetActive(true);
            }

            if (compressed_paper)
            {
                C_finish_ui.SetActive(true); //�ϼ� uiȰ��ȭ
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // ��Ŭ�� ����
            {
                TryDeleteHeldObject();
            }

        }

        private void TryDeleteHeldObject()
        {
            string item_name = " ";
            if (cameraScript == null)
                return;

            Ray ray = new Ray(cameraScript.transform.position, cameraScript.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, cameraScript.m_RaycastDistance))
            {
                string heldTag = "����";
                Transform child = null;

                // �ڽ��� ���� ���� GetChild(0) ���
                if (handPos != null && handPos.transform.childCount > 0)
                {
                    child = handPos.transform.GetChild(0);
                    heldTag = child.tag;
                }

                // Ŭ���� ������Ʈ�� machine �±��� ���� ����
                if (hit.collider.CompareTag("machine"))
                {
                    //���� ������� ������, ptthread(��Ʈ��)�� ��������� ���� �����϶�
                    if (handPos != null && handPos.transform.childCount > 0 && ptthread == false)
                    {
                        if (heldTag == "pt") //�տ� pt�� ��� ���� ��
                        {
                            Destroy(child.gameObject); //������Ʈ ����
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            pt_deleteCount++; //�� �� �־����� ī��Ʈ
                            UpdateText(); //UI ����

                            if (pt_deleteCount == 1) //��Ʈ�� ����� ��Ʈ���� �� ���� ��
                            {
                                item_name = "pt_thread"; //���� ������: ��Ʈ��
                                StartCoroutine(DelayTime(item_name)); //�ڷ�ƾ ȣ��
                            }

                        }
                    }
                    else
                    {
                        if(ptthread == true && handPos != null && handPos.transform.childCount == 0) //���� ���������
                        {
                            finish_ui.SetActive(false); //�ϼ�UI ��Ȱ��ȭ
                            ptthread = false; //�̿ϼ����� ����                            
                            InventorySelectionManager.Instance.AddItemToSelectedSlot("PetRope");
                            machine--;
                            GameObject Machine = GameObject.FindWithTag("machine");
                            if(machine == 0)
                            {
                                if (Machine != null)
                                {
                                    Destroy(Machine);
                                    machine = 10;
                                }
                            }
                            
                        }
                    }
                }

                //Ŭ���� ������Ʈ�� �±װ� breaker�� ��
                else if (hit.collider.CompareTag("breaker"))
                {
                    //��� ��ᵵ �ϼ����� �ʾҰ� �տ� ������Ʈ�� ���� ��
                    if (handPos != null && handPos.transform.childCount > 0 && glass_break == false && plastic_break == false && can_break == false)
                    {
                        if (heldTag == "glass") //������ ��� ���� ��
                        {
                            Destroy(child.gameObject); //�տ� �ִ� ������Ʈ ����
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            glass_deleteCount++; //���� +1
                            UpdateText(); //UI �ݿ�

                            if (glass_deleteCount == 1) //���� ����
                            {
                                item_name = "breakGlass"; //���� ������: �� ����
                                StartCoroutine(DelayTime(item_name)); //�ڷ�ƾ ȣ��
                            }
                        }
                        else if (heldTag == "plastic") //�ö�ƽ�� ��� ���� ��
                        {
                            Destroy(child.gameObject); //�տ� �ִ� ������Ʈ ����
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            plastic_deleteCount++; //�ö�ƽ +1
                            UpdateText(); //UI �ݿ�

                            if(plastic_deleteCount == 1) //����
                            {
                                item_name = "breakPlastic"; //���� ������: �� �ö�ƽ
                                StartCoroutine(DelayTime(item_name)); //�ڷ�ƾ ȣ��
                            }
                        }
                        else if (heldTag == "can") //ĵ�� ��� ���� ��
                        {
                            Destroy(child.gameObject); //�տ� �ִ� ������Ʈ ����
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            can_deleteCount++; //ĵ +1
                            UpdateText(); //UI �ݿ�

                            if(can_deleteCount == 1) //����
                            {
                                item_name = "breakCan"; //���� ������: �� ĵ
                                StartCoroutine(DelayTime(item_name)); //�ڷ�ƾ ȣ��
                            }
                        }
                    }
                    else
                    {
                        if (glass_break == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            B_finish_ui.SetActive(false); //�ϼ�UI ��Ȱ��ȭ
                            glass_break = false;
                            InventorySelectionManager.Instance.AddItemToSelectedSlot("Glass_Powder");
                            breaker--;
                        }
                        else if (plastic_break == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            B_finish_ui.SetActive(false); //�ϼ�UI ��Ȱ��ȭ
                            plastic_break = false;
                            InventorySelectionManager.Instance.AddItemToSelectedSlot("Plastic_Powder");
                            breaker--;
                        }
                        else if (can_break == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            B_finish_ui.SetActive(false); //�ϼ�UI ��Ȱ��ȭ
                            can_break = false;
                            InventorySelectionManager.Instance.AddItemToSelectedSlot("Can_Powder");
                            breaker--;
                        }
                        GameObject Breaker = GameObject.FindWithTag("breaker");
                        if (breaker == 0)
                        {
                            if (Breaker != null)
                            {
                                Destroy(Breaker);
                                breaker = 45;
                            }
                        }

                    }
                }

                //Ŭ���� ������Ʈ�� �±װ� BlastFurnace�� ��
                else if (hit.collider.CompareTag("BlastFurnace"))
                {
                    //��� ��ᵵ �ϼ����� �ʾҰ� �տ� ������Ʈ�� ���� ��
                    if (handPos != null && handPos.transform.childCount > 0 && glass_molten == false && plastic_molten == false && glass_molten == false)
                    {
                        if (heldTag == "breakglass") //�� ������ ��� ���� ��
                        {
                            Destroy(child.gameObject); //�տ� �ִ� ������Ʈ ����
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            b_glass_deleteCount++; //�� ���� +1
                            UpdateText(); //UI �ݿ�

                            if (b_glass_deleteCount == 3) //���� ����
                            {
                                item_name = "moltenGlass"; //���� ������: ���� ����
                                StartCoroutine(DelayTime(item_name)); //�ڷ�ƾ ȣ��
                            }
                        }
                        else if (heldTag == "breakplastic") //�� �ö�ƽ�� ��� ���� ��
                        {
                            Destroy(child.gameObject); //�տ� �ִ� ������Ʈ ����
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            b_plastic_deleteCount++; //�� �ö�ƽ +1
                            UpdateText(); //UI �ݿ�

                            if (b_plastic_deleteCount == 3) //����
                            {
                                item_name = "moltenPlastic"; //���� ������: ���� �ö�ƽ
                                StartCoroutine(DelayTime(item_name)); //�ڷ�ƾ ȣ��
                            }
                        }
                        else if (heldTag == "breakcan") //�� ĵ�� ��� ���� ��
                        {
                            Destroy(child.gameObject); //�տ� �ִ� ������Ʈ ����
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            b_can_deleteCount++; //�� ĵ +1
                            UpdateText(); //UI �ݿ�

                            if (b_can_deleteCount == 3) //����
                            {
                                item_name = "aluminum"; //���� ������: �˷�̴�
                                StartCoroutine(DelayTime(item_name)); //�ڷ�ƾ ȣ��
                            }
                        }
                    }
                    else
                    {
                        if (glass_molten == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            BF_finish_ui.SetActive(false); //�ϼ�UI ��Ȱ��ȭ
                            glass_molten = false;
                            InventorySelectionManager.Instance.AddItemToSelectedSlot("MeltGlass");
                            blastFurnace--;
                        }
                        else if (plastic_molten == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            BF_finish_ui.SetActive(false); //�ϼ�UI ��Ȱ��ȭ
                            plastic_break = false;
                            InventorySelectionManager.Instance.AddItemToSelectedSlot("MeltPla");
                            blastFurnace--;
                        }
                        else if (can_molten == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            BF_finish_ui.SetActive(false); //�ϼ�UI ��Ȱ��ȭ
                            can_molten = false;
                            InventorySelectionManager.Instance.AddItemToSelectedSlot("Al");
                            blastFurnace--;
                        }
                        GameObject BlastFurnace = GameObject.FindWithTag("BlastFurnace");
                        if (blastFurnace == 0)
                        {
                            if (BlastFurnace != null)
                            {
                                Destroy(BlastFurnace);
                                blastFurnace = 15;
                            }
                        }
                    }

                }

                //�±װ� compressor�϶�
                else if (hit.collider.CompareTag("compressor"))
                {
                    Debug.Log($"������: {hit.collider.gameObject.name}, �±�: {hit.collider.tag} ��� �ִ� �� �±�: {heldTag}, �ϼ� ����: {compressed_paper}");
                    //���� ������� ������, �������̰� ��������� ���� �����϶�
                    if (handPos != null && handPos.transform.childCount > 0 && compressed_paper == false)
                    {
                        if (heldTag == "paper") //�տ� ���̸� ��� ���� ��
                        {
                            Destroy(child.gameObject); //������Ʈ ����
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            paper_deleteCount++; //�� �� �־����� ī��Ʈ
                            UpdateText(); //UI ����

                            if (paper_deleteCount == 1) //��Ʈ�� ����� ��Ʈ���� �� ���� ��
                            {
                                item_name = "compressedPaper"; //���� ������: ��Ʈ��
                                StartCoroutine(DelayTime(item_name)); //�ڷ�ƾ ȣ��
                            }
                        }
                    }
                    else
                    {
                        if (compressed_paper == true && handPos != null && handPos.transform.childCount == 0) //���� ���������
                        {
                            C_finish_ui.SetActive(false); //�ϼ�UI ��Ȱ��ȭ
                            compressed_paper = false; //�̿ϼ����� ����
                            InventorySelectionManager.Instance.AddItemToSelectedSlot("PressedPaper");
                            compressor--;
                            GameObject Compressor = GameObject.FindWithTag("compressor");
                            if (compressor == 0)
                            {
                                if (Compressor != null)
                                {
                                    Destroy(Compressor);
                                    compressor = 15;
                                }
                            }
                        }
                    }
                }

            }
        }

        private void UpdateText()
        {
            //������ ���� UI
            countText.text = $"{pt_deleteCount}/1"; //��Ʈ ����

            //�м�� ���� UI
            breaker_countText_glass.text = $"{glass_deleteCount}/1";
            breaker_countText_plastic.text = $"{plastic_deleteCount}/1";
            breaker_countText_can.text = $"{can_deleteCount}/1";

            //�뱤�� ���� UI
            breaker_countText_glass.text = $"{b_glass_deleteCount}/3";
            breaker_countText_plastic.text = $"{b_plastic_deleteCount}/3";
            breaker_countText_can.text = $"{b_can_deleteCount}/3";

            //������ ���� UI
            C_countText.text = $"{paper_deleteCount}/1"; //��Ʈ ����
        }

        IEnumerator DelayTime(string item_name) //��� ���� �ð�
        {
            if(item_name == "pt_thread")
            {
                loading_ui.SetActive(true); //������ �ε� uiȰ��ȭ
                yield return new WaitForSeconds(2.0f); // 2�� ���� ���
                loading_ui.SetActive(false);//�ε� ui ��Ȱ��ȭ

                pt_deleteCount = 0; //0���� ��������
                UpdateText(); //UI ����

                ptthread = true; //������ �غ� �Ϸ�
                finish_ui.SetActive(true); //�ϼ� uiȰ��ȭ
            }

            if(item_name == "breakGlass")
            {
                B_loading_ui.SetActive(true);//�м�� �ε� uiȰ��ȭ
                yield return new WaitForSeconds(2.0f); //2�� ���
                B_loading_ui.SetActive(false); //�м�� �ε� ui ��Ȱ��ȭ

                glass_deleteCount = 0; //0���� �ٽ� �ʱ�ȭ
                UpdateText(); //ui ������Ʈ

                glass_break = true; //�� ���� �غ� �Ϸ�
                B_finish_ui.SetActive(true); //�ϼ� ui Ȱ��ȭ
            }
            else if (item_name == "breakPlastic")
            {
                B_loading_ui.SetActive(true);//�м�� �ε� uiȰ��ȭ
                yield return new WaitForSeconds(2.0f); //2�� ���
                B_loading_ui.SetActive(false); //�м�� �ε� ui ��Ȱ��ȭ

                plastic_deleteCount = 0; //0���� �ٽ� �ʱ�ȭ
                UpdateText(); //ui ������Ʈ

                plastic_break = true; //�� �ö�ƽ �غ� �Ϸ�
                B_finish_ui.SetActive(true); //�ϼ� ui Ȱ��ȭ
            }
            else if (item_name == "breakCan")
            {
                B_loading_ui.SetActive(true);//�м�� �ε� uiȰ��ȭ
                yield return new WaitForSeconds(2.0f); //2�� ���
                B_loading_ui.SetActive(false); //�м�� �ε� ui ��Ȱ��ȭ

                can_deleteCount = 0; //0���� �ٽ� �ʱ�ȭ
                UpdateText(); //ui ������Ʈ

                can_break = true; //�� ĵ �غ� �Ϸ�
                B_finish_ui.SetActive(true); //�ϼ� ui Ȱ��ȭ
            }

            if (item_name == "moltenGlass")
            {
                BF_loading_ui.SetActive(true);//�뱤�� �ε� uiȰ��ȭ
                yield return new WaitForSeconds(2.0f); //2�� ���
                BF_loading_ui.SetActive(false); //�뱤�� �ε� ui ��Ȱ��ȭ

                b_glass_deleteCount = 0; //0���� �ٽ� �ʱ�ȭ
                UpdateText(); //ui ������Ʈ

                glass_molten = true; //���� ���� �غ� �Ϸ�
                BF_finish_ui.SetActive(true); //�ϼ� ui Ȱ��ȭ
            }
            else if (item_name == "moltenPlastic")
            {
                BF_loading_ui.SetActive(true);//�뱤�� �ε� uiȰ��ȭ
                yield return new WaitForSeconds(2.0f); //2�� ���
                BF_loading_ui.SetActive(false); //�뱤�� �ε� ui ��Ȱ��ȭ

                b_plastic_deleteCount = 0; //0���� �ٽ� �ʱ�ȭ
                UpdateText(); //ui ������Ʈ

                plastic_molten = true; //���� �ö�ƽ �غ� �Ϸ�
                BF_finish_ui.SetActive(true); //�ϼ� ui Ȱ��ȭ
            }
            else if (item_name == "aluminum")
            {
                BF_loading_ui.SetActive(true);//�뱤�� �ε� uiȰ��ȭ
                yield return new WaitForSeconds(2.0f); //2�� ���
                BF_loading_ui.SetActive(false); //�뱤�� �ε� ui ��Ȱ��ȭ

                b_can_deleteCount = 0; //0���� �ٽ� �ʱ�ȭ
                UpdateText(); //ui ������Ʈ

                can_molten = true; //�� ĵ �غ� �Ϸ�
                BF_finish_ui.SetActive(true); //�ϼ� ui Ȱ��ȭ
            }

            if (item_name == "compressedPaper")
            {
                C_loading_ui.SetActive(true); //������ �ε� uiȰ��ȭ
                yield return new WaitForSeconds(2.0f); // 2�� ���� ���
                C_loading_ui.SetActive(false);//�ε� ui ��Ȱ��ȭ

                paper_deleteCount = 0; //0���� ��������
                UpdateText(); //UI ����

                compressed_paper = true; //������ �غ� �Ϸ�
                C_finish_ui.SetActive(true); //�ϼ� uiȰ��ȭ
            }

        }

    }
}
