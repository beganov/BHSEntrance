using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System;

public class RayScan : MonoBehaviour
{
    private TextMeshProUGUI HealthTxt;
    public int rays = 6;
    public int distance = 15;
    public float angle = 20;
    public Vector3 offset;
    public Vector3 Posn = new Vector3(5, 1, 0);
    public Vector3[] VecPos = new Vector3[10];
    public GameObject bullet;
    public int number = 0;
    public int number1 = 0;
    private health dognail;
    private Transform target;
    private bool flagStart=false;
    private bool flagOut = false;

    void Start()
    {
        dognail = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<health>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        HealthTxt = GameObject.FindGameObjectWithTag("FagTMP").GetComponent<TextMeshProUGUI>();
    }

    bool GetRaycast(Vector3 dir)
    {
        bool result = false;
        RaycastHit hit = new RaycastHit();
        Vector3 pos = transform.position + offset;
        if (Physics.Raycast(pos, dir, out hit, distance))
        {
            if (hit.transform == target)
            {
                result = true;
                Debug.DrawLine(pos, hit.point, Color.green);
            }
            else
            {
                Debug.DrawLine(pos, hit.point, Color.blue);
            }
        }
        else
        {
            Debug.DrawRay(pos, dir * distance, Color.red);
        }
        return result;
    }

    bool RayToScan()
    {
        bool result = false;
        bool a = false;
        bool b = false;
        float j = 0;
        for (int i = 0; i < rays; i++)
        {
            var x = Mathf.Sin(j);
            var y = Mathf.Cos(j);

            j += angle * Mathf.Deg2Rad / rays;

            Vector3 dir = transform.TransformDirection(new Vector3(x, -y, 0));
            if (GetRaycast(dir)) a = true;

            if (x != 0)
            {
                dir = transform.TransformDirection(new Vector3(-x, -y, 0));
                if (GetRaycast(dir)) b = true;
            }
        }

        if (a || b) result = true;
        return result;
    }
    void Update()
    {
        //transform.LookAt(target.position);
        //transform.rotation = Quaternion.LookRotation(transform.position - target.position);
        // rotate = transform.eulerAngles;
        //rotate.y = rotate.y - 90;
        // = rotate.y + 60;
        //rotate.y = 0;
        
 
        if (Vector3.Distance(transform.position, target.position) < distance)
        {
            
            if ((!flagStart) &(RayToScan()))
            {

                flagStart = true;
                if (!dognail.EnemyPos)
                {
                    dognail.EnemyPos = true;
                    Posn = dognail.Pos1;
                    number = 1;
                }
                else {
                    if (!dognail.EnemyPos2)
                    {
                        dognail.EnemyPos2 = true;
                        Posn = dognail.Pos2;
                        number = 2;
                    }
                }
                StartCoroutine(Firing());
            }

            if (!flagStart)
            {
                transform.position = new Vector3(transform.position.x + 0.005f, transform.position.y, transform.position.z);
            }
            if (flagStart)
            {
                transform.position = ((Posn + transform.position*999) / 1000);
                Vector3 rotate = transform.eulerAngles;
                rotate.z = Mathf.Atan(-transform.position.x / (transform.position.y - target.position.y)) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(rotate);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out RayScan ors))
        {
            transform.Translate(0, (this.transform.position.y - collision.gameObject.transform.position.y) / 2, 0);
        }
        else
        {
            if (collision.gameObject.tag == "Wall")
            {
               transform.position = new Vector3(transform.position.x *9/10, transform.position.y + (transform.position.y - collision.gameObject.transform.position.y)  / 10, transform.position.z);
    
            }
        }
    }
    IEnumerator Firing()
    {
        while (int.Parse(HealthTxt.text) > 0)
        {

            if (Vector3.Distance(Posn, transform.position) < 1)
            {
                if (number == 0)
                {
                    dognail = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<health>();
                    if (Vector3.Distance(Posn, VecPos[number1]) < 1)
                    {

                        if (number1 > 4)
                        {
                            number1 = 1;
                        }
                        else
                        {
                            if (number1 == 0)
                            {
                                number1 = dognail.number + 1;
                            }
                            else
                            {
                                number1++;
                            }
                        }
                        Posn = VecPos[number1];
                        dognail.number = number1;

                    }

                    

                }
                else
                {
                    if (!flagOut)
                    {
                        Posn.x = Posn.x / 4;
                        flagOut = true;
                    }
                    else
                    {
                        Posn.x = Posn.x * 4;
                        flagOut = false;
                    }
                }

            }
            if ((!dognail.EnemyPos) &(number == 0))
            {
                dognail.EnemyPos = true;
                number = 1;
                number1 = 0;
                Posn = dognail.Pos1;
                dognail.number = 0;
                GameObject[] rmEnemy = GameObject.FindGameObjectsWithTag("Enemy");
                if (rmEnemy.Length > 0)
                {

                    foreach (GameObject mre in rmEnemy)
                    {
                        if ((mre.GetComponent<RayScan>().number1 > dognail.number) & (mre.GetComponent<RayScan>().number == 0))
                        {
                            dognail.number = mre.GetComponent<RayScan>().number1;
                        }
                    }
                }
                else
                {
                    dognail.number = 0;
                }


            }

            if ((!dognail.EnemyPos2) & (number == 0))
            {
                dognail.EnemyPos2 = true;
                number = 2;
                number1 = 0;
                Posn = dognail.Pos2;
                dognail.number = 0;
                GameObject[] rmEnemy = GameObject.FindGameObjectsWithTag("Enemy");
                if (rmEnemy.Length > 0)
                {

                    foreach (GameObject mre in rmEnemy)
                    {
                        if ((mre.GetComponent<RayScan>().number1 > dognail.number) & (mre.GetComponent<RayScan>().number == 0))
                        {
                            dognail.number = mre.GetComponent<RayScan>().number1;
                        }
                    }
                }
                else
                {
                    dognail.number = 0;
                }
            }
            int newName = Convert.ToInt32(Vector3.Distance(transform.position, target.position));
            Debug.Log(newName);
            if ((RayToScan()) & (newName < distance))
            {

                Instantiate(bullet, (transform.position* newName + target.position) / (newName+1), Quaternion.identity);
            }
             
            yield return new WaitForSeconds(0.5f);

        }
    }

}