using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RL_Animator : MonoBehaviour {

    private Animator aliveAnim;
    private GameObject _aliveGO;
    //private RedLukerController _redLukerController;

    private void Awake() {
        _aliveGO = transform.Find(TagConstants.RED_LUKER).gameObject;
        aliveAnim = _aliveGO.GetComponent<Animator>();
        //_redLukerController = GetComponent<RedLukerController>();

        //_redLukerController.OnSetAnimatorBool += SetAnimatorBool;
    }

    private void Update() {

    }

    private void SetAnimatorBool(string paramName, bool value) {
        aliveAnim.SetBool(paramName, value);
    }

    private void SetAnimatorFloat(string paramName, float value) {
        aliveAnim.SetFloat(paramName, value);
    }
}
