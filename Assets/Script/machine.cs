using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEditor;
#if UNITY_EDITOR
using static UnityEditor.Progress; // 에디터 전용 코드
#endif


namespace Controller
{
    public class Machine : MonoBehaviour
    {
        private ThirdPersonCamera cameraScript;
        private BlockController machineController;

        //손 위치
        public GameObject handPos;

        //재료들
        [Header("items")]
        public GameObject pt_thread; //페트실

        public GameObject breakGlass; //간 유리
        public GameObject breakPlastic; //간 플라스틱
        public GameObject breakCan; //간 캔

        public GameObject moltenGlass; //녹은 유리
        public GameObject moltenPlastic; //녹은 플라스틱
        public GameObject aluminum;//알루미늄

        public GameObject compressedPaper; //압축종이

        //방적기 UI
        [Header("machine")]
        public GameObject loading_ui;//빙글빙글 로딩 UI
        public GameObject finish_ui; //재료 완성 UI
        public TextMeshProUGUI countText; //페트 개수 확인 UI

        //분쇄기 UI
        [Header("breaker")]
        public GameObject B_loading_ui;//빙글빙글 로딩 UI
        public GameObject B_finish_ui; //재료 완성 UI
        public TextMeshProUGUI breaker_countText_glass; //분쇄기 잔량 UI
        public TextMeshProUGUI breaker_countText_plastic; //분쇄기 잔량 UI
        public TextMeshProUGUI breaker_countText_can; //분쇄기 잔량 UI

        //용광로 UI
        [Header("Blast Furnace")]
        public GameObject BF_loading_ui;//빙글빙글 로딩 UI
        public GameObject BF_finish_ui;//재료 완성 UI
        public TextMeshProUGUI BF_countText_glass; //용광로 잔량 UI
        public TextMeshProUGUI BF_countText_plastic; //용광로 잔량 UI
        public TextMeshProUGUI BF_countText_can; //용광로 잔량 UI

        //압축기 UI
        [Header("Compressor")]
        public GameObject C_loading_ui;//빙글빙글 로딩 UI
        public GameObject C_finish_ui; //재료 완성 UI
        public TextMeshProUGUI C_countText; //페트 개수 확인 UI

        //기계 내구도
        private int machine = 10;
        private int breaker = 45;
        private int blastFurnace = 15;
        private int compressor = 10;

        //방적기 재료 관련
        private int pt_deleteCount = 0; //페트 개수 확인
        private bool ptthread = false; //페트실 완성 여부

        //분쇄기 재료 관련
        private int glass_deleteCount = 0; //유리 개수 확인
        private int plastic_deleteCount = 0;//플라스틱 개수 확인
        private int can_deleteCount = 0; //캔 개수 확인
        private bool glass_break = false; //간 유리 완성 여부
        private bool plastic_break = false; //간 플라스틱 완성 여부
        private bool can_break = false; //간 캔 완성 여부

        //용광로 재료 관련
        private int b_glass_deleteCount = 0; //간 유리 개수 확인
        private int b_plastic_deleteCount = 0;//간 플라스틱 개수 확인
        private int b_can_deleteCount = 0; //간 캔 개수 확인
        private bool glass_molten = false; //녹은 유리 완성 여부
        private bool plastic_molten = false; //녹은 플라스틱 완성 여부
        private bool can_molten = false; //알루미늄 완성 여부

        //압축기 재료 관련
        private int paper_deleteCount = 0; //종이 개수 확인
        private bool compressed_paper = false; //압축종이 완성 여부

        // ■■■■■■■■■■게터세터■■■■■■■■■■■■■■■■■
        // 1. 내구도 관리
        //방적기
        public int GetMachineDurability() => machine; 
        public void SetMachineDurability(int value) { machine = value; UpdateText(); } 
        //분쇄기
        public int GetBreakerDurability() => breaker;
        public void SetBreakerDurability(int value) { breaker = value; UpdateText(); }
        //용광로
        public int GetBlastFurnaceDurability() => blastFurnace;
        public void SetBlastFurnaceDurability(int value) { blastFurnace = value; UpdateText(); }
        //압축기
        public int GetCompressorDurability() => compressor;
        public void SetCompressorDurability(int value) { compressor = value; UpdateText(); }

