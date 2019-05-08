using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    private bool isIngameUIEnable = false;

    // Use this for initialization
    void Start() {
        Screen.fullScreen = false;
    }

    // Update is called once per frame
    void Update() {

    }

    public void MoveSceneStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void MoveSceneStageSelect2()
    {
        SceneManager.LoadScene("StageSelect2");
    }

    public void MoveSceneStageSelect3()
    {
        SceneManager.LoadScene("StageSelect3");
    }

    public void MoveSceneHardStageSelect()
    {
        SceneManager.LoadScene("HardStageSelect");
    }

    public void MoveSceneMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MoveSceneStageSelectAuto()
    {
        string _sceneName = SceneManager.GetActiveScene().name;
        string _sceneNumber = _sceneName.Substring(5, 3);
        int _number = int.Parse(_sceneNumber);
        
        if(0 < _number && _number < 16)
        {
            SceneManager.LoadScene("StageSelect");
        } else if(16 <= _number && _number <= 30)
        {
            SceneManager.LoadScene("StageSelect2");
        } else if(31<= _number && _number <= 35)
        {
            SceneManager.LoadScene("StageSelect3");
        } else
        {
            SceneManager.LoadScene("HardStageSelect");
        }
        
    }

    public void MoveSceneStage(string text)
    {
        SceneManager.LoadScene("Stage" + text);
    }

    public void MoveSceneNextStage()
    {
        string _sceneName = SceneManager.GetActiveScene().name;
        string _sceneNumber = _sceneName.Substring(5, 3);
        string _nextStageName = null;
        int _number = int.Parse(_sceneNumber);
        _number += 1;
        if (_number < 10)
        {
            if (_number > 35) MoveSceneMainMenu();
            _nextStageName = "Stage00" + _number;
        } else if (_number < 100)
        {
            _nextStageName = "Stage0" + _number;
        } else
        {
            if (_number > 444) MoveSceneMainMenu();
            _number += 110;
            _nextStageName = "Stage" + _number;
        }
        SceneManager.LoadScene(_nextStageName);
    }

    public void MoveSceneDifficultySelect()
    {
        SceneManager.LoadScene("DifficultySelect");
    }

    public void RetryScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        GameManger.Instance.ResetGame();
    }

    public void AllUnlock()
    {
        GameManger.Instance.AllUnlock();
    }

    public bool IsIngameUIEnable{
        get { return isIngameUIEnable; }
        set { isIngameUIEnable = value; }
    }
}
