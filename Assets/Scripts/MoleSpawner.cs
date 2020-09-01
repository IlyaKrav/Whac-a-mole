using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleSpawner : MonoBehaviour
{
    [SerializeField] private AnimMole _prefabMole;

    [SerializeField] private HoleSpawner _holeSpawner;

    private Stack<AnimMole> _disactiveMoles = new Stack<AnimMole>();
    public Stack<AnimMole> ActiveMoles = new Stack<AnimMole>();

    private Transform _startGrid;

    public void SpawnMole()
    {
        _startGrid = _holeSpawner.ActiveHolse[0];

        AnimMole mole;

        if (_disactiveMoles.Count < _holeSpawner.HoleCount && _disactiveMoles.Count == 0)
        {
            var moleGameObject = Instantiate(_prefabMole.gameObject, _startGrid);
            moleGameObject.gameObject.SetActive(false);
            mole = moleGameObject.GetComponent<AnimMole>();
            ActiveMoles.Push(mole);
        }
        else
        {
            mole = _disactiveMoles.Pop();
            ActiveMoles.Push(mole);
        }

        var count = _holeSpawner.HoleCount;
        var moleScale = 2f / count;
        mole.transform.localScale = new Vector3(moleScale, moleScale);

        StartCoroutine(StartSetMoleToHole());
    }

    public void ReturnMole()
    {
        foreach (var mole in ActiveMoles)
        {
            mole.ShowPeriod = 0.4f;
            mole.StayDelay = 2f;
            _disactiveMoles.Push(mole);
            mole.gameObject.SetActive(false);
        }

        ActiveMoles.Clear();
    }

    IEnumerator StartSetMoleToHole()
    {
        while (true)
        {
            yield return StartCoroutine(SetMoleToHole());
        }
    }

    IEnumerator SetMoleToHole()
    {
        var rand = Random.Range(0, transform.childCount);

        foreach (var mole in ActiveMoles)
        {
            if (!mole.gameObject.activeSelf && _holeSpawner.ActiveHolse[rand].childCount == 0)
            {
                mole.gameObject.SetActive(true);
                mole.transform.SetParent(_holeSpawner.ActiveHolse[rand]);
                mole.transform.localPosition = Vector3.zero;
                mole.StartShow();

                break;
            }
        }

        yield return null;
    }
}
