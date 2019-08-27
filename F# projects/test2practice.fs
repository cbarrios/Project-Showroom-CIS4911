//1.Discriminated Union
//a)
type Coordinates<'a> =
    Tuple of 'a * 'a
    |Threeple of 'a * 'a * 'a
    |Fourple of 'a * 'a * 'a * 'a
//b)
let int_tuple = Tuple(1,10)
let float_threeple = Threeple(1.0,2.0,3.0)
let string_fourple = Fourple("I","love","computer","science")
//c)
let doit f = function
| Tuple(x,y) -> f x y
| Threeple(x,y,z) -> f (f x y) z
| Fourple(w,x,y,z) -> f (f (f w x) y) z
//d)
printfn "%A" (doit (+) int_tuple)
printfn "%A" (doit (+) float_threeple)
printfn "%A" (doit (+) string_fourple)
//e)
printfn "%A" (doit (-) int_tuple)
printfn "%A" (doit (-) float_threeple)

//2.Parser
//  S -> if E then S else S | begin S L | print E
//  L -> end | ; S L
//  E -> i
type TERMINAL = IF|THEN|ELSE|BEGIN|END|PRINT|SEMICOLON|ID|EOF

let EAT t = function
|[] -> failwith "Nothing to eat"
|x::xs -> if x = t then xs else failwith "EATING DISORDER"

let rec S = function
|[] -> failwith "Early termination"
|x::xs ->
    match x with 
    |IF -> xs |> E |> EAT THEN |> S |> EAT ELSE |> S
    |BEGIN -> xs |> S |> L
    |PRINT -> xs |> E
    |_ -> failwith "Error at S"

and L = function
|[] -> failwith "Early termination"
|x::xs ->
    match x with 
    |END -> xs
    |SEMICOLON -> xs |> S |> L
    |_ -> failwith "Error at L"

and E = function
|[] -> failwith "Early termination"
|x::xs ->
    match x with 
    |ID -> xs
    |_ -> failwith "Error at E"

let test_program program =
      let result = program |> S
      match result with 
      | [] -> failwith "Early termination or missing EOF"
      | x::xs -> if x = EOF then "accept" else "error"

//Testing
printfn "%A" (test_program [IF;ID;THEN;BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;END;ELSE;PRINT;ID;EOF])
printfn "%A" (test_program [IF;ID;THEN;IF;ID;THEN;PRINT;ID;ELSE;PRINT;ID;ELSE;BEGIN;PRINT;ID;END;EOF])
printfn "%A" (test_program [IF;ID;THEN;BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;SEMICOLON;END;ELSE;PRINT;ID;EOF])

//3.Math parser
//M -> M + T | M - T | T
//T -> T * F | T / F | F
//F -> i | (M)
type MATH = ADD|SUB|MUL|DIV|ID|LPARENT|RPARENT|EOF|ERROR

let eat t = function
|[] -> failwith "Nothing to eat"
|x::xs -> if x = t then xs else [ERROR]

let rec F = function
|[] -> failwith "Early termination at F"
|x::xs ->
    match x with 
    |EOF -> if xs.IsEmpty then [EOF] else [ERROR]
    |ID -> 
        let tail = xs |> M
        match tail with
        |ID::ID::xs -> [ERROR]
        |EOF::[] -> [EOF]
        |ID::xs -> tail |> F
        |LPARENT::xs -> tail |> F
        |RPARENT::xs -> RPARENT::xs
        |_ -> [ERROR] //failwith "INNER Error at F"
    |LPARENT -> xs |> M |> eat RPARENT
    |RPARENT -> RPARENT::xs
    |_ -> [ERROR] //failwith "Error at F"


and M = function 
|[] -> failwith "Early termination at M"
|x::xs -> 
    match x with 
    |ADD -> xs 
    |SUB -> xs 
    |x-> x::xs |> T

and T = function
|[] -> failwith "Early termination at T"
|x::xs -> 
    match x with 
    |MUL -> xs
    |DIV -> xs
    |x-> x::xs |> F

