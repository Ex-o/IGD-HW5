using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    private Animator _animator;

    private float _vX = 0.0f;
    private float _vZ = 0.0f;
    private int _hash_vX;
    private int _hash_vZ;

    private const float _ACCELERATION = 2.0f;
    private const float _DECELERATION = 3.0f;
    private const float _MAX_VELOCITY = 1.0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _hash_vX = Animator.StringToHash("Velocity X");
        _hash_vZ = Animator.StringToHash("Velocity Z");
    }

    private static void CheckControlls(bool dir, bool opposite_dir, ref float velocity)
    {
        if (!dir && !opposite_dir &&
            velocity > -0.06f && velocity < 0.06f)
        {
            velocity = 0.0f;
        }
    }

    private static float GetInterval(float x)
    {
        return Time.deltaTime * x;
    }
     
    private static void ChangeSpeed(bool dir, bool opposite_dir, ref float velocity, bool side)
    {
        float nMAXV = _MAX_VELOCITY;
         
        if (!side && dir && !opposite_dir)
        {
            nMAXV *= 1.3f;
        }
        else if (!side && opposite_dir && !dir)
        {
            nMAXV *= 0.9f;
        }

        if (side)
        {
            nMAXV *= 1.2f;
        }

        if (dir)
        {
            if (velocity < nMAXV)
            {
                velocity += GetInterval(_ACCELERATION);
            }
        }
        else
        {
            if (velocity > 0)
            {
                velocity -= GetInterval(_DECELERATION);
            }
        }

        if (opposite_dir)
        {
            if (velocity > -nMAXV)
            {
                velocity -= GetInterval(_ACCELERATION);
            }
        }
        else
        {
            if (velocity < 0)
            {
                velocity += GetInterval(_DECELERATION);
            }
        }

        CheckControlls(dir, opposite_dir, ref velocity);
        Debug.Log(velocity);
    }

    private void Update()
    {
        var wDown = Input.GetKey(KeyCode.W);
        var sDown = Input.GetKey(KeyCode.S);
        var aDown = Input.GetKey(KeyCode.A);
        var dDown = Input.GetKey(KeyCode.D);
        
        ChangeSpeed(wDown, sDown, ref _vZ, false);
        ChangeSpeed(dDown, aDown, ref _vX, true);
        
        _animator.SetFloat(_hash_vX, _vX);
        _animator.SetFloat(_hash_vZ, _vZ);
    }
}
