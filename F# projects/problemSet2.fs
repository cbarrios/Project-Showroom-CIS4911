//PROBLEM SET 2
(*
1. Discriminated Union
a) Create a discriminated union for Coordinates that can be a Tuple, Threeple or Fourple that represent tuples of size two, three and four. The type for the union should be polymorphic.
b) Instantiate a Tuple of integers, a Threeple of floats and a Fourple of strings.
c) Create a function that has a parameter of a binary function and Coordinate. Apply the function to the Coordinate like List.reduce.
d) Call the function with (+) for each of the Coordinates in part (b).
e) Call the function with (-) for the numeric Coordinates in part (b). Be sure that your function implements the normal associativity for (-).
*)

//(a)
type 'a Coordinates = 
    Tuple of 'a * 'a
    | Threeple of 'a * 'a * 'a
    | Fourple of 'a * 'a * 'a * 'a

 //(b)
let int_tuple = Tuple(4,5)
let float_threeple = Threeple(3.0,4.5,5.0)
let string_fourple = Fourple("We","Are","The","People")

//(c) Got it :)
let addorsub f a = 
    match a with
    | Tuple(x,y) -> f x y                  
    | Threeple(x,y,z) -> f (f x y) z
    | Fourple(x,y,z,w) -> f (f (f x y) z) w
    
//(d)
printfn "%A" (addorsub (+) int_tuple)
printfn "%A" (addorsub (+) float_threeple)
printfn "%A" (addorsub (+) string_fourple)

//(e)
printfn "%A" (addorsub (-) int_tuple)
printfn "%A" (addorsub (-) float_threeple)

//2. Simple F# Syntax Parser 
//  S -> if E then S else S | begin S L | print E
//  L -> end | ; S L
//  E -> i
type SIMPLETERMINAL = IF|THEN|ELSE|BEGIN|END|PRINT|SEMICOLON|ID|EOF

//Im returning [ID] on failure to match the "error" on the test program so that I can run multiple statements without exception handling but alternatively we can failwith
let eat t = function
| [] -> failwith "Nothing to eat"
| x::xs -> if x = t then xs else [ID] //failwith "I do not want this dish..."

let rec S = function
| [] -> []
| IF::xs -> xs |> E |> eat THEN |> S |> eat ELSE |> S
| BEGIN::xs -> xs |> S |> L
| PRINT::xs -> xs |> E
| _::xs -> [ID] //failwith "Error at S"

and L = function 
| [] -> []
| END::xs -> xs 
| SEMICOLON::xs -> xs |> S |> L
| _::xs -> [ID] //failwith "Error at L"

and E = function
| [] -> []
| x::xs -> eat ID (x::xs)

//function to test input
let test_program program =
 let result = program |> S
 match result with 
 | [] -> failwith "Early termination or missing EOF"
 | x::xs -> if x = EOF then "accept" else "error"

let statement1 = [IF;ID;THEN;BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;END;ELSE;PRINT;ID;EOF]     
let statement2 = [IF;ID;THEN;IF;ID;THEN;PRINT;ID;ELSE;PRINT;ID;ELSE;BEGIN;PRINT;ID;END;EOF]
let statement3 = [IF;ID;THEN;BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;SEMICOLON;END;ELSE;PRINT;ID;EOF] //Causes error: 
let statement4 = [PRINT;ID;SEMICOLON;PRINT;ID;EOF]  //Causes error: 
let statement5 = [BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;END;EOF]
let statement6 = [PRINT;ID;EOF]
let statement7 = [IF;ID;THEN;BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;END;ELSE;BEGIN;PRINT;ID;END;EOF] 

printfn "%A" (test_program statement1) // Output is: "accept"
printfn "%A" (test_program statement2) // Output is: "accept"
printfn "%A" (test_program statement3) // Output is: "error"
printfn "%A" (test_program statement4) // Output is: "error"
printfn "%A" (test_program statement5) // Output is: "accept"
printfn "%A" (test_program statement6) // Output is: "accept"
printfn "%A" (test_program statement7) // Output is: "accept"

