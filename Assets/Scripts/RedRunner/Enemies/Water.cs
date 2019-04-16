﻿using RedRunner.Characters;
using UnityEngine;

namespace RedRunner.Enemies
{

    public class Water : Enemy
    {

        [SerializeField]
        private readonly Collider2D m_Collider2D;

        public override Collider2D Collider2D
        {
            get
            {
                return m_Collider2D;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Character character = other.GetComponent<Character>();
            if (character != null)
            {
                Kill(character);
            }
        }

        public override void Kill(Character target)
        {
            target.Die();
            Vector3 spawnPosition = target.transform.position;
            spawnPosition.y += -1f;
            ParticleSystem particle = Instantiate<ParticleSystem>(target.WaterParticleSystem, spawnPosition, Quaternion.identity);
            Destroy(particle.gameObject, particle.main.duration);
            AudioManager.Singleton.PlayWaterSplashSound(transform.position);
            var scalePosition = 105 / spawnPosition.z;

        }

    }

}