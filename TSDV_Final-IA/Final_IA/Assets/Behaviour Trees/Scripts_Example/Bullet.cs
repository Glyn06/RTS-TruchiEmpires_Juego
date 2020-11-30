using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Bullet : MonoBehaviour
    {
        public enum Shooter
        {
            Player,
            Enemy,
        }
        private Shooter shooter;
        [SerializeField] private float speedBullet = 250;
        [SerializeField] private float timeLife = 3.0f;
        private float damage;
        private Rigidbody rig;

        void Start()
        {
            rig = GetComponent<Rigidbody>();
            rig.AddForce(transform.forward * speedBullet, ForceMode.Impulse);
        }

        void Update()
        {
            CheckTimeLifeBullet();
        }

        private void CheckTimeLifeBullet()
        {
            if (timeLife > 0) timeLife -= Time.deltaTime;
            else if (timeLife <= 0) Destroy(gameObject);
        }

        public void SetDamage(float _damage) => damage = _damage;
        public float GetDamage() { return damage; }

        public Shooter GetShooter() { return shooter; }
        public void SetShooter(Shooter _shooter) => shooter = _shooter;

        void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Enemy")
                Destroy(gameObject);

            if (collision.gameObject.tag == "Player" && shooter == Shooter.Enemy)
            {
                Player player = collision.gameObject.GetComponent<Player>();
                player.TakeDamage(damage);
                player.CheckDie();
                Destroy(gameObject);
            }
            if (collision.gameObject.tag == "Enemy" && shooter == Shooter.Player)
            {
                EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
                enemyAI.TakeDamage(damage);
                enemyAI.CheckDie();
                Destroy(gameObject);
            }
        }
    }
}
