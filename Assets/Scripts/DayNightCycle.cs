using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // PLACE SCRIPT IN DIRECTIONAL LIGHT AND PRESS RESET ON THE TRANSFORM TO HAVE THIS WORK

    // Rotation variables
    private Vector3 rotationVector = Vector3.zero;
    private float degreesPerSecond;

    // Timer variables
    private float timer;
    public float timeInOneDay;

    // Set end of "day" just after sunfall
    private float endDayAt = 215;

    // Pause bool for future proofing
    public bool pause;

    private void Start()
    {
        // Sets pause to be false
        pause = false;
        timer = 0f;

        // Calculates exactly how many degrees per second for one day
        degreesPerSecond = endDayAt / timeInOneDay;
    }

    private void Update()
    {
        // Only runs when unpaused
        if (!pause)
        {
            // Increases timer
            timer += Time.deltaTime;

            // Checks if rotations still need to occur
            if (timer <= timeInOneDay)
            {
                // Rotates the directional light
                rotationVector.x = degreesPerSecond * Time.deltaTime;
                transform.Rotate(rotationVector, Space.World);
            }
            else
            {
                pause = true;
            }
        }
    }


    // FUTURE PROOFED PAUSE TIME FUNCTIONS FOR PAUSE MENU AND SUCH

    public void PauseTime()
    {
        pause = true;
    }

    public void ResumeTime()
    {
        pause = false;
    }
}
