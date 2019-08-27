/*
*******************************************************************************************************
Purpose/Description: Problem #1: (25 points)
(a) Implement a recursive search function in Java
int terSearch(int arr[], int l, int r, int x)
that returns location of x in a given sorted array arr[l…r] is present, otherwise -1.
The terSearch search function, unlike the binary search, must consider two dividing points
int d1 = l + (r - l)/3
int d2 = d1 + (r - l)/3

(b) What is the running time complexity of your program? Justify.
-->ANSWER: My program implements the ternary search algorithm. It recursively divides the array in 3 parts
until it finds (or does not find) the value your are looking for. For this reason, the running time complexity
of my solution does not directly relate with the input size. In other words, this program runs in sub-linear time,
with complexity O(log3 n).

Author’s Panther ID: 6048821
Certification: I hereby certify that this work is my own and none of it is the work of any other person. 
********************************************************************************************************
*/
package problem1;

public class Problem1 {
    
    public int terSearch(int arr[], int l, int r, int x)
    {
        // If l > r, return -1 (x not found)
        if ( l > r )
        {
            return -1;
        }
        
        int d1 = l + (r - l)/3;  //First dividing point 
        int d2 = d1 + (r - l)/3; //Second dividing point

        if (arr[d1] == x)
        {
            return d1; //Return position of x if x is present exactly at position of the first dividing point
        }
        else if (arr[d2] == x)
        {
            return d2; //Return position of x if x is present exactly at position of the second dividing point
        }
        else if ( x < arr[d1])
        {
            return terSearch(arr, l, d1 - 1, x); //Recursive call in case x is less than the value at index of the first dividing point. We then start the search from l to d1 - 1(r)
        }
        else if ( x > arr[d2])
        {
            return terSearch(arr, d2 + 1, r, x); //Recursive call in case x is greater than the value at index of the second dividing point. We then start the search from d2 + 1(l) to r
        }
        else
        {
            return terSearch(arr, d1, d2, x); //Recursive call when x is not at any of the 4 previous positions (x is in between the values from d1 to d2). We then start the search from d1(l) to d2(r) 
                                              //Ex: In the test class below this occur when x = 3 because the dividing points are indexes 1 and 2 respectively, which are values 2 and 4 in the array. 
                                              
        }
        
    }
    
    public static void main(String[] args) {
        
        //Update your array here
        int arr[] = {0,2,4,6,8,10};
        
        //Update your x here
        int x = 3;
        
        //Test your function
        Problem1 test = new Problem1();
        if(test.terSearch(arr, 0, arr.length - 1, x) == -1) //If return -1, x was not found.
        {
            System.out.println("Return " + test.terSearch(arr, 0, arr.length - 1, x)
                 + "\n" +"Number " + x + " was not found!");
        }
        else //If not -1, x is present at that index
        {
            System.out.println("Number " + x + " is present at index "
                 + test.terSearch(arr, 0, arr.length - 1, x) + ".");
            
        }
        
    }
    
}
