//1. Building a simple tree.
//a. Create a discriminated union that can represent a linked list of integers.
//b. Write a function that converts a list into a linked list of nodes.

//(a)
type linkedlist = 
  | Terminator
  | Node of value:int * link: linkedlist

//(b)
let rec fromListToLinkedList = function
|[] -> Terminator
|x::xs -> Node(x, xs |> fromListToLinkedList)

let listofints = [1..5]
printfn "%A" (fromListToLinkedList listofints) //Node (1,Node (2,Node (3,Node (4,Node (5,Terminator)))))

(*
  2. This CFG recognizes some strings of zeros and ones.
     S → 0A | 1B
     A → 0AA | 1S | 1
     B → 1BB | 0S | 0 
  a. Describe the strings that the CFG recognizes.
  b. This language is ambiguous. Find a string that is recognized by this grammar which has two derivations.
  c. Show the two derivation trees for the string in part (b).


(a)  S → 0A | 1B        //S produces a 0 followed by an A production OR a 1 followed by a B production
     A → 0AA | 1S | 1   //A produces a 0 followed by an A followed by an A OR a 1 followed by a S OR simply a 1
     B → 1BB | 0S | 0   //B produces a 1 followed by a B followed by a B OR a 0 followed by a S OR simply a 0
                        //In other words, the strings that this CFG recognizes must be 0s followed by 1s or viceversa with the condition that the number of 0s must be equal the number of 1s
(b) S: 001101


(c) 
S                     S
0A                    0A
0 0AA                 0 0AA
00 1 1S               00 1S 1
0011 0A               001 1B 1
00110 1               0011 0 1
*)

(*
3. Symmetrical Palindromes separated by a vetical bar '|'
(a)Write a CFG to recognize palindromes over the alphabet {a, b, |}, with the bar in the middle.
   S → aSa | bSb | |
*)
//(b)Write a parse function that accepts a string and generates tokens for the language.
type tokens = A | B | BAR | EOF | ERROR

let rec parse = function
| "" -> [EOF]
| s -> 
    match s.Chars 0 with
    | 'a' -> A::parse(s.Substring 1)
    | 'b' -> B::parse(s.Substring 1)
    | '|' -> BAR::parse(s.Substring 1)
    | c -> failwith (sprintf "PARSE: cannot match char %A" c)

let string = "ab|ba"
let tokensList = string |> parse
printfn "%A" tokensList //[A; B; BAR; B; A; EOF]

//(c) Write a syntax checker that verifies if a list of tokens represents a palindrome.
//Eat function
let eat t = function
| [] -> [ERROR]
| x::xs -> if x = t then xs else [ERROR]

let rec S = function
| [] -> failwith "Early Termination"
| x::xs -> 
    match x with
    | A -> xs |> S |> eat A
    | B -> xs |> S |> eat B
    | BAR -> xs
    | EOF -> x::xs
    | _ -> failwith (sprintf "S: Syntax Error while processing token %A" x)

//function to test input
let test_program program =
 let result = program |> S
 match result with 
 | [] -> "Early termination or missing EOF"
 | x::xs -> if x = EOF then "accept" else "error"

let aa = "a|a" |> parse
let bb = "b|b" |> parse
let baab = "ba|ab" |> parse
let aaabbaaa = "aaab|baaa" |> parse
let abab = "ab|ab" |> parse
printfn "%A" (test_program [A;BAR;A;EOF]) //accept
printfn "%A" (test_program tokensList)    //accept
printfn "%A" (test_program aa)            //accept
printfn "%A" (test_program bb)            //accept
printfn "%A" (test_program baab)          //accept
printfn "%A" (test_program aaabbaaa)      //accept
printfn "%A" (test_program abab)          //error

//(d) Extend the syntax checker so it generates an abstract syntax tree and displays it, for valid palindromes.
type tree = 
    | Lf of tokens
    | Br_aSa of tree * tree * tree
    | Br_bSb of tree * tree * tree
    | Bar of tree

let rec S_ext = function
| [] -> failwith "Early Termination"
| x::xs -> 
    match x with
    | A -> 
        let (S_tree, tail) = xs |> S_ext
        let tail = tail |> eat x
        if S_tree = (Lf(ERROR)) 
        then (Lf(ERROR),x::xs) 
        else (Br_aSa(Lf(x),S_tree,Lf(x)),tail)
    | B -> 
        let (S_tree, tail) = xs |> S_ext
        let tail = tail |> eat x
        if S_tree = (Lf(ERROR)) 
        then (Lf(ERROR),x::xs) 
        else (Br_bSb(Lf(x),S_tree,Lf(x)),tail)
    | BAR -> (Bar(Lf(x)),xs)
    | _ -> (Lf(ERROR), x::xs)

let buildtree tokensList = 
    let (tree,tokens) = tokensList |> S_ext
    if tree = Lf(ERROR) || tokens <> [EOF]
    then 
        printfn "Want [EOF], got %A" tokens
        Lf(ERROR)
    else 
        tree

let abaaba = "aba|aba" |> parse 
printfn "%A" (buildtree aa)       //valid
printfn "%A" (buildtree bb)       //valid
printfn "%A" (buildtree baab)     //valid
printfn "%A" (buildtree aaabbaaa) //valid
printfn "%A" (buildtree abaaba)   //valid
printfn "%A" (buildtree abab)     //ERROR

