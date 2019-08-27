// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
//2 + 5 * 10 // good (int)
//10I * 20I //good (BigInt)
//4 + 5.6 // wrong : (type conflict int vs float)
//"4" + "5.6" // good (string)
    "Q1"
    0 // return an integer exit code

(*
Q2.A curried function has a type of which form? Select one:
t1 * t2 -> t3
t1 -> t2 * t3
t1 -> (t2 -> t3) // this one is correct
(t1 -> t2) -> t3
*)

(*
Q3.If an F# function has type 'a -> 'b when 'a : comparison, which of the following is not a legal type for it? Select one:
(float -> float) -> bool
string -> (int -> int) //MAYBE??
int -> int //I now think this one is correct since int -> int means 'a -> 'a because we are not changing the type for the return value
int list -> bool list
*)

(*
Q4.Which of the following statements about F# lists is not true? Select one:
They are immutable.
Their built-in functions are polymorphic.
They can be of any length.
They can be heterogeneous. // this one is false (elements in list must be of same type)
*)

(*
Q5.Which of the following F# expressions evaluates to [1; 2; 3]? Select one:
1::2::3::[] //this one is correct
1@2@3@[]
[1; 2; 3]::[]
((1::2)::3)::[]

> 1::2::3::[];;
val it : int list = [1; 2; 3]
*)

