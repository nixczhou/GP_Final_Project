using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarController2 : MonoBehaviour
{
    Image m_Image;

    public Sprite black_Sprite;
    public Sprite red_Sprite;
    public Sprite blue_Sprite;

    // Start is called before the first frame update
    void Start()
    {
        int player2_selectedCharacter = PlayerPrefs.GetInt("player2_selectedCharacter");
        m_Image = GetComponent<Image>();

        if (player2_selectedCharacter == 0)
        {
            m_Image.sprite = black_Sprite;
        }

        if (player2_selectedCharacter == 1)
        {
            m_Image.sprite = red_Sprite;
        }

        if (player2_selectedCharacter == 2)
        {
            m_Image.sprite = blue_Sprite;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
