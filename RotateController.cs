using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour
{
    [SerializeField] private float rotationDegree = 90f; // how far you want the level to rotate. May need to be made public in order to change it for different levels.
    [SerializeField] private float rotateSpeed = 5f; // speed of the rotation
    [SerializeField] GameObject player;

    // public flags for use in other scripts. Other objects will need to know if the world is moving clock or counter clockwise, or even if it is moving
    private bool rotatingClockwise = false, rotatingCounterClock = false, levelRotating = false;


    private Quaternion toAngle, startAngle;
    private int rotateCount = 0;
    private Transform level;


    private void Awake()
    {
 
        level = this.transform; //shorthand, chached.

        startAngle = Quaternion.identity; // Make sure the level startAngle is set to No Rotation to start.

        if (360 % rotationDegree > 0)
            Debug.Log("ERROR: This Rotation Degree amount will result in unsatisfactory rotations when nearing a full 360 degree rotation.");
    }
    private void FixedUpdate()
    {
        if (!levelRotating)
        {
            
            if (Input.GetAxisRaw("Rotate") > 0)
            {
                //Debug.Log("Clockwise");
                //clockwise rotation via the e key
                rotatingClockwise = true; // set the direction flag - These direction flags are currently unusued, but are available in case needed for other classes (EnemyController?)
                levelRotating = true;
                rotateCount--;
                toAngle = transform.rotation * Quaternion.AngleAxis(rotationDegree, -transform.forward);

            }
            else if (Input.GetAxisRaw("Rotate") < 0)
            {
                //Debug.Log("Counterclockwise");
                //counterclockwise rotation via the q key
                rotatingCounterClock = true;
                levelRotating = true;
                rotateCount++;
                toAngle = transform.rotation * Quaternion.AngleAxis(rotationDegree, transform.forward);
            }

        }
        else // levelRotationg = true;
        {
            // set the start angle. Using variable so we can hold it for later if need be. Currently Unused.
            startAngle = transform.rotation;


            /* The actual rotate. This figures out Clockwise and Counterclockwise by switching the axis of the rotate. Forward rotates counter clockwise (Left Handed Cordinate System
             * means that X -> Y.) -Forward will routate clockwise. This is done in the toAngle calculation above.
             */


            // Bug Possiblity: If the level locks up after X Rotations (of some high value of X) in the same direction, this if statement may be the culprit. Check to see if it is returning
            // false even when the rotations "equal" - it shouldn't, not after changing to Lerp from transform.rotate, but ... The working of this if statement has been the issue for a long time.
            if(transform.rotation != toAngle)   
            {
                player.transform.SetParent(this.transform); // adds the player object to the level, so it will rotate with the level and not get tossed out


                //use Lerp to adjust the Transform - much cleaner, allows for more consistant rotation (No jerks at end) and with the error correction below, SHOULD prevent floating point errors
                // in the above if statement (where despite appearing for all purposes equal, transform.rotation != toAngle continues to report that they are not equal)
                transform.rotation = Quaternion.Lerp(transform.rotation, toAngle, rotateSpeed / 100); 
                

            }
            else //Cleanup
            {
                
                if( rotateCount < 360/rotationDegree)
                {

                    transform.rotation = toAngle;

                    
                } else //destination rotate count is greater than or equal to the amount of times the degRotate can divide into a circle)
                {
                
                    transform.rotation = Quaternion.identity; // if we hit 360 degrees, we've made a full circle. Quaternion.identity will make sure it is zero it out.

                    rotateCount = 0;
                    /* Note:
                     * 
                     * Currently no Error checking if rotationDegree is not a whole divisor of 360. Error checking will be needed in awake
                     */
                }

                rotatingCounterClock = false;
                rotatingClockwise = false;
                levelRotating = false;
                player.transform.parent = null;
            }
        }
        
    }
    
}

