using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class basicAI : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent;
        public ThirdPersonCharacter character;

        public enum State
        {
            PATROL,
            CHASE

        }
        public State state;
        private bool alive;

        //variables for patrolling
        public GameObject[] waypoints;
        private int waypointInd;
        public float patrolSpeed = 0.5f;

        //variables for chasing
        public float chaseSpeed = 1f;
        public GameObject target;
		private GameObject tempttarget;

        // Use this for initialization
        void Start()
        {
			tempttarget = target;
			agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updatePosition = true;
            agent.updateRotation = false;

			waypoints = GameObject.FindGameObjectsWithTag ("waypoint");
			waypointInd = Random.Range(0, waypoints.Length);

            state = basicAI.State.PATROL;

            alive = true;

            StartCoroutine("FSM");

        }

        IEnumerator FSM()
        {
            while (alive)
            {
                switch (state)
                {
					case State.PATROL:
					{
						Patrol();
						break;
					}
					case State.CHASE:
					{
						Chase();
						break;
					}
                }
                yield return null;
            }
        }

        void Patrol()
        {
            agent.speed = patrolSpeed;
            if (Vector3.Distance (this.transform.position, waypoints[waypointInd].transform.position) > 3)
            {   
				agent.SetDestination(waypoints[waypointInd].transform.position);

				character.Move(agent.desiredVelocity, false, false);
                
            }
            else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 3)
            {
				waypointInd = Random.Range(0, waypoints.Length);

            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
        }

        void Chase()
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(target.transform.position);
            character.Move(agent.desiredVelocity, false, false);

        }

        void OnTriggerEnter(Collider coll)
        {
            if (coll.tag == "Player")
            {
                state = basicAI.State.CHASE;
                target = coll.gameObject;
            }
        }

		void OnTriggerExit(Collider coll)
		{
			if (coll.tag == "Player") 
			{
				state = basicAI.State.PATROL;
				target = tempttarget;
			}
		}


        
    }


}