(*
6.	How does F# interpret the expression List.map List.head foo @ baz? Select one:
a.	(List.map List.head) (foo @ baz)
b.	((List.map List.head) foo) @ baz //CORRECT
c.	List.map (List.head (foo @ baz))
d.	(List.map (List.head foo)) @ baz

7.	How does F# interpret the type int * bool -> string list? Select one:
a.	(int * (bool -> string)) list
b.	((int * bool) -> string) list
c.	int * (bool -> (string list))
d.	(int * bool) -> (string list) //CORRECT

8.	Let F# function foo be defined as follows:
let rec foo = function
	    | (xs, [])    -> xs
	    | (xs, y::ys) -> foo (xs@[y], ys)
If foo is supposed to append its two list parameters, which of the following is true? Select one:
a.	foo fails Step 1 of the Checklist for Programming with Recursion.
b.	foo fails Step 2 of the Checklist for Programming with Recursion.
c.	foo fails Step 3 of the Checklist for Programming with Recursion.
d.	foo satisfies all three steps of the Checklist for Programming with Recursion. //CORRECT

9.	Which of the following is the type that F# infers for (fun f -> f 17)? Select one:
a.	('a -> 'b) -> 'b
b.	(int -> int) -> int
c.	(int -> 'a) -> 'a //CORRECT
d.	('a -> 'a) -> 'a

10.	Which of the following has type int -> int list? Select one:
a.	(@) [5]
b.	[fun x -> x+1]
c.	fun x -> 5::x
d.	fun x -> x::[5] //CORRECT

11.	What type does F# infer for the expression (3, [], true) ? Select one:
a.	int * 'a list * bool //CORRECT
b.	int * 'a * bool
c.	int * int list * bool
d.	Type error.

12.	What type does F# infer for the expression fun x y -> x+y+"." ? Select one:
a.	string -> string -> string //CORRECT
b.	string * string -> string
c.	Type error.
d.	int -> int -> string

13.	What type does F# infer for the expression fun xs -> List.map (+) xs ? Select one:
a.	int list -> int -> int list
b.	int list -> int list
c.	Type error.
d.	int list -> (int -> int) list //CORRECT

14.	Which of the following does F# infer to have type string -> string -> string ? Select one:
a.	fun x -> fun y -> x y "."   (MAYBE)(‘+’ might be missing) maybe not?.. ultimately WRONG (confirmed by instructor)
b.	fun x y -> String.length x * String.length y //wrong
c.	fun (x, y) -> x + y + "." //wrong
d.	(+) //wrong

15.	Which of the following does F# infer to have type (string -> string) -> string ? Select one:
a.	fun f -> String.length (f "cat")
b.	fun x y -> x + " " + y
c.	fun f -> f (f "cat") //CORRECT
d.	fun f -> f "cat”
*)

//Q.16
let rec gcd = function
 | (a,0) -> a
 | (a,b) -> gcd (b, a % b)

printfn "%A" (gcd (10,25)) //Output is 5 here

let (.+) (a,b) (c,d) =
 if gcd ((d*a + b*c),b*d) > 1 then ((d*a + b*c)/(gcd ((d*a + b*c),b*d)),(b*d)/(gcd ((d*a + b*c),b*d)))
 else ((d*a + b*c),b*d)

let addedfractions = (1,5) .+ (1,5) // fractions 1/5 + 1/5 = 10/25 which gets simplified to 2/5 ->> gcd  gets called with (10,25) which returns 5, which is used to divide by each element in that tuple resulting in (2,5)
printfn "%d %d" <|| addedfractions //Output is 2 5 which means fraction 2/5

//Multiply fractions
let (.*) (a,b) (c,d) =
 if gcd ((a*c),(b*d))  > 1 then ((a*c)/(gcd((a*c),(b*d))),(b*d)/(gcd((a*c),(b*d))))
 else ((a*c),(b*d)) 

let multipliedfractions = (2,5) .* (3,2)
printfn "%d %d" <|| multipliedfractions //Output is 3/5

let both = (2,5) .+ (2,5) .* (3,2) //Here multiplication is done first, then addition (2/5 + (2/5*3/2) = 2/5 + 3/5 = 5/5 = 1)
printfn "%d %d" <|| both // Must give 1 1 which means it is fraction 1/1 = 1

//Q.17: Function to reverse all lists inside a list (xs: list list)
let revlists xs = List.map (fun x -> List.rev x) xs;;
let list = [[1;2;3];[];[3;2;1]]
let reversedlist = revlists list
printfn "%A" reversedlist //Output is [[3;2;1];[];[1;2;3]]

//Q.18: Function to interleave two lists xs and ys in a tuple argument
let rec interleave (xs,ys) =
 match xs, ys with 
 | [], ys -> ys
 | xs, [] -> xs
 | x::xs, y::ys -> x::y::interleave (xs,ys)

let l1 = [1;3;5;7;9] //odd
let l2 = [2;4;6;8;10] //even
let interleavelists = interleave (l1,l2)
printfn "%A" interleavelists //Output must be sequence from 1-10

//Q.19: Function to cut a list where the first list is of size n
let gencut (n, list) = 
 let rec aux = function
  | 0, xs, ys -> (List.rev xs,ys) //Here we must reverse xs since in the 3rd pattern matching, second argument, we are concatenating the head of ys with xs(tail)
  | n, xs, [] -> (xs, [])
  | n, xs, ys -> aux(n-1, List.head ys :: xs, List.tail ys)
 aux (n, [], list);; //this line runs every time first for the pattern matching to work

 printfn "%A" (gencut(2,[1;2;3;4]));; //Output is ([1;2],[3;4])

//Function 'cut' uses function 'gencut' to do the job
let cut list = 
 let n = (List.length list)/2 //Here we use library function 'List.lenght list' to get the lenght of the list given as parameter and divide it in half
 gencut(n,list);; //call gencut with tuple as its parameter

 printfn "%A" (cut [1;2;3;4;5;6]);; //Output is ([1;2;3],[4;5;6])

//Q.20: Now let's shuffle a list (using functions 'cut' and 'interleave' above)
let shuffle xs = interleave(cut(xs))
printfn "%A" (shuffle [1;2;3;4;5;6;7;8]) //Output is [1; 5; 2; 6; 3; 7; 4; 8]

//Q.21: Helper function for 'countshuffles' that returns number (n) of shuffles needed to revert back to original deck
let countaux (deck, target) = 
 let rec aux = function
  | (n, deck, target) when deck = target -> n //if lists are equal then return n
  | (n, deck, target) when deck <> target -> aux( n+1, shuffle deck, target) // if not equal then reshuffle and increment n 
 aux (1, deck, target);; //n set to 1 since we want to pass an already shuffled deck 

//Counts how many times a deck of size 'n'(having 'n' cards) needs to be shuffled to get back to original deck.
let countshuffles n = 
 let deck = [1..n]
 let target = deck
 countaux(shuffle deck,target);;

printfn "%A" (countshuffles 52) //Output is 8. Therefore 8 calls to shuffle are needed to set back the original deck of 52 cards

//Q.22: Cartesian product of two lists inside a tuple
let rec cartesian = function
 | ([],_) -> [] //this works if first input list is empty or as final concatenation of the non-base case below(3rd one)
 | (_,[]) -> [] //this works if second input list is empty
 | (x::xs, ys) -> (List.map(fun y -> x,y) ys) @ (cartesian (xs,ys)) //we only reduce first input list tail (xs) and concatenate until xs becomes []

printfn "%A" (cartesian([1;2],["a";"b";"c"])) //Output is [(1, "a"); (1, "b"); (1, "c"); (2, "a"); (2, "b"); (2, "c")]

//Q.23: Powerset function taking a 'list' as input parameter
let rec powerset = function
 | [] -> [[]]
 | (x::xs) -> 
    (
        let xss = powerset (xs) 
        List.map (fun xs' -> x::xs') xss @ xss
    )

printfn "%A" (powerset [1;2;3]) //Output is [[1; 2; 3]; [1; 2]; [1; 3]; [1]; [2; 3]; [2]; [3]; []]

//Q.24: Function to compute the transpose of an m-by-n matrix: (input parameter must be 'list list')
let rec transpose M = 
 match M with 
  | []::_ -> []
  | _ -> List.map List.head M::transpose(List.map List.tail M)

printfn "%A" (transpose [[1;2;3];[4;5;6]]) //Output is [[1; 4]; [2; 5]; [3; 6]]

//Q.25: Sorting algorithm (increasing order)
(*
let rec sort = function
 | [] -> []
 | [x] -> [x]
 | x1::x2::xs -> 
  if x1 <= x2 then x1 :: sort (x2::xs)
  else x2 :: sort (x1::xs)

printfn "%A" (sort [1;2;5;4;3]) //Problem appears to be that more recursion is needed: more sorting cycles. It currently runs as only one bubble sort cycle
*)
//GOT IT...
let rec sortaux = function
| []         -> []
| [x]        -> [x]
| x1::x2::xs -> if x1 <= x2 then (x1 :: sortaux (x2::xs))
                else  (x2 :: sortaux (x1::xs))

let rec sort = function
 | []         -> []
 | [x]        -> [x]
 | x1::x2::xs -> if x1 <= x2 then sortaux  (x1 :: sort (x2::xs))
                 else sortaux  (x2 :: sort (x1::xs))
printfn "%A" (sort [0;2;-4;-3;10;1;99;100;3;4;6;5]) //Output is: [-4; -3; 0; 1; 2; 3; 4; 5; 6; 10; 99; 100]

//Q.26: Merge Sort
let rec merge = function //good
 | ([], ys)       -> ys
 | (xs, [])       -> xs
 | (x::xs, y::ys) -> 
 if x < y then x :: merge (xs, y::ys)
 else y :: merge (x::xs, ys)

let rec split = function //good
 | []       -> ([], [])
 | [a]      -> ([a], [])
 | a::b::cs -> let (M,N) = split cs
               (a::M, b::N)     
	       
let rec mergesort = function //missing a base case
 | []  -> []
 | [x] -> [x] //this base case had to be added for the program to work
 | L   -> let (M, N) = split L
          merge ( mergesort M, mergesort N) //Type for 'mergesort' was: 'a list -> 'b list when 'b : comparison but it must be 'a list -> 'a list

printfn "%A" (mergesort [3;6;2;1;9;5;7;10;4;8]) //Output is [1; 2; 3; 4; 5; 6; 7; 8; 9; 10]

//Q.27: Modify it to support exponential operation (right associative)
//ORIGINAL
//E -> E+T | E-T | T
//	T -> T*F | T/F | F
//	F -> i | (E)

//MODIFIED
(*
E -> E+T | E-T | T
      T -> T*F | T/F | F
      F -> G^F | G       //This one was added. Notice that G^F is right associative. (F^G would be left associative)
      G -> i | (E)
*)

//Q.28: Show that this grammar is ambiguous
//S -> if E then S | if E then S else S | begin S L | print E
//	    L -> end | ; S L
//	    E -> i

(*
The problem with this grammar is its first line with the if-then if-then-else combinations
String to analyse: 'if E then if E then S else S' is ambiguous because we can have two ways of association. 
In other words, we don't know whether the 'else' matches the first or the second 'if' in our string.

Proof:
First tree is : if E then (if E then S) else S
Second tree is: if E then (if E then S else S)
*)
