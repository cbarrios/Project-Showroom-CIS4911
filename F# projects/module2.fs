//TAIL RECURSION
let rec doit = function
| [],ys,acc -> acc
| xs,[],acc -> acc
| x::xs, y::ys, acc -> doit(xs, ys, acc@[x*y]) //This was bad idea because appending to the end of the list is O(n^2)

let product list1 list2 = doit(list1,list2,[]) 

printfn "%A" (product [1;2;3] [4;5;6]) //Output is: [4; 10; 18]

let rec doitfast = function
| [],ys,acc -> acc
| xs,[],acc -> acc
| x::xs, y::ys, acc -> doitfast(xs, ys, [x*y]@acc) //Appending to the end of the list is not very efficient so lets change it to the front...
                                                   //Notice that (x*y)::acc is also equivalent
let innerproduct list1 list2 = doitfast(list1,list2,[]) 

printfn "%A" (innerproduct [1I..100000I] [100001I..200000I]) 
//With this new version I get the results right away, but with the older version the system waits for a very long period of time

//RECORDS
type AreaCode = {Code:int; City:string; State:string}
let MIAMI_AC = {Code = 305; City = "Miami"; State = "FL"}
printfn "%A" MIAMI_AC.Code //Output is: 305

//Compare records (only if they are the same type)
//type FIU_Student1 = {PID:int ; MAJOR:string}
type FIU_Student2 = {PID:int ; MAJOR:string;}
let stu2 = {PID = 6048821; MAJOR = "PSY"}
let stu1 = {PID = 6048821; MAJOR = "CS"}

let boolResult = stu2 > stu1
printfn "%A" boolResult //TRUE


//DISCRIMINATED UNIONS
type TupleConstructor = 
    | IntOnlyTuple of integer1:int * integer2:int
    | IntStrTuple of integer:int * string:string
    | IntFloTuple of integer:int * float:float

let id = IntOnlyTuple(10,45)
// >val id : TupleConstructor = IntOnlyTuple (10,45)

//HETEROGENEOUS LISTS (using discriminated unions)
type AnyNum = 
    | Byte of byte
    | Int of int
    | Long of int64
    | Decimal of decimal
    | Float of float32
    | Double of double
    | BigInt of bigint

let any_num_list = [Int 10; BigInt 21335345I; Float 15.9f; Double 34.5; Int 33; Byte 4uy; Long 9000L; Decimal 0.6331M; Int 88]
//> val any_num_list : AnyNum list = [Int 10; BigInt 21335345; Float 15.8999996f; Double 34.5; Int 33; Byte 4uy; Long 9000L; Decimal 0.6331M; Int 88]

let rec extractints = function
|[] -> []
|Int n::xs -> n::extractints xs
|_::xs -> extractints xs

printfn "%A" (extractints any_num_list) //Output is: [10; 33; 88]

//Last question class quiz 2 : Given a list of floats, do the average. (Must be tail recursive)
let rec average = function
| [],0.0,0.0 -> None
| [],acc,n -> Some (acc/n)
| x::xs,acc,n -> average(xs,x+acc,n+1.0)

let call_average list = average(list,0.0,0.0)
printfn "%A" (call_average [1.0;4.0;10.0]) //Some 5