        // 2. 방적기 관련
        //페트 개수
        public int GetPtDeleteCount() => pt_deleteCount; 
        public void SetPtDeleteCount(int value) { pt_deleteCount = value; UpdateText(); }
        //완성 여부
        public bool IsPtThreadCompleted() => ptthread;
        public void SetPtThreadCompletion(bool value) { ptthread = value; UpdateText(); }

        // 3. 분쇄기 재료 관련
        //유리 개수
        public int GetGlassDeleteCount() => glass_deleteCount;
        public void SetGlassDeleteCount(int value) { glass_deleteCount = value; UpdateText(); }
        //플라스틱 개수
        public int GetPlasticDeleteCount() => plastic_deleteCount;
        public void SetPlasticDeleteCount(int value) { plastic_deleteCount = value; UpdateText(); }
        //캔 개수
        public int GetCanDeleteCount() => can_deleteCount;
        public void SetCanDeleteCount(int value) { can_deleteCount = value; UpdateText(); }
        //유리가루 완성 여부
        public bool IsGlassBreakCompleted() => glass_break;
        public void SetGlassBreakCompletion(bool value) { glass_break = value; UpdateText(); }
        //플라스틱 가루 완성 여부
        public bool IsPlasticBreakCompleted() => plastic_break;
        public void SetPlasticBreakCompletion(bool value) { plastic_break = value; UpdateText(); }
        //캔 가루 완성 여부
        public bool IsCanBreakCompleted() => can_break;
        public void SetCanBreakCompletion(bool value) { can_break = value; UpdateText(); }

        // 4. 용광로 재료 관련
        //유리가루 개수
        public int GetBGlassDeleteCount() => b_glass_deleteCount;
        public void SetBGlassDeleteCount(int value) { b_glass_deleteCount = value; UpdateText(); }
        //플라스틱 가루 개수
        public int GetBPlasticDeleteCount() => b_plastic_deleteCount;
        public void SetBPlasticDeleteCount(int value) { b_plastic_deleteCount = value; UpdateText(); }
        //캔 가루 개수
        public int GetBCanDeleteCount() => b_can_deleteCount;
        public void SetBCanDeleteCount(int value) { b_can_deleteCount = value; UpdateText(); }
        //녹은 유리 완성 여부
        public bool IsGlassMoltenCompleted() => glass_molten;
        public void SetGlassMoltenCompletion(bool value) { glass_molten = value; UpdateText(); }
        //녹은 플라스틱 완성 여부
        public bool IsPlasticMoltenCompleted() => plastic_molten;
        public void SetPlasticMoltenCompletion(bool value) { plastic_molten = value; UpdateText(); }
        //알루미늄 완성 여무
        public bool IsCanMoltenCompleted() => can_molten;
        public void SetCanMoltenCompletion(bool value) { can_molten = value; UpdateText(); }

        // 5. 압축기 재료 관련
        //종이 개수
        public int GetPaperDeleteCount() => paper_deleteCount;
        public void SetPaperDeleteCount(int value) { paper_deleteCount = value; UpdateText(); }
        //압축 종이 완성 여부
        public bool IsCompressedPaperCompleted() => compressed_paper;
        public void SetCompressedPaperCompletion(bool value) { compressed_paper = value; UpdateText(); }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■


