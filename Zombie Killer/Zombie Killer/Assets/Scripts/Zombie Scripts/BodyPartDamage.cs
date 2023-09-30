using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartDamage : MonoBehaviour
{

    public enum BodyPart
    {
        LeftArm,
        RightArm,
        Head,
        Body
    }

    [SerializeField] BodyPart _bodyPart;
    [SerializeField] EnemyAI _enemyAI;
    [SerializeField] GameObject _bodyPartProp;
    [SerializeField] GameObject _bloodParticle;
    [SerializeField] GameObject _impactParticle;

 
    public void DealDamage(float damage)
    {
        float health = _enemyAI.TakeDamage(damage);
        
        if(_bodyPart == BodyPart.LeftArm)
        {
            if(health <= 75)
            {
                _bloodParticle.SetActive(true);
                _bodyPartProp.SetActive(false);
                Instantiate(_impactParticle, transform);
            }
        }

        if (_bodyPart == BodyPart.RightArm)
        {
            if (health <= 75)
            {
                _bloodParticle.SetActive(true);
                _bodyPartProp.SetActive(false);
                Instantiate(_impactParticle, transform);
            }
        }

        if (_bodyPart == BodyPart.Head)
        {
            if (health <= 25)
            {
                _bloodParticle.SetActive(true);
                _bodyPartProp.SetActive(false);
                Instantiate(_impactParticle, transform);
            }
        }

        if (_bodyPart == BodyPart.Body)
        {
            return;
        }

    }
}
