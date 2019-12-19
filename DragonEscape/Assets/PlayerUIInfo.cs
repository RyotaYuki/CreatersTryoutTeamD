using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _UIs;//
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void UIFeedIn()
    {
        foreach (GameObject ui in _UIs)
        {
            Color c = Color.black;
            if (ui.GetComponent<Image>())
            {
                Image image = ui.GetComponent<Image>();
                c = image.color;
                image.color = new Color(c.r, c.g, c.b, c.a + 0.1f);
            }
            if (ui.GetComponent<Text>())
            {
                Text text = ui.GetComponent<Text>();
                c = text.color;
                text.color = new Color(c.r, c.g, c.b, c.a + 0.1f);
            }
        }
    }

    void UIFeedOut()
    {

    }
}
