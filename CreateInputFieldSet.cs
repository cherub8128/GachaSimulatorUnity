using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreateInputFieldSet : MonoBehaviour
{
    public GameObject Input;
    public GameObject Content;
    public InputField NumberInput;
    public InputField SaveNameInput;
    public Dropdown DropdownSaveList;
    private int numData = 0;
    public static List<CardData> data;
    private Stack<GameObject> dataline;
    private Vector2 lastInputFieldPos = new Vector2(0f,830f);
    private void Start()
    {
        DirectoryInfo dir = new DirectoryInfo($"{Application.dataPath}/Save/");
        NumberInput.text = "0";
        dataline = new Stack<GameObject>();

        // 저장된 파일 목록 드롭다운으로 불러오기.
        DropdownSaveList.AddOptions(Directory.GetFiles($"{Application.dataPath}/Save/","*.json")
                                                .Select(filename => Path.GetFileNameWithoutExtension(filename))
                                                    .ToList<string>());
    }
    public void addline()
    {
            GameObject temp = Instantiate(Input,Content.transform);
            temp.GetComponent<RectTransform>().anchoredPosition = lastInputFieldPos;
            lastInputFieldPos -= new Vector2(0f, 90f);
            dataline.Push(temp);
            NumberInput.text = (++numData).ToString();
    }
    public void addlines()
    {
        int exists = numData;
        int now = int.Parse(NumberInput.text);
        if (now >= exists) for (int i=0; i< now-exists; i++) addline();
        else 
        {
            for (int i = 0; i < exists-now; i++) 
            {
                Destroy(dataline.Pop());
                lastInputFieldPos += new Vector2(0f, 90f);
            }
            numData = now;
        }
    }

    //데이터 저장하기
    public void saveData()
    {
        // Inputfield의 텍스트를 파일이름으로 생성. 드롭다운에 추가.
        string saveName = SaveNameInput.text == "" ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : SaveNameInput.text;
        // 이름 중복 체크하기
        bool exists = false;
        foreach (var i in DropdownSaveList.options) 
        {
            if (i.text == SaveNameInput.text) 
            {
                exists = true;
                break;
            }
        }
        if (!exists) DropdownSaveList.options.Add(new Dropdown.OptionData(saveName));
        //else 팝업창.
        data = new List<CardData>();
        foreach (GameObject obj in dataline)
        {
            InputField[] line = obj.GetComponentsInChildren<InputField>();
            data.Add(new CardData(line[0].text, line[1].text, Double.Parse(line[2].text), int.Parse(line[3].text)));
        }
        Json.saveJson<DataSet>(saveName, new DataSet(data));
        //TODO:안드로이드에서 저장되게 하기
        //TODO:저장메시지 띄우기
    }

    // 데이터 불러오기
    public void loadData()
    {
        
        DataSet tmp = Json.loadSaveFile<DataSet>(DropdownSaveList.captionText.text);
        if (tmp != null)
        {
            // 화면초기화
            lastInputFieldPos = new Vector2(0f,830f);
            foreach (var obj in dataline) Destroy(obj);
            dataline = new Stack<GameObject>();
            numData = 0;

            // 불러온 값으로 세팅
            SaveNameInput.text = DropdownSaveList.captionText.text;
            NumberInput.text = tmp.CardsData.Count.ToString();
            addlines();
            data = tmp.CardsData;
            foreach (var obj in dataline.Zip(data, Tuple.Create))
            {
                InputField[] line = obj.Item1.GetComponentsInChildren<InputField>();
                line[0].text = obj.Item2.Grade;
                line[1].text = obj.Item2.Name;
                line[2].text = obj.Item2.Probability.ToString();
                line[3].text = obj.Item2.Number.ToString();
            }
            //TODO:로드메시지 띄우기
        }
        else print("nulldata"); //TODO:로드 실패 메시지 띄우기
    }

    //세팅 패널 닫을 때 자동저장.
    public void exitSetting()
    {
        GetComponent<Canvas>().enabled = false;
        //저장메시지 띄우기. 저장하시겠습니까?
        //saveData();
    }
}
