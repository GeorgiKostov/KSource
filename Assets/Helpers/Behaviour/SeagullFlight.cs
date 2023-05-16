using UnityEngine;

namespace Assets.Scripts.Helpers.Behaviour
{
    public class SeagullFlight : MonoBehaviour
    {
        public GameObject[] Parts;

        public float Speed;

        private Vector3 _moveDirection;

        private float angle;

        private Vector3 Center = new Vector3(0, 0, 0);

        private int clockwise;

        private float direction;

        private float height;

        private Vector3 Pos;

        private float Radius;

        private float time;

        public Vector3 RandomCircle(Vector3 theCenter, float theRadius, float anAngle)
        {
            Vector3 aPosOnCircle = new Vector3(theCenter.x + theRadius * Mathf.Sin(anAngle * Mathf.Deg2Rad), theCenter.y + this.height, theCenter.z + theRadius * Mathf.Cos(anAngle * Mathf.Deg2Rad));
            return aPosOnCircle;
        }

        private void Start()
        {
            this.Speed = Utilities.RandomFloat(15, 25);
            this.Radius = Utilities.RandomFloat(0.5f, 2f);
            this.height = Utilities.RandomFloat(.3f, 1f);
            this.direction = Utilities.RandomFloat(-1f, 1f);
            if (this.direction >= 0)
            {
                foreach (GameObject part in this.Parts)
                {
                    part.transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                this.clockwise = 1;
            }
            else
            {
                foreach (GameObject part in this.Parts)
                {
                    part.transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                this.clockwise = -1;
            }
        }

        private void Update()
        {
            this.time += Time.deltaTime * this.Speed * this.clockwise;
            this.Pos = this.RandomCircle(this.Center, this.Radius, -this.time);
            this.transform.localPosition = this.Pos;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(this.Pos), Time.deltaTime * 5f);
        }
    }
}