//Find the nodes of a tree and put them in a list
let rec getnodes (tree:Tree<'a>) = 
    match tree with 
    | Empty -> []
    | Node(x,Empty,Empty) -> [x]
    | Node(x,l,r) -> (x::getnodes l)@(getnodes r)

printfn "%A" (getnodes tree) //[8; 2; 1; -6; -10; 4; 3; 7; 11; 10; 15; 20; 19; 35]

//Terminals and productions like Question 3 from problem set 2
// E -> E + T | E - T | T
// T -> T * F | T / F | F
// F -> i | (E)

//How do we translate that language into F#? 
//We only include the terminals or, in other words, we exclude the non-terminals: E T F
//We also must include EOF or end of file
type SIMPLELANG = ADD | SUB | MUL | DIV | ID | LPARENT | RPARENT | EOF

//CURRENT LIMITATION: LPARENT AND RPARENT are not being analyzed as pairs. 
//For instance, the list [LPARENT;ID;MUL;LPARENT;ID;SUB;ID;RPARENT;ADD;ID;EOF] is missing the last RPARENT before EOF
//So it should output "error" but still displays "accept"
//GOT FIXED BY PREPROCESSING THE LIST OF TOKENS// LOOK BELOW
//However there is a new limitation: ORDER in which LPARENT and RPARENT are declared
//GOT FIXED TOO //LOOK BELOW

let rec E = function 
| [] -> []
| [EOF] -> [EOF]
| ADD::xs -> xs |> F
| SUB::xs -> xs |> F
| _::xs -> failwith "Error at E"

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
| ID::xs ->      if xs.IsEmpty then [ID] |> F else if xs.Head = ADD || xs.Head = SUB then xs |> E 
                 else if xs.Head = MUL || xs.Head = DIV then xs |> T 
                 else if xs.Head = RPARENT || xs.Head = EOF then xs |> F 
                 else [ID] |> F

| LPARENT::xs -> xs |> F
| RPARENT::xs -> if xs.IsEmpty then [ID] |> F else if xs.Head = ADD || xs.Head = SUB then xs |> E 
                 else if xs.Head = MUL || xs.Head = DIV then xs |> T 
                 else if xs.Head = RPARENT || xs.Head = EOF then xs |> F 
                 else [ID] |> F
| _::xs -> failwith "Error at F"

//HOWEVER, this can be achieved by pre-processing the given list of tokes
//Whenever we see a LPARENT we increase the counter to 1 more and whenever we see a RPARENT we decrease that counter 1 less
//So, this function returns 0 if successful. Otherwise it returns -1 or a positive integer (greater than 0) on failure
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

/// A variant of List.map. (Actually built-in as List.collect.)
let rec appmap f = function
| []    -> []
| x::xs -> f x @ appmap f xs

/// Insert a value into a list in all possible ways.
let rec insert x = function
| []    -> [[x]]
| y::ys -> (x::y::ys) :: List.map (fun zs -> y::zs) (insert x ys)

/// Find all permutations of a list of distinct elements.
let rec permute = function
| []    -> [[]]
| x::xs -> appmap (insert x) (permute xs)

printfn "%A" (insert 3 [1;2])
printfn "%A" (permute [1;2;3])

//Instructor Example: Building a parse tree
//Eat function
let EAT t = function
| [] -> failwith "Nothing to eat"
| x::xs -> if x = t then xs else failwith "I do not want this dish..."

//Had to implement this instead of using .Value
let getValue = function
| Some(x) -> x
| None -> failwith "No value from None"

type tokens = ZERO | ONE | EOF | ERROR

type parse_tree = 
    | Lf of tokens
    | Sub of parse_tree * parse_tree * parse_tree
    | ZeroOne of parse_tree * parse_tree

let rec S_mod = function
| [] -> failwith "Early termination"
| x::xs ->
    match x with 
    | ZERO ->
        let (S_tree, remain) = xs |> S_mod
        let remain = remain |> EAT ONE
        if S_tree = None
        then (Some (ZeroOne (Lf(ZERO), Lf(ONE))), remain)
        else (Some (Sub (Lf(ZERO), (getValue S_tree) , Lf(ONE))), remain)
    | ONE -> (None, x::xs)
    | _ -> failwith "Error"      

let buildTree2 = function
| [] -> failwith "Error"
| xs -> let (S_tree, tokens) = xs |> S_mod
        if tokens <> [EOF] || S_tree = None
        then printfn "Want [EOF], got %A" tokens 
             Lf(ERROR)
        else (getValue S_tree)

let listzeroone = [ZERO;ZERO;ZERO;ZERO;ZERO;ZERO;ZERO;ONE;ONE;ONE;ONE;ONE;ONE;ONE;EOF]
printfn "%A" (buildTree2 listzeroone)
