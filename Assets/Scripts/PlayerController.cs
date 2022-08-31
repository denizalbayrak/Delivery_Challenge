using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public MissionManager missionManager;
    public InventoryManager inventoryManager;

    public float moveSpeed = 1f;
    public float collisionOfset = 0.05f;
    bool canMove = true;
   public bool canChoose, haveMission, isHoldingHoe, isHoldingMushroom;
    int day = 1;
    int money = 0;
    string keyname;
    string whichObj;
    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;
    Keyboard kb;
    public ContactFilter2D movementFilter;
    public GameObject HouseGrid;
    GameObject foundedMushroom;
    public GameObject HouseDoor;
    public GameObject Outside;
    public GameObject TalkingBox;
    public GameObject SleepPanel;
    public GameObject Hoe;
    public Text WhoTalking;
    public Text SayWhat;
    public Text Day;
    public Text Mission;
    public Text Money;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    void FixedUpdate()
    {
       if ((int)Mouse.current.scroll.ReadValue().normalized.y == 1 || (int)Mouse.current.scroll.ReadValue().normalized.y ==-1)
        {
            inventoryManager.ScrollObjects((int)Mouse.current.scroll.ReadValue().normalized.y);
        }
        if (canChoose)
        {

            if (Keyboard.current.yKey.isPressed)
            {
                if (whichObj == "Bed")
                {
                    canChoose = false;
                    whichObj = "Empty";
                    StartCoroutine(SleepChoice());

                }
                if (whichObj == "toOutDoor")
                {
                    canChoose = false;
                    whichObj = "Empty";
                    StartCoroutine(GoOutsideChoice());
                }
                if (whichObj == "toInDoor")
                {
                    canChoose = false;
                    whichObj = "Empty";
                    StartCoroutine(GoInsideChoice());
                }
                if (whichObj == "NPCanswer")
                {
                    canChoose = false;
                    whichObj = "Empty";
                    StartCoroutine(NPCAnswer());
                }
                if (whichObj == "TakeMission")
                {
                    canChoose = false;
                    haveMission = true;
                    whichObj = "Empty";
                    missionManager.TakeMission();
                    TalkingBox.SetActive(false);
                    canMove = true;
                }
            }
            if (Keyboard.current.nKey.isPressed)
            {
                canChoose = false;
                NoChoice();
            }

            if (Keyboard.current.eKey.isPressed)
            {
                if (whichObj == "NPC")
                {
                    canChoose = false;
                    whichObj = "Empty";
                    NPCChoice();
                }
                if (whichObj == "TakingHoe" && haveMission)
                {
                    canChoose = false;
                    whichObj = "Empty";
                    TakeHoe();
                }

                if (whichObj == "giveNPCMushroom" && haveMission && isHoldingMushroom)
                {
                    canChoose = false;
                    whichObj = "Empty";
                    GiveMushroom();
                }

                
            }

            if (Mouse.current.leftButton.isPressed)
            {
                if (whichObj == "Pickmushroom" && haveMission && isHoldingHoe)
                {
                    canChoose = false;
                    whichObj = "Empty";
                    StartCoroutine(PickMushroom());
                }
            }

        

        }
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);
                animator.SetBool("isMoving", success);
                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));

                    if (!success)
                    {
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }
                if (movementInput.y < 0)
                {
                    animator.SetBool("isWalkUp", false);
                    animator.SetBool("isWalkLeft", false);
                }
                else if (movementInput.y > 0)
                {
                    animator.SetBool("isWalkLeft", false);
                    animator.SetBool("isWalkUp", true);
                }


                if (movementInput.x < 0)
                {
                    animator.SetBool("isWalkLeft", true);
                    spriteRenderer.flipX = false;
                }
                else if (movementInput.x > 0)
                {
                    animator.SetBool("isWalkLeft", true);
                    spriteRenderer.flipX = true;
                }
            }
            else
            {
                animator.SetBool("isWalkUp", false);
                animator.SetBool("isWalkLeft", false);
                animator.SetBool("isMoving", false);
            }
        }


    }

    #region Movement
    bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOfset);

        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bed")
        {
            canChoose = true;
            TalkingBox.SetActive(true);
            WhoTalking.text = "Myself";
            SayWhat.text = "Sleep? (y/n)";
            whichObj = "Bed";
        }
        else if (collision.tag == "toOutDoor")
        {
            canChoose = true;
            TalkingBox.SetActive(true);
            WhoTalking.text = "Myself";
            SayWhat.text = "Do you want to go out? (y/n)";
            whichObj = "toOutDoor";
        }
        else if (collision.tag == "toInDoor")
        {
            canChoose = true;
            TalkingBox.SetActive(true);
            WhoTalking.text = "Myself";
            SayWhat.text = "Do you want to return home?   (y/n)";
            whichObj = "toInDoor";
        }
        else if (collision.tag == "NPC")
        {
            canMove = false;
            canChoose = true;
            TalkingBox.SetActive(true);
            WhoTalking.text = "Miss Mouse";
            SayWhat.text = "Hi. I lost my mushrooms.    (talk e/n cancel)";
            whichObj = "NPC";
            if (haveMission)
            {
                if (missionManager.mushroomNum >= 5)
                {
                    WhoTalking.text = "Miss Mouse";
                    SayWhat.text = "Did you bring my mushrooms?    (give e/n cancel)";
                    whichObj = "giveNPCMushroom";
                }
                else
                {
                    WhoTalking.text = "Miss Mouse";
                    SayWhat.text = "You need to find more mushrooms. Please continue.   (n cancel)";
                }
                
            }
        }
        else if (collision.tag == "Hoe")
        {
            if (haveMission)
            {
                canMove = false;
                TalkingBox.SetActive(true);
                canChoose = true;
                WhoTalking.text = "Myself";
                SayWhat.text = "I found the hoe!  (take e/n)";
                whichObj = "TakingHoe";
            }


        }
        else if (collision.tag == "Mushroom")
        {
            if (isHoldingHoe)
            {
                canMove = false;
                canChoose = true;
                TalkingBox.SetActive(true);
                WhoTalking.text = "Myself";
                SayWhat.text = "I found the mushroom!  (pick press left mouse button /n)";
                whichObj = "Pickmushroom";
                foundedMushroom = collision.gameObject;
            }


        }

    }
    void NoChoice()
    {
        canMove = true;
        TalkingBox.SetActive(false);
    }

    IEnumerator GoOutsideChoice()
    {
        SleepPanel.GetComponent<Image>().DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.6f);
        gameObject.transform.DOMove(Vector3.zero, 0.1f);
        yield return new WaitForSeconds(0.1f);
        SleepPanel.GetComponent<Image>().DOFade(0, 1f);
        HouseGrid.SetActive(false);
        Outside.SetActive(true);
        TalkingBox.SetActive(false);
    }
    IEnumerator GoInsideChoice()
    {
        SleepPanel.GetComponent<Image>().DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.6f);
        gameObject.transform.DOMove(Vector3.zero, 0.1f);
        yield return new WaitForSeconds(0.1f);
        SleepPanel.GetComponent<Image>().DOFade(0, 1f);
        HouseGrid.SetActive(true);
        Outside.SetActive(false);
        TalkingBox.SetActive(false);
    }
    IEnumerator SleepChoice()
    {
        SleepPanel.GetComponent<Image>().DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.6f);
        SleepPanel.GetComponent<Image>().DOFade(0, 1f);
        day++;
        Day.text = day.ToString();
        TalkingBox.SetActive(false);
    }
    void NPCChoice()
    {
        WhoTalking.text = "Myself";
        whichObj = "NPCanswer";
        canChoose = true;
        SayWhat.text = "Hi! May I help you? (next y/cancel n)";
    }

    IEnumerator NPCAnswer()
    {
        WhoTalking.text = "Miss Mouse";
        SayWhat.text = "The farmer's cows have escaped. I'm afraid so I can't go. Can you go and collect 5 mushrooms for me? You can find them nearby the trees.    (accept mission y/cancel n)";
        yield return new WaitForSeconds(0.5f);
        whichObj = "TakeMission";
        canChoose = true;

    }

    void TakeHoe()
    {
        TalkingBox.SetActive(false);
        Hoe.SetActive(false);
        canChoose = true;
        isHoldingHoe = true;
        canMove = true;        
        Mission.text = "Find 5 Mushroom";
        inventoryManager.putInventory("Hoe");      
    }
    IEnumerator PickMushroom()
    {
        TalkingBox.SetActive(false);
        canMove = false;
        animator.SetBool("isPicking", true);
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(0.7f);
        missionManager.mushroomNum++;
        inventoryManager.putInventory("Mushroom");
        missionManager.MushroomNumText.text = missionManager.mushroomNum.ToString();
        if (missionManager.mushroomNum ==5)
        {
            Mission.text = "Give Mushrooms to Miss Mouse";
        }
        animator.SetBool("isPicking", false);
        animator.SetBool("isMoving", true);
        foundedMushroom.SetActive(false);
        canMove = true;
        yield return new WaitForSeconds(0.7f);
        Destroy(foundedMushroom);
    }

    void GiveMushroom()
    {
        TalkingBox.SetActive(true);
        Mission.text = "";
        
        missionManager.Mushroom.gameObject.SetActive(false);
        WhoTalking.text = "Miss Mouse";
        SayWhat.text = "Thank you! And that's the reward for your work.  (cancel n)";
        missionManager.mushroomNum -= 5;
        inventoryManager.removeInventory("Mushroom");
        canChoose = true;
        whichObj = "Empty";
        haveMission = false;
        money += 100;
        Money.text = money.ToString();
    }
    public void HoldHoe()
    {
        Debug.Log("tuttu hoe");
        isHoldingHoe = true;
        Debug.Log(isHoldingHoe);
        isHoldingMushroom = false;
    }
    public void HoldMushroom()
    {
        Debug.Log("tuttu mush");
        isHoldingHoe = false;
        isHoldingMushroom = true;
    }
    public void HoldEmpty()
    {
        Debug.Log("tuttu empty");
        isHoldingHoe = false;
        isHoldingMushroom = false;
    }
}
