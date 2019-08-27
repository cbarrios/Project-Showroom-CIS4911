/*
*******************************************************************************************************
Purpose/Description: Problem #3: (25 points)
(a) Implement a program in Java to sort a stack of integer in ascending order using for
this purpose another auxiliary (temp) stack.
You should not make any assumptions about how the stack is implemented. The
following are the only functions that should be used to write this program: push, pop,
peek, and isEmpty.
peek - operation that returns the value of the top element of the stack without
removing it.

(b) What is the running time complexity of your program? Justify.
-->ANSWER: I analyzed both best and worst case scenarios for this algorithm. The running
time complexity I got for best case is linear or O(n). However, for the worst case I got
the following formula for number of iteratioms of any input n: ((n^2)/2 + n/2). Taking the Big O
from this formula and the linear complexity for the best case I get: O((n^2)/2 + n/2 + n) ->
= O(max((n^2)/2 + n/2 + n))) = O((n^2)/2) = O(n^2). Therefore, I conclude that the running
time complexity for this solution is at most quadratic (> than linear but < than quadratic).

Author’s Panther ID: 6048821
Certification: I hereby certify that this work is my own and none of it is the work of any other person. 
********************************************************************************************************
*/
package problem3;

import java.util.Stack;

public class Problem3 {
    
    //Method sortStack(Stack<Integer>) accepts the original stack to be sorted and returns the sorted stack as temporal
    public static Stack<Integer> sortStack(Stack<Integer> original){
        
        //Temporal stack to be returned (sorted)
        Stack<Integer> temporal = new Stack<>(); 
        
        //While there are elements in the original stack...
        while(!original.isEmpty()) { 
            int temp = original.pop(); //A "pop" is executed from the original stack and its value is assigned to temp
            
            //While there are elements in the temporal stack AND(&&) the top element(peek) of this stack is greater than the value of temp...
            while(!temporal.isEmpty() && temporal.peek() > temp) {
                original.push(temporal.pop()); //A "pop" is executed from the temporal stack and its value is pushed back into the original stack
            }
            
            //As long as temp > temporal.peek() or after the execution of the while loop above, temp is pushed into the temporal stack
            temporal.push(temp);
           
        }
        return temporal; //return final sorted stack
    }
    
    public static void main(String[] args) {
        //Create instance of Stack<Integer>
        Stack<Integer> original = new Stack<>();
        
        //Add elements to the stack. Update them here.
        original.push(1);
        original.push(2);
        original.push(3);
        original.push(4);
        original.push(5);
        //Note: When the original array is already sorted, this algorithm runs its worst case like in the example above.
        //Note: Best case is when the original stack is sorted in descending order (Inverse of the previous example or worst case).
       
        //Display original stack and final sorted stack
        System.out.println("Original Stack: " + original);
        System.out.println("Final Sorted Stack: " + sortStack(original));
    }
    
}
