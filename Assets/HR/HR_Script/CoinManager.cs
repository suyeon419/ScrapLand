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
    public int _coin = 1000; //ĳ������ ����
    public int HappyP; //���߿� ���ϰ� ���� �������������� ��ü
                       //public List<ItemData> inventory = new List<ItemData>(); ���� X

    public int coin
    {
        get => _coin;
        set => _coin = value;
    }

    public static CoinManager Instance;

    public TextMeshProUGUI CoinText; //���� �� ��Ÿ���� GUI
    //public TextMeshProUGUI SellShop_CoinText; //������ �Ǹ� ����
    //public TextMeshProUGUI MachineShop_CoinText; //��� ���� ����

    //������
    public TextMeshProUGUI HappyText;

    void Awake()
    {
        Instance = this;
        coin = 1000;
        //0���� �صΰ� ���� �ý��� ���� �Ǹ� ����� coin �ҷ�����
    }

    public void UpdateMoneyText(int coin) 
    {
        CoinText.text = $"Coin: {coin:N0}"; //õ���� ��ǥ ǥ��
    }

    public void OnAddMoney(int amount)
    {
        //amount = 20; //���� ���� �ý��ۿ��� amount�� �� ����
        coin += amount;
        CoinManager.Instance.UpdateMoneyText(coin);
    }

    public void OnMinusMoney(int amount_m)
    {
        //amount_m = 10; //���� ���� �ý��ۿ��� ��� �� ����
        coin -= amount_m;
        if(coin < 0) //���� ��� ������ ���� �ݾ��� ���� ���� ���� �ý��ۿ��� ���� ����
        {
            coin = 0;
        }
        CoinManager.Instance.UpdateMoneyText(coin);
    }

}
