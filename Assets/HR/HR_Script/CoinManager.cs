using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using InventorySystem;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;

[System.Serializable]

public class CoinManager : MonoBehaviour
{
    public int _coin = 1000; //캐릭터의 코인
    public int HappyP; //나중에 민하가 만든 해피지구력으로 교체
                       //public List<ItemData> inventory = new List<ItemData>(); 저장 X

    public int coin
    {
        get => _coin;
        set => _coin = value;
    }

    public static CoinManager Instance;

    public TextMeshProUGUI CoinText; //코인 수 나타내는 GUI
    //public TextMeshProUGUI SellShop_CoinText; //아이템 판매 상점
    //public TextMeshProUGUI MachineShop_CoinText; //기계 구매 상점

    //디버깅용
    public TextMeshProUGUI HappyText;

    void Awake()
    {
        Instance = this;
        coin = 1000;
        //0으로 해두고 저장 시스템 구현 되면 저장된 coin 불러오기
    }

    public void UpdateMoneyText(int coin) 
    {
        CoinText.text = $"Coin: {coin:N0}"; //천단위 쉼표 표시
    }

    public void OnAddMoney(int amount)
    {
        //amount = 20; //추후 상점 시스템에서 amount의 값 전달
        coin += amount;
        CoinManager.Instance.UpdateMoneyText(coin);
    }

    public void OnMinusMoney(int amount_m)
    {
        //amount_m = 10; //추후 상점 시스템에서 기계 값 전달
        coin -= amount_m;
        if(coin < 0) //만약 기계 값보다 소지 금액이 적은 경우는 상점 시스템에서 개발 예정
        {
            coin = 0;
        }
        CoinManager.Instance.UpdateMoneyText(coin);
    }

}
