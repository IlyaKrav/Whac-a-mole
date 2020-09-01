using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleSpawner : MonoBehaviour
{
    public int HoleCount;

    [SerializeField] private GameObject _prefabHole;
    [SerializeField] private Transform _saveGrid;

    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private RectTransform _rectTransformCanvas;

    private int _prefabWeight;

    public List<Transform> ActiveHolse = new List<Transform>();
    private Stack<Transform> _disactiveHolse = new Stack<Transform>();

    private const int _maxHoles = 25;

    private void Awake()
    {
        for (int i = 0; i < _maxHoles; i++)
        {
            var hole = Instantiate(_prefabHole, _saveGrid).transform;
            _disactiveHolse.Push(hole);
        }
    }

    public void SpawnHole()
    {
        GetHole();

        var minSize = Mathf.Min(_rectTransformCanvas.sizeDelta.x, _rectTransformCanvas.sizeDelta.y);
        _prefabWeight = (int)(minSize / HoleCount - _gridLayoutGroup.spacing.x);

        _gridLayoutGroup.constraintCount = HoleCount;
        _gridLayoutGroup.cellSize = new Vector2(_prefabWeight, _prefabWeight);
    }

    private void GetHole()
    {
        var countHole = HoleCount * HoleCount;

        for (int i = 0; i < countHole; i++)
        {
            var hole = _disactiveHolse.Pop();
            ActiveHolse.Add(hole);
            hole.SetParent(transform);
            hole.localScale = Vector3.one;
        }
    }

    public void ReturnHole()
    {
        var count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            var hole = ActiveHolse[i];
            _disactiveHolse.Push(hole);
            hole.SetParent(_saveGrid.transform);
        }

        ActiveHolse.Clear();
    }
}
