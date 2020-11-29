using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace BehaviourTree
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float startingHealth;
        [SerializeField] private float lowHealthThreshold;
        [SerializeField] private float healthRestoreRate;

        [SerializeField] private float chasingRange;
        [SerializeField] private float shootingRange;


        [SerializeField] private Transform playerTransform;
        [SerializeField] private Cover[] avaliableCovers;



        private Material material;
        private Transform bestCoverSpot;
        private NavMeshAgent agent;

        private Node topNode;

        [SerializeField] private float _currentHealth;
        public float currentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            material = GetComponentInChildren<MeshRenderer>().material;
        }

        private void Start()
        {
            GameObject[] coversGO = GameObject.FindGameObjectsWithTag("Wall");
            avaliableCovers = new Cover[coversGO.Length];
            for (int i = 0; i < coversGO.Length; i++)
            {
                avaliableCovers[i] = coversGO[i].GetComponent<Cover>();
            }
            playerTransform = Player.InstancePlayer.transform;
            _currentHealth = startingHealth;
            ConstructBehahaviourTree();
        }

        private void ConstructBehahaviourTree()
        {
            IsCovereAvaliableNode coverAvaliableNode = new IsCovereAvaliableNode(avaliableCovers, playerTransform, this);
            GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
            HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
            IsCoveredNode isCoveredNode = new IsCoveredNode(playerTransform, transform);
            ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
            RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform);
            RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTransform, transform);
            ShootNode shootNode = new ShootNode(agent, this, playerTransform);

            Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
            Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });

            Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvaliableNode, goToCoverNode });
            Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });
            Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });
            Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });

            topNode = new Selector(new List<Node> { mainCoverSequence, shootSequence, chaseSequence });


        }

        private void Update()
        {
            topNode.Evaluate();
            if (topNode.nodeState == NodeState.FAILURE)
            {
                SetColor(Color.red);
                agent.isStopped = true;
            }
            if (_currentHealth < startingHealth)
            {
                _currentHealth += Time.deltaTime * healthRestoreRate;
            }
            else if (_currentHealth >= startingHealth)
            {
                _currentHealth = startingHealth;
            }
        }


        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
        }

        public void SetColor(Color color)
        {
            material.color = color;
        }

        public void SetBestCoverSpot(Transform bestCoverSpot)
        {
            this.bestCoverSpot = bestCoverSpot;
        }

        public Transform GetBestCoverSpot()
        {
            return bestCoverSpot;
        }


    }
}
