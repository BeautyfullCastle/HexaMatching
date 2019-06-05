using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject hexagonPref;
    [SerializeField]
    private Transform parentTr;
    [SerializeField]
    private Transform originTr;
    [SerializeField]
    private Transform toTr;

    [SerializeField]
    private Vector3 vec;
    private Vector3 dir;
    private float dist;


    private void Awake()
    {
        vec = toTr.localPosition - originTr.localPosition;
        Debug.Log(vec.magnitude);
        //vec.z = 0f;
        dir = vec.normalized;
        dist = vec.magnitude;

        var obj = Instantiate(hexagonPref, parentTr);
        obj.transform.Translate(dir * dist);

        //obj = Instantiate(hexagonPref, parentTr);
        //obj.transform.Translate(vec * -1);

        float angle = 45f * Mathf.Deg2Rad;
        var rotZ = new Matrix4x4()
        {
            m00 = Mathf.Cos(angle),
            m01 = -Mathf.Sin(angle),
            m10 = Mathf.Sin(angle),
            m11 = Mathf.Cos(angle),
            m22 = 1f,
            m33 = 1f
        };
        var rotX = new Matrix4x4()
        {
            m00 = 1f,
            m11 = Mathf.Cos(angle),
            m12 = -Mathf.Sin(angle),
            m21 = Mathf.Sin(angle),
            m22 = Mathf.Cos(angle),
            m33 = 1f
        };
        var rotY = new Matrix4x4()
        {
            m00 = Mathf.Cos(angle),
            m02 = Mathf.Sin(angle),
            m11 = 1f,
            m20 = -Mathf.Sin(angle),
            m22 = Mathf.Cos(angle),
            m33 = 1f
        };
        var rotM = rotX * rotY * rotZ;
        //dir = rotM.MultiplyPoint(dir);

        var transM = new Matrix4x4()
        {
            m00 = 1f,
            m03 = 0.5f,
            m11 = 1f,
            m13 = 0.75f,
            m22 = 1f,
            m23 = 0f,
            m33 = 1f
        };
        var M = /*transM * */ rotM;
        var newV = M.MultiplyPoint(dir);

        obj = Instantiate(hexagonPref, parentTr);
        //float d = Mathf.Sqrt((0.5f * 0.5f) + (0.75f * 0.75f));
        //var v = new Vector3(dir.x * 0.5f, dir * 0.75f);
        obj.transform.Translate(newV * dist);// * d);

        //obj = Instantiate(hexagonPref, parentTr);
        //obj.transform.Translate(vec * -1);

        dir = rotM.MultiplyVector(dir);
        newV = M.MultiplyPoint(dir);
        obj = Instantiate(hexagonPref, parentTr);
        obj.transform.Translate(newV * dist);

        //obj = Instantiate(hexagonPref, parentTr);
        //obj.transform.Translate(vec * -1);
    }

    public struct Matrix3x3
    {
        public float m00;
        public float m01;
        public float m02;
        public float m10;
        public float m11;
        public float m12;
        public float m20;
        public float m21;
        public float m22;

        public Vector3 Rotate(Vector3 vector)
        {
            vector.x = vector.x * m00
                + vector.y * m10
                + vector.z * m20;
            vector.y = vector.x * m01
                + vector.y * m11
                + vector.z * m21;
            vector.z = vector.x * m02
                + vector.y * m12
                + vector.z * m22;

            return vector;
        }
    }
}
