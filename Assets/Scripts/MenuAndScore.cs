using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuAndScore : MonoBehaviour
{
    [SerializeField] private Text _scoreCount;
    [SerializeField] private Text _hpCount;

    [SerializeField] private Text _sliderHoleValueText;
    [SerializeField] private Slider _sliderHole;

    [SerializeField] private GameObject _grid;
    [SerializeField] private GameObject _retryButton;

    [SerializeField] private GameObject _scoreSave;
    [SerializeField] private GameObject _scoreSaveText;

    [SerializeField] private HoleSpawner _holeSpawnerGrid;
    [SerializeField] private MoleSpawner _moleSpawnerGrid;
    [SerializeField] private SaveScore _saveScore;

    private int _score = 0;
    private int _count = 0;
    private float _scoreDecimal = 15f;

    private int _hp = 5;
    private const float _scoreDecimalPlus = 15f;
    private const float _showPeriodDecimal = 1.5f;
    private const float _stayDelayDecimal = 2f;

    private static MenuAndScore _instance;

    public static MenuAndScore Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MenuAndScore>();
            }

            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;

        _count = _holeSpawnerGrid.HoleCount;
        Application.targetFrameRate = 60;
    }

    public void PlayButton()
    {
        _scoreSave.SetActive(false);
        _scoreSaveText.SetActive(false);

        _holeSpawnerGrid.SpawnHole();
        _moleSpawnerGrid.SpawnMole();
    }

    public void PlusScore()
    {
        _score++;
        _scoreCount.text = _score.ToString();

        SetDifficulty();
    }

    private void SetDifficulty()
    {
        var moleCount = _moleSpawnerGrid.ActiveMoles.Count;

        if (_score == _scoreDecimal)
        {
            if (moleCount < _count)
            {
                _moleSpawnerGrid.SpawnMole();
            }

            foreach (var mole in _moleSpawnerGrid.ActiveMoles)
            {
                mole.GetComponent<AnimMole>().ShowPeriod = mole.GetComponent<AnimMole>().ShowPeriod / _showPeriodDecimal;
                mole.GetComponent<AnimMole>().StayDelay = mole.GetComponent<AnimMole>().StayDelay / _stayDelayDecimal;
            }

            _scoreDecimal += _scoreDecimalPlus;
        }
    }

    public void CheckHp()
    {
        _hp -= 1;
        _hpCount.text = _hp.ToString();

        if (_hp < 1)
        {
            _moleSpawnerGrid.StopAllCoroutines();

            _hpCount.text = "0";
            _retryButton.transform.localScale = Vector3.one;
            _sliderHole.transform.localScale = Vector3.one;

            _moleSpawnerGrid.ReturnMole();
            _holeSpawnerGrid.ReturnHole();

            _scoreSave.SetActive(true);
            _scoreSaveText.SetActive(true);
            _saveScore.Save();
        }
    }

    public void HideButton(GameObject button)
    {
        button.transform.localScale = Vector3.zero;
        _sliderHole.transform.localScale = Vector3.zero;
    }

    public void RetryButton()
    {
        _hp = 5;
        _hpCount.text = _hp.ToString();

        _score = 0;
        _scoreCount.text = _score.ToString();

        _scoreDecimal = 15;
    }

    public void SliderHole()
    {
        _sliderHoleValueText.text = _sliderHole.value.ToString();
        _holeSpawnerGrid.HoleCount = (int)_sliderHole.value;
    }
}
