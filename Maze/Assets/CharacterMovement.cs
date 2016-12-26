//Character Movement Scirpt

using UnityEngine;

public class CharacterMovement:MonoBehavior
{
    //variables
    public float speed = 6f;
    
    Vector3 movement;
    Animator anim;
    RigidBody playerRigidBody; 
    float camRayLenght = 100f;
    
    void Awake(){
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<RigidBody>();
        
    }
    
    void FixedUpdate(){
        //store the input axes
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        //Move the player around the scene
        Move(h,v);
        
        //Turn the player 
        Turning();
        Animating(h,v);

        
    }
    
    void Move(float h , float v){
        //Set the movement vector based on the axis input
        movement.Set(h,0f ,v);
        
        //normalize the movement vector and make it proportional to the speed per second
        movement = movement.normalized * speed * Time.deltaTime;
        
        //Move the player to it's current position plus the movement
        playerRigidBody.MovePosition(transform.position * movement);
        
    }
  
    }
}