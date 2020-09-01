using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimMole : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private CapsuleCollider2D _capsuleCollider2D;
    [SerializeField] private AnimMole _animMole;

    public float StayDelay = 2f;
    public float ShowPeriod = 0.4f;

    private bool _tap = false;



    public void OnTap()
    {
        StopAllCoroutines();
        _tap = true;
        StartCoroutine(HideMoleAnim());
    }

    public void StartShow()
    {
        StartCoroutine(ShowMoleAnim());
    }

    private void StartHide()
    {
        StartCoroutine(HideMoleAnim());
    }

    IEnumerator ShowMoleAnim()
    {
        _tap = false;

        var time = 0f;
        var period = ShowPeriod;
        var prePos = transform.localPosition;

        while (time < period)
        {
            time += Time.deltaTime;
            var nTime = time / period;
            var lValueFill = Mathf.Lerp(0f, 1f, nTime * 1.2f);
            var lValuePos = Mathf.Lerp(prePos.y - 100f, prePos.y + 100f, nTime);

            _image.fillAmount = lValueFill;
            transform.localPosition = new Vector3(prePos.x, lValuePos);

            yield return null;
        }

        yield return new WaitForSeconds(StayDelay);

        StartHide();
    }

    IEnumerator HideMoleAnim()
    {
        var time = 0f;
        var period = 0.5f;
        var prePos = transform.localPosition;

        while (time < period)
        {
            time += Time.deltaTime;
            var nTime = time / period;
            var lValueFill = Mathf.Lerp(1f, 0f, nTime * 1.2f);
            var lValuePos = Mathf.Lerp(prePos.y, prePos.y - 200f, nTime);

            _image.fillAmount = lValueFill;
            transform.localPosition = new Vector3(prePos.x, lValuePos);

            yield return null;
        }

        _image.color = Color.white;
        _capsuleCollider2D.enabled = true;

        if (!_tap)
        {
            var menuAndScore = MenuAndScore.Instance;
            menuAndScore.CheckHp();
        }

        gameObject.SetActive(false);
    }
}