//3. Mathematical F# Parser
// M -> M + T | M - T | T
// T -> T * F | T / F | F
// F -> i | (M)
type MATHTERMINAL = ID|ADD|SUB|MUL|DIV|LPARENT|RPARENT|EOF

let rec M = function 
| [] -> []
| [EOF] -> [EOF]
| ADD::xs -> xs |> F
| SUB::xs -> xs |> F
| _::xs -> failwith "Error at M"

and T = function 
| [] -> []
| [EOF] -> [EOF]
| MUL::xs -> xs |> F
| DIV::xs -> xs |> F
| _::xs -> failwith "Error at T"

and F = function 
| [] -> []
| [ID] -> [ID]
| [EOF] -> [EOF]
| ID::xs ->      if xs.IsEmpty then [ID] |> F else if xs.Head = ADD || xs.Head = SUB then xs |> M 
                 else if xs.Head = MUL || xs.Head = DIV then xs |> T 
                 else if xs.Head = RPARENT || xs.Head = EOF then xs |> F 
                 else [ID] |> F

| LPARENT::xs -> xs |> F
| RPARENT::xs -> if xs.IsEmpty then [ID] |> F else if xs.Head = ADD || xs.Head = SUB then xs |> M 
                 else if xs.Head = MUL || xs.Head = DIV then xs |> T 
                 else if xs.Head = RPARENT || xs.Head = EOF then xs |> F 
                 else [ID] |> F
| _::xs -> failwith "Error at F"

let rec preprocess = function 
| [],0 -> 0
| _, -1 -> -1 //This line fixes the ORDER LIMITATION of RPARENT declared BEFORE LPARENT
| [],counter -> counter
| LPARENT::xs, counter -> preprocess (xs, counter+1)
| RPARENT::xs, counter -> preprocess (xs, counter-1)
| _::xs , counter -> preprocess (xs, counter)

let validateparents list = preprocess(list,0) //counter starts at 0

//function to test input
let test_simplelang program =
 let isValid = validateparents program
 let result = if isValid = 0 then program |> F else [ID] |> F
 match result with 
 | [] -> failwith "Early termination or missing EOF"
 | x::xs -> if x = EOF then "accept" else "error"

let ss4 = [ID;ADD;ID;ADD;ID;ADD;ID;EOF]
let ss5 = [ID;SUB;ID;MUL;ID;EOF]
let ss6 = [LPARENT;ID;SUB;ID;RPARENT;MUL;ID;EOF]
let ss7 = [ID;ADD;LPARENT;ID;DIV;ID;RPARENT;SUB;LPARENT;ID;MUL;ID;MUL;ID;RPARENT;SUB;ID;EOF]
let ss8 = [ID;ADD;ID;ID;MUL;ID;EOF]
let ss9 = [ID;ADD;ID;MUL;ID;EOF]
let ss10 = [LPARENT;ID;MUL;LPARENT;ID;SUB;ID;RPARENT;ADD;ID;RPARENT]
let ss11 = [LPARENT;ID;MUL;LPARENT;ID;SUB;ID;RPARENT;ADD;ID;EOF]
let ss12 = [RPARENT;MUL;LPARENT;ID;SUB;ID;RPARENT;ADD;LPARENT;EOF]

printfn "%A" (test_simplelang ss4)  // Output is: "accept"
printfn "%A" (test_simplelang ss5)  // Output is: "accept"
printfn "%A" (test_simplelang ss6)  // Output is: "accept"
printfn "%A" (test_simplelang ss7)  // Output is: "accept"
printfn "%A" (test_simplelang ss8)  // Output is: "error" //ID;ID
printfn "%A" (test_simplelang ss9)  // Output is: "accept"
printfn "%A" (test_simplelang ss10) // Output is: "error" //EOF missing
printfn "%A" (test_simplelang ss11) // Output is: "error" //RPARENT missing before EOF
printfn "%A" (test_simplelang ss12) // Output is: "error" //Starts with RPARENT

