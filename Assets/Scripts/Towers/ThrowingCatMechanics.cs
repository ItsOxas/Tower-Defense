using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ThrowingCatMechanics : MonoBehaviour
{
    public RangeEnemyTargeting ret;
    public Transform target;

    public int damage = 1;

    public float coolDownDuration = 1f;
    public bool coolingDown = false;

    public GameObject ballPrefab;

    public GameObject TowerPrefab;
    public GameObject tower;

    public UpgradeMechanics upgradeManager;

    public int level = 1;

    public float[] modificators = { 0, 1.6f, 2.6f };

    public AudioClip catThrowSound;

    void Start()
    {
        tower = Instantiate(TowerPrefab, transform.position + new Vector3(-0.3f, 1.7f, 0), Quaternion.identity);
        tower.GetComponent<CatStationMechanics>().upgradeManager = upgradeManager;
        tower.GetComponent<CatStationMechanics>().catParent = transform;
        tower.GetComponent<CatStationMechanics>().catType = "ThrowingCat";
        var towers = GameObject.FindGameObjectsWithTag("Tower");


        ret = GetComponent<RangeEnemyTargeting>();
    }


    void Update()
    {
        if (ret.target != null)
        {
            CatAwake();
        }
        else
        {
            CatSleep();
        }
    }

    public void CatAwake()
    {
        target = ret.target;

        if (!coolingDown)
        {
            Throw();
        }
    }

    public void CatSleep()
    {
        target = null;
    }
    public void Throw()
    {
        AudioManager.Play(catThrowSound, 1f);
        coolingDown = true;
        var obj = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<BallMechanics>().target = target;
        obj.GetComponent<BallMechanics>().damage = Mathf.RoundToInt(damage * modificators[level - 1]);
        ResetCoolDown();

    }
    public async void ResetCoolDown()
    {
        await new WaitForSeconds(coolDownDuration);
        coolingDown = false;
    }

    public void LevelUp()
    {
        this.level++;
        coolDownDuration /= modificators[level - 1];
    }


}