let test_math program = 
   let result = program |> F
   match result with 
   | [] -> failwith "Early termination or missing EOF"
   | x::xs -> if x = EOF then "accept" else "error"

//Testing 
let ss4 = [ID;ADD;ID;ADD;ID;ADD;ID;EOF]
let ss5 = [ID;SUB;ID;MUL;ID;EOF]
let ss6 = [LPARENT;LPARENT;RPARENT;RPARENT;EOF]
let ss7 = [ID;ADD;LPARENT;ID;DIV;ID;RPARENT;EOF]
let ss8 = [ID;ADD;ID;ID;MUL;ID;EOF]
let ss9 = [ID;ADD;ID;MUL;ID;EOF]
let ss10 = [LPARENT;ID;MUL;LPARENT;ID;SUB;ID;RPARENT;ADD;ID;RPARENT;EOF]
let ss11 = [LPARENT;ID;MUL;LPARENT;ID;SUB;ID;RPARENT;EOF]
let ss12 = [RPARENT;MUL;LPARENT;ID;SUB;ID;RPARENT;ADD;LPARENT;EOF]

printfn "%A" (test_math ss4)  // Output is: "accept"
printfn "%A" (test_math ss5)  // Output is: "accept"
printfn "%A" (test_math ss6)  // Output is: "accept"
printfn "%A" (test_math ss7)  // Output is: "accept"
printfn "%A" (test_math ss8)  // Output is: "error" //ID;ID
printfn "%A" (test_math ss9)  // Output is: "accept"
printfn "%A" (test_math ss10) // Output is: "error" //There is ADD;ID; after first RPARENT
printfn "%A" (test_math ss11) // Output is: "error" //second RPARENT missing before EOF
printfn "%A" (test_math ss12) // Output is: "error" //Starts with RPARENT

//4. Curry and Uncurry
let curry f x y = f (x,y)
let uncurry f (x,y) = f x y

let plus = uncurry (+)
let cplus = curry (plus)

//5.Tail recursion vs non-tail
//Non-tail recursive
let rec non_tail_inner_helper = function
|[],[] -> 0I
|[],_ -> failwith "Lists with different length not allowed"
|_,[] -> failwith "Lists with different length not allowed"
|x::xs,y::ys -> (x*y)+non_tail_inner_helper(xs,ys)

let non_tail_inner list1 list2 = non_tail_inner_helper (list1,list2)
//printfn "%A" (non_tail_inner [1I..5830I] [5831I..11660I]) //STACK OVERFLOW

//Tail Recursive Version
let rec tail_inner_helper = function
|[],[],acc -> acc
|[],_,_ -> failwith "Lists with different length not allowed"
|_,[],_ -> failwith "Lists with different length not allowed"
|x::xs,y::ys,acc -> tail_inner_helper(xs,ys,acc+(x*y))

let tail_inner list1 list2 = tail_inner_helper (list1,list2,0I)
printfn "%A" (tail_inner  [1I..2000000I] [2000001I..4000000I]) //Calculated in 4-5 seconds

//6.Matrix multiplication
let rec transpose = function
    |[]::[[]] -> []
    |[]::_ -> failwith "Lists do not have same length"
    |_::[[]] -> failwith "Lists do not have same length"
    |listlist -> List.map List.head listlist::transpose(List.map List.tail listlist) 

let listlist = [[1;2;3];[4;5;6]]
printfn "%A" (transpose listlist) 
              //[[1;4]; [2;5]; [3;6]]
let listlist2 = [[0;1]; [3;2]; [1;2]]

//To do the matrix multiplication we need to follow this syntax:
let syntax = ([9]@[11])::[21::[26]]
printfn "%A" syntax //[[9; 11]; [21; 26]]
//Now to calculate the first 9 we need this:
let headhead = tail_inner (List.map List.head (transpose listlist)) (List.map List.head listlist2) 
printfn "%A" headhead //9
//To calculate the 11:
let headtail = tail_inner (List.map List.head (transpose listlist)) (List.map List.head (List.map List.tail listlist2))
printfn "%A" headtail //11
//To do the 21:
let tailhead = tail_inner (List.map List.head (List.map List.tail (transpose listlist))) (List.map List.head listlist2)
printfn "%A" tailhead //21
//To get the last element 26:
let tailtail = tail_inner (List.map List.head (List.map List.tail (transpose listlist ))) (List.map List.head (List.map List.tail listlist2))
printfn "%A" tailtail //26

