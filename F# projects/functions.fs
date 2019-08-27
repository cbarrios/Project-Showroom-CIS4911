//FUNCTIONS
//Let's have 3 functions
let negate x = x * -1 
let square x = x * x 
let print x = printfn "The number is: %d" x

//Function that calls other functions. Calls are made Inner-Out
let squareNegateThenPrint x = 
    print (negate (square x)) 
//Function Call
squareNegateThenPrint 4 

//Alternative 'squareNegateThenPrint' done iteratively
let expr1 = square 4
let expr2 = negate expr1
print expr2 //display

//RECURSIVE FUNCTIONS (the keyword 'rec' must be used)
let rec fact x =
 if x < 1 then 1
 else x * fact (x - 1)

printfn "%A" (fact 4) //Output must be 24

//Mutually recursive functions (those functions which call each other) are indicated by 'and' keyword:
let rec even x =
 if x = 0 then true
 else odd (x - 1)

and odd x =
 if x = 1 then true
 else even (x - 1)

printfn "%A" (even 4)  //Output must be true

//PATTERN MATCHING
//'match' followed by 'with'
let rec fib n =
 match n with
 |0 -> 0
 |1 -> 1
 |_ -> fib (n - 1) + fib (n - 2) // '_' this means anything diff from 0 or 1 (we could use 'n' too)

printfn "%A" (fib 10) //Output must be 55

//Alternate version using 'function' (replaces 'match' n 'with' )
let rec fib2 = function
 |0 -> 0
 |1 -> 1
 |n -> fib2 (n - 1) + fib2 (n - 2) //n is used here in pattern matching only

printfn "%A" (fib2 10) //Output must be 55

//In order to match sophisticated inputs, one can use 'when' to
//create filters or guards on patterns:
let sign x =
 match x with
 | 0 -> 0
 | x when x < 0 -> -1
 | x -> 1

printfn "%A" (sign -5) // this activates the comparison "x when x < 0 -> -1" so the output is -1

//Pattern matching can be done directly on arguments:
let lol (x, _) = x

printfn "%A" (lol(3,45454)) //Output must be 3 regardless of the second element in tuple

let foo = [[1;2;3];[4;5]]
let baz = [6;7;8;9]
printfn "%A" (List.map List.head foo @ baz) //Output must be [1;4;6;7;8;9]
printfn "%A" (((List.map List.head) foo) @ baz) //Output must be the same (this is how F# interprets the line above)

let hw (x,y) = ["hello";"world"] //Type ('a*'b) -> (string list)

let rec append = function // this passes 1,2,3 points for the recursion checklist
 | (xs, [])    -> xs
 | (xs, y::ys) -> append (xs@[y], ys) //'@' concatenates

printfn "%A" (append ([1;2;3],[4;5]))//Output must be [1;2;3;4;5] since it appends the second list elements to the first list and returns it

fun f -> f 17 //Type: (int -> 'a) -> 'a

fun x -> x::[5] //Type: int -> int list

let list1 = [1;2;3]
let list2 = [1;90;100]
let comparison = list1 > list2 //Output must be false since (it goes comparing one by one and) 2 is not greater than 90... If we make 90 less than 2, then it's true or if we make it equal to 2 and change 100 to less than 3
printfn "%A" comparison

fun xs -> List.map (+) xs //Type: int list -> (int -> int) list

fun x -> fun y -> x y "."  //Is this same as string->string->string or is it missing the infix '+' in between x y "." ??? Given type by F# is: val it : x:('a -> string -> 'b) -> y:'a -> 'b 

fun f -> f (f "cat") //Type: (string -> string) -> string

//Example of List.map 
let ys = [1;2;3]
let ex = ["a"]
let outputlist = (List.map(fun y -> (List.head ex,y))) ys
printfn "%A" (outputlist) //Output is [("a", 1); ("a", 2); ("a", 3)]

//These functions were in Quiz 1
//This one takes the head of every list inside a list and maps it to one list
let quiz1function1 xs = List.map List.head xs
printfn "%A" (quiz1function1 [[3;2;1];[4;5;6];[1;2;3]]) //Output: [3;4;1]

//This one does multiplication of first two elements of two lists then continues doing the same with the tail
let rec quiz1function2 = function
 | xs,[] -> xs
 | [],ys -> ys
 |x::xs, y::ys -> x*y::quiz1function2(xs,ys)
printfn "%A" (quiz1function2 ([2;4],[5;8])) //Output: [10; 32]

//Discover the error...
let quiz1q1 = function
 | [] -> []
 | [x] -> [x]
 | x::xs -> x@xs  //'x' is 'a but expression expects list
 //Why type error(mismatch)?? Simply because 'x' is not a list or can't be a list from compiler's perspective.. 
 //But why?? because both left and right sides of the pattern matchings must be a list. Now, by definition, the last part of '::' is a list so 'xs' must be a list but 'x' can't in this case
 //since left side of the pattern matchings must be a list, because if x is a list then pattern matching would fail because it would expect a list but it would be given a list list, so therefore
 //it assumes that x cannot be a list and then the expression x@xs fails because @ expects two lists and the compiler gives it 'a and a list which can't be unified or concatenated
