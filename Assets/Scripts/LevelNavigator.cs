using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNavigator : MonoBehaviour
{
    private GameObject _currentLevel;
    private bool _justMoved;
    private readonly Vector3 _offset = Vector3.up * 0.5f;
    private LevelIndicator _levelIndicator;
    private bool _start = true;

    [SerializeField] private GameObject [] levelIndicators; 
    private void Start()
    {
        var highestLevel = GameManager.Instance.GetHighestLevelCompleted();
        if (highestLevel < 0) highestLevel = 0;
        _currentLevel = levelIndicators[highestLevel];
        transform.position = _currentLevel.transform.position + _offset;
        _justMoved = false;
        _levelIndicator = _currentLevel.GetComponent<LevelIndicator>();
    }

    private void Update()
    {
        if (_start)
        {
            var highestLevel = GameManager.Instance.GetHighestLevelCompleted();
            if (highestLevel < 0) highestLevel = 0;
            for (var i = 0; i < levelIndicators.Length; i++)
            {
                if (GameManager.Instance.GetLevelIsCompleted(i))
                {
                    levelIndicators[i].GetComponent<LevelIndicator>().SetColor(Color.green);
                }
            }
            levelIndicators[highestLevel].GetComponent<LevelIndicator>().SetColor(Color.blue);
            _start = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && !_levelIndicator.levelSceneName.Equals(""))
        {
            if (_levelIndicator.SceneNumber <= GameManager.Instance.GetHighestLevelCompleted() + 1)
            {
                Debug.Log(_levelIndicator.SceneNumber);
                Debug.Log(GameManager.Instance.GetHighestLevelCompleted() + 1);
                SceneManager.LoadScene(_levelIndicator.levelSceneName);
                
            }
        }
           

        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        if (_justMoved)
        {
            if (horizontalInput == 0 && verticalInput == 0)
            {
                _justMoved = false;
            }
        }
        else if (horizontalInput > 0 && _levelIndicator.HasRightLocation())
        {
            _currentLevel = _levelIndicator.rightLocation;
            _levelIndicator = _currentLevel.GetComponent<LevelIndicator>();
            _justMoved = true;
        }
        else if (horizontalInput < 0 && _levelIndicator.HasLeftLocation())
        {
            _currentLevel = _levelIndicator.leftLocation;
            _levelIndicator = _currentLevel.GetComponent<LevelIndicator>();
            _justMoved = true;
        }
        else if (verticalInput < 0 && _levelIndicator.HasDownLocation())
        {
            _currentLevel = _levelIndicator.downLocation;
            _levelIndicator = _currentLevel.GetComponent<LevelIndicator>();
            _justMoved = true;
        }
            
        else if (verticalInput > 0 && _levelIndicator.HasUpLocation())
        {
            _currentLevel = _levelIndicator.upLocation;
            _levelIndicator = _currentLevel.GetComponent<LevelIndicator>();
            _justMoved = true;
        }
        transform.position = _currentLevel.transform.position + _offset;
    }
}