let multiply (listlist, listlist2) =
    let headhead = tail_inner (List.map List.head (transpose listlist)) (List.map List.head listlist2)
    let headtail = tail_inner (List.map List.head (transpose listlist)) (List.map List.head (List.map List.tail listlist2))
    let tailhead = tail_inner (List.map List.head (List.map List.tail (transpose listlist))) (List.map List.head listlist2)
    let tailtail = tail_inner (List.map List.head (List.map List.tail (transpose listlist ))) (List.map List.head (List.map List.tail listlist2))
    ([headhead]@[headtail])::[tailhead::[tailtail]]

printfn "%A" (multiply ([[1;2;3];[4;5;6]], [[0;1];[3;2];[1;2]]))
//[[9; 11]; [21; 26]] 

//7. Time Complexity
let rec oddeven = function
| [] -> []
| x::xs -> if x % 2 = 0             //If there is at least one even number in the list -> O(n^2) else -> O(n) 
           then oddeven xs @ [x]    //Appending  -> O(n) but we make n recursive calls -> O(n^2)
           else x :: oddeven xs     //Prepending -> O(1) but we make n recursive calls -> O(n)

printfn "%A" (oddeven [3..2..24525]) // -> O(n)   //STACK OVERFLOW
printfn "%A" (oddeven [2..2..24350]) // -> O(n^2) //STACK OVERFLOW with less values than the previous list

//8. Time Complexity
let rec fold f a = function
    | []    -> a  //tail recursive: when xs is empty we return a, which is equivalent to the last computed (f a x)
    | x::xs -> fold f (f a x) xs //(f a x): a is the inner accumulator so we are appending in each recursive call the element x to the tail of the inner accumulator -> O(n^2)

let rec foldBack f xs a =
    match xs with
    | []    -> a
    | y::ys -> f y (foldBack f ys a) //We are prepending the element 'y' n times since there are n recursive calls -> O(n)
                                     //This is equivalen to a statement like this:  | y::ys -> y::(foldBack ys a). 
                                     //Therefore this is NON-tail recursive: We build the list going backwards -> easy STACK OVERFLOW
//Equivalent foldBack               
let rec foldBack2 xs a =
    match xs with
    | []    -> a
    | y::ys -> y::(foldBack2 ys a)

let flatten1 xs = fold (@) [] xs
let flatten2 xs = foldBack (@) xs []

let testlist = List.map (fun n -> [n]) [1..100000]

printfn "%A" (flatten1 testlist) //NO STACK OVERFLOW -> despite being O(n^2) it is tail-recursive (takes 1 minute and 18 seconds in my system)
printfn "%A" (flatten2 testlist) //STACK OVERFLOW -> despite being O(n) it is non-tail recursive

//9. Get last element with None or Some
type 'a option = None | Some of 'a
let last_helper = function
|None -> "Invalid Input"
|Some x -> x.ToString() 

let rec last = function
|[] -> last_helper None
|[x] -> last_helper (Some x)
|_::xs -> last xs

printfn "The last element of %A is %A" [] (last [])           //"Invalid Input"
printfn "The last element of %A is %A" ["cat"] (last ["cat"]) //"cat"
printfn "The last element of %A is %A" [1;2;3] (last [1;2;3]) //"3"

//10. Math Interpreter
type Exp = 
    Num of int
  | Neg of Exp
  | Sum of Exp * Exp
  | Diff of Exp * Exp
  | Prod of Exp * Exp
  | Quot of Exp * Exp

