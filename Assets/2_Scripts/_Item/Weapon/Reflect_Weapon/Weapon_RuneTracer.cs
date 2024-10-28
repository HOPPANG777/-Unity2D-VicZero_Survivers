using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon_RuneTracer : Weapon
{
    public Vector3 BoxSize = new Vector3(0.5f, 0.5f);
    public Vector3 Dir;
    private Camera mainCamera;

    public override void Start()
    {
        base.Start();
        Dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        mainCamera = Camera.main;
        TR.enabled = true;
        this.Weapon_Trail();
    }

    public override void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            base.Update();

            Rune_Attack();

            this.Weapon_KnockBack_Weapon();

            this.Weapon_Timer();
        }

        else
        {

        }


    }

    public void Rune_Attack()
    {
        // Normalize the direction
        Dir = Dir.normalized;

        // Move the object
        this.transform.position += Dir * Weapon_Speed * Time.deltaTime;

        // Check for collisions with screen edges and reflect
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);

        // Reflect on x-axis
        if (viewPos.x <= 0.0f)
        {
            viewPos.x = 0.0f; // Prevent going beyond the left boundary
            Dir = Vector3.Reflect(Dir, Vector3.right);
        }
        else if (viewPos.x >= 1.0f)
        {
            viewPos.x = 1.0f; // Prevent going beyond the right boundary
            Dir = Vector3.Reflect(Dir, Vector3.right);
        }

        // Reflect on y-axis
        if (viewPos.y <= 0.0f)
        {
            viewPos.y = 0.0f; // Prevent going beyond the bottom boundary
            Dir = Vector3.Reflect(Dir, Vector3.up);
        }
        else if (viewPos.y >= 1.0f)
        {
            viewPos.y = 1.0f; // Prevent going beyond the top boundary
            Dir = Vector3.Reflect(Dir, Vector3.up);
        }

        // Update position after reflection
        transform.position = mainCamera.ViewportToWorldPoint(viewPos);
    }

    //public override void OnTriggerStay2D(Collider2D collision)
    //{
    //    base.OnTriggerStay2D(collision);
    //}

    public override void Weapon_To_Enemy_Collision()
    {
        base.Weapon_To_Enemy_Collision();
    }
}

#region
//EdgeCollider2D edgeCollder;
//void Awake()
//{
//    edgeCollder = this.GetComponent<EdgeCollider2D>();
//    CreateEdgeCollider();
//}
////call this at start and whenever the resolution changes
//void CreateEdgeCollider()
//{
//    List<Vector2> edges = new List<Vector2>();
//    edges.Add(Camera.main.ScreenToWorldPoint(Vector2.zero));
//    edges.Add(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)));
//    edges.Add(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)));
//    edges.Add(Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)));
//    edges.Add(Camera.main.ScreenToWorldPoint(Vector2.zero));
//    edgeCollder.SetPoints(edges);
//}
////runs when colliding if collider is Not set to Trigger
//void OnCollisionEnter2D(Collision2D collision)
//{
//    //Because of colliders default physics behaviour, and the timing of it
//    //you have to use the rigidbody's velocity from the previous frame to avoid using a "post default interaction" velocity
//    //Throwable.velocity allows for that if it's updated regularly (in Throwable.Update(), Path.VisualizePath() for example)
//    Rigidbody2D collidingRB = collision.transform.GetComponent<Rigidbody2D>();
//    collidingRB.velocity = Vector3.Reflect(collision.transform.GetComponent<Throwable>().velocity, -collision.contacts[0].normal);
//}
////runs when colliding if collider set to Trigger
//void OnTriggerEnter2D(Collider2D collider)
//{
//    Rigidbody2D colliderRB = collider.GetComponent<Rigidbody2D>();
//    //contact point is gotten by raycasting in the colliders velocity direction at the colliders position.
//    RaycastHit2D[] hit2D = Physics2D.RaycastAll(collider.transform.position, colliderRB.velocity);
//    //second one is being used because first one is self, could probably ignore self-layer and get as Physics2D.Raycast() instead
//    Vector2 contactPoint = hit2D[1].point;
//    //Get normal of contact point by creating a line from the contact point to the closest collider point and rotating 90¡Æ
//    Vector2 normal = Vector2.Perpendicular(contactPoint - GetClosestPoint(collider.transform.position)).normalized;
//    //reflect the current velocity at the edge normal
//    colliderRB.velocity = Vector2.Reflect(colliderRB.velocity, normal);
//}
////Goes through edgeCollider Points and returns the one closest to position
//Vector2 GetClosestPoint(Vector2 position)
//{
//    Vector2[] points = edgeCollder.points;
//    float shortestDistance = Vector2.Distance(position, points[0]);
//    Vector2 closestPoint = points[0];
//    foreach (Vector2 point in points)
//    {
//        if (Vector2.Distance(position, point) < shortestDistance)
//        {
//            shortestDistance = Vector2.Distance(position, point);
//            closestPoint = point;
//        }
//    }
//    return closestPoint;

//var dist = (this.transform.position - Camera.main.transform.position).z;

//var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
//var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
//var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
//var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

//Vector3 playerSize = this.gameObject.renderer.bounds.size;

//this.transform.position = new Vector3(

//Mathf.Clamp(this.
//transform.position.x,
//leftBorder + playerSize.x / 2,
//rightBorder - playerSize.x / 2),
//Mathf.Clamp
//(this.transform.position.y,
//topBorder + playerSize.y / 2,
//bottomBorder - playerSize.y / 2),
//this.transform.position.z);

//public void Pong_Attack()
//{
//    Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
//    if ((screenPosition.y > Screen.height) || (screenPosition.y < 0f) || (screenPosition.x > Screen.width) || (screenPosition.x < 0f))
//    {
//        screenPosition.x = Mathf.Clamp(screenPosition.x, 0f, Screen.width);
//        screenPosition.y = Mathf.Clamp(screenPosition.y, 0f, Screen.height);
//        Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
//        this.transform.position = new Vector2(newWorldPosition.x, newWorldPosition.y);
//        Move();
//    }
//}

//private void Move()
//{
//    this.transform.position = this.transform.position + ()
//    }


//public void Pong_Attack()
//{
//    var Dist = (this.transform.position - Camera.main.transform.position).z;

//    var Left_Border = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Dist)).x;
//    var Right_Border = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Dist)).x;
//    var Top_Border = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Dist)).y;
//    var Bottom_Border = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Dist)).y;

//    Vector3 Pong_Size = this.gameObject.transform.position;

//    this.transform.position = new Vector3(

//    Mathf.Clamp(this.
//    transform.position.x,
//    Left_Border + Pong_Size.x / 2,
//    Right_Border - Pong_Size.x / 2),
//    Mathf.Clamp
//    (this.transform.position.y,
//    Top_Border + Pong_Size.y / 2,
//    Bottom_Border - Pong_Size.y / 2),
//    this.transform.position.z);

//}
#endregion