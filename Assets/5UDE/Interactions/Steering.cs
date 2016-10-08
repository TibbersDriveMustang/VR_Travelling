using UnityEngine;
using System.Collections;

public class Steering : MonoBehaviour {

	// Enumerate the states of steering
	public enum SteeringState {
		NotSteering,
		SteeringForward,
		SteeringBackward,
		SteeringLeft,
		SteeringRight,
		SpeedUp,
		SteeringStuck
	};
	
    // Inspector parameters
    [Tooltip("The tracking device used to determine absolute direction for steering.")]
    public Tracker tracker;

	[Tooltip("Body to detect collision")]
	public Interactive body;

    [Tooltip("The controller joystick used to determine relative direction (forward/backward) and speed.")]
	public Axis joystick;

	[Tooltip("A button required to be pressed to activate steering.")]
	public Button button;

	[Tooltip("A button to speedup travelling speed")]
	public Button buttonSpeedUp;

    [Tooltip("The space that is translated by this interaction. Usually set to the physical tracking space.")]
    public Space space;

    [Tooltip("The median speed for movement expressed in meters per second.")]
    public float speed = 1.0f;

	// Private interaction variables
	private SteeringState state;

	// Called at the end of the program initialization
	void Start () {

		// Set initial steering state to not steering
		state = SteeringState.NotSteering;
	}
		
    // FixedUpdate is not called every graphical frame but rather every physics frame
	void FixedUpdate () {

		// If state is not steering
		if (state == SteeringState.NotSteering) {
		
			// If the joystick is pressed forward and the button is pressed
			if (joystick.GetAxis ().y > 0.0f && button.GetPress ()) {

				// Change state to steering forward
				state = SteeringState.SteeringForward;
			}
				
			// If the joystick is pressed backward and the button is pressed
			else if (joystick.GetAxis ().y < 0.0f && button.GetPress ()) {

				// Change state to steering backward
				state = SteeringState.SteeringBackward;
			} 

// new states
			// If the joystick is pressed left and the button is pressed
			else if (joystick.GetAxis ().x > 0.0f && button.GetPress ()) {

				//Change state to steeringLeft
				state = SteeringState.SteeringLeft;
			}

			// If the joystick is pressed right and the button is pressed
			else if (joystick.GetAxis ().x < 0.0f && button.GetPress ()) {

				//Change state to steeringRight
				state = SteeringState.SteeringRight;
			}


			// Process current not steering state
			else {

				// Nothing to do for not steering
			}
		} 

		else if (state == SteeringState.SteeringStuck) {
			Debug.Log ("=================At SteeringStuck===============");
		}

		// If state is steering forward
		else if (state == SteeringState.SteeringForward) {

			// If the button is not pressed
			if (!button.GetPress ()) {

				// Change state to not steering 
				state = SteeringState.NotSteering;
			}

			// If the joystick is pressed backward and the button is pressed
			else if (joystick.GetAxis ().y < 0.0f && button.GetPress ()) {

				// Change state to steering backward
				state = SteeringState.SteeringBackward;
			}

			// Process current steering forward state
			else {

				Vector3 direction = tracker.transform.forward;
				direction.y = 0.0f;
				float speedChange = 1.0f;
				//If press speedUp button, speed x 2
				if (buttonSpeedUp.GetPress ()) {
					speedChange = 3.0f;	
				}
				// Translate the space based on the tracker's absolute forward direction and the joystick's forward value
				space.transform.position += joystick.GetAxis ().y * direction * speed * speedChange * Time.deltaTime;
				if (body.collisionOngoing) {
					Debug.Log ("=================Go to SteeringStuck===============");
					state = SteeringState.SteeringStuck;
				}
			}
		}

		// If state is steering backward
		else if (state == SteeringState.SteeringBackward) {

			// If the button is not pressed
			if (!button.GetPress ()) {

				// Change state to not steering 
				state = SteeringState.NotSteering;
			}

			// If the joystick is pressed forward and the button is pressed
			else if (joystick.GetAxis ().y > 0.0f && button.GetPress ()) {

				// Change state to steering forward
				state = SteeringState.SteeringForward;
			}

			// Process current steering backward state
			else {
				Vector3 direction = tracker.transform.forward;
				direction.y = 0.0f;
				float speedChange = 1.0f;
				//If press speedUp button, speed x 2
				if (buttonSpeedUp.GetPress ()) {
					speedChange = 3.0f;	
				}
				// Translate the space based on the tracker's absolute forward direction and the joystick's backward value
				space.transform.position += joystick.GetAxis ().y * direction * speed * speedChange * Time.deltaTime;
			}
		} 
		else if (state == SteeringState.SteeringLeft) {

			//If the button is not pressed
			if (!button.GetPress ()) {
				//Change state to not steering
				state = SteeringState.NotSteering;
			} 
			else if (joystick.GetAxis ().x < 0.0f && button.GetPress ()) {
				//Change state to steering right
				state = SteeringState.SteeringRight;
			} 
			else {
				Vector3 direction = -1 * tracker.transform.right;
				direction.y = 0.0f;
				float speedChange = 1.0f;
				//If press speedUp button, speed X 2
				if(buttonSpeedUp.GetPress()){
					speedChange = 3.0f;
				}
				// Translate the space based on the tracker's absolute forward direction and the joystick's backward value
				space.transform.position += joystick.GetAxis().x * direction * speed * speedChange * Time.deltaTime;
			}
		
		}
		else if (state == SteeringState.SteeringRight) {

			//If the button is not pressed
			if (!button.GetPress ()) {
				//Change state to not steering
				state = SteeringState.NotSteering;
			} 
			else if (joystick.GetAxis ().x > 0.0f && button.GetPress ()) {
				//Change state to steering right
				state = SteeringState.SteeringLeft;
			} 
			else {
				Vector3 direction = -1 * tracker.transform.right;
				direction.y = 0.0f;
				float speedChange = 1.0f;
				//If press speedUp button, speed X 2
				if(buttonSpeedUp.GetPress()){
					speedChange = 3.0f;
				}
				// Translate the space based on the tracker's absolute forward direction and the joystick's backward value
				space.transform.position += joystick.GetAxis().x * direction * speed * speedChange * Time.deltaTime;
			}

		}
	}
}