using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private Player player;
    private Television television;
    private LevelUI levelUI;
    private float currentFrameRate = 30f;
    private ControlPanel controlPanel;
    private bool levelIsEnded = false;
    public int currentLevel = 0;
    private bool gameOver = false;

    private ComputerExplosion computerExplosion;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        television = GameObject.FindGameObjectWithTag("Television").GetComponent<Television>();
        levelUI = GameObject.FindGameObjectWithTag("LevelUI").GetComponent<LevelUI>();
        computerExplosion = GameObject
            .FindGameObjectWithTag("ComputerExplosion")
            .GetComponent<ComputerExplosion>();

        controlPanel = GameObject
            .FindGameObjectWithTag("ControlPanel")
            .GetComponent<ControlPanel>();

        StartNewLevel(currentLevel);

        GameManager.instance.PlayMusic("Too Cool");
    }

    public bool GameIsPaused()
    {
        return television.isPaused();
    }

    void StartNewLevel(int levelNumber)
    {
        gameOver = false;
        levelIsEnded = false;
        string levelName = GetLevelName(levelNumber);
        Level levelResource = LoadFromResources(levelNumber);

        if (levelResource == null)
        {
            SceneManager.LoadScene("Thanks");
            return;
        }

        levelUI.SetLabel(levelNumber <= 0 ? "Introduction" : levelName);
        levelUI.SetTitle(levelResource.levelTitle);
        levelUI.SetCallToAction("Press ENTER to start!");
        SetLevelUIActive(true);
        ConfigAllMachinePieces(levelResource);
        television.OnVideoEnd.AddListener(() =>
        {
            SetLevelUIActive(true);
            int nextLevel = levelNumber + 1;
            Level nextLevelResource = LoadFromResources(nextLevel);
            levelUI.SetLabel("LEVEL CLEAR");
            levelUI.SetTitle("");
            levelUI.SetCallToAction("Press ENTER to Next Level!");
            levelIsEnded = true;
            gameOver = false;
        });
    }

    void ConfigAllMachinePieces(Level levelResource)
    {
        ConfigMachinePiece(
            controlPanel.processor,
            levelResource.machinePieces.Find(piece =>
            {
                return piece.type == MachinePieceType.Processor;
            })
        );

        ConfigMachinePiece(
            controlPanel.cooler,
            levelResource.machinePieces.Find(piece =>
            {
                return piece.type == MachinePieceType.Cooler;
            })
        );

        ConfigMachinePiece(
            controlPanel.memory,
            levelResource.machinePieces.Find(piece =>
            {
                return piece.type == MachinePieceType.Memory;
            })
        );

        ConfigMachinePiece(
            controlPanel.video,
            levelResource.machinePieces.Find(piece =>
            {
                return piece.type == MachinePieceType.Video;
            })
        );
    }

    void ConfigMachinePiece(MachinePiece piece, LevelMachinePieceConfig configData)
    {
        piece.autoRecoveryIncrement = configData.autoRecoveryIncrement;
        piece.wearDownDecrement = configData.wearDownDecrement;
        piece.temperaturePercent = configData.initTemperature;
        piece.performancePercent = configData.initPerformancePercent;
    }

    string GetLevelName(int levelNumber)
    {
        return levelNumber <= 0 ? "Level" : "Level " + levelNumber;
    }

    Level LoadFromResources(int levelNumber)
    {
        string levelName = GetLevelName(levelNumber);
        string path = "Scriptables/Levels/" + levelName;
        Level levelResource = Resources.Load<Level>(path);
        if (levelResource == null)
        {
            Debug.LogError(
                $"Cant load Scriptable Object in path: \"{path}\", its null, file name is {typeof(Level)}, path is Specific"
            );
        }

        return levelResource;
    }

    void SetLevelUIActive(bool active)
    {
        levelUI.gameObject.SetActive(active);
    }

    public void StartPlaying()
    {
        levelUI.gameObject.SetActive(false);
        player.SetPlaying(true);
        television.PlayVideo();
    }

    public void PausePlaying()
    {
        player.SetPlaying(false);
        television.PauseVideo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausePlaying();
        }

        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                StartNewLevel(currentLevel);
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (levelIsEnded)
            {
                currentLevel++;
                StartNewLevel(currentLevel);
                return;
            }

            StartPlaying();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentFrameRate += 5;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentFrameRate -= 5;
        }

        currentFrameRate = controlPanel.CalculateFrameRate();
        television.SetPlaybackFrameRate(currentFrameRate);

        if (controlPanel.IsWarning())
        {
            GameManager.instance.PlayMusic("Le Grand Chase");
        }

        if (controlPanel.IsCritical())
        {
            GameOver();
        }
    }

    void GameOver()
    {
        controlPanel.menu.ContractMenu();
        computerExplosion.Explode();
        gameOver = true;
        television.ResetVideo();
        GameManager.instance.PlayMusic("Too Cool");
        PausePlaying();
        SetLevelUIActive(true);
        levelUI.SetLabel("GAME OVER");
        levelUI.SetTitle("Hottest!!");
        levelUI.SetCallToAction("Press ENTER to try again!");
        currentLevel = 0;
    }
}
