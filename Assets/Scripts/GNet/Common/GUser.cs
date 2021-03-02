using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DeepGes
{

    public class GUser : MonoBehaviour
    {



        //Raycast 필요 변수
        [HideInInspector]
        public Vector3 rayHitPoint;
        //Vector Localized 필요 변수 
        [HideInInspector]
        public Transform headTransform;


        void Start()
        {
            headTransform = transform.Find("Perpindicular_head");
            if (headTransform == null)
            {
                headTransform = new GameObject("Perpindicular_head").transform;
                headTransform.parent = this.transform;
            }


        }


        //벡터 로컬라이징 함수 
        public Vector3 getLocalizedPoint(Vector3 rawVector, Transform player_head)
        {
            // 유저의 머리 위치 갱신 
            headTransform.position = player_head.position;
            //Y axis 기준 회전 
            headTransform.rotation = Quaternion.Euler(0, player_head.eulerAngles.y, 0);

            //InverseTransformPoint 함수 적용 
            return headTransform.InverseTransformPoint(rawVector);
        }

        //레이캐스트(마우스) 함수
        public GameObject DetectRaycast_Mouse()
        {
            RaycastHit hit;
            GameObject target = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 포인트 근처 좌표를 만든다. 

            //Ray ray = new Ray(
            //    Camera.main.transform.FindChild("dir_start").transform.position,
            //    Camera.main.transform.FindChild("dir_end").transform.position - Camera.main.transform.FindChild("dir_start").transform.position
            //    );
            if (Physics.Raycast(ray.origin, ray.direction, out hit) == true)   //마우스 근처에 오브젝트가 있는지 확인
            {
                //있으면 오브젝트를 저장한다.
                target = hit.collider.gameObject;
                rayHitPoint = hit.point;
            }
            return target;
        }

    }
}