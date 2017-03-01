using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellScript : MonoBehaviour
{
    private GameObject itemsMenu;
    private GameObject itemsMenuBackground;
    private GameObject viewingPanel;
    private GameObject moneyBackground;
    private GameObject[] items;
    private GameObject arrow;
    private GameObject detailsBackground;
    private GameObject priceText;
    private GameObject typeText;
    private GameObject descriptionText;
    private Vector2 startPosition;
    private Vector2 itemsYOffset = new Vector2(0f, 65f);
    private Vector2 confirmationXOffset = new Vector2(410f, 0f);
    private int arrowIndex;
    private int itemIndex;
    private int totalOptions;
    private List<Item> playerInventory;
    public bool isActive;
    public bool isConfirmationActive;
    private GameObject confirmationBackground;
    private GameObject confirmationArrow;
    private GameObject quantityBackground;
    private PlayerController player;
    private bool isYes;
    private InventoryManager inventory;
    private GameObject denialBackground;
    private GameObject quantityText;
    private bool isDenied;
    private bool quantityAsked;
    private int quantityNum;

    // Use this for initialization
    void Start()
    {
        quantityNum = 1;
        quantityAsked = false;
        player = FindObjectOfType<PlayerController>();
        isYes = true;
        inventory = InventoryManager.instance;
        //Fills the shop inventory with test items, in reality individual shop's inventory will be passed in
        playerInventory = inventory.items;
        arrowIndex = 0;
        itemIndex = 0;
        itemsMenu = transform.GetChild(0).gameObject;
        itemsMenuBackground = itemsMenu.transform.GetChild(0).gameObject;
        viewingPanel = itemsMenuBackground.transform.GetChild(0).gameObject;
        detailsBackground = itemsMenu.transform.GetChild(1).gameObject;
        quantityBackground = itemsMenu.transform.GetChild(3).gameObject;
        confirmationBackground = itemsMenu.transform.GetChild(4).gameObject;
        denialBackground = itemsMenu.transform.GetChild(5).gameObject;
        moneyBackground = itemsMenu.transform.GetChild(2).gameObject;
        quantityText = quantityBackground.transform.GetChild(0).gameObject;
        priceText = detailsBackground.transform.GetChild(0).gameObject;
        typeText = detailsBackground.transform.GetChild(1).gameObject;
        descriptionText = detailsBackground.transform.GetChild(2).gameObject;
        totalOptions = viewingPanel.transform.childCount;
        items = new GameObject[totalOptions];
        //Update the Details Box if there only if there are items in the shop
        if (playerInventory.Count > 0)
        {
            updateDetails();
        }
        //Fill in the necessary items
        for (int i = 0; i < totalOptions; i++)
        {
            items[i] = viewingPanel.transform.GetChild(i).gameObject;
            if (i < playerInventory.Count)
            {
                items[i].GetComponent<Text>().text = playerInventory[i].name;
            }
            else
            {
                items[i].GetComponent<Text>().text = "";
            }
        }
        //Get the selector arrows
        confirmationArrow = confirmationBackground.GetComponentInChildren<Animator>().gameObject;
        arrow = itemsMenuBackground.GetComponentInChildren<Animator>().gameObject;
        startPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
        turnOff();
        updateMoney();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            toggle();
            updateDetails();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            turnOff();
        }
        //If Shop is pulled up and not asking for confirmation
        if (isActive && !isConfirmationActive && !quantityAsked)
        {
            //Continue up list of items
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (arrowIndex > 0)
                {
                    arrowIndex--;
                    itemIndex--;
                    Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
                    arrowPosition += itemsYOffset;
                    arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
                    arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
                    updateDetails();
                }
                else if (arrowIndex == 0 && itemIndex > 0)
                {
                    itemIndex--;
                    for (int i = totalOptions - 1; i >= 1; i--)
                    {
                        items[i].GetComponent<Text>().text = items[i - 1].GetComponent<Text>().text;
                    }
                    items[0].GetComponent<Text>().text = playerInventory[itemIndex].name;
                    updateDetails();
                }
            }
            //Continue Down list of items
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (arrowIndex < totalOptions - 1 && arrowIndex < playerInventory.Count - 1)
                {
                    itemIndex++;
                    arrowIndex++;
                    Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
                    arrowPosition -= itemsYOffset;
                    arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
                    arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
                    updateDetails();
                }
                else if (arrowIndex == totalOptions - 1)
                {
                    if (itemIndex < playerInventory.Count - 1)
                    {
                        itemIndex++;
                        for (int i = 0; i < totalOptions - 1; i++)
                        {
                            items[i].GetComponent<Text>().text = items[i + 1].GetComponent<Text>().text;
                        }
                        items[totalOptions - 1].GetComponent<Text>().text = playerInventory[itemIndex].name;
                        updateDetails();
                    }
                }
                //select an item
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                //If there are multiple
                if (playerInventory[itemIndex].quantity > 1)
                {
                    quantityNum = 1;
                    quantityAsked = true;
                    quantityBackground.SetActive(true);
                    updateQuantity();
                }else
                {
                    quantityNum = 1;
                    displayConfirmation();
                }
                
            }
        }
        //If asking the amount to sell
        else if (isActive && quantityAsked) {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (quantityNum == playerInventory[itemIndex].quantity)
                {
                    quantityNum = 1;
                }else
                {
                    quantityNum++;
                }
                updateQuantity();
            } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (quantityNum == 1)
                {
                    quantityNum = playerInventory[itemIndex].quantity;

                }else
                {
                    quantityNum--;
                }
                updateQuantity();
            } else if (Input.GetKeyDown(KeyCode.Space))
            {
                quantityAsked = false;
                quantityBackground.SetActive(false);
                displayConfirmation();
            }
        }
        //if shop is pulled and asking for confirmation
        else if (isActive && !isDenied)
        {

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //if arrow hovering over "No"
                if (!isYes)
                {

                    Vector2 arrowPosition = confirmationArrow.GetComponent<RectTransform>().anchoredPosition;
                    arrowPosition -= confirmationXOffset;
                    confirmationArrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
                    confirmationArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
                    isYes = true;
                }

            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                //if arrow hovering over "Yes"
                if (isYes)
                {
                    Vector2 arrowPosition = confirmationArrow.GetComponent<RectTransform>().anchoredPosition;
                    arrowPosition += confirmationXOffset;
                    confirmationArrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
                    confirmationArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
                    isYes = false;
                }

            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                //if hovering over "yes". Edit player's inventory, shop's inventory and indices and gui information
                if (isYes)
                {
                    //If item is special
                    if (playerInventory[itemIndex].special)
                    {
                        denialBackground.SetActive(true);
                        isDenied = true;
                    }
                    else
                    {
                        inventory.money += playerInventory[itemIndex].price * quantityNum;
                        inventory.destroyItem(playerInventory[itemIndex], quantityNum);
                        updateMoney();
                        //Wasn't sure if item should go in shop so player can potentially buyback 
                        
                        //If there are less items than spots for text in gui
                        if (playerInventory.Count < totalOptions)
                        {
                            if (playerInventory.Count == arrowIndex)
                            {
                                arrowIndex--;
                                itemIndex--;
                                Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
                                arrowPosition += itemsYOffset;
                                arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
                                arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
                                items[arrowIndex + 1].GetComponent<Text>().text = "";
                            }
                            else
                            {
                                for (int i = 0; i < totalOptions - arrowIndex; i++)
                                {
                                    if (itemIndex + i < playerInventory.Count)
                                    {
                                        items[arrowIndex + i].GetComponent<Text>().text = playerInventory[itemIndex + i].name;
                                    }
                                    else
                                    {
                                        items[arrowIndex + i].GetComponent<Text>().text = "";
                                    }

                                }

                            }
                            if (playerInventory.Count > 0)
                                updateDetails();
                            else
                                turnOff();
                        }
                        else
                        {
                            if (itemIndex == playerInventory.Count)
                            {
                                itemIndex--;
                                for (int i = 0; i < totalOptions; i++)
                                {
                                    items[arrowIndex - i].GetComponent<Text>().text = playerInventory[itemIndex - i].name;
                                }
                            }else
                            {
                                for (int i = 0; i < totalOptions - arrowIndex; i++)
                                {
                                    items[arrowIndex + i].GetComponent<Text>().text = playerInventory[itemIndex + i].name;
                                }
                            }
                            
                            updateDetails();
                        }
                        confirmationBackground.SetActive(false);
                        isConfirmationActive = false;
                    }
                }
                else
                {
                    Vector2 arrowPosition = confirmationArrow.GetComponent<RectTransform>().anchoredPosition;
                    arrowPosition -= confirmationXOffset;
                    confirmationArrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
                    confirmationArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
                    confirmationBackground.SetActive(false);
                    isConfirmationActive = false;
                    isYes = true;
                }

            }
        }
        //if player didn't have enough money
        else if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                denialBackground.SetActive(false);
                isDenied = false;
            }
        }
    }
    private void updateDetails()
    {
        priceText.GetComponent<Text>().text = "Price: " + playerInventory[itemIndex].price;
        typeText.GetComponent<Text>().text = "Type: " + playerInventory[itemIndex].type;
        descriptionText.GetComponent<Text>().text = "Description: " + playerInventory[itemIndex].description;
    }

    private void updateQuantity()
    {
        quantityText.GetComponent<Text>().text = quantityNum.ToString();
    }
    private void displayConfirmation()
    {
        confirmationBackground.SetActive(true);
        isConfirmationActive = true;
        confirmationBackground.transform.GetChild(0).GetComponent<Text>().text = "Are you sure you want to sell " + playerInventory[itemIndex].name + " (" + quantityNum +") for " +
            playerInventory[itemIndex].price * quantityNum + " gold?";
    }
    public void turnOff()
    {
        isActive = false;
        isConfirmationActive = false;
        isDenied = false;
        denialBackground.SetActive(isActive);
        detailsBackground.SetActive(isActive);
        itemsMenuBackground.SetActive(isActive);
        quantityBackground.SetActive(isActive);
        confirmationBackground.SetActive(isActive);
        moneyBackground.SetActive(isActive);
        arrow.GetComponent<RectTransform>().anchoredPosition = startPosition;
        player.frozen = false;
    }
    public void turnOn()
    {
        updateMoney();
        isActive = true;
        detailsBackground.SetActive(isActive);
        itemsMenuBackground.SetActive(isActive);
        player.frozen = true;
        itemIndex = 0;
        arrowIndex = 0;

    }
    public void updateMoney()
    {
        moneyBackground.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Gold: " + inventory.money;
    }
    public void toggle()
    {
        updateMoney();
        isActive = !isActive;
        detailsBackground.SetActive(isActive);
        itemsMenuBackground.SetActive(isActive);
        moneyBackground.SetActive(isActive);
        if (!isActive)
        {
            isConfirmationActive = false;
            isDenied = false;
            confirmationBackground.SetActive(false);
            denialBackground.SetActive(false);
        }
        arrow.GetComponent<RectTransform>().anchoredPosition = startPosition;
        player.frozen = isActive;
        itemIndex = 0;
        arrowIndex = 0;
        for (int i = 0; i < totalOptions; i++)
        {
            items[i] = viewingPanel.transform.GetChild(i).gameObject;
            if (i < playerInventory.Count)
            {
                items[i].GetComponent<Text>().text = playerInventory[i].name;
            }
            else
            {
                items[i].GetComponent<Text>().text = "";
            }
        }
    }
}