//4. Curry and Uncurry Functions
let uncurry f = (fun (x,y) -> f x y)        //val uncurry  : f:('a -> 'b -> 'c) -> x:'a * y:'b -> 'c
let uncurry2 f (x,y) = f x y                //val uncurry2 : f:('a -> 'b -> 'c) -> x:'a * y:'b -> 'c

let curry f = (fun x -> fun y -> f(x,y))      //val curry  : f:('a * 'b -> 'c) -> x:'a -> y:'b -> 'c
let curry2 f x y = f (x,y)                    //val curry2 : f:('a * 'b -> 'c) -> x:'a -> y:'b -> 'c

let plus = uncurry (+);;  //val plus : (int * int -> int)
printfn "%A" (plus (2,3)) //val it : int = 5
let cplus = curry plus;;  //val cplus : (int -> int -> int)
let plus3 = cplus 3;;     //val plus3 : (int -> int)
printfn "%A" (plus3 10)   //val it : int = 13

//5. Inner function taking two vectors and returning the sum of their pair product 
//TAIL RECURSIVE VERSION
let rec inner_rec1 = function
| [],[],acc -> acc
| _,[],_ -> failwith "Lists do not have the same length."
| [],_,_-> failwith "Lists do not have the same length."
| x::xs, y::ys, acc -> inner_rec1(xs, ys, acc+(x*y))

let inner1 list1 list2 = inner_rec1(list1,list2,0I) 
printfn "%A" (inner1  [1I..2000000I] [2000001I..4000000I]) //Output is: 6666670666667000000. This takes about 4-5 seconds in my system

//NON-TAIL RECURSIVE VERSION
let rec inner_rec2 = function
| [],[] -> 0I
| _,[] -> failwith "Lists do not have the same length."
| [],_-> failwith "Lists do not have the same length."
| x::xs, y::ys -> x*y+inner_rec2(xs,ys)

let inner2 list1 list2 = inner_rec2(list1,list2) 
printfn "%A" (inner2  [1I..5830I] [5831I..11660I]) //Output varies: sometimes is calculated (165163395705) and other times is StackOverFlow

//6. Matrix multiplication using tranpose, inner and List.map
let xs1 = [[1;2;3];[4;5;6]]
let ys2 = [[0;1];[3;2];[1;2]]

//Without these statements I would not be able to do this problem
let xss1 = transpose xs1
let headhead = (inner1 (List.map List.head xss1) (List.map List.head ys2))
printfn "%A" headhead
let headtail = (inner1 (List.map List.head xss1) (List.map List.head (List.map List.tail ys2)))
printfn "%A" headtail
let tailhead = (inner1 (List.map List.head (List.map List.tail xss1)) (List.map List.head ys2))
printfn "%A" tailhead
let tailtail = (inner1 (List.map List.head (List.map List.tail xss1)) (List.map List.head (List.map List.tail ys2)))
printfn "%A" tailtail
//THESE WERE VERY HELPFUL
//let testing22 = List.map List.head (List.map List.tail xs1)
//printfn "%A" testing22
//let list123 = ([3]@[4])::[3::[4]]
//printfn "%A" list123

//FINAL FUNCTION DONE :)))
let multiply_aux (xs1,ys2) = ([(inner1 (List.map List.head xs1) (List.map List.head ys2))]@[(inner1 (List.map List.head xs1) (List.map List.head (List.map List.tail ys2)))])::[(inner1 (List.map List.head (List.map List.tail xs1)) (List.map List.head ys2))::[(inner1 (List.map List.head (List.map List.tail xs1)) (List.map List.head (List.map List.tail ys2)))]]
let multiply (xs1,ys2) = multiply_aux(transpose xs1, ys2)
printfn "%A" (multiply(xs1,ys2)) //Output is: [[9; 11]; [21; 26]] 

