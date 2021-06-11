using System.Linq;
using UnityEngine;
using static System.Linq.Enumerable;
/// <summary>
/// This is for one of the display boards. Takes in gameobjects that have the script SevenSegmentDisplay.
/// </summary>
public class DigitDisplay : Singleton<DigitDisplay>
{
    public GameObject[] displays;
    private int value;

    //Must be less than 10 and that the digit has to be within 1-6 range.
    public void SetDigit(int indexDigit, int singleValue)
    {
        SevenSegmentDisplay newDisplay = displays[indexDigit].GetComponent<SevenSegmentDisplay>();
        newDisplay.SetDigit(singleValue);
        //value /= 10;
    }    
    //Adds the amount to the total.
    public void IncreaseNumber(int amount)
    {
        value += amount;
        SetDisplay(value);
    }
    //Decrease amount to total.
    public void DecreaseNumber(int amount)
    {
        value -= amount;
        SetDisplay(value);
    }
    //OriginalAmount of total.
    public void StartAmount(int amountStart)
    {
        value += amountStart;
        SetDisplay(value);
    }
    //Returns the main amount.
    public int ReturnAmount()
    {
        return value;
    }
    //Resets the value
    public void ResetValues()
    {
        foreach (var i in Range(0, displays.Length))
        {
            SevenSegmentDisplay newDisplay = displays[i].GetComponent<SevenSegmentDisplay>();
            newDisplay.TurnOff();  // Suppress leading zeroes
            value = 0;
        }
    }
    //Sets the number on all of the boards.
    public void SetNumber(int digits)
    {
        value = digits;
        SetDisplay(value);
    }
    //Goes through all the boards and sets the displays.
    private void SetDisplay(int value)
    {
        foreach (var i in Range(0, displays.Length))
        {
            SevenSegmentDisplay newDisplay = displays[i].GetComponent<SevenSegmentDisplay>();
            if (i > 0 && value == 0)
                newDisplay.TurnOff();  // Suppress leading zeroes
            else
            {
                var rightmostDigit = value % 10;
                newDisplay.SetDigit(rightmostDigit);
                value /= 10;
            }
        }
    }
}