(*
4. Natural Semantics: Writing derivations for judgements
(a) ({i=5; j=8}, i := 2*j + i) => {i=21; j=8}
(a.1) i is in dom(M)  (M,i) => 5
(a.2) (M,2) => 2
(a.3) j is in dom(M)  (M,j) => 8
(a.4) ({i=5; j=8}, 2*j) => 16
(a.5) ({i=5; j=8}, 2*j + i) => 21
(a.6) ({i=5; j=8}, i := 2*j + i) => {i=21; j=8}

(b) ({i=3; j=8}, if (2*i > j) then i := 2*j else j := 2*i) => {i=3; j=6}
(b.1) (M,2) => 2
(b.2) i is in dom(M)  (M,i) => 3
(b.3) ({i=3; j=8}, 2*i) => 6
(b.4) j is in dom(M)  (M,j) => 8
(b.5) ({i=3; j=8}, 2*i > j) => 0
(b.6) ({i=3; j=8}, j := 2*i) => {i=3; j=6}
(b.7) ({i=3; j=8}, if (2*i > j) then i := 2*j else j := 2*i) => {i=3; j=6}

(c) ({i=1; j=10}, while (3*i <= j) do i := 3*i) => {i=9; j=10}
(c.1) (M,3) => 3
(c.2) i is in dom(M)  (M,i) => 1
(c.3) ({i=1; j=10}, 3*i) => 3
(c.4) j is in dom(M)  (M,j) => 10
(c.5) ({i=1; j=10}, 3*i <= j) => 1
(c.6) ({i=1; j=10}, i := 3*i) => {i=3; j=10}
(c.7) ({i=3; j=10}, while (3*i <= j) do i := 3*i) => {i=9; j=10}
(c.8) (M,3) => 3
(c.9) i is in dom(M)  (M,i) => 3
(c.10) ({i=3; j=10}, 3*i) => 9
(c.11) j is in dom(M)  (M,j) => 10
(c.12) ({i=3; j=10}, 3*i <= j) => 1
(c.13) ({i=3; j=10}, i := 3*i) => {i=9; j=10}
(c.14) ({i=9; j=10}, while (3*i <= j) do i := 3*i) => {i=9; j=10}
(c.15) (M,3) => 3
(c.16) i is in dom(M)  (M,i) => 9
(c.17) ({i=9; j=10}, 3*i) => 27
(c.18) j is in dom(M)  (M,j) => 10
(c.19) ({i=9; j=10}, 3*i <= j) => 0
(c.20) ({i=9; j=10}, while (3*i <= j) do i := 3*i) => {i=9; j=10}
(c.21) ({i=3; j=10}, while (3*i <= j) do i := 3*i) => {i=9; j=10}
(c.22) ({i=1; j=10}, while (3*i <= j) do i := 3*i) => {i=9; j=10}
*)

//5. Write a tail-recursive F# function interleave(xs,ys) that interleaves two lists
let interleave(list1,list2) =
    let rec interleave_helper = function
    |[],[],acc -> List.rev acc
    |[], ys,acc -> ys
    |xs, [],acc -> xs
    |x::xs,y::ys,acc -> interleave_helper(xs,ys,(y::x::acc))
    interleave_helper(list1,list2,[])

printfn "%A" (interleave([1;3;5;7;9],[2;4;6;8;10])) //[1; 2; 3; 4; 5; 6; 7; 8; 9; 10]

//6. Alternating series
//a. Generate an infinite sequence for the alternating series of 1/(2**n): 1/2, -1/4, 1/8, -1/16, 1/32, -1/64, ...
//b. Display the 5th through 15th numbers in the series. The numbers should display as the floating point version of the fractions.
//c. Repeat the exercise using an infinite stream

//(a)
let seqInfinite = Seq.initInfinite (fun index ->
    let n = float( index + 1 )
    1.0 / ((if ((index + 1) % 2 <> 0) then (2.0**n) else (-1.0)*(2.0**n))))
printfn "%A" seqInfinite //seq [0.5; -0.25; 0.125; -0.0625; ...]

//(b)
let calculate_range(l,u,seq) =
    let rec aux = function
    | (lower, upper, range) when lower = upper -> List.rev range 
    | (lower, upper, range) when lower <> upper -> aux(lower+1, upper, (Seq.item (lower-1) seq)::range)
    aux(l,u+1,[])
printfn "%A" (calculate_range(5,15,seqInfinite)) //Inclusive: from number 5 through number 15
//[0.03125; -0.015625; 0.0078125; -0.00390625; 0.001953125; -0.0009765625; 0.00048828125; -0.000244140625; 0.0001220703125; -6.103515625e-05; 3.051757813e-05]

//Alternative way: Seq.skip  Seq.take  Seq.toList
let skip4 = (Seq.skip 4) seqInfinite 
let take11 = (Seq.take 11) skip4
let tolist = Seq.toList take11
printfn "%A" tolist
//[0.03125; -0.015625; 0.0078125; -0.00390625; 0.001953125; -0.0009765625; 0.00048828125; -0.000244140625; 0.0001220703125; -6.103515625e-05; 3.051757813e-05]

