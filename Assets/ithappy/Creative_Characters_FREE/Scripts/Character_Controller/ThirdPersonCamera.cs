using UnityEngine;

namespace Controller
{
    public class ThirdPersonCamera : PlayerCamera
    {
        [SerializeField, Range(0f, 2f)]
        private float m_Offset = 1.5f;
        [SerializeField, Range(0f, 2f)]
        private float m_HorizontalOffset = 0.5f;
        [SerializeField, Range(0f, 360f)]
        public float m_CameraSpeed = 90f;
        [SerializeField, Range(0f, 360f)]
        private float m_RotationSpeed = 100f;
        [SerializeField, Range(0f, 50f)]
        public float m_RaycastDistance = 10f;

        private Vector3 m_LookPoint;
        private Vector3 m_TargetPos;

        //UI관련(키입력하시오 이런거)
        public GameObject machine_ui;
        public GameObject breaker_ui;
        public GameObject blastFurnace_ui;
        public GameObject compressor_ui;
        public GameObject sewing_ui;
        public GameObject Sewing;
        public GameObject maker_ui;
        //public GameObject Maker;

        //감도 공유를 위한 게터세터
        public float CameraSpeed
        {
            get => m_CameraSpeed;
            set => m_CameraSpeed = value;
        }

        public void Start()
        {
            Sewing.SetActive(false);
        }

        private void LateUpdate()
        {
            Move(Time.deltaTime);
            CheckAimTarget();
        }

        public override void SetInput(in Vector2 delta, float scroll)
        {
            base.SetInput(delta, scroll);

            m_Angles.y += delta.x * m_RotationSpeed * Time.deltaTime;
            m_Angles.x -= delta.y * m_RotationSpeed * Time.deltaTime;
            m_Angles.x = Mathf.Clamp(m_Angles.x, -90f, 90f);

            var dir = new Vector3(0, 0, -m_Distance);
            var rot = Quaternion.Euler(m_Angles.x, m_Angles.y, 0f);

            var playerPos = (m_Player == null) ? Vector3.zero : m_Player.position;
            Vector3 rightOffset = m_Player != null ? m_Player.right : Vector3.right;
            m_LookPoint = playerPos + m_Offset * Vector3.up + m_HorizontalOffset * rightOffset.normalized;
            m_TargetPos = m_LookPoint + rot * dir;
        }

        private void Move(float deltaTime)
        {
            camera();
            target();

            void camera()
            {
                var direction = m_TargetPos - m_Transform.position;
                var delta = m_CameraSpeed * deltaTime;

                if (delta * delta > direction.sqrMagnitude)
                {
                    m_Transform.position = m_TargetPos;
                }
                else
                {
                    m_Transform.position += delta * direction.normalized;
                }

                m_Transform.LookAt(m_LookPoint);
            }

            void target()
            {
                if (m_Target == null)
                {
                    return;
                }

                m_Target.position = m_Transform.position + m_Transform.forward * TargetDistance;
            }
        }

        private void CheckAimTarget()
        {
            Ray ray = new Ray(m_Transform.position, m_Transform.forward);
            RaycastHit hit;

            // 디버깅을 위해 레이 시각화
            Debug.DrawRay(m_Transform.position, m_Transform.forward * m_RaycastDistance, Color.red);

            if (Physics.Raycast(ray, out hit, m_RaycastDistance))
            {
                // 충돌한 오브젝트의 이름과 태그를 출력
                Debug.Log($"조준된 오브젝트: {hit.collider.gameObject.name}, 태그: {hit.collider.tag}");

                //방적기(machine) UI 
                if (hit.collider.tag == "machine")
                {
                    machine_ui.SetActive(true);
                }
                else if(hit.collider.tag == "breaker")
                {
                    breaker_ui.SetActive(true);
                }
                else if(hit.collider.tag == "BlastFurnace")
                {
                    blastFurnace_ui.SetActive(true);
                }
                else if(hit.collider.tag == "compressor")
                {
                    compressor_ui.SetActive(true);
                }
                else if(hit.collider.tag == "sewing")
                {
                    sewing_ui.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        Sewing.SetActive(true);
                    }
                }
                else if(hit.collider.tag == "maker")
                {
                    maker_ui.SetActive(true);
                    //if (Input.GetKeyDown(KeyCode.F)) { Maker.SetActive(true); }
                }
                else
                {
                    machine_ui.SetActive(false);
                    breaker_ui.SetActive(false);
                    blastFurnace_ui.SetActive(false);
                    compressor_ui.SetActive(false);
                    sewing_ui.SetActive(false);
                    maker_ui.SetActive(false);
                }
            }
            else
            {
                Debug.Log("아무 오브젝트도 감지되지 않음");
                machine_ui.SetActive(false); //방적기(machine) UI 
                breaker_ui.SetActive(false);
                blastFurnace_ui.SetActive(false);
                compressor_ui.SetActive(false);
                sewing_ui.SetActive(false);
                maker_ui.SetActive(false);
            }
        }
    }
}