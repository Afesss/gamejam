using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] Animation _animation;

    private void Awake()
    {
        _animation.Play();
    }
}
