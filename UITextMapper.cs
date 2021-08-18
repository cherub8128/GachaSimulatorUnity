// TODO 커스텀 컴포넌트화 해보자.

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//nullable
public class UITextMapper : MonoBehaviour
{
    private TextMeshProUGUI[] TextMeshProTexts;
    private Text[] Texts;
    public static byte currLocale {get; set;} = 0;
    
    private void Start() 
    {
        TextMeshProTexts = GetComponentsInChildren<TextMeshProUGUI>();
        Texts = GetComponentsInChildren<Text>();
        for (int i=0; i<TextMeshProTexts.Length; i++) TextMeshProTexts[i].text = Json.UIText[TextMeshProTexts[i].name][currLocale];
        for (int i=0; i<Texts.Length; i++) Texts[i].text = Json.UIText[Texts[i].name][currLocale];
    }
}