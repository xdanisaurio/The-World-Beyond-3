using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        if (_anim == null)
            _anim = GetComponent<Animator>();
    }
    public void SetFloat(string parameter, float value)
    {
        _anim.SetFloat(parameter, value);
    }
    public void SetBool(string parameter, bool value)
    {
        _anim.SetBool(parameter, value);
    }
    public void SetTrigger(string parameter)
    {
        _anim.SetTrigger(parameter);
    }
    public void ResetTrigger(string parameter)
    {
        _anim.ResetTrigger(parameter);
    }
    public float GetFloat(string parameter)
    {
        return _anim.GetFloat(parameter);
    }
    public bool GetBool(string parameter)
    {
        return _anim.GetBool(parameter);
    }

}