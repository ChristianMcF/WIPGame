using UnityEngine;
using System.Collections;

public class CardPoolClass : MonoBehaviour
{

    //RandomisePools
    //RarityPool
    public enum RarityEnum { Common, UnCommon, Rare, Epic, Legendary };
    public RarityEnum cardRarity;
    [Range(0.0f, 10000.0f)]
    public int commonDropChances;
    [Range(0.0f, 10000.0f)]
    public int unCommonDropChances;
    [Range(0.0f, 10000.0f)]
    public int rareDropChances;
    [Range(0.0f, 10000.0f)]
    public int epicDropChances;

    public GameObject[] commonCards;
    public GameObject[] unCommonCards;
    public GameObject[] rareCards;
    public GameObject[] epicCards;
    public GameObject[] legendaryCards;

    RarityEnum RandomizeRarity()
    {
        int rand = Random.Range(0, 10000);

        if (rand < commonDropChances)
        {
            return RarityEnum.Common;
        }
        else if (rand < unCommonDropChances)
        {
            return RarityEnum.UnCommon;
        }
        else if (rand < rareDropChances)
        {
            return RarityEnum.Rare;
        }
        else if (rand < epicDropChances)
        {
            return RarityEnum.Epic;
        }
        else
        {
            return RarityEnum.Legendary;
        }
    }

    public GameObject GenerateCard()
    {
        cardRarity = RandomizeRarity();
        switch (cardRarity)
        {
            case RarityEnum.Common:
                return commonCards[Random.Range(0, commonCards.Length)];
            case RarityEnum.UnCommon:
                return unCommonCards[Random.Range(0, unCommonCards.Length)];
            case RarityEnum.Rare:
                return rareCards[Random.Range(0, rareCards.Length)];
            case RarityEnum.Epic:
                return epicCards[Random.Range(0, epicCards.Length)];
            case RarityEnum.Legendary:
                return legendaryCards[Random.Range(0, legendaryCards.Length)];
            default:
                return commonCards[0];
        }
    }

}
