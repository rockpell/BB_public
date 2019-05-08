using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Value : MonoBehaviour {

    GameObject Player = null;
    Transform GameOver = null;
    Transform GameClear = null;
    Transform GameEndPanel = null;
    Transform StageText = null;

    Text Bouncing;
    Text Shooting;

    int BouncingCount;
    int ShootingChance;

	// Use this for initialization
	void Start () {
        GameManger.Instance.InitTrigger();
        
        Player = GameObject.FindGameObjectWithTag("Player");
        Bouncing = transform.Find("CountText").Find("Value").GetComponent<Text>();
        Shooting = transform.Find("BulletTime").Find("Value").GetComponent<Text>();
        GameOver = transform.Find("GameOver");
        GameClear = transform.Find("GameClear");
        GameEndPanel = transform.Find("GameEndPanel");
        StageText = transform.Find("StageText");

        StartCoroutine("StageNumberIntroduce");
    }
	
	// Update is called once per frame
	void Update () {

        if (GameManger.Instance.IsGameStart)
        {
            if (GameManger.Instance.IsGameOver)
            {
                GameEndPanel.gameObject.SetActive(true);
                GameOver.gameObject.SetActive(true);
                GameManger.Instance.IsGameStart = false;
                return;
            } else if(GameManger.Instance.IsGameClear)
            {
                GameEndPanel.gameObject.SetActive(true);
                GameClear.gameObject.SetActive(true);
                GameManger.Instance.IsGameStart = false;
            }

            BouncingCount = Player.GetComponent<BounceBullet>().GetcollisionCounter();
            ShootingChance = Player.GetComponent<ShootBullet>().GetshootChance();

            if (BouncingCount < 0)
            {
                BouncingCount = 0;
            }

            Bouncing.text = "" + BouncingCount;
            Shooting.text = "" + ShootingChance;
        }
	}

    public void initProcess()
    {
        
    }

    private IEnumerator StageNumberIntroduce()
    {
        string _sceneName = SceneManager.GetActiveScene().name;
        string _sceneNumber = _sceneName.Substring(5, 3);
        int _number = int.Parse(_sceneNumber);

        if(_number > 100)
        {
            if (_number == 111) _number = 1;
            else if (_number == 222) _number = 2;
            else if (_number == 333) _number = 3;
            else if (_number == 444) _number = 4;
            else if (_number == 555) _number = 5;
        }

        StageText.Find("Value").GetComponent<Text>().text = "" + _number;

        yield return new WaitForSeconds(1.5f);

        StageText.gameObject.SetActive(false);
        GameEndPanel.gameObject.SetActive(false);
    }
}
