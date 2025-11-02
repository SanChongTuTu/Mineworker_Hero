using UnityEngine;
using UnityEngine.UI;

public class PlayerSet : MonoBehaviour
{
    [Header("���ý���")]
    public GameObject sets;
    [Header("�������˵�ȷ�Ͻ���")]
    public GameObject ifbackhome;
    [Header("������ͼ��")]
    public Sprite openvoice;
    [Header("�ر�����ͼ��")]
    public Sprite closevoice;

    [Header("������")]
    [Range(0, 100)]
    public float totalvolumn;
    public bool istotalvolumn;
    [Header("��Ϸ��������")]
    [Range(0, 100)]
    public float volumn;
    public bool isvolumn;
    [Header("��Ч")]
    [Range(0, 100)]
    public float voiceeffect;
    public bool isvoiceeffect;

    private Button setbtn;//���������ð�ť
    private Slider totalvolumnslider;//������������
    private Slider volumnslider;//�������ֻ�����
    private Slider voiceeffectslider;//��Ч������
    private Button istotalvolumnbutton;//������һ��������ť
    private Button isvolumnbutton;//��������һ��������ť
    private Button isvoiceeffectbutton;//��Чһ��������ť

    private static PlayerSet instance;
    public static PlayerSet Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerSet>();
                if (instance == null)
                {
                    Debug.Log("No PlayerSet");
                }
            }
            return instance;
        }
    }

    void Start()
    {
        totalvolumnslider = sets.transform.GetChild(2).GetChild(0).GetComponent<Slider>();
        volumnslider = sets.transform.GetChild(2).GetChild(1).GetComponent<Slider>();
        voiceeffectslider = sets.transform.GetChild(2).GetChild(2).GetComponent<Slider>();
        istotalvolumnbutton=sets.transform.GetChild(2).GetChild(3).GetComponent<Button>();
        isvolumnbutton = sets.transform.GetChild(2).GetChild(4).GetComponent<Button>();
        isvoiceeffectbutton=sets.transform.GetChild(2).GetChild(5).GetComponent<Button>();

        sets.SetActive(false);
        ifbackhome.SetActive(false);

        totalvolumn = PlayerPrefs.GetFloat("TotalVolumn", 100f);
        volumn = PlayerPrefs.GetFloat("Volumn", 100f);
        voiceeffect = PlayerPrefs.GetFloat("VoiceEffect", 100f);
        istotalvolumn = PlayerPrefs.GetInt("IsTotalVolumn",1)==1;
        isvolumn = PlayerPrefs.GetInt("IsVolumn", 1) == 1;
        isvoiceeffect = PlayerPrefs.GetInt("IsVoiceEffect", 1) == 1;

        totalvolumnslider.value = istotalvolumn?totalvolumn :0;
        volumnslider.value = isvolumn?volumn:0;
        voiceeffectslider.value = isvoiceeffect?voiceeffect:0;
        
    }


    void Update()
    {
        if (setbtn == null)
        {
            setbtn = GameObject.Find("SetBtn").GetComponent<Button>();
            setbtn.onClick.RemoveAllListeners();
            setbtn.onClick.AddListener(() =>
            {
                sets.SetActive(true);
            });
        }

        if(totalvolumnslider.value != totalvolumn)
        {
            totalvolumn = totalvolumnslider.value;
            istotalvolumn = totalvolumn > 0;
            PlayerPrefs.DeleteKey("IsTotalVolumn");
            PlayerPrefs.SetInt("IsTotalVolumn", istotalvolumn?1:0);
            PlayerPrefs.Save();

            if (istotalvolumn)
            {
                PlayerPrefs.DeleteKey("TotalVolumn");
                PlayerPrefs.SetFloat("TotalVolumn", totalvolumn);
                PlayerPrefs.Save();
            }
        }

        if(volumnslider.value != volumn)
        {
            volumn = volumnslider.value;
            isvolumn = volumn > 0;
            PlayerPrefs.DeleteKey("IsVolumn");
            PlayerPrefs.SetInt("IsVolumn", isvolumn?1:0);
            PlayerPrefs.Save();

            if (isvolumn)
            {
                PlayerPrefs.DeleteKey("Volumn");
                PlayerPrefs.SetFloat("Volumn", volumn);
                PlayerPrefs.Save();
            }
        }

        if(voiceeffectslider.value != voiceeffect)
        {
            voiceeffect = voiceeffectslider.value;
            isvoiceeffect = voiceeffect > 0;
            PlayerPrefs.DeleteKey("IsVoiceEffect");
            PlayerPrefs.SetInt("IsVoiceEffect", isvoiceeffect?1:0);
            PlayerPrefs.Save();

            if (isvoiceeffect)
            {
                PlayerPrefs.DeleteKey("VoiceEffect");
                PlayerPrefs.SetFloat("VoiceEffect", voiceeffect);
                PlayerPrefs.Save();
            }
        }

        istotalvolumnbutton.image.sprite = istotalvolumn?openvoice:closevoice;
        isvolumnbutton.image.sprite = isvolumn ? openvoice : closevoice;
        isvoiceeffectbutton.image.sprite = isvoiceeffect ? openvoice : closevoice;
    }

    public void ClickTotal()
    {
        istotalvolumn=!istotalvolumn;
        totalvolumnslider.value = istotalvolumn ? PlayerPrefs.GetFloat("TotalVolumn",100) : 0;
        if (totalvolumnslider.value <1&&istotalvolumn)
        {
            totalvolumnslider.value = 1;
        }
    }

    public void ClickVolumn()
    {
        isvolumn = !isvolumn;
        volumnslider.value = isvolumn ? PlayerPrefs.GetFloat("Volumn", 100) : 0;
        if (volumnslider.value <1 && isvolumn)
        {
            volumnslider.value = 1;
        }
    }

    public void ClickVoiceEffect()
    {
        isvoiceeffect = !isvoiceeffect;
        voiceeffectslider.value = isvoiceeffect ? PlayerPrefs.GetFloat("VoiceEffect", 100) : 0;
        if (voiceeffectslider.value <1 && isvoiceeffect)
        {
            voiceeffectslider.value = 1;
        }
    }
}
