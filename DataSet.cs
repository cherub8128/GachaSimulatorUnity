using System;
using System.Collections.Generic;

[Serializable]
public class DataSet
{
    public List<CardData> CardsData = null;
    public DataSet(List<CardData> cardsData)
    {
        CardsData = cardsData;
    }

}

[Serializable]
public class CardData
{
    public CardData(string grade, string name, double prob, int num)
    {
        Grade = grade;
        Name = name;
        Probability = prob;
        Number = num;
    }
    public string Grade = "";
    public string Name = "";
    public double Probability = 0;
    public int Number = 0;
}