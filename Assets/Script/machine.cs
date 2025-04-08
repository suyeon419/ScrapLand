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
        //몇 개 넣었는지 확인
        public TextMeshProUGUI countText; //페트

        //재료들
        public GameObject pt_thread; //페트실

        //UI
        public GameObject loading_ui;//빙글빙글 로딩 UI
        public GameObject finish_ui; //재료 완성 UI

        private int pt_deleteCount = 0;
        private bool ptthread = false;

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

                // 클릭된 오브젝트가 machine 태그일 때만 실행
                if (hit.collider.CompareTag("machine"))
                {
                    //손이 비어있지 않을때, ptthread(페트실)이 만들어지지 않은 상태일때
                    if (handPos != null && handPos.transform.childCount > 0 && ptthread == false)
                    {
                        Transform child = handPos.transform.GetChild(0);
                        heldTag = child.tag;

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
                            GameObject newObj = Instantiate(pt_thread, handPos.transform); //손에 아이템 장착
                        }
                    }
                }
                Debug.Log($"클릭된 오브젝트: {hit.collider.gameObject.name}, 태그: {hit.collider.tag} [손에 있는 오브젝트 태그: {heldTag}]");

            }
        }

        private void UpdateText()
        {
            if (countText != null)
            {
                countText.text = $"{pt_deleteCount}/1";
            }
        }

        IEnumerator DelayTime(string iteam_name) //기계 제작 시간
        {
            loading_ui.SetActive(true); //로딩 ui활성화
            yield return new WaitForSeconds(2.0f); // 2초 동안 대기
            Debug.Log("2초 후 호출됨");
            loading_ui.SetActive(false);//로딩 ui 비활성화

            if(iteam_name == "pt_thread") //만약 페트실 완성이라면
            {
                pt_deleteCount = 0; //0으로 돌려놓고
                UpdateText(); //UI 설정

                ptthread = true; //아이템 준비 완료
                finish_ui.SetActive(true); //완성 ui활성화
                Debug.Log($"ptthread: {ptthread}");
                
            }
        }

    }
}