let rec evaluate = function
  | Num n -> Some n

  | Neg e -> match evaluate e with
             |Some e -> Some (-e)
             |_ -> None

  | Sum(e1,e2) -> match (evaluate e1, evaluate e2) with
                  |Some x, Some y -> Some(x+y)
                  |_ -> None
  
  | Diff(e1,e2) -> match (evaluate e1, evaluate e2) with
                   |Some x, Some y -> Some(x-y)
                   |_ -> None
  
  | Prod(e1,e2) -> match (evaluate e1, evaluate e2) with
                   |Some x, Some y -> Some(x*y)
                   |_ -> None

  | Quot(e1,e2) -> match (evaluate e1, evaluate e2) with
                   |Some x, Some 0 -> None
                   |Some x, Some y -> Some(x/y)
                   |_ -> None

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
//Create a record type for Name, Credits and GPA.
//Create a record instance with the values "Jones", 109, 3.85.
type record = {Name:string ; Credits:int ; GPA:float}
let student = {Name="Jones" ; Credits=109 ; GPA=3.85}
printfn "Student's Name: %A | Credits: %A | GPA: %A" student.Name student.Credits student.GPA

//12.Binary Search Tree
//tree structure
type 'a binaryTree =
     Empty
    |Node of value:'a * left: 'a binaryTree * right: 'a binaryTree

//Create a tree
let tree = Node(3,Node(1,Empty,Empty),Node(5,Empty,Empty))
printfn "%A" tree

//Get all the nodes (increasing order)
let rec getvalues = function
|Empty -> []
|Node(v,l,r) -> (getvalues l)@(v::getvalues r)
printfn "%A" (getvalues tree)

//Search for a particular value in the tree
let rec inTree arg = function
|Empty -> None
|Node(v,l,r) ->if v = arg then (Some arg) else (
                                                 let value = inTree arg l
                                                 if value = None then (inTree arg r) else value
                                               )
printfn "%A" (inTree 3 tree) //Some 3
printfn "%A" (inTree 1 tree) //Some 1
printfn "%A" (inTree 5 tree) //Some 5
printfn "%A" (inTree 8 tree) //None

