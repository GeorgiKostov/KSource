/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation


/// To make an FPS style character:
/// - Create a capsule.
/// - Add a rigid body to the capsule
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSWalker script to the capsule


/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)

using UnityEngine;

namespace Assets.Scripts.Helpers {
    [AddComponentMenu("Camera-Control/Mouse Look")]
    public class MouseLook : MonoBehaviour {
    	public bool enable = true;
    	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    	public RotationAxes axes = RotationAxes.MouseXAndY;
    	public float sensitivityX = 15F;
    	public float sensitivityY = 15F;
    	public float minimumX = -60F;
    	public float maximumX = 60F;
    	public float minimumY = -60F;
    	public float maximumY = 60F;
    	public float offsetY = 0F;
    	float rotationX = 0F;
    	GameObject cmra = null;
    	public float rotationY = 0F;
    	Quaternion originalRotation;

    	void Update ()
    	{
    		if (this.enable == true) {
    			if (this.axes == RotationAxes.MouseXAndY) {
    				// Read the mouse input axis
    				this.rotationX += Input.GetAxis ("Mouse X") * this.sensitivityX / 60 * this.cmra.GetComponent<Camera> ().fieldOfView;
    				this.rotationY += (Input.GetAxis ("Mouse Y") * this.sensitivityY / 60 * this.cmra.GetComponent<Camera> ().fieldOfView + this.offsetY);

    				this.rotationX = ClampAngle (this.rotationX, this.minimumX, this.maximumX);
    				this.rotationY = ClampAngle (this.rotationY, this.minimumY, this.maximumY);

    				Quaternion xQuaternion = Quaternion.AngleAxis (this.rotationX, Vector3.up);
    				Quaternion yQuaternion = Quaternion.AngleAxis (this.rotationY, Vector3.left);

    				transform.localRotation = this.originalRotation * xQuaternion * yQuaternion;
    			} else if (this.axes == RotationAxes.MouseX) {
    				this.rotationX += Input.GetAxis ("Mouse X") * this.sensitivityX;
    				this.rotationX = ClampAngle (this.rotationX, this.minimumX, this.maximumX);

    				Quaternion xQuaternion = Quaternion.AngleAxis (this.rotationX, Vector3.up);
    				transform.localRotation = this.originalRotation * xQuaternion;
    			} else {
    				this.rotationY += Input.GetAxis ("Mouse Y") * this.sensitivityY + this.offsetY;
    				this.rotationY = ClampAngle (this.rotationY, this.minimumY, this.maximumY);

    				Quaternion yQuaternion = Quaternion.AngleAxis (this.rotationY, Vector3.left);
    				transform.localRotation = this.originalRotation * yQuaternion;
    			}
    			this.offsetY = 0F;
    		}
    	}

    	void Start ()
    	{
    		this.cmra = GameObject.FindWithTag("MainCamera");
    		// Make the rigid body not change rotation
    		if (GetComponent<Rigidbody>())
    			GetComponent<Rigidbody>().freezeRotation = true;
    		this.originalRotation = transform.localRotation;
    	}
    		
    	public static float ClampAngle (float angle, float min, float max)
    	{
    		if (angle < -360F) 
    			angle += 360F;
    		if (angle > 360F)
    			angle -= 360F;
    		return Mathf.Clamp (angle, min, max);
    	}

    	public void SetState(bool enabled) {
    		this.enable = enabled;
    	}
    }
}