//7. Evaluate the asymptotic time complexity of this function:
let rec oddeven = function
| [] -> []
| x::xs -> if x % 2 = 0 
           then oddeven xs @ [x] //Appending to list is O(n) but doing it 'n' times means worst-case time complexity of O(n^2)
           else x :: oddeven xs //Prepending is O(1) but doing it 'n' times means O(n) complexity

let evenlist = [ 0..2+1..100 ]
printfn "%A" (oddeven evenlist)

//8. Time complexity
let rec fold f a = function
 | []    -> a                   
 | x::xs -> fold f (f a x) xs;; // here with (f a x) we are bringing the empty list first but then 'a' increases its length as we append only one element to the end, which is not very efficient -> O(n^2)
                                // then we are eventually appending a bigger list with a smaller list, but this is tail recursive, which it's definitely an improvement -> still O(n^2)
let rec foldBack f xs a =
 match xs with
 | []    -> a
 | y::ys -> f y (foldBack f ys a);; // here with (foldBack f ys a) we are recursively calling the function with a shorter list 'ys' but 'a' remains as the empty list
                                    // however, from the outer view we, after recursion we are eventually prepending only one element with a list that increases its length which is efficient O(n) 
                                    // but notice that this version is non tail recursive, which ends up being not very efficient in terms of stack space

let flatten1 xs = fold (@) [] xs // in fold we have 3 parameters: (@) is 'f' , [] is 'a' , xs is hidden paremeter ('a list list) via pattern matching
let flatten2 xs = foldBack (@) xs [] // in foldBack we have 3 parameters: (@) is 'f' , xs is 'xs' ('a list list) , [] is 'a'

printfn "%A" (flatten1 [["a"];["bvvv"];["aaaaa"]])
printfn "%A" (flatten2 [[1];[2];[3]])

//9.Find last element (recursively) using the discriminated union 'option'
type 'a option = None | Some of 'a

let optionHelper = function
 | None -> "Invalid Input"
 | Some x -> x.ToString()

let rec lastElem = function
 | [] -> None
 | [x] -> Some x
 | _::xs -> lastElem xs

let emptyList = []
let oneElemList = ["cat"]
let mulElemList = [1;2;3;4;5]

let functionTester xs = optionHelper (lastElem xs)
printfn "The last element of %A is %A" emptyList (functionTester emptyList)
printfn "The last element of %A is %A" oneElemList (functionTester oneElemList)
printfn "The last element of %A is %A" mulElemList (functionTester mulElemList)

//10. Interpeter (evaluating numerical expressions)
type Exp =
   Num of int
 | Neg of Exp
 | Sum of Exp * Exp
 | Diff of Exp * Exp
 | Prod of Exp * Exp
 | Quot of Exp * Exp

 //Got it :)))
let rec evaluate = function
    | Num n -> Some n
    | Neg e -> match evaluate e with 
               | Some e -> Some -e
               | None -> None 
  
    | Sum(e1,e2) -> match evaluate e2 with
                      | Some e2 -> match evaluate e1 with 
                                   | Some e1 -> Some(e1+e2)
                                   | None -> None
                      | None -> None
 
    | Diff(e1,e2) -> match evaluate e2 with
                      | Some e2 -> match evaluate e1 with 
                                   | Some e1 -> Some(e1-e2)
                                   | None -> None
                      | None -> None
   
    | Prod(e1,e2) -> match evaluate e2 with
                      | Some e2 -> match evaluate e1 with 
                                   | Some e1 -> Some(e1*e2)
                                   | None -> None
                      | None -> None
    
    | Quot(e1,e2) -> match evaluate e2 with
                      | Some e2 -> match evaluate e1 with 
                                   | Some e1 -> if e2 = 0 then None else Some (e1/e2)
                                   | None -> None
                      | None -> None

printfn "%A" ( evaluate (Num 3) )                                          //Some 3
printfn "%A" ( evaluate (Neg(Neg(Neg(Num 3)))) )                           //Some -3
printfn "%A" ( evaluate (Sum(Num 3, Num 4)) )                              //Some 7
printfn "%A" ( evaluate (Diff(Num 3, Num 4)) )                             //Some -1
printfn "%A" ( evaluate (Prod(Num 3, Num 4)) )                             //Some 12
printfn "%A" ( evaluate (Quot(Num 3, Num 3)) )                             //Some 1
printfn "%A" ( evaluate (Prod(Num 3, Diff(Num 5, Num 1))) )                //Some 12
printfn "%A" ( evaluate (Diff(Num 3, Quot(Num 5, Prod(Num 7, Num 0)))) )   //None
printfn "%A" ( evaluate (Sum((Neg(Neg(Neg(Num 3)))),Num 3)) )              //Some 0
printfn "%A" ( evaluate (Sum((Neg(Neg(Neg((Sum(Num 3, Num 0)))))),Prod(Num 3, Diff(Quot(Num 1, Num 1),Num 0)) )) ) //Some 0  :))
printfn "%A" ( evaluate (Sum((Neg(Neg(Neg((Sum(Num 3, Num 0)))))),Prod(Num 3, Diff(Quot(Num 1, Num 0),Num 0)) )) ) //None    :))

//11. Records
// Create a record type for Name, Credits and GPA.
// Create a record instance with the values "Jones", 109, 3.85.
type Student = { Name:string ; Credits:int ; GPA:float }
let rec_student = { Name="Jones" ; Credits=109 ; GPA=3.85 }

//12. Binary Search Tree (Init tree, Delete nodes of tree)
//tree structure
type Tree<'a> = 
  | Empty
  | Node of value: 'a * left: Tree<'a> * right: Tree<'a>

//tree init
let tree =
  Node (8, 
    Node (2, 
      Node (1,
        Node (-6,
          Node(-10,Empty,Empty),
          Empty),
        Empty),
      Node (4, 
        Node (3, Empty, Empty), 
        Node (7, Empty, Empty))), 
    Node (11, 
      Node (10, Empty, Empty), 
      Node (15, 
        Empty, 
        Node (20,
          Node (19,Empty,Empty),
          Node (35,Empty,Empty)))))

