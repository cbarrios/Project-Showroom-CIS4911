/*
*******************************************************************************************************
Purpose/Description: Problem #4: (25 points)
Given an array A of n integers, a leader element of the array A is the element that appears
more than half of the time in A.
Implement in Java an O(n) worst-case algorithm that uses a stack
static int leader(int[] A)
to find a leader element and return the index (any) of the leader in A. The program must
returns -1 if no leader element exists.
Examples:
int[] a = {23, 23, 67, 23, 67, 23, 45}; ====> leader(a) = 5
int[] a = {23, 24, 67, 23, 67, 23, 45}; ====> leader(a) = -1

Author’s Panther ID: 6048821
Certification: I hereby certify that this work is my own and none of it is the work of any other person. 
********************************************************************************************************
*/
package problem4;

import java.util.Stack;

public class Problem4 {

    public int leader(int[] arr){
    
        //This first portion of code finds the index(any) of the leader element(if any).
        Stack<Integer> stack = new Stack<>();    //stack to be used
        int leaderIndex = 0, i = 0, counter = 0; //variables to be used
       
        //Traverse the list while i < arr.length (Complete or full traversal -> O(n))
        while(i < arr.length){
            
            //Push the first element of the array into the stack. Move on(i++)...
            if(i == 0)
            {
                stack.push(arr[i]);
                i++;
            }
           
            //If top element in the stack is the same as the element at index i, leaderIndex = i and move on...
            if(stack.peek() == arr[i])
            {
                leaderIndex = i;
                i++;
            }
            
            //If top element in the stack is different from the element at index i, push this element(arr[i]) into the stack and move on...
            else
            {   
                stack.push(arr[i]);
                i++;
            }
        }
        
        //This second portion of code counts the number of times the leader element is present (or not) in the array.
        //If this number of times(counter) is greater than half the lenght of the array, it returns the index(any) of the leader element. Otherwise, it returns -1 (no leader element found).
        
        //Traverse the list while i < arr.length (Complete or full traversal -> O(n))
        for (i = 0; i < arr.length ; i++)
            if (arr[i] == arr[leaderIndex])
                counter++;
            
        if(counter > (arr.length/2))
            return leaderIndex;
        else
        {
            return -1;
        }  
    }
    
    public static void main(String[] args) {
         //Create instance of our class.
        Problem4 test = new Problem4();
        
        //Update the array here.
        int [] arr = {23,67,23,67,23,67,23};
        
        //If method leader() returns -1, display that no leader was found.
        if(test.leader(arr) == -1)
        {
            System.out.println("Return -1");
            System.out.println("There is no leader element in the array!");
        }
        else //If method leader() returns any integer different than -1, display that a leader was found at that index.
        {
            System.out.println("Leader element was found at index " + test.leader(arr) + ".");
        }
    }
    
}
