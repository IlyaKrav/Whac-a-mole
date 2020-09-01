using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveScore : MonoBehaviour
{
    [SerializeField] private Text _score;
    [SerializeField] private List<Text> _scoreRows;

    private void Awake()
    {
        Load();
    }

    public void Save()
    {
        if (!int.TryParse(_score.text, out var newScore))
        {
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.HasKey(i.ToString()))
            {
                var scoreStr = PlayerPrefs.GetString(i.ToString());
                if (!int.TryParse(scoreStr, out var score))
                {
                    return;
                }

                if (newScore > score)
                {
                    var newI = i + 1;

                    while (newI < 3)
                    {
                        _scoreRows[newI].text = PlayerPrefs.GetString((newI - 1).ToString());
                        newI++;
                    }

                    PlayerPrefs.SetString(i.ToString(), newScore.ToString());
                    PlayerPrefs.Save();
                    _scoreRows[i].text = newScore.ToString();

                    break;
                }

                continue;
            }

            PlayerPrefs.SetString(i.ToString(), newScore.ToString());
            PlayerPrefs.Save();
            _scoreRows[i].text = newScore.ToString();

            break;
        }
    }

    private void Load()
    {
        for (int i = 0; i < _scoreRows.Count; i++)
        {
            _scoreRows[i].text = PlayerPrefs.GetString(i.ToString());
        }
    }
}
