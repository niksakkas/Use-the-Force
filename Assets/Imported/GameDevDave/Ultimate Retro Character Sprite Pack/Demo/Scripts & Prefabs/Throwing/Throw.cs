using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameDevDave;

public class Throw : MonoBehaviour
{
    public float deadzone;
    [Header("Import Texture2D Images here")]
    public Texture2D[] frames, framesSword;
    private Text title;
    private SpriteRenderer spriteRenderer;
    private Texture2D currentFrame;
    // Rest Image
    public Texture2D[] restPose, restPoseWithSword;
    // Flipping the character image
    Vector3 flipX = new Vector3 (-1, 1, 1);
    [HideInInspector]
    public bool withSword;
    public bool InAir;

    void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Establish origin
        Vector3 Origin = transform.position + transform.up / 2;

        // Get the mouse location (screen to world)
        Vector3 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get the direction towards the mouse position
        Vector3 Target = (mouseLoc - Origin);

        // Take out the z
        Target.z = 0;

        // Continuously loading up the frame loop with the walk animation that has the correct angle
        int indexAngle = Angles.XYTo8Angle(Target.x, Target.y);

        // Mirror the Animation (Except for Top & Bottom angles)
        //TODO: HEREEEE
        //if (Target.x > 0 || indexAngle == 2 || indexAngle == 6) 
        //    spriteRenderer.transform.localScale = Vector3.one;
        //else 
        //    spriteRenderer.transform.localScale = flipX;

        // IndexAnim Local Var
        int frameCount;
        
        // IndexAnim offset
        if (InAir) 
            frameCount = 4;
        else 
            frameCount = 7;

        int IndexAnimOffset = indexAngle * frameCount;

        // Simple Repeater Animator
        int indexAnim = Mathf.FloorToInt(Mathf.Repeat(Time.fixedTime * 10, frameCount - 0.01f));


        if (!withSword)
        {
            currentFrame = frames[IndexAnimOffset + indexAnim];
            setFrame(currentFrame);
        }
        else
        {
            currentFrame = frames[IndexAnimOffset + indexAnim];
            setFrame(currentFrame);
        }

        // Rest pose if we are below the deadzone
        if (Target.magnitude < deadzone) 
        {
            // Simple Pingpong Animator
            int indexAnimRest = Mathf.FloorToInt(Mathf.PingPong(Time.fixedTime * 10, 10.99f));

            if (!withSword)
            {
                currentFrame = restPose[indexAnimRest];
                setFrame(currentFrame);
            }
            else
            {
                currentFrame = restPoseWithSword[indexAnimRest];
                setFrame(currentFrame);
            }
        }   
    }
    void setFrame(Texture2D frame)
    {
        spriteRenderer.sprite = Sprite.Create(frame, new Rect(0.0f, 0.0f, frame.width, frame.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}