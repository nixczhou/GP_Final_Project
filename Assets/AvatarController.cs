using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarController : MonoBehaviour
{
    Image m_Image;

    public Sprite black_Sprite;
    public Sprite red_Sprite;
    public Sprite blue_Sprite;

    // Start is called before the first frame update
    void Start()
    {
        int player1_selectedCharacter = PlayerPrefs.GetInt("player1_selectedCharacter");
        m_Image = GetComponent<Image>();

        if (player1_selectedCharacter == 0)
        {
            m_Image.sprite = black_Sprite;
        }

        if (player1_selectedCharacter == 1)
        {
            m_Image.sprite = red_Sprite;
        }

        if (player1_selectedCharacter == 2)
        {
            m_Image.sprite = blue_Sprite;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
