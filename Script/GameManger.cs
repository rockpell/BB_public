using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour {

	public int normalLevel { get; set; }
	public int hardLevel { get; set; }

    private Ads ads;
    private int leftCountPlay = 0;
    private const int playCountForAd = 6;

	private bool isGameStart = false;
    private bool isGameOver = false;
    private bool isGameClear = false;
    private bool isPlayerDead = false;
    private bool isStageEnd = false;

    private static GameManger instance;

    public static GameManger Instance {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<GameManger>();
            DontDestroyOnLoad(this.gameObject);
            ads = GetComponent<Ads>();
            Input.multiTouchEnabled = false;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Use this for initialization
    void Start ()
	{
		if (!PlayerPrefs.HasKey("NormalLevel"))
		{
			normalLevel = 1;
			PlayerPrefs.SetInt("NormalLevel", 1);
		}
		else normalLevel = PlayerPrefs.GetInt("NormalLevel");

		if(!PlayerPrefs.HasKey("HardLevel"))
		{
			hardLevel = 1;
			PlayerPrefs.SetInt("HardLevel", 1);
		}
		else hardLevel = PlayerPrefs.GetInt("HardLevel");
	}
	
	// Update is called once per frame
	void Update () {
        if (isGameStart)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                isGameClear = true;
                string _sceneName = SceneManager.GetActiveScene().name;
                string _sceneNumber = _sceneName.Substring(5, 3);
                int temp = 0;
                if (int.TryParse(_sceneNumber, out temp))
                {
                    int _nowLevel = temp;
                    if (_nowLevel > 100)
                    {
                        _nowLevel /= 111;
                        ClearGame(_nowLevel, true);
                    } else
                    {
                        ClearGame(_nowLevel, false);
                    }
                }

                if (!isStageEnd)
                {
                    isStageEnd = true;
                    if(leftCountPlay > playCountForAd)
                    {
                        leftCountPlay = 0;
                        ads.ShowAds();
                    } else
                    {
                        leftCountPlay += 1;
                    }
                }
            }
            if (isPlayerDead)
            {
                if (!isGameClear)
                {
                    isGameOver = true;
                    if (!isStageEnd)
                    {
                        isStageEnd = true;
                        if (leftCountPlay > playCountForAd)
                        {
                            leftCountPlay = 0;
                            ads.ShowAds();
                        }
                        else
                        {
                            leftCountPlay += 1;
                        }
                    }
                }
            }
        }
	}

    public void InitTrigger()
    {
        GameManger.Instance.IsGameStart = true;
        GameManger.Instance.IsGameOver = false;
        GameManger.Instance.IsGameClear = false;
        GameManger.instance.isStageEnd = false;
    }

    public bool IsGameStart {
        get { return isGameStart; }
        set { isGameStart = value; }
    }

    public bool IsGameOver {
        get { return isGameOver; }
        set { isGameOver = value; }
    }

    public bool IsGameClear {
        get { return isGameClear; }
        set { isGameClear = value; }
    }

    public bool IsPlayerDead {
        get { return isPlayerDead; }
        set { isPlayerDead = value; }
    }

    public bool IsStageEnd {
        get { return IsStageEnd; }
        set { IsStageEnd = value; }
    }

	public void ClearGame(int level, bool isHard = false)
	{
		if(isHard)
		{
			//int tmp = PlayerPrefs.GetInt("HardLevel");
			hardLevel = hardLevel == level ? level + 1 : hardLevel;
			PlayerPrefs.SetInt("HardLevel", hardLevel);
		}
		else
		{
			//int tmp = PlayerPrefs.GetInt("NormalLevel");
			normalLevel = normalLevel == level ? level + 1 : normalLevel;
			PlayerPrefs.SetInt("NormalLevel", normalLevel);
		}
	}
	public void ResetGame()
	{
		//PlayerPrefs.DeleteAll();
		normalLevel = 1;
		PlayerPrefs.SetInt("NormalLevel", 1);

		hardLevel = 1;
		PlayerPrefs.SetInt("HardLevel", 1);
	}
	public void AllUnlock()
	{
		normalLevel = 99;
		PlayerPrefs.SetInt("NormalLevel", 99);

		hardLevel = 9;
		PlayerPrefs.SetInt("HardLevel", 9);
	}
}