//(c)
let rec upfrom2 n = 
    let id = int (n)
    Cons(1.0 / ((if (id % 2 <> 0) then (2.0**n) else (-1.0)*(2.0**n))), fun() -> upfrom2(n+1.0))

let alt_stream = upfrom2 1.0 
printfn "%A" (take 5 alt_stream) //[0.5; -0.25; 0.125; -0.0625; 0.03125]

//Let's display values 5th to 15th from alt_stream
let auxiliar = drop 4 alt_stream
let display5to15 = take 11 auxiliar
printfn "%A" display5to15
//[0.03125; -0.015625; 0.0078125; -0.00390625; 0.001953125; -0.0009765625; 0.00048828125; -0.000244140625; 0.0001220703125; -6.103515625e-05; 3.051757813e-05]

(*
7. Multiples of a list
(a) Generate an infinite stream for the the natural numbers greater than zero that are divisible by each element in a list of four elements. 
Use four, nested calls of filter on the infinite stream of natural numbers starting at one. 
For example the output for the list [2;3;21;10]: 210, 420, 630, 840, 1050, ...
*) 
type 'a stream = Cons of 'a * (unit -> 'a stream)
let rec upfrom n = Cons(n, fun() -> upfrom(n+1))
let naturals = upfrom 1 //naturals greater than zero

let rec filter p (Cons(x,xsf)) = 
    if p x then Cons(x, fun() -> filter p (xsf()))
    else filter p (xsf())

let generate (list:int list) =
    if list.Length <> 4 
    then failwith (sprintf "List Length must be 4, got %A" list.Length)
    else 
        let stream1 = (filter (fun n -> n%list.[0]=0) naturals) 
        let stream2 = (filter (fun n -> n%list.[1]=0) stream1) 
        let stream3 = (filter (fun n -> n%list.[2]=0) stream2) 
        let stream4 = (filter (fun n -> n%list.[3]=0) stream3) 
        stream4
printfn "%A" (generate [2;3;21;10]) //Cons (210,<fun:filter@647>) :))
printfn "%A" (generate [2;3;21])    //List Length must be 4, got 3

//(b) Display the 20th through 30th numbers in the series (inclusive)
let rec take n (Cons(x,xsf)) = 
    if n = 0 then []
    else x::take (n-1) (xsf())

let rec drop n (Cons(x,xsf)) = 
    if n = 0 then Cons(x,xsf)
    else drop (n-1) (xsf())

let series = generate [2;3;21;10]
let aux = drop 19 series
let display = take 11 aux
printfn "%A" display //[4200; 4410; 4620; 4830; 5040; 5250; 5460; 5670; 5880; 6090; 6300]

//Take n from stream: Imperative Implementation
let take2 n (Cons(x,xsf)) =
    let list = ref []
    let stream = ref (Cons(x,xsf))
    for i = 1 to n do
        let (Cons(head,xsf)) = !stream
        list := head::(!list)
        let aux = xsf()
        stream := aux
    List.rev (!list)
printfn "%A" (take2 5 alt_stream) //[0.5; -0.25; 0.125; -0.0625; 0.03125]

//(c)
// Repeat the exercise using an infinite sequence. 
// Sequences also have a filter function, so it can be solved similarly to the infinite stream version. 
// Just for fun, try to solve it without using the filter function.
//(d) Be sure to dislay an appropriate error message if the list does not have exactly four elements.

let seq = Seq.initInfinite (fun n -> n+1) 
printfn "%A" seq //seq [1; 2; 3; 4; ...]

let generate_seq (list:int list) = 
    if list.Length <> 4 
    then failwith (sprintf "List Length must be 4, got %A" list.Length)
    else 
        let seq1 = Seq.filter((fun n -> n%list.[0]=0)) seq
        let seq2 = Seq.filter((fun n -> n%list.[1]=0)) seq1
        let seq3 = Seq.filter((fun n -> n%list.[2]=0)) seq2
        Seq.filter((fun n -> n%list.[3]=0)) seq3
        
        //Using Seq.where is also an option
        //let seq1 = Seq.where ((fun n -> n%list.[0]=0)) seq
        //let seq2 = Seq.where ((fun n -> n%list.[1]=0)) seq1
        //let seq3 = Seq.where ((fun n -> n%list.[2]=0)) seq2
        //let seq4 = Seq.where ((fun n -> n%list.[3]=0)) seq3
        //seq4

printfn "%A" (generate_seq [2;3;21;10]) //seq [210; 420; 630; 840; ...]

//Display values 20th to 30th from seq
let sequence = generate_seq [2;3;21;10]
let aux2 = (Seq.skip 19) sequence
let aux3 = (Seq.take 11) aux2 
printfn "%A" aux3 //seq [4200; 4410; 4620; 4830; ...]
let listdisplay = (Seq.toList) aux3
printfn "%A" listdisplay //[4200; 4410; 4620; 4830; 5040; 5250; 5460; 5670; 5880; 6090; 6300]

//Let's compare the previous result with a function I wrote for Question 6
let valuesfrom20to30 = calculate_range(20,30,sequence)
printfn "%A" valuesfrom20to30 //[4200; 4410; 4620; 4830; 5040; 5250; 5460; 5670; 5880; 6090; 6300]

//8. Create a tail-recursive function that has a big integer as input and calculates 2I raised to that power.
//   Calculate these powers of 2I: 0I, 1I, 2I, 4I, 16I, 256I, 1024I, 32768I and 65536I.
let power n =
    let rec power_helper = function
    |n,acc when n <= 0I -> acc
    |n,acc -> power_helper(n-1I,2I*acc)
    power_helper(n,1I)

printfn "%A" (power 0I)     //1
printfn "%A" (power 1I)     //2
printfn "%A" (power 2I)     //4
printfn "%A" (power 4I)     //16
printfn "%A" (power 16I)    //65536
printfn "%A" (power 256I)   //BigInt...
printfn "%A" (power 1024I)  //BigInt...
printfn "%A" (power 32768I) //BigInt...
printfn "%A" (power 65536I) //BigInt...

//9. Functions twice and successor - Parenthesis affect output
let twice f = (fun x -> f (f x));
let successor n = n+1;;

let example1 = twice twice twice twice successor 
let example2 = (twice (twice (twice (twice successor)))) 

printfn "%A" (example1 0) // (2^(2^(2^2))) + 0 = 65536
printfn "%A" (example2 0) // 2*2*2*2 + 0 = 16
printfn "%A" (example1 5) // (2^(2^(2^2))) + 5 = 65541
printfn "%A" (example2 5) // 2*2*2*2 + 5 = 21

//10. Infer the type
let f = (fun f -> f (f 17.3))

//Steps:
//(1) fun f: 'a -> 'a //Simplest form
//(2) (f 17.3): (float -> 'b) //Input for f must be float
//(3) f (f 17.3)): (float -> 'b) -> 'c 
//Input for this part is (float -> 'b) but since we declared in (2) that "Input for f must be float", also the type for 'b must be float 
//and consequently for 'c .So, we unify float with 'b and then 'b with 'c which gives the final type val f:f:(float->float)->float

//11. Imperative F# : Iterative vs Tail-Recursive Fibonacci

//Iterative
let fiboIter n =
    let fibo = ref 1I
    if n <= 0 then fibo := 0I else fibo := 1I
    let fiboPrev = ref 1I;
    let temp = ref 0I;
    let i = ref 2
    while !i <= n-1 do //alternative: for i = 2 to n-1 do and comment out the ref i
        temp := !fibo
        fibo := !fibo + !fiboPrev
        fiboPrev := !temp
        i := !i + 1
    !fibo;;
(*
#time
for i in 0..1000 do
 printfn "fiboIter of %A => %A" i (fiboIter i)
#time
//fiboIter of 1000 => 43466557686937456435688527675040625802564660517371780402481729089536555417949051890403879840079255169295922593080322634775209689623239873322471161642996440906533187938298969649928516003704476137795166849228875
//Real: 00:00:00.154, CPU: 00:00:00.156, GC gen0: 6, gen1: 6, gen2: 0
*)

//Tail Recursive
let fiboTailRec n =
  let rec fiboHelper (n1,n2) i =
    if i < n
    then fiboHelper (n1+n2,n1) (i+1)
    else n1
  fiboHelper (0I,1I) 0

(*
#time
for i in 0..1000 do
 printfn "fiboTailRec of %A => %A" i (fiboTailRec i)
#time
//fiboTailRec of 1000 => 43466557686937456435688527675040625802564660517371780402481729089536555417949051890403879840079255169295922593080322634775209689623239873322471161642996440906533187938298969649928516003704476137795166849228875
//Real: 00:00:00.153, CPU: 00:00:00.156, GC gen0: 6, gen1: 6, gen2: 0
*)

//12. Functions on records
type Student = {
    getGPA: unit -> float;      //getter
    getCredits: unit -> float;  //getter
    addCredits: float -> unit;  //setter
    addGrades: float -> unit;   //setter
}
let studentCreate() =
    let credits = ref 0.0
    let grades = ref 0.0
    let ocurrences = ref 0.0
    {
        getGPA = fun () -> 
            if !ocurrences > 0.0 then !grades/(!ocurrences) else 0.0;
        getCredits = fun () -> !credits;
        addCredits = fun c -> credits := !credits + c; 
        addGrades = fun g -> 
            grades := !grades + g; 
            ocurrences := !ocurrences + 1.0
            if g >= 2.0 then credits := !credits + 3.0; 
            //Automatic credits count assuming all classes are 3.0 credits and assuming 2.0 grade is sufficient 
    }

let student1 = studentCreate()
let student2 = studentCreate()
printfn "student 1 gpa %A" (student1.getGPA())
printfn "student 1 credits %A" (student1.getCredits())
student1.addGrades 3.33
printfn "student 1 gpa %A" (student1.getGPA())
printfn "student 1 credits %A" (student1.getCredits())
student1.addGrades 4.0
printfn "student 1 gpa %A" (student1.getGPA())
printfn "student 1 credits %A" (student1.getCredits())
student1.addGrades 1.67
printfn "student 1 gpa %A" (student1.getGPA())
printfn "student 1 credits %A" (student1.getCredits())
student1.addCredits 3.0
printfn "student 1 after addCredits %A" (student1.getCredits())

printfn "student 2 gpa %A" (student2.getGPA())
printfn "student 2 credits %A" (student2.getCredits())
student2.addGrades 3.67
printfn "student 2 gpa %A" (student2.getGPA())
printfn "student 2 credits %A" (student2.getCredits())
student2.addGrades 4.0
printfn "student 2 gpa %A" (student2.getGPA())
printfn "student 2 credits %A" (student2.getCredits())
student2.addCredits 3.0
printfn "student 2 after addCredits %A" (student2.getCredits())

//13. Imperative F#: Integer Tuple Stack - Factorial implementation
let stack init =
    let stk = ref init
    ((fun x -> stk := x::(!stk)),           //push
     (fun () -> stk := List.tail (!stk)),   //pop
     (fun () -> List.head (!stk)),          //top
     (fun () -> List.isEmpty (!stk)))       //isEmpty

let factorialStack2 n =
    let fact = ref 1I
    let bigint(x:int) = bigint(x)
    let (_,pop,top,isempty) = stack [1..n]
    while isempty()=false do
        fact := !fact * (top() |> bigint)
        pop ()
        //printf "%A" (isempty())
    !fact;;

printfn "%A" (factorialStack2 -1) //1
printfn "%A" (factorialStack2 0)  //1
printfn "%A" (factorialStack2 1)  //1
printfn "%A" (factorialStack2 2)  //2
printfn "%A" (factorialStack2 3)  //6
printfn "%A" (factorialStack2 4)  //24
printfn "%A" (factorialStack2 5)  //120
printfn "%A" (factorialStack2 10) //3628800

(*
#time
for i in 0..100 do
 printfn "factorialStack2 of %A => %A" i (factorialStack2 i)
#time
//factorialStack2 of 100 => 93326215443944152681699238856266700490715968264381621468592963895217599993229915608941463976156518286253697920827223758251185210916864000000000000000000000000
//Real: 00:00:00.013, CPU: 00:00:00.015, GC gen0: 0, gen1: 0, gen2: 0
*)

//tail recursive factorial
let factorialTailRec n =
    let rec factorial_helper = function
    | n, acc when n = 0I -> acc
    | n, acc when n <> 0I -> factorial_helper(n-1I, acc*n)
    factorial_helper(n,1I)
printfn "%A" (factorialTailRec 5I) //120
(*
#time
for i in 0I..100I do
 printfn "factorialTailRec of %A => %A" i (factorialTailRec i)
#time
//factorialTailRec of 100 => 93326215443944152681699238856266700490715968264381621468592963895217599993229915608941463976156518286253697920827223758251185210916864000000000000000000000000
//Real: 00:00:00.014, CPU: 00:00:00.015, GC gen0: 0, gen1: 0, gen2: 0
*)

//14. Code: Well typed or not?
(*
{ int *x;
  int a[15];

  *x = 7;
  a[*x] = *x + 4;
}
*)

//(1) x : int* var | by LETVAR --> Good!
//(2) a : int* | by LETARR --> Good!
//Therefore expressions int *x; int a[15]; are well typed
//(3) *x = 7: for this to be well typed we need: t var = t (ASSIGN)
//(3.1) E(x) = int* var
//(3.2) x : int* | by R-VAL
//(3.3) *x : int var | by L-VAL --> Good!
//(3.4) 7 : int | by LIT --> Good! 
//Therefore expression *x = 7; is well typed since it follows that int var = int
//(4) a[*x] = *x + 4: for this to be well typed we need:  t var = t (ASSIGN)
//(4.1) E(a) = int*
//(4.2) *x : int | by applying R-VAL on (3.3)
//(4.3) a[*x] : int var | by SUBSCRIPT --> Good!
//(4.4) 4 : int | by LIT --> Good! So *x + 4 : int
//Therefore expression a[*x] = *x + 4; is well typed since it follows that int var = int

//15. Value restriction
let makeMonitoredFun f =
      let c = ref 0
      (fun x -> c := !c+1; printf "Called %d times.\n" !c; f x)

let msqrt = makeMonitoredFun sqrt
let x = msqrt 25.0 //Called 1 times
printfn "%A" x

//let mrev = makeMonitoredFun List.rev //Value restriction because List.rev has type T'list -> T'list where T is obj (generic)
//To fix it we give it some list:
let mrev list = makeMonitoredFun List.rev list
printfn "%A" (mrev [1;2;3;4;5]) //Called 1 times
let x1 = msqrt 25.0 //Called 2 times
let x2 = msqrt 25.0 //Called 3 times
printfn "%A" (mrev [1;2;3;4;5]) //Called 1 times
let x3 = msqrt 25.0 //Called 4 times

//As we can see above, everytime we call 'mrev' val c is recreated from scratch like in functional programming
//because the parameter 'list' has generic type:'a list. So the type of 'mrev' is 'a list -> 'a list
//With 'sqrt' we know that it is float -> float so we have no problems

//Eta expansion still same problem as before: 'mrev2' has type 'a list -> 'a list
let mrev2 = fun x -> (makeMonitoredFun List.rev) x
printfn "%A" (mrev2 [1;2;3;4;5]) //Called 1 times
printfn "%A" (mrev2 [1;2;3;4;5]) //Called 1 times

//16. PCF Interpreter
(*(b) Implement substitution for PCF by writing an F# function 
  subst e x t that takes a term e, a string x representing an identifier, and a term t, 
  and returns e with all free occurrences of ID x replaced by t. *)
let rec subst e x t = 
    match e with
    | ID s when s = x -> t 
    | ID s when s <> x -> e
    | IF(e1,e2,e3) -> IF(subst e1 x t, subst e2 x t, subst e3 x t)
    | APP(e1,e2) -> APP(subst e1 x t, subst e2 x t)
    | FUN (str,term) -> if str = x then e else FUN(str,subst term x t)
    | REC (str,term) -> if str = x then e else REC(str,subst term x t)
    | t -> t

printfn "%A" (subst (NUM 6) "a" (NUM 3))                //NUM 6
printfn "%A" (subst (BOOL true) "a" (NUM 3))            //BOOL true
printfn "%A" (subst SUCC "a" (NUM 3))                   //SUCC
printfn "%A" (subst (APP(SUCC, ID "a")) "a" (NUM 3))    //APP (SUCC,NUM 3)
printfn "%A" (subst (IF (BOOL true, FUN ("a", APP (SUCC, ID "a")), FUN ("b", APP (SUCC, ID "a")))) "a" (NUM 3))
//IF (BOOL true,FUN ("a",APP (SUCC,ID "a")),FUN ("b",APP (SUCC,NUM 3)))

//(a)
let rec interp = function
| SUCC -> SUCC 
| PRED -> PRED
| ISZERO -> ISZERO
| NUM n -> NUM n
| ID s -> if s = "x" then ERROR("ID: 'id' needs bound identifier, not x") else ID s //(c) test for unbound ID "x"
| BOOL b -> BOOL b
| ERROR e -> ERROR e 
| IF (e1,e2,e3) -> 
    match (interp e1, e2 ,e3) with
    | (BOOL true,e2,_) -> interp e2
    | (BOOL false,_,e3) -> interp e3
    | (v,_,_) -> ERROR (sprintf "IF: 'if' needs boolean argument, not '%A'" v)
| APP (e1, e2) ->
    match (interp e1, interp e2) with
    | (ERROR s, _)  -> ERROR s        // ERRORs are propagated
    | (_, ERROR s)  -> ERROR s
    | (SUCC, NUM n) -> NUM (n+1)      // Rule (6)
    | (SUCC, v)     -> ERROR (sprintf "APP: 'succ' needs int argument, not '%A'" v)
    | (PRED, NUM n) -> if n = 0 then NUM 0 else NUM (n-1)      
    | (PRED, v)     -> ERROR (sprintf "APP: 'pred' needs int argument, not '%A'" v)
    | (ISZERO, NUM n) ->  if n = 0 then BOOL true else BOOL false
    | (ISZERO, v)     -> ERROR (sprintf "APP: 'iszero' needs int argument, not '%A'" v)
    | (FUN(str,term),e) -> interp(subst term str (interp e)) //(c) FUN is subset of APP
    | (f,v) -> ERROR(sprintf "APP: cannot apply function %A with arg %A" f v)
| FUN (e1,e2) -> FUN (e1,e2) //(c) FUN is FUN
| REC (s,FUN(str,term)) -> interp(FUN(str, (subst term s (REC(s, FUN(str,term))))))    //(d) REC is FUN (for a while)
| REC (_,v) -> ERROR(sprintf "REC: 'rec' needs fun argument, not '%A'" v) 

// Here are two convenient abbreviations for using your interpreter.
let interpfile filename = filename |> parsefile |> interp
let interpstr sourcecode = sourcecode |> parsestr |> interp

//Tests
printfn "%A" (interpstr "succ 0")                   //NUM 1
printfn "%A" (interpstr "succ 1")                   //NUM 2
printfn "%A" (interpstr "pred 0")                   //NUM 0
printfn "%A" (interpstr "pred 10")                  //NUM 9
printfn "%A" (interpstr "succ (succ (succ 0))")     //NUM 3
printfn "%A" (interpstr "iszero succ")              //ERROR "APP: 'iszero' needs int argument, not 'SUCC'"
printfn "%A" (interpstr "succ pred 7")              //ERROR "APP: 'succ' needs int argument, not 'PRED'"
printfn "%A" (interpstr "succ (pred 7)")            //NUM 7
printfn "%A" (interpstr "iszero 0")                 //BOOL true
printfn "%A" (interpstr "iszero 1")                 //BOOL false
printfn "%A" (interpstr "if succ 4 then 0 else 1")  //ERROR "IF: 'if' needs boolean argument, not 'NUM 5'"
printfn "%A" (interpfile "if.txt")                  //NUM 8
printfn "%A" (interpfile "complex1.txt")            //NUM 1
printfn "%A" (interpfile "complex2.txt")            //NUM 2
printfn "%A" (interpfile "complex3.txt")            //NUM 3
printfn "%A" (interpfile "complex4.txt")            //NUM 4
//(c) tests for fun
printfn "%A" (interpstr "(fun x -> succ x) 4")      //NUM 5
printfn "%A" (interpstr "(fun x -> fun y -> fun z -> if iszero x then succ y else pred z) 0 10 20") //NUM 11
printfn "%A" (interpstr "(fun x -> fun y -> fun x -> if iszero x then succ y else pred x) 0 10 20") //NUM 19
printfn "%A" (interpfile "twice.txt")               //NUM 65536 :))
//(d) tests for rec
printfn "%A" (interpstr "(rec d -> fun n -> if iszero n then 0 else succ (succ (d (pred n)))) 37" ) //double.txt -> NUM 74 :))
printfn "%A" (interpstr "let minus = rec m -> fun x -> fun y -> if iszero y then x else m (pred x) (pred y) in minus 125 79" ) //minus.txt -> NUM 46 :))
printfn "%A" (interpfile "fibonacci.txt")   //NUM 6765
printfn "%A" (interpfile "factorial.txt")   //NUM 720
printfn "%A" (interpfile "lists.txt")       //BOOL true
printfn "%A" (interpfile "ackermann.txt")   //NUM 509
printfn "%A" (interpfile "divisor.txt")     //NUM 191

//17. W algorithm for PCF
let rec W (env, e) = 
  match e with
  | ID s -> (I, env s)
  | NUM n  -> (I, INTEGER)
  | BOOL b -> (I, BOOLEAN)
  | ERROR s -> failwith "W: found token ERROR"
  | SUCC -> (I,ARROW(INTEGER,INTEGER))
  | PRED -> (I,ARROW(INTEGER,INTEGER))
  | ISZERO -> (I,ARROW(INTEGER,BOOLEAN))
  | IF (e1, e2, e3) ->
      let (s1, t1) = W (env, e1)
      let s2 = unify (t1, BOOLEAN)
      let (s3, t2) = W (s2 << s1 << env, e2)
      let (s4, t3) = W (s3 << s2 << s1 << env, e3)
      let s5 = unify (s4 t2, t3)
      (s5 << s4 << s3 << s2 << s1, s5 t3)
  | APP (e1,e2) ->
      let a = newtypevar()
      let (s1, t1) = W (env, e2)
      let (s2, t2) = W (s1 << env, e1)
      let s3 = unify(s2 t2, ARROW((s2 << s1) t1, a))
      (s3 << s2 << s1,  (s3 << s2 << s1) a)
  | FUN (x,e) -> 
      let a = newtypevar()
      let (s1,t1) = W (update env x a, e)
      (s1, s1 (ARROW (a,t1)))
  | REC (x,e) ->
      let a = newtypevar()
      let (s1,t1) = W (update env x a, e)
      let s2 = unify(s1 a, s1 t1)
      (s2 << s1, (s2 << s1) a)

/// infer e finds the principal type of e
let infer e =
  reset ();
  let (s, t) = W (emptyenv, e)
  printf "The principal type is\n %s\n" (typ2str t)

let test () =
    infer (NUM 12)                                                                   // int
    infer (BOOL true)                                                                // bool
    infer (IF(BOOL true, NUM 1, NUM 2))                                              // int
    infer (IF(BOOL true, IF(BOOL true, NUM 1, NUM 2), IF(BOOL false, NUM 3, NUM 4))) // int
    infer (SUCC)                                                                     // int -> int
    infer (PRED)                                                                     // int -> int
    infer (ISZERO)                                                                   // int -> bool
    infer (APP(SUCC,NUM 5))                                                          // int 
    infer (APP(PRED,NUM 5))                                                          // int 
    infer (APP(ISZERO,NUM 5))                                                        // bool 
    infer (IF(APP (ISZERO, NUM 0), APP (SUCC, NUM 5), APP (PRED, NUM 50)))           // int
    infer (IF(APP (ISZERO, NUM 0), APP (ISZERO, NUM 5), APP (ISZERO, NUM 50)))       // bool
    infer (FUN ("n", NUM 7))                                                         // 'a -> int
    infer (FUN ("x", ID "x"))                                                        // 'a -> 'a
    infer (FUN ("b", IF (ID "b", NUM 1, NUM 0)))                                     // bool -> int
    infer (FUN ("n", APP (SUCC, ID "n")))                                            // int -> int
    infer (FUN ("f", APP (ID "f", NUM 7)))                                           // (int -> 'b) -> 'b                  
    infer (FUN ("f", APP (ID "f", APP (ID "f", BOOL true))))                         // (bool -> bool) -> bool
    infer (FUN ("x", FUN ("y", ID "x")))                                             // 'a -> 'b -> 'a
    infer (FUN ("x", FUN ("y", APP (SUCC, ID "x"))))                                 // int -> 'b -> int
    infer (FUN ("f", FUN ("x",APP (ID "f",APP (ID "f",ID "x")))))                    // ('c -> 'c) -> 'c -> 'c
    infer (FUN ("f", FUN ("g", FUN ("x",APP (ID "f",APP (ID "g",ID "x"))))))         // ('e -> 'd) -> ('c -> 'e) -> 'c -> 'd
    infer (FUN ("x", FUN ("y", FUN ("b", IF (ID "b", ID "x", ID "y")))))             // 'b -> 'b -> bool -> 'b
    infer (FUN ("x", FUN ("y", FUN ("z", ID "z"))))                                  // 'a -> 'b -> 'c -> 'c
    //infer (FUN ("x", FUN ("y", ID "z")))                                           //  Unhandled Exception: System.Exception: identifier z is unbound
    infer (REC ("m", FUN ("x", FUN ("y", IF( APP(ISZERO, ID "y"),ID "x", APP (APP (ID "m", APP (PRED, ID "x")), APP (PRED, ID "y")))))))
    //int -> int -> int
    infer (REC ("f", FUN ("b", IF( ID "b", NUM 1, APP (ID "f", BOOL true))))) // bool -> int
    infer (REC ("f", FUN ("x", APP (ID "f", ID "x"))))                        // 'b -> 'c
    infer (REC ("even", FUN ("n", IF (APP(ISZERO, ID "n"), BOOL true, IF (APP (ISZERO, APP (PRED, ID "n")), BOOL false, APP (ID "even", APP (PRED, APP (PRED, ID "n"))))))))
    //int -> bool
printfn "%A" (test())

//Check with F# 
let succ n = n+1
let pred n = if n = 0 then 0 else n-1
let iszero n = if n = 0 then true else false
let type1 = fun n -> 7                           // 'a -> int
let type2 = fun x -> x                           // 'a -> 'a
let type3 = fun b -> if b then 1 else 0          // bool -> int
let type4 = fun n -> succ n                      // int -> int
let type5 = fun f -> f 7                         // (int -> 'a) -> 'a
let type6 = fun f -> f (f true)                  // (bool -> bool) -> bool
let type7 = fun x -> fun y -> x                  // 'a -> 'b -> 'a
let type8 = fun x -> fun y -> succ x             // int -> 'a -> int
let type9 = fun f -> fun x -> f ( f x)           // ('a -> 'a) -> 'a -> 'a
let type10 = fun f -> fun g -> fun x -> f ( g x) // ('a -> 'b) -> ('c -> 'a) -> 'c -> 'b
let type11 = fun x -> fun y -> fun b ->          // 'a -> 'a -> bool -> 'a
    if b then x else y
let type12 = fun x -> fun y -> fun z -> z        // 'a -> 'b -> 'c -> 'c
//let type13 = fun x -> fun y -> z               // z is not defined
let rec type14 = fun x -> fun y -> if iszero y then x else type14 (pred x) (pred y) // int -> int -> int
let rec type15 = fun b -> if b then 1 else type15 true                              // bool -> int
let rec type16 = fun x -> type16 x                                                  // 'a -> 'b
let rec type17 = fun n -> if iszero n then true                                     // int -> bool
                          else if iszero (pred n) then false else type17 (pred (pred n))
//Overall the W algorithm above is working as intended with the exception 
//that F# resets back to 'a when the vars are the same then to 'b.. etc

(*
18. Measures
 a. Declare type measures for seconds, microseconds, milliseconds, and nanoseconds.
 b. Declare constants for the number of seconds in each of the other types.
 c. Create functions that convert seconds to each of the other types. What is the principal type of each function?
 d. Create functions that convert each of the other types to seconds. What is the principal type of each function?
 e. Convert 5000 milliseconds to seconds and then to microseconds.
 f. Convert 0.00000009 seconds to microseconds and to nanoseconds.
*)

//(a)
[<Measure>] type sec
[<Measure>] type microsec
[<Measure>] type millisec
[<Measure>] type nanosec

//(b)
let nanosec_per_sec = 1000000000.0<nanosec/sec>
let microsec_per_sec = 1000000.0<microsec/sec>
let millisec_per_sec = 1000.0<millisec/sec>

//(c)
let toNano (sec:float<sec>)  = sec * nanosec_per_sec  //val toNano : sec:float<sec> -> float<nanosec>
let toMicro (sec:float<sec>) = sec * microsec_per_sec //val toMicro : sec:float<sec> -> float<microsec>
let toMilli (sec:float<sec>) = sec * millisec_per_sec //val toMilli : sec:float<sec> -> float<millisec>
printfn "5.0 seconds = %A nanoseconds" (toNano 5.0<sec>)
printfn "5.0 seconds = %A microseconds" (toMicro 5.0<sec>)
printfn "5.0 seconds = %A milliseconds" (toMilli 5.0<sec>)

//(d)
let fromNano (nanosec:float<nanosec>) = nanosec/nanosec_per_sec      //val fromNano : nanosec:float<nanosec> -> float<sec>
let fromMicro (microsec:float<microsec>) = microsec/microsec_per_sec //val fromMicro : microsec:float<microsec> -> float<sec>
let fromMilli (millisec:float<millisec>) = millisec/millisec_per_sec //val fromMilli : millisec:float<millisec> -> float<sec>
printfn "5000000000.0 nanoseconds = %A seconds" (fromNano 5000000000.0<nanosec>)
printfn "5000000.0 microseconds = %A seconds" (fromMicro 5000000.0<microsec>)
printfn "5000.0 milliseconds = %A seconds" (fromMilli 5000.0<millisec>)

//(e)
let conversion1 = 5000.0<millisec> |> fromMilli |> toMicro 
printfn "5000.0 milliseconds = %A microseconds" conversion1 //5000000.0

//(f)
let conversion2 = 0.00000009<sec> |> toMicro |> fromMicro |> toNano
printfn "0.00000009 seconds = %A nanoseconds" conversion2 //90.0
