using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEditor;


namespace Controller
{
    public class Machine : MonoBehaviour
    {
        private ThirdPersonCamera cameraScript;

        //손 위치
        public GameObject handPos;


        //재료들
        [Header("items")]
        public GameObject pt_thread; //페트실

        public GameObject breakGlass; //간 유리
        public GameObject breakPlastic; //간 플라스틱
        public GameObject breakCan; //간 캔

        //방적기 UI
        [Header("machine")]
        public GameObject loading_ui;//빙글빙글 로딩 UI
        public GameObject finish_ui; //재료 완성 UI
        public TextMeshProUGUI countText; //페트 개수 확인 UI

        //분쇄기 UI
        [Header("breaker")]
        public GameObject B_loading_ui;//빙글빙글 로딩 UI
        public GameObject B_finish_ui; //재료 완성 UI
        public TextMeshProUGUI breaker_countText; //분쇄기 잔량 UI

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

        private void Start()
        {
            cameraScript = GetComponent<ThirdPersonCamera>();

            if (cameraScript == null)
            {
                Debug.LogError("ThirdPersonCamera 스크립트를 찾을 수 없습니다!");
            }

            if (handPos == null)
            {
                Debug.LogWarning("handPos가 지정되지 않았습니다.");
            }

            UpdateText();
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
                            
                            pt_deleteCount++; //몇 개 넣었는지 카운트
                            UpdateText(); //UI 변경

                            if (pt_deleteCount == 1) //페트실 만드는 페트병이 다 모였을 때
                            {
                                item_name = "pt_thread"; //만들 아이템: 페트실
                                StartCoroutine(DelayTime(item_name)); //코루틴 호출
                            }

                            Debug.Log($"클릭된 오브젝트: {hit.collider.gameObject.name}, 태그: {hit.collider.tag} [손에 있는 오브젝트 태그: {heldTag}]");
                        }
                    }
                    else
                    {
                        Debug.Log($"클릭된 오브젝트: {hit.collider.gameObject.name}, 태그: {hit.collider.tag} [손에 있는 오브젝트 없음]");
                        if(ptthread == true && handPos != null && handPos.transform.childCount == 0) //손이 비었을때만
                        {
                            finish_ui.SetActive(false); //완성UI 비활성화
                            ptthread = false; //미완성으로 변경
                            GameObject newObj = Instantiate(pt_thread, handPos.transform); //손에 아이템 장착
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

                            can_deleteCount++; //캔 +1
                            UpdateText(); //UI 반영

                            if(can_deleteCount == 1) //조건
                            {
                                item_name = "breakCan"; //만들 아이템: 간 캔
                                StartCoroutine(DelayTime(item_name)); //코루틴 호출
                            }
                        }
                        else
                        {
                            Debug.Log("아무일도 없었다.");
                        }
                    }
                    else
                    {
                        if (glass_break == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            B_finish_ui.SetActive(false); //완성UI 비활성화
                            glass_break = false;
                            GameObject newObj = Instantiate(breakGlass, handPos.transform); //손에 아이템 장착
                        }
                        else if (plastic_break == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            B_finish_ui.SetActive(false); //완성UI 비활성화
                            plastic_break = false;
                            GameObject newObj = Instantiate(breakPlastic, handPos.transform); //손에 아이템 장착
                        }
                        else if (can_break == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            B_finish_ui.SetActive(false); //완성UI 비활성화
                            can_break = false;
                            GameObject newObj = Instantiate(breakCan, handPos.transform); //손에 아이템 장착
                        }
                    }

                }
                Debug.Log($"클릭된 오브젝트: {hit.collider.gameObject.name}, 태그: {hit.collider.tag} [손에 있는 오브젝트 태그: {heldTag}]");

            }
        }

        private void UpdateText()
        {
            //방적기 관련 UI
            countText.text = $"pt: {pt_deleteCount}/1"; //페트 개수

            //분쇄기 관련 UI
            breaker_countText.text = $" glass: {glass_deleteCount}/1 \n plastic: {plastic_deleteCount}/1 \n can: {can_deleteCount}/1";
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
                Debug.Log($"ptthread: {ptthread}");
            }
            else if(item_name == "breakGlass")
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

        }

    }
}
