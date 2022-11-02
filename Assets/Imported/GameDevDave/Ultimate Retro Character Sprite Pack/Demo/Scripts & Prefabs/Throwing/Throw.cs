using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameDevDave;

public class Throw : MonoBehaviour
{
    [Header("Import Texture2D Images here")]
    public Texture2D[] frames, framesSword;
    private SpriteRenderer spriteRenderer;
    private Texture2D currentFrame;
    private Animator playerAnimator;
    [HideInInspector]
    public ShootingController shootingController;
    // Rest Image
    public Texture2D[] restPose, restPoseWithSword;
    // Flipping the character image
    public bool withSword;
    private bool alreadyShot = false;
    [HideInInspector]
    public float floatIndexAnim = 0;


    void Awake () 
    {
        playerAnimator = GetComponent<Animator>();
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
        int frameCount = 7;

        int IndexAnimOffset = indexAngle * frameCount;

        
        // Simple Animator
        int indexAnim = Mathf.FloorToInt(floatIndexAnim);
        floatIndexAnim += 0.25f;

        currentFrame = frames[IndexAnimOffset + indexAnim];
        setFrame(currentFrame);
        //if we reached the third frame, shoot the bullet
        if (indexAnim % 7 == 3)
        {
            //don't shoot more than one bullet for each click
            if (alreadyShot == false)
            {
                shootingController.shoot();
                alreadyShot = true;
            }
        }
        else
        {
            alreadyShot = false;
        }
        //if we reached the last frame, stop this script
        if (indexAnim % 7 == 6)
        {
            playerAnimator.enabled = true;
            this.enabled = false;
        }
    }
    void setFrame(Texture2D frame)
    {
        spriteRenderer.sprite = Sprite.Create(frame, new Rect(0.0f, 0.0f, frame.width, frame.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
    private void Flip()
    {
        transform.Rotate(0, 180f, 0);
    }
}