//Insert a node
let rec insert node (tree: 'a binaryTree) =
    match tree with
    |Empty -> Node(node,Empty,Empty)
    |Node(root,l,r) when node < root -> 
        let newleft = insert node l
        Node(root,newleft,r)
                                      
    |Node(root,l,r) when node > root -> 
        let newright = insert node r
        Node(root,l,newright)                                                                    
    |_ -> tree //when node = root of any tree(including subtrees) we return the initial given tree (we dont want to insert repeating values)

let newtree = Empty |> insert 2 |> insert 1 |> insert 5 |> insert 4 |> insert 0 |> insert 3 
let newtree2 = Empty |> insert 2 |> insert 1 |> insert 5 |> insert 4 |> insert 0 |> insert 3 |> insert 3 |> insert 2
printfn "%A" newtree
//Node (2,Node (1, Node (0,Empty,Empty),Empty), Node (5,Node (4,Node (3,Empty,Empty),Empty),Empty))
printfn "%A" newtree2
//Node (2,Node (1, Node (0,Empty,Empty),Empty), Node (5,Node (4,Node (3,Empty,Empty),Empty),Empty)) SAME TREE

//Delete a node
//When deleting the root we need to replace the root with the maximum value on the left branch 
let rec findReplaceableRoot (tree : 'a binaryTree) = 
    match tree with
    |Empty -> Empty
    |Node(_,_,Empty) -> tree
    |Node(_,_,right) -> findReplaceableRoot right

let rec delete node = function
|Empty -> Empty

|Node(root,l,r) when node < root ->
    let newleft = delete node l
    Node(root,newleft,r)

|Node(root,l,r) when node > root ->
    let newright = delete node r
    Node(root,l,newright)
|Node(_,Empty,Empty) -> Empty
|Node(_,l,Empty) -> l
|Node(_,Empty,r) -> r
|Node(_,l,r) ->
    let (Node(newroot,_,_)) = findReplaceableRoot l
    let newleft = delete newroot l
    Node(newroot,newleft,r)

printfn "%A" (delete 80 tree)      //Node (3,Node (1,Empty,Empty),Node (5,Empty,Empty))  ->Since 80 is not in the tree we return the initial given tree
printfn "%A" (delete 3 tree)       //Node (1,Empty,Node (5,Empty,Empty))
printfn "%A" (delete 2 newtree)    //Node (1,Node (0,Empty,Empty),Node (5,Node (4,Node (3,Empty,Empty),Empty),Empty))
printfn "%A" (delete 0 newtree2)   //Node (2,Node (1,Empty,Empty),Node (5,Node (4,Node (3,Empty,Empty),Empty),Empty))

//13. Build a parse tree for this language
//  S -> if E then S else S | begin S L | print E
//  L -> end | ; S L
//  E -> i
type SIMPLELANG = IF|THEN|ELSE|BEGIN|END|PRINT|SEMICOLON|ID|EOF|ERROR

let rec Eat t = function
|[] -> failwith "hungry"
|x::xs -> if t = x then xs else [ERROR]

//parse tree structure
type parsetree =
     Lf of SIMPLELANG
    |IF_STATEMENT of parsetree * parsetree * parsetree * parsetree * parsetree * parsetree
    |BEGIN_BLOCK of parsetree * parsetree * parsetree
    |PRINT_STATEMENT of  parsetree * parsetree
    |END_BLOCK of parsetree
    |SEPARATOR of parsetree * parsetree * parsetree

let rec S_parse = function
|[]-> failwith "Early Termination at S"
|IF::xs -> 
    let (E_tree,tail) = xs |> E_parse
    let tail = tail |> Eat THEN
    let (S1_tree,tail) = tail |> S_parse
    let tail = tail |> Eat ELSE
    let (S2_tree,tail) = tail |> S_parse
    if S2_tree = (Lf(ERROR)) then ((Lf(ERROR)),(IF::xs)) else ((IF_STATEMENT(Lf(IF),E_tree,Lf(THEN),S1_tree,Lf(ELSE),S2_tree)), tail)
|BEGIN::xs -> 
    let (S_tree,tail) = xs |> S_parse
    let (L_tree,tail) = tail |> L_parse
    if L_tree = (Lf(ERROR)) then ((Lf(ERROR)),(BEGIN::xs)) else ((BEGIN_BLOCK(Lf(BEGIN),S_tree,L_tree)), tail)
|PRINT::xs ->
    let (E_tree,tail) = xs |> E_parse
    if E_tree = (Lf(ERROR)) then ((Lf(ERROR)),(PRINT::xs)) else ((PRINT_STATEMENT(Lf(PRINT),E_tree)), tail)
|_::xs -> (Lf(ERROR),[ERROR])

and L_parse = function
|[] -> failwith "Early Termination at L"
|END::xs -> (END_BLOCK(Lf(END)),xs)
|SEMICOLON::xs -> 
    let (S_tree,tail) = xs |> S_parse
    let (L_tree,tail) = tail |> L_parse
    if L_tree = (Lf(ERROR)) then ((Lf(ERROR)),(SEMICOLON::xs)) else ((SEPARATOR(Lf(SEMICOLON),S_tree,L_tree)), tail)
|_::xs ->  (Lf(ERROR),[ERROR])

and E_parse = function
|[]-> failwith "Early Termination at E"
|ID::xs -> (Lf(ID), xs)
|_::xs -> (Lf(ERROR), [ERROR])

let buildTree prog = 
    let (tree,tokens) = prog |> S_parse
    if tree = Lf(ERROR) || tokens <> [EOF]
    then printfn "Want [EOF], got %A" tokens 
         Lf(ERROR)
    else tree

let test0 = [IF;ID;THEN;PRINT;ID;ELSE;PRINT;ID;EOF]
let test1 = [IF;ID;THEN;BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;END;ELSE;PRINT;ID;EOF]     
let test2 = [IF;ID;THEN;IF;ID;THEN;PRINT;ID;ELSE;PRINT;ID;ELSE;BEGIN;PRINT;ID;END;EOF]
let test3 = [IF;ID;THEN;BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;SEMICOLON;END;ELSE;PRINT;ID;EOF] //Causes error: 
let test4 = [PRINT;ID;SEMICOLON;PRINT;ID;EOF]  //Causes error: 
let test5 = [BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;END;EOF]
let test6 = [PRINT;ID;EOF]
let test7 = [IF;ID;THEN;BEGIN;PRINT;ID;SEMICOLON;PRINT;ID;END;ELSE;BEGIN;PRINT;ID;END;EOF] 

printfn "%A" (buildTree test0)
printfn "%A" (buildTree test1)
printfn "%A" (buildTree test2)
printfn "%A" (buildTree test3)   //-> Want [EOF], got [IF; ID; THEN; BEGIN; PRINT; ID; SEMICOLON; PRINT; ID; SEMICOLON; END; ELSE; PRINT; ID; EOF]
printfn "%A" (buildTree test4)   //-> Want [EOF], got [SEMICOLON; PRINT; ID; EOF] Lf ERROR
printfn "%A" (buildTree test5)
printfn "%A" (buildTree test6)
printfn "%A" (buildTree test7)

//Post Test 2
//Adding all elements of an int list using Tail Recursion
let rec add = function
|[],acc -> acc
|x::xs,acc -> add(xs,x+acc)

let addelements list = add(list,0)
printfn "%A" (addelements [2;3;5;8]) //18

//Input: 'a option * 'a option -> 'a option
let findlargerOption = function
|Some x, Some y when x > y -> Some x     //Equivalent to: |Some x, Some y -> if x > y then Some x else Some y
|Some x, Some y when y >= x -> Some y    //We assume to return any value if they are equal
|None, Some y -> Some y
|Some x, None -> Some x
|None,None -> None

printfn "%A" (findlargerOption(Some 2, Some 5)) //Some 5
printfn "%A" (findlargerOption(Some 5, Some 5)) //Some 5
printfn "%A" (findlargerOption(Some 5, Some 2)) //Some 5
printfn "%A" (findlargerOption(Some 5, None))   //Some 5
printfn "%A" (findlargerOption(None, Some 5))   //Some 5
printfn "%A" (findlargerOption(None, None))     //None

//Find the max node value in a binary search tree. We expect to input a valid BST
let rec findmax = function
|Empty -> None
|Node(max,_,Empty) -> Some max
|Node(_,_,right) -> findmax right

//Create a BST
let treeTest1 = Node(4,Node(2,Empty,Node(3,Empty,Empty)),Node(8,Node(7,Empty,Empty),Node(9,Empty,Empty)))
let treeTest2 = Node(4,Node(2,Empty,Node(3,Empty,Empty)),Empty) 
let treeTest3 = Empty
printfn "%A" (findmax treeTest1) //Some 9
printfn "%A" (findmax treeTest2) //Some 4
printfn "%A" (findmax treeTest3) //None

//Trick or Treat
let rec what3 = function
|[] -> 0
|x::xs -> x + what3 xs
|[x] -> if x%3 = 0 then x/3 else x //This rule will never be matched so it gets ignored and never runs

printfn "%A" (what3 [5;8;3]) //16 :(

//Time complexity
let rec what2 = function
|[],n -> n
|[x],n -> n
|x::y::[],n -> n 
|x::y::yz,n -> what2 ((x::yz),n+1) + what2 ((y::yz),n+1)

printfn "%A" (what2 ([1..3],0)) //2
printfn "%A" (what2 ([1..4],0)) //8
printfn "%A" (what2 ([1..5],0)) //24
printfn "%A" (what2 ([1..6],0)) //64
printfn "%A" (what2 ([1..7],0)) //160
printfn "%A" (what2 ([1..8],0)) //384
printfn "%A" (what2 ([1..9],0)) //896
printfn "%A" (what2 ([1..10],0))//2048
//Seems like the functions calls are increasing exponentially -> O(2^n)
