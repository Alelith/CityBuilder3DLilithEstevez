using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixieRotate : MonoBehaviour
{
    [SerializeField]
    private GameObject pixie;
    [SerializeField]
    private float duration;

    private void Start()
    {
        FirstHalfLoop();
        FirstHalfTransform();
    }

    private void FirstHalfLoop()
    {
        transform.DORotate(new Vector3(0, 180, 0), duration).OnComplete(SecondHalfLoop).SetEase(Ease.InOutBounce);
    }

    private void SecondHalfLoop()
    {
        transform.DORotate(new Vector3(0, 360, 0), duration).OnComplete(FirstHalfLoop).SetEase(Ease.InOutBounce);
    }

    private void FirstHalfTransform()
    {
        pixie.transform.DOLocalMoveY(1, duration).OnComplete(SecondHalfTransform).SetEase(Ease.InOutBounce);
    }

    private void SecondHalfTransform()
    {
        pixie.transform.DOLocalMoveY(0, duration).OnComplete(FirstHalfTransform).SetEase(Ease.InOutBounce);
    }
}
