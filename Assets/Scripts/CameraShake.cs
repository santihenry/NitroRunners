using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update
    Animator _animator;
    [Range(0,10)]
    public float frecuency;
    float _height;
    [Range(3, 20)]
    public float heightMax;
    [Range(3, 10)]
    public float heightMultiplier;
    float _time;
    float _decayTime = 2;
    bool _shaking;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if(_height>0)
         _height -= Time.deltaTime*_decayTime;
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -9+Mathf.Sin(_time*frecuency)*_height));
        RaiseHeight(_shaking);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarMainMenu>())
        {
            //   _animator.SetTrigger("Shake");
            StartCoroutine(Shaking());
        }
    }
    void RaiseHeight(bool on)
    {
        if (on && _height<heightMax) _height += Time.deltaTime*heightMultiplier;
    }
    IEnumerator Shaking()
    {
        _shaking = true;
        yield return new WaitForSeconds(3);
        _shaking = false;
    }
}
