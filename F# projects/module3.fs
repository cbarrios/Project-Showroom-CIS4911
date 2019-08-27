//Streams Review
type 'a stream = Cons of 'a * (unit -> 'a stream)
let rec upfrom n = Cons(n, fun() -> upfrom(n+1))
let naturals = upfrom 0

let rec take n (Cons(x,xsf)) = 
    if n = 0 then []
    else x::take (n-1) (xsf())
printfn "%A" (take 10 naturals) //[0; 1; 2; 3; 4; 5; 6; 7; 8; 9]

let rec drop n (Cons(x,xsf)) = 
    if n = 0 then Cons(x,xsf)
    else drop (n-1) (xsf())
let natsfrom10 = drop 10 naturals
printfn "%A" (take 10 natsfrom10) //[10; 11; 12; 13; 14; 15; 16; 17; 18; 19]

let rec filter p (Cons(x,xsf)) = 
    if p x then Cons(x, fun() -> filter p (xsf()))
    else filter p (xsf())
let take10evenfromnats = take 10 (filter (fun n -> n%2=0) naturals)
printfn "%A" take10evenfromnats //[0; 2; 4; 6; 8; 10; 12; 14; 16; 18]

//Post quiz 3. Review
//Generate the floating-point series: 1/n*n for n>0 as a stream or sequence
let rec upfrom3 n = Cons(1.0/(n*n),fun()->upfrom3(n+1.0))
let seriesfloatstream = upfrom3 1.0 //generate stream
printfn "%A" (take 5 seriesfloatstream) //show first 5 elements [1.0; 0.25; 0.1111111111; 0.0625; 0.04]

//Sequence Alternative:
let seriesfloatseq = Seq.initInfinite (fun i -> 
   let n = float(i + 1)
   1.0/(n*n) )
printfn "%A" seriesfloatseq //seq [1.0; 0.25; 0.1111111111; 0.0625; ...]

//Tail Recursion: Given an int list return the sum of all its odd elements
let addOdds list =
    let rec helper = function
    |[],acc -> acc
    |x::xs,acc -> helper(xs, if x%2=1 then x+acc else acc)
    helper(list,0)

let randomlist = [1..20]
let result = addOdds randomlist
printfn "%A" result //100 because 1+3+5+7+9+11+13+15+17+19 = 100

//Imperative F#: Declare a variable with type int list ref. 
//Then use it to create a function that only adds an int to the front of that list and saves the change (Setter)
let list = ref [0]
let modify = (fun n -> list := n::(!list))
modify 2
printfn "%A" !list //[2; 0]
modify 5
printfn "%A" !list //[5; 2; 0]

//Type Inference
let muchfun = (fun g -> g [5] < 9)      // muchfun:  (int list -> int) -> bool   ;  g: (int list -> int)
let muchfun2 = (fun g -> g [5] < 9.65)  // muchfun2: (int list -> float) -> bool ;  g: (int list -> float)

let result2 = muchfun addOdds
printfn "%A" result2 //true since 5 < 9

// Parser: Check if a token list is valid for this language
// P -> APB | BPA | C

type tokens2 = A | B | C | EOF | ERROR

let Eat t = function
|[] -> [ERROR]
|x::xs -> if x = t then xs else [ERROR]

let rec P = function
|[] -> [ERROR]
|x::xs -> 
    match x with
    |A -> xs |> P |> Eat B
    |B -> xs |> P |> Eat A
    |C -> xs
    |_ -> [ERROR]

let evaluate prog = 
    let ret = prog |> P
    if ret = [EOF] then "accept" else "error"

printfn "%A" (evaluate [A;B;C;A;B;EOF])
printfn "%A" (evaluate [A;B;A;B;EOF])

//Question of the Extra Credit Quiz (Post Exam 3):
//Given an int list find the minimum and maximum in a tuple(min,max) (Must be Tail Recursive)
let rec findminmax = function
|[],(a,b) -> (a,b)
|x::xs,(a,b) -> findminmax(xs, if x > b && a = 2147483647 && b = -2147483648 then (x,x) else if x < a then (x,b) else if x > b then (a,x) else (a,b))

let findminandmax list =  findminmax (list,(2147483647,-2147483648))
printfn "%A" (findminandmax [])          //(2147483647, -2147483648)
printfn "%A" (findminandmax [1])         //(1, 1)
printfn "%A" (findminandmax [2;1])       //(1, 2)
printfn "%A" (findminandmax [3;2;1])     //(1, 3)
printfn "%A" (findminandmax [4;2;1;3])   //(1, 4)
printfn "%A" (findminandmax [1..100])    //(1, 100)
printfn "%A" (findminandmax [1..2..100]) //(1, 99)
printfn "%A" (findminandmax [-1;0;0])    //(-1, 0)
printfn "%A" (findminandmax [-10;2;500;40;3;-40;100]) //(-40, 500)