//helper function to find the in-order predecessor node (right-most node of a given tree)
//this helper is used in the delete function when we want to delete the root node of a given tree
//to delete the root node we call this helper with the left subtree of the original given tree
//this way we are guaranteed to get the max value in this left subtree which is the one we want to switch as the new root
let rec findInOrderPredecessor (tree : Tree<'a>) =
  match tree with
  | Empty -> Empty
  | Node (_, _, Empty) -> tree
  | Node (_, _, right) -> findInOrderPredecessor right
printfn "%A" (findInOrderPredecessor tree)

let rec delete value (tree : Tree<'a>) =
  match tree with
  | Empty -> Empty
 
  | Node (root, left, right) when value < root -> 
    let newleft = delete value left 
    Node (root, newleft, right)
  
  | Node (root, left, right) when value > root ->
    let newright = delete value right
    Node (root, left, newright)
 
  | Node (root, Empty, Empty) -> Empty
  | Node (root, left, Empty) -> left
  | Node (root, Empty, right) -> right

  | Node (root, left, right) when value = root ->
    let (Node(newroot, _, _)) = findInOrderPredecessor left
    let newleft = delete newroot left
    Node (newroot, newleft, right)

//delete does nothing because 0 is not in the tree -> original tree is returned
printfn "%A" (delete 0 tree) 

//delete root -> new tree is returned with 7 as the new root and with a new left subtree
//(8's in-order predecessor (7) gets eliminated from left subtree and then replaced as the new root)
printfn "%A" (delete 8 tree) 

//delete 2 (having left and right) -> new tree is returned with same root but with a new left subtree 
//(2's in-order predecessor (1) gets eliminated from left subtree of 2 and then replaced as the new root of the new left subtree of 8(root))
printfn "%A" (delete 2 tree) 

//delete 11 (having left and right) -> new tree is returned with same root but with a new right subtree 
//(11's in-order predecessor (10) gets eliminated from left subtree of 11 and then replaced as the new root of the new right subtree of 8(root))
printfn "%A" (delete 11 tree) 

//delete 1 (having only left) -> new tree is returned with same root but with a new left subtree 
//(In this case "| Node (root, left, Empty) -> left" is matched. Therefore, we just replace the left subtree of 1 as the new left subtree of 2)
printfn "%A" (delete 1 tree) 

//delete 15 (having only right) -> new tree is returned with same root but with a new right subtree 
//(In this case "| Node (root, Empty, right) -> right" is matched. Therefore, we just replace the right subtree of 15 as the new right subtree of 11)
printfn "%A" (delete 15 tree) 

//13. Building a parse tree
// (a)
//  S -> if E then S else S | begin S L | print E
//  L -> end | ; S L
//  E -> i
//type SIMPLETERMINAL = IF|THEN|ELSE|BEGIN|END|PRINT|SEMICOLON|ID|EOF|ERROR

//Eat function
let EAT t = function
| [] -> failwith "Nothing to eat"
| x::xs -> if x = t then xs else [ERROR] //failwith "I do not want this dish..."

//Had to implement this instead of using .Value
let getValue = function
| Some(x) -> x
| None -> failwith "No value from None"

//tree structure
type parseTree = 
  | Lf of SIMPLETERMINAL
  | Sif of parseTree * parseTree * parseTree * parseTree * parseTree * parseTree
  | Sbegin of parseTree * parseTree * parseTree
  | Sprint of parseTree * parseTree
  | Lend of parseTree
  | Lsemi of parseTree * parseTree * parseTree
  | Ei of parseTree

let rec S_parsetree = function
| [] -> failwith "Early termination"
| x::xs -> 
    match x with
    | IF -> 
        let(E_tree, remain) = xs |> E_parsetree 
        let remain = remain |> EAT THEN 
        let(S_tree1, remain) = remain |> S_parsetree 
        let remain = remain |> EAT ELSE 
        let(S_tree2, remain) = remain |> S_parsetree
        if S_tree2 = None then (None, x::xs) else (Some(Sif(Lf(IF),getValue E_tree,Lf(THEN),getValue S_tree1,Lf(ELSE),getValue S_tree2)), remain)
    | BEGIN -> 
        let(S_tree, remain) = xs |> S_parsetree 
        let(L_tree, remain)= remain |> L_parsetree
        if L_tree = None then (None, x::xs) else (Some(Sbegin(Lf(BEGIN),getValue S_tree,getValue L_tree)), remain)
    | PRINT -> 
        let(E_tree, remain) = xs |> E_parsetree 
        if E_tree = None then (None, x::xs) else (Some(Sprint(Lf(PRINT),getValue E_tree)), remain)
    | _ -> (None, [ERROR]) //failwith "Error at S_parsetree"

and L_parsetree = function 
| [] -> failwith "Early termination"
| x::xs -> 
    match x with
    | END -> (Some(Lend(Lf(END))), xs)
    | SEMICOLON -> 
        let(S_tree, remain) = xs |> S_parsetree 
        let(L_tree, remain) = remain |> L_parsetree
        if L_tree = None then (None, x::xs) else (Some(Lsemi(Lf(SEMICOLON),getValue S_tree,getValue L_tree)), remain)
    | _ -> (None, [ERROR]) //failwith "Error at L_parsetree"

and E_parsetree = function
| [] -> failwith "Early termination"
| x::xs -> 
    match x with
    | ID -> (Some(Ei(Lf(ID))), xs)
    | _ -> (None, [ERROR]) //failwith "Error at E_parsetree"

let buildTree = function
| [] -> failwith "Error"
| xs -> let (S_tree, tokens) = xs |> S_parsetree
        if tokens <> [EOF] || S_tree = None
        then printfn "Want [EOF], got %A" tokens 
             Lf(ERROR)
        else (getValue S_tree)

let test1 = [IF;ID;THEN;BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;END;ELSE;PRINT;ID;EOF]     
let test2 = [IF;ID;THEN;IF;ID;THEN;PRINT;ID;ELSE;PRINT;ID;ELSE;BEGIN;PRINT;ID;END;EOF]
let test3 = [IF;ID;THEN;BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;SEMICOLON;END;ELSE;PRINT;ID;EOF] //Causes error: 
let test4 = [PRINT;ID;SEMICOLON;PRINT;ID;EOF]  //Causes error: 
let test5 = [BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;END;EOF]
let test6 = [PRINT;ID;EOF]
let test7 = [IF;ID;THEN;BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;END;ELSE;BEGIN;PRINT;ID;END;EOF] 

printfn "%A" (buildTree test1)
printfn "%A" (buildTree test2)
printfn "%A" (buildTree test3)   //-> Want [EOF], got [IF; ID; THEN; BEGIN; PRINT; ID; SEMICOLON; PRINT; ID; SEMICOLON; END; ELSE; PRINT; ID; EOF]
printfn "%A" (buildTree test4)   //-> Want [EOF], got [SEMICOLON; PRINT; ID; EOF] Lf ERROR
printfn "%A" (buildTree test5)
printfn "%A" (buildTree test6)
printfn "%A" (buildTree test7)

//Current limitations
//(1) Associativity fails since it is done backwards
//(2) Only one pair of parents supported
//13.Terminals and productions
// (b)
// E -> E + T | E - T | T
// T -> T * F | T / F | F
// F -> i | (E)
type SIMPLEMATH = ADD | SUB | MUL | DIV | ID | LPARENT | RPARENT | EOF | ERROR

//tree structure
type parseTree_math = 
  | Lf of SIMPLEMATH
  | Add of parseTree_math * parseTree_math
  | Sub of parseTree_math * parseTree_math
  | Mul of parseTree_math * parseTree_math
  | Div of parseTree_math * parseTree_math
  | Id of parseTree_math
  | Parents of parseTree_math * parseTree_math * parseTree_math


let rec E_math = function 
| [] -> failwith "Early termination"
| x::xs -> 
    match x with
    | ADD ->
        let(F_tree, remain) = xs |> F_math 
        if F_tree = None then (None, x::xs) else (Some(getValue F_tree), remain)
    | SUB -> 
        let(F_tree, remain) = xs |> F_math 
        if F_tree = None then (None, x::xs) else (Some(getValue F_tree), remain)
    | _ -> (None, [ERROR]) //failwith "Error at E_math"

and T_math = function 
| [] -> failwith "Early termination"
| x::xs -> 
    match x with
    | MUL ->
        let(F_tree, remain) = xs |> F_math 
        if F_tree = None then (None, x::xs) else (Some(getValue F_tree), remain)
    | DIV -> 
        let(F_tree, remain) = xs |> F_math 
        if F_tree = None then (None, x::xs) else (Some(getValue F_tree), remain)
    | _ -> (None, [ERROR]) //failwith "Error at T_math"

and F_math = function 
| [] -> failwith "Early termination"
| [ERROR] -> (None, [ERROR])
| x::xs -> 
    match x with
    | ID ->      if xs.IsEmpty then (None, [ERROR]) 
                 else if xs.Head = ADD then
                                          ( 
                                            let(E_tree, remain) = xs |> E_math 
                                            if E_tree = None then (None, x::xs) else (Some(Add(Id(Lf(ID)),getValue E_tree)), remain)
                                          )
                 else if xs.Head = SUB then
                                          (
                                            let(E_tree, remain) = xs |> E_math 
                                            if E_tree = None then (None, x::xs) else (Some(Sub(Id(Lf(ID)),getValue E_tree)), remain)
                                          )
                       
                 else if xs.Head = MUL then
                                          ( 
                                            let(T_tree, remain) = xs |> T_math 
                                            if T_tree = None then (None, x::xs) else (Some(Mul(Id(Lf(ID)),getValue T_tree)), remain)
                                          )
                 else if xs.Head = DIV then
                                          (
                                            let(T_tree, remain) = xs |> T_math 
                                            if T_tree = None then (None, x::xs) else (Some(Div(Id(Lf(ID)),getValue T_tree)), remain)
                                          )
                 else if xs.Head = RPARENT then
                                              (
                                                let(F_tree, remain) = xs |> F_math 
                                                if F_tree = None then (None, x::xs) else (Some(getValue F_tree), remain)
                                              )               
                 else if xs.Head = EOF then (Some(Id(Lf(ID))), xs)
                                        
                 else (None, [ERROR])

    | LPARENT -> 
        let(F_tree, remain) = xs |> F_math 
        if F_tree = None then (None, x::xs) else (Some(Parents(Lf(LPARENT),getValue F_tree,Lf(RPARENT))), remain)
    | RPARENT ->  if xs.IsEmpty then (None, [ERROR]) 
                  else if xs.Head = ADD then
                                          ( 
                                            let(E_tree, remain) = xs |> E_math 
                                            if E_tree = None then (None, x::xs) else (Some(Add(Id(Lf(ID)),getValue E_tree)), remain)
                                          )
                  else if xs.Head = SUB then
                                          (
                                            let(E_tree, remain) = xs |> E_math 
                                            if E_tree = None then (None, x::xs) else (Some(Sub(Id(Lf(ID)),getValue E_tree)), remain)
                                          )
                       
                  else if xs.Head = MUL then
                                          ( 
                                            let(T_tree, remain) = xs |> T_math 
                                            if T_tree = None then (None, x::xs) else (Some(Mul(Id(Lf(ID)),getValue T_tree)), remain)
                                          )
                  else if xs.Head = DIV then
                                          (
                                            let(T_tree, remain) = xs |> T_math 
                                            if T_tree = None then (None, x::xs) else (Some(Div(Id(Lf(ID)),getValue T_tree)), remain)
                                          )
                  else if xs.Head = RPARENT then
                                               (
                                                 let(F_tree, remain) = xs |> F_math 
                                                 if F_tree = None then (None, x::xs) else (Some(getValue F_tree), remain)
                                               ) 
                  else if xs.Head = EOF then (Some(Id(Lf(ID))), xs)
                                        
                  else (None, [ERROR])
    | _ -> (None, [ERROR]) //failwith "Error at F_math"

let buildmathTree = function
| [] -> failwith "Error"
| xs -> let (F_tree, tokens) = xs |> F_math
        if tokens <> [EOF] || F_tree = None
        then printfn "Want [EOF], got %A" tokens 
             Lf(ERROR)
        else (getValue F_tree)

let mathtest1 = [ID;ADD;ID;ADD;ID;SUB;ID;EOF]
let mathtest2 = [ID;SUB;ID;MUL;ID;EOF]
let mathtest3 = [LPARENT;ID;SUB;ID;RPARENT;EOF]
let mathtest4 = [ID;ADD;LPARENT;ID;DIV;ID;RPARENT;SUB;LPARENT;ID;MUL;ID;MUL;ID;RPARENT;SUB;ID;EOF]
let mathtest5 = [ID;ADD;ID;ID;MUL;ID;EOF]
let mathtest6 = [ID;ADD;ID;MUL;ID;ADD;ID;EOF]
let mathtest7 = [LPARENT;ID;MUL;ID;RPARENT;EOF]
let mathtest8 = [LPARENT;ID;MUL;LPARENT;ID;SUB;ID;RPARENT;ADD;ID;EOF]
let mathtest9 = [RPARENT;MUL;LPARENT;ID;SUB;ID;RPARENT;ADD;LPARENT;EOF]

printfn "%A" (buildmathTree mathtest1)
printfn "%A" (buildmathTree mathtest2)
printfn "%A" (buildmathTree mathtest3)
printfn "%A" (buildmathTree mathtest4)
printfn "%A" (buildmathTree mathtest5)
printfn "%A" (buildmathTree mathtest6)
printfn "%A" (buildmathTree mathtest7)
printfn "%A" (buildmathTree mathtest8)
printfn "%A" (buildmathTree mathtest9)
