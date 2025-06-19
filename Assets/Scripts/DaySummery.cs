using Controller;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DaySummery : MonoBehaviour
{
    public static DaySummery instance;

    private int[] passing_check = { 0, 50, 100, 200, 400, 650, 900 };
    private int decrease_happy = 100;

    [SerializeField] GameObject summeryPan;
    //[SerializeField] TextMeshProUGUI summeryText;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI happyText;
    [SerializeField] TextMeshProUGUI coinText;

    private void Start()
    {
        instance = this;
        //Debug.Log(GameManager_ScrapLand.instance.GetDayNum()+"일차 시작!!");
        summeryPan.SetActive(false);
    }

    int day;
    int happy;
    int coin;

    public void EndOneDay()
    {
        day = GameManager_ScrapLand.instance.GetDayNum();
        happy = HappyEarth.instance.GetHappyGage();
        coin = CoinManager.Instance.coin;
        Debug.Log(day + "/" + happy);
        Check_Passing(day, happy);
    }

    public int GetPassingHappyEarth(int day)
    {
        return passing_check[day - 1];
    }


    private void Check_Passing(int day, int happy)
    {
        if (happy >= passing_check[day-1])
        {
            if (day < 7)
            {
                // 해피 지구력 조건 충족 및 다음 날 이동
                SummeryText(day, happy, coin);
            }
            else // 7일 완료
            {
                Debug.Log("7일완료");
                // 해피 지구력 조건 충족
                GameManager_ScrapLand.instance.EndingChecking();
            }
        }
        else // 배드엔딩
        {
            Debug.Log(day + "일차: " + happy + "/" + passing_check[day - 1]);
            Debug.Log(happy-passing_check[day-1]);
            SoundManager.instance.OnAndOffBGM(true);
            SaveManager.instance.ResetGame();
            SceneManager.LoadScene("BadEndingScene");
        }
    }

    private void SummeryText(int day, int happy, int coin)
    {
        titleText.text = day + "일차 요약";
        happyText.text = happy + " / " + passing_check[day - 1];

        happyText.text += "\n" + (day + 1) + "일차 시작 : " + (happy - decrease_happy < 0 ? 0 : happy - decrease_happy);

        coinText.text = (coin - GameManager_ScrapLand.instance.GetCoin() > 0 ? "+" : "") + (coin - GameManager_ScrapLand.instance.GetCoin()) + "coin";

        UIManager.Instance.CloseAllPanels();
        StopMoving();
        SoundManager.instance.PlayBGM("Summery");
        summeryPan.SetActive(true);
    }

    void StopMoving()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            MovePlayerInput input = playerObject.GetComponent<MovePlayerInput>();
            if (input != null)
            {
                input.enabled = false;
            }
            else
            {
                Debug.LogWarning("Player 오브젝트에 MovePlayerInput 컴포넌트가 없습니다!");
            }
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다!");
        }

    }

    public bool IsActiveSummeryPannel()
    {
        return summeryPan.activeSelf;
    }

    public void OnNextDayButton()
    {
        var game = GameManager_ScrapLand.instance;
        if (game != null)
        {
            if(day < 7)
            {
                if (happy - 100 <= 0) happy = 0;
                else happy -= 100;

                game.SetHappyGage(happy);
                game.SetCoin(coin);
                game.SetDayNum(++day);
                game.SaveGame();
                SoundManager.instance.PlayBGM("Main");
                SceneManager.LoadScene("PlayScene");
            }
        }
    }
}
