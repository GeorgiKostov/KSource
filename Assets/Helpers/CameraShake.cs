///Daniel Moore (Firedan1176) - Firedan1176.webs.com/
///26 Dec 2015
///
///Shakes camera parent object

using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
	public class CameraShake : MonoBehaviour {
 
		public bool debugMode = false;//Test-run/Call ShakeCamera() on start
 
		public float shakeAmount;//The amount to shake this frame.
		public float shakeDuration;//The duration this frame.
 
		//Readonly values...
		float shakePercentage;//A percentage (0-1) representing the amount of shake to be applied when setting rotation.
		float startAmount;//The initial shake amount (to determine percentage), set when ShakeCamera is called.
		float startDuration;//The initial shake duration, set when ShakeCamera is called.
 
		bool isRunning = false;	//Is the coroutine running right now?
 
		public bool smooth;//Smooth rotation?
		public float smoothAmount = 5f;//Amount to smooth
 	

		void Start () {
 
			if(this.debugMode) ShakeCamera ();
		}
 
 
		void ShakeCamera() {
 
			this.startAmount = this.shakeAmount;//Set default (start) values
			this.startDuration = this.shakeDuration;//Set default (start) values
 
			if (!this.isRunning) StartCoroutine (Shake ());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
		}
 
		public void ShakeCamera(float amount, float duration) {
			this.shakeAmount += amount;//Add to the current amount.
			this.startAmount = this.shakeAmount;//Reset the start amount, to determine percentage.
			this.shakeDuration += duration;//Add to the current time.
			this.startDuration = this.shakeDuration;//Reset the start time.
 		
			if (!this.isRunning) StartCoroutine (Shake ());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
		}
 
		public bool isShaking() {
			return this.isRunning;
		}
		IEnumerator Shake() {
			this.isRunning = true;
			this.GetComponent<Animation> ().enabled = false;
			while (this.shakeDuration > 0.01f) {
				Vector3 rotationAmount = Random.insideUnitSphere * this.shakeAmount;//A Vector3 to add to the Local Rotation
				rotationAmount.z = 0;//Don't change the Z; it looks funny.
 
				this.shakePercentage = this.shakeDuration / this.startDuration;//Used to set the amount of shake (% * startAmount).
 
				this.shakeAmount = this.startAmount * this.shakePercentage;//Set the amount of shake (% * startAmount).
				this.shakeDuration = Mathf.Lerp(this.shakeDuration, 0, Time.deltaTime);//Lerp the time, so it is less and tapers off towards the end.
 
 
				if(this.smooth)
					transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotationAmount), Time.deltaTime * this.smoothAmount);
				else
					transform.localRotation = Quaternion.Euler (rotationAmount);//Set the local rotation the be the rotation amount.
 
				yield return null;
			}
			transform.localRotation = Quaternion.identity;//Set the local rotation to 0 when done, just to get rid of any fudging stuff.
			this.GetComponent<Animation> ().enabled = true;
			this.isRunning = false;
		}
 
	}
}