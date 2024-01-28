using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class health : MonoBehaviour
{
    public TextMeshProUGUI HealthTxt;
    public GameObject Win;
    public GameObject Me;
    public GameObject Enemy;
    public Vector3 Pos1;
    public Vector3 Pos2;
    public int number = 0;
    public bool EnemyPos=false;
    public bool EnemyPos2 = false;
    private int quant = 1;
    void Start()
    {
        HealthTxt.text = "250";
        StartCoroutine(TestCoroutine());
    }

    IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(5f);
        while (int.Parse(HealthTxt.text)>0)
        {
            if (quant < 4)
            {
                Instantiate(Enemy, new Vector3(-6, 4, -2), Quaternion.identity); 
                quant++;
            }
            yield return new WaitForSeconds(5f);
        }
    }
    void Update()
    {
       if (int.Parse(HealthTxt.text) <= 0)
        {
            Win.SetActive(true);
        }

    }

    public void Kill() {
        if (quant > 0)
        {
            GameObject[] rmEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        int num = Random.Range(0, rmEnemy.Length);
            if (rmEnemy[num].GetComponent<RayScan>().number == 1)
            {
                EnemyPos = false;
            }
            else
            {
                if (rmEnemy[num].GetComponent<RayScan>().number == 2)
                {
                    EnemyPos2 = false;
                }
            }
            rmEnemy[num].GetComponent<RayScan>().number1 = 0;
            number = 0;
            if (rmEnemy.Length > 0)
            {

                foreach (GameObject mre in rmEnemy)
                {
                    if ((mre.GetComponent<RayScan>().number1 > number)&(mre.GetComponent<RayScan>().number==0))
                    {
                        number = mre.GetComponent<RayScan>().number1;
                    }
                }
            }
            else
            {
                number = 0;
            }


            Destroy(rmEnemy[num]);
        
            quant--; }
    }
}