        private void Start()
        {
            cameraScript = GetComponent<ThirdPersonCamera>();            

            //BlockController를 사용하기 위해
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
                C_finish_ui.SetActive(true); //완성 ui활성화
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // 좌클릭 감지
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
                string heldTag = "없음";
                Transform child = null;

                // 자식이 있을 때만 GetChild(0) 사용
                if (handPos != null && handPos.transform.childCount > 0)
                {
                    child = handPos.transform.GetChild(0);
                    heldTag = child.tag;
                }

                // 클릭된 오브젝트가 machine 태그일 때만 실행
                if (hit.collider.CompareTag("machine"))
                {
                    //손이 비어있지 않을때, ptthread(페트실)이 만들어지지 않은 상태일때
                    if (handPos != null && handPos.transform.childCount > 0 && ptthread == false)
                    {
                        if (heldTag == "pt") //손에 pt를 들고 있을 때
                        {
                            Destroy(child.gameObject); //오브젝트 삭제
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            pt_deleteCount++; //몇 개 넣었는지 카운트
                            UpdateText(); //UI 변경

                            if (pt_deleteCount == 1) //페트실 만드는 페트병이 다 모였을 때
                            {
                                item_name = "pt_thread"; //만들 아이템: 페트실
                                StartCoroutine(DelayTime(item_name)); //코루틴 호출
                            }

                        }
                    }
                    else
                    {
                        if(ptthread == true && handPos != null && handPos.transform.childCount == 0) //손이 비었을때만
                        {
                            finish_ui.SetActive(false); //완성UI 비활성화
                            ptthread = false; //미완성으로 변경                            
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

                //클릭된 오브젝트의 태그가 breaker일 때
                else if (hit.collider.CompareTag("breaker"))
                {
                    //어떠한 재료도 완성되지 않았고 손에 오브젝트가 있을 때
                    if (handPos != null && handPos.transform.childCount > 0 && glass_break == false && plastic_break == false && can_break == false)
                    {
                        if (heldTag == "glass") //유리를 들고 있을 때
                        {
                            Destroy(child.gameObject); //손에 있는 오브젝트 삭제
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            glass_deleteCount++; //유리 +1
                            UpdateText(); //UI 반영

                            if (glass_deleteCount == 1) //조건 충족
                            {
                                item_name = "breakGlass"; //만들 아이템: 간 유리
                                StartCoroutine(DelayTime(item_name)); //코루틴 호출
                            }
                        }
                        else if (heldTag == "plastic") //플라스틱을 들고 있을 때
                        {
                            Destroy(child.gameObject); //손에 있는 오브젝트 삭제
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            plastic_deleteCount++; //플라스틱 +1
                            UpdateText(); //UI 반영

                            if(plastic_deleteCount == 1) //조건
                            {
                                item_name = "breakPlastic"; //만들 아이템: 간 플라스틱
                                StartCoroutine(DelayTime(item_name)); //코루틴 호출
                            }
                        }
                        else if (heldTag == "can") //캔을 들고 있을 때
                        {
                            Destroy(child.gameObject); //손에 있는 오브젝트 삭제
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            can_deleteCount++; //캔 +1
                            UpdateText(); //UI 반영

                            if(can_deleteCount == 1) //조건
                            {
                                item_name = "breakCan"; //만들 아이템: 간 캔
                                StartCoroutine(DelayTime(item_name)); //코루틴 호출
                            }
                        }
                    }
                    else
                    {
                        if (glass_break == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            B_finish_ui.SetActive(false); //완성UI 비활성화
                            glass_break = false;
                            InventorySelectionManager.Instance.AddItemToSelectedSlot("Glass_Powder");
                            breaker--;
                        }
                        else if (plastic_break == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            B_finish_ui.SetActive(false); //완성UI 비활성화
                            plastic_break = false;
                            InventorySelectionManager.Instance.AddItemToSelectedSlot("Plastic_Powder");
                            breaker--;
                        }
                        else if (can_break == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            B_finish_ui.SetActive(false); //완성UI 비활성화
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

                //클릭된 오브젝트의 태그가 BlastFurnace일 때
                else if (hit.collider.CompareTag("BlastFurnace"))
                {
                    //어떠한 재료도 완성되지 않았고 손에 오브젝트가 있을 때
                    if (handPos != null && handPos.transform.childCount > 0 && glass_molten == false && plastic_molten == false && glass_molten == false)
                    {
                        if (heldTag == "breakglass") //간 유리를 들고 있을 때
                        {
                            Destroy(child.gameObject); //손에 있는 오브젝트 삭제
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            b_glass_deleteCount++; //간 유리 +1
                            UpdateText(); //UI 반영

                            if (b_glass_deleteCount == 3) //조건 충족
                            {
                                item_name = "moltenGlass"; //만들 아이템: 녹인 유리
                                StartCoroutine(DelayTime(item_name)); //코루틴 호출
                            }
                        }
                        else if (heldTag == "breakplastic") //간 플라스틱을 들고 있을 때
                        {
                            Destroy(child.gameObject); //손에 있는 오브젝트 삭제
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            b_plastic_deleteCount++; //간 플라스틱 +1
                            UpdateText(); //UI 반영

                            if (b_plastic_deleteCount == 3) //조건
                            {
                                item_name = "moltenPlastic"; //만들 아이템: 녹인 플라스틱
                                StartCoroutine(DelayTime(item_name)); //코루틴 호출
                            }
                        }
                        else if (heldTag == "breakcan") //간 캔을 들고 있을 때
                        {
                            Destroy(child.gameObject); //손에 있는 오브젝트 삭제
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            b_can_deleteCount++; //간 캔 +1
                            UpdateText(); //UI 반영

                            if (b_can_deleteCount == 3) //조건
                            {
                                item_name = "aluminum"; //만들 아이템: 알루미늄
                                StartCoroutine(DelayTime(item_name)); //코루틴 호출
                            }
                        }
                    }
                    else
                    {
                        if (glass_molten == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            BF_finish_ui.SetActive(false); //완성UI 비활성화
                            glass_molten = false;
                            InventorySelectionManager.Instance.AddItemToSelectedSlot("MeltGlass");
                            blastFurnace--;
                        }
                        else if (plastic_molten == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            BF_finish_ui.SetActive(false); //완성UI 비활성화
                            plastic_break = false;
                            InventorySelectionManager.Instance.AddItemToSelectedSlot("MeltPla");
                            blastFurnace--;
                        }
                        else if (can_molten == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            BF_finish_ui.SetActive(false); //완성UI 비활성화
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

                //태그가 compressor일때
                else if (hit.collider.CompareTag("compressor"))
                {
                    Debug.Log($"때린거: {hit.collider.gameObject.name}, 태그: {hit.collider.tag} 들고 있는 거 태그: {heldTag}, 완성 여부: {compressed_paper}");
                    //손이 비어있지 않을때, 압축종이가 만들어지지 않은 상태일때
                    if (handPos != null && handPos.transform.childCount > 0 && compressed_paper == false)
                    {
                        if (heldTag == "paper") //손에 종이를 들고 있을 때
                        {
                            Destroy(child.gameObject); //오브젝트 삭제
                            InventorySelectionManager.Instance.RemoveSelectedHotBarItem();

                            paper_deleteCount++; //몇 개 넣었는지 카운트
                            UpdateText(); //UI 변경

                            if (paper_deleteCount == 1) //페트실 만드는 페트병이 다 모였을 때
                            {
                                item_name = "compressedPaper"; //만들 아이템: 페트실
                                StartCoroutine(DelayTime(item_name)); //코루틴 호출
                            }
                        }
                    }
                    else
                    {
                        if (compressed_paper == true && handPos != null && handPos.transform.childCount == 0) //손이 비었을때만
                        {
                            C_finish_ui.SetActive(false); //완성UI 비활성화
                            compressed_paper = false; //미완성으로 변경
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
            //방적기 관련 UI
            countText.text = $"{pt_deleteCount}/1"; //페트 개수

            //분쇄기 관련 UI
            breaker_countText_glass.text = $"{glass_deleteCount}/1";
            breaker_countText_plastic.text = $"{plastic_deleteCount}/1";
            breaker_countText_can.text = $"{can_deleteCount}/1";

            //용광로 관련 UI
            breaker_countText_glass.text = $"{b_glass_deleteCount}/3";
            breaker_countText_plastic.text = $"{b_plastic_deleteCount}/3";
            breaker_countText_can.text = $"{b_can_deleteCount}/3";

            //방적기 관련 UI
            C_countText.text = $"{paper_deleteCount}/1"; //페트 개수
        }

        IEnumerator DelayTime(string item_name) //기계 제작 시간
        {
            if(item_name == "pt_thread")
            {
                loading_ui.SetActive(true); //방적기 로딩 ui활성화
                yield return new WaitForSeconds(2.0f); // 2초 동안 대기
                loading_ui.SetActive(false);//로딩 ui 비활성화

                pt_deleteCount = 0; //0으로 돌려놓고
                UpdateText(); //UI 설정

                ptthread = true; //아이템 준비 완료
                finish_ui.SetActive(true); //완성 ui활성화
            }

            if(item_name == "breakGlass")
            {
                B_loading_ui.SetActive(true);//분쇄기 로딩 ui활성화
                yield return new WaitForSeconds(2.0f); //2초 대기
                B_loading_ui.SetActive(false); //분쇄기 로딩 ui 비활성화

                glass_deleteCount = 0; //0으로 다시 초기화
                UpdateText(); //ui 업데이트

                glass_break = true; //간 유리 준비 완료
                B_finish_ui.SetActive(true); //완성 ui 활성화
            }
            else if (item_name == "breakPlastic")
            {
                B_loading_ui.SetActive(true);//분쇄기 로딩 ui활성화
                yield return new WaitForSeconds(2.0f); //2초 대기
                B_loading_ui.SetActive(false); //분쇄기 로딩 ui 비활성화

                plastic_deleteCount = 0; //0으로 다시 초기화
                UpdateText(); //ui 업데이트

                plastic_break = true; //간 플라스틱 준비 완료
                B_finish_ui.SetActive(true); //완성 ui 활성화
            }
            else if (item_name == "breakCan")
            {
                B_loading_ui.SetActive(true);//분쇄기 로딩 ui활성화
                yield return new WaitForSeconds(2.0f); //2초 대기
                B_loading_ui.SetActive(false); //분쇄기 로딩 ui 비활성화

                can_deleteCount = 0; //0으로 다시 초기화
                UpdateText(); //ui 업데이트

                can_break = true; //간 캔 준비 완료
                B_finish_ui.SetActive(true); //완성 ui 활성화
            }

            if (item_name == "moltenGlass")
            {
                BF_loading_ui.SetActive(true);//용광로 로딩 ui활성화
                yield return new WaitForSeconds(2.0f); //2초 대기
                BF_loading_ui.SetActive(false); //용광로 로딩 ui 비활성화

                b_glass_deleteCount = 0; //0으로 다시 초기화
                UpdateText(); //ui 업데이트

                glass_molten = true; //녹은 유리 준비 완료
                BF_finish_ui.SetActive(true); //완성 ui 활성화
            }
            else if (item_name == "moltenPlastic")
            {
                BF_loading_ui.SetActive(true);//용광로 로딩 ui활성화
                yield return new WaitForSeconds(2.0f); //2초 대기
                BF_loading_ui.SetActive(false); //용광로 로딩 ui 비활성화

                b_plastic_deleteCount = 0; //0으로 다시 초기화
                UpdateText(); //ui 업데이트

                plastic_molten = true; //녹인 플라스틱 준비 완료
                BF_finish_ui.SetActive(true); //완성 ui 활성화
            }
            else if (item_name == "aluminum")
            {
                BF_loading_ui.SetActive(true);//용광로 로딩 ui활성화
                yield return new WaitForSeconds(2.0f); //2초 대기
                BF_loading_ui.SetActive(false); //용광로 로딩 ui 비활성화

                b_can_deleteCount = 0; //0으로 다시 초기화
                UpdateText(); //ui 업데이트

                can_molten = true; //간 캔 준비 완료
                BF_finish_ui.SetActive(true); //완성 ui 활성화
            }

            if (item_name == "compressedPaper")
            {
                C_loading_ui.SetActive(true); //방적기 로딩 ui활성화
                yield return new WaitForSeconds(2.0f); // 2초 동안 대기
                C_loading_ui.SetActive(false);//로딩 ui 비활성화

                paper_deleteCount = 0; //0으로 돌려놓고
                UpdateText(); //UI 설정

                compressed_paper = true; //아이템 준비 완료
                C_finish_ui.SetActive(true); //완성 ui활성화
            }

        }

    }
}
