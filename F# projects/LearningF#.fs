(* This is block comment *)
// And this is line comment

//Print a string and ADD NEW LINE (printfn)
printfn "A string: %s" "Hello World"

//The 'let' keyword defines an (immutable) value (cannot be changed)
let result = 1 + 1 = 2
//result <- 5  (compiler will complain since result is immutable) 

//(The 'let' keyword to declare) ('mutable' allows the variable to change its value)
let mutable pantherid = 1234567 
// Print the variable as an int and DO NOT ADD NEW LINE (printf)
printf "%d" pantherid

//This statement is allowed only if 'let' is followed by 'mutable'
//in the initial declaration of the variable
pantherid <- 6048821 
printfn "%d" pantherid

//STRINGS:
//String concatenation
let string = "I am currently taking "
let sentence = string + "COP4555:" + "Principles of Programming Languages."
printfn "%s" sentence

//Use '@' before a string to allow for quotes to display inside this string(surround it with ""string"") Ex: beautiful will be quoted below
let str1 = @"You are ""beautiful"" and you too "
printfn "%s" str1

//The alternative is to use triple quotes instead of single quotes for a given string. That guarantees that we can use "string" inside to display them
let str2 = """ "This text will be quoted" but this one will not """
printfn "%s" str2

//Backslash strings indent string contents by stripping leading spaces
(* \n and \n\ are different *)
let poem = 
    "The lesser world was daubed\n\
     By a colorist of modest skill\n\
     A master limned you in the finest inks\n\
     And with a fresh-cut quill."
printfn "%s" poem

//BASIC TYPES
let str = "Hello COP4555"
(*
> let str = "Hello COP4555";;
val str : string = "Hello COP4555"
*)

let b, i, l = 86uy, 86, 86L
(* 
> let b, i, l = 86uy, 86, 86L;;
val l : int64 = 86L
val i : int = 86
val b : byte = 86uy 
*)

let s, f, d, bi = 4.14F, 4.14, 0.7833M, 9999I
(*
> let s, f, d, bi = 4.14F, 4.14, 0.7833M, 9999I;;
val s : float32 = 4.13999987f
val f : float = 4.14
val d : decimal = 0.7833M
val bi : System.Numerics.BigInteger = 9999
*)

//LISTS
//A list is an immutable collection of elements of the same type.
//Lists use square brackets and ‘;‘ delimiter
let list1 = [ "a"; "b" ]

// :: is prepending
let list2 = "c" :: list1 //here 'c' goes first. Ex: ["c";"a";"b"]

// @ is concat
let list3 = list1 @ list2 //["a";"b";"c";"a";"b"]

// Recursion on list using 'cons' (::) operator
let rec sum list =
match list with
| [] -> 0
| x :: xs -> x + sum xs

printfn "%A" (sum [1;3;5;7;9]) //Output is: 25 

//ARRAYS
//Arrays are fixed-size, zero-based, mutable collections of consecutive data elements.
//Arrays use square brackets with bar. In other words, they are lists with bars 
let array1 = [| "a"; "b" |]

// Indexed access using dot
let first = array1.[0]
printfn "%A" first //Output is: "a"

//Arrays are mutable so they allow us to change their values
array1.[0] <- "c"
printfn "%A" array1.[0] //Now the value was changed to "c"

//Higher-order functions on collections(lists and arrays)
//The same list [ 1; 3; 5; 7; 9 ] or array [| 1; 3; 5; 7; 9|] can be generated in various ways.
//1.Using range operator ..
let xs = [ 1..2..9 ] //Here number '2' indicates that the numbers in the lists must increase by 2 each time from 1 to 9 (including them)
printfn "%A" xs

//2.Using list or array comprehensions
let ts = [| for i in 0..4 -> 2 * i + 1 |] //i is inclusive in range from 0 to 4. Values in the array are being initialized by 2*i+1
printfn "%A" ts

//3.Using init function
let zs = List.init 5 (fun i -> 2 * i + 1) //'5' represents the number of elements we want to initialize(size of the array). Values in the array are being initialized by 2*i+1 (i starting from 0)
printfn "%A" zs

//Lists and arrays have comprehensive sets of higher-order functions for manipulation. 
//fold starts from the left of the list (or array) and foldBack goes in the opposite direction
let xs' = Array.fold (fun str n -> sprintf "%s,%i" str n) "" [| 0..9 |]
printfn "%A" xs' //Output is: ",0,1,2,3,4,5,6,7,8,9"

//reduce doesn’t require an initial accumulator
let last xs = List.reduce (fun acc x -> x) xs
printfn "%A" (last [0..9]) //Output is: 9

//map transforms every element of the list (or array)
let ys' = Array.map (fun x -> x * x) [| 0..9 |] //map needs a function as first parameter, then it takes a list to transform
printfn "%A" ys' //Output is: [|0; 1; 4; 9; 16; 25; 36; 49; 64; 81|]

//iterate through a list and produce side effects
let _ = List.iter (printf "%i") [ 0..9 ] //Output is: 0123456789

//EXPLORING RESTRICTIONS
//Immutability
let value = 1
value = value + 1 // Produces a 'bool' value! 

let exp4 = List.map List.head [[];[1;2]]
printfn "%A" exp4 //problem at run time : 'The input list was empty'

//let e1 = List.head [] //value restriction complain
