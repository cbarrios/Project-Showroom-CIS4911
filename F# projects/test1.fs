//QUESTIONS FROM TEST 1
let first_element (x,y,z) = x
printfn "%A"  (first_element (1,2,3))

let split_even_odd list = List.filter(fun x -> x%2 = 0)list @ List.filter(fun y -> y%2 = 1)list
printfn "%A" (split_even_odd [1..10])
//If we want to reverse the second list we do this:
let split_even_odd2 list = List.filter(fun x -> x%2 = 0)list @ List.rev (List.filter(fun y -> y%2 = 1) list)
printfn "%A" (split_even_odd2 [1..10])

let inorder (x,y,z) = (x<y)&&(y<z)
//> let inorder (x,y,z) = (x<y)&&(y<z);;
//val inorder : x:'a * y:'a * z:'a -> bool when 'a : comparison

let add2 x = x+2
printfn "%A" (add2 3*4) //takes parameter first then multiplies afterwards...
//Output is 20

let add3 y = y+3

let add2_or_add3 z = if z > 10 then add2 z else add3 z
printfn "%A" (add2_or_add3 13) //Output is 15

let sphere_volume R:float = 4.0/3.0 * System.Math.PI * R ** 3.0 //Make sure the numbers involved are with .0
printfn "%A" (sphere_volume 2.0) //Output is 33.51032164

let rec calculate_volumes list =
    match list with 
    |[]->[]
    |x::xs-> (sphere_volume x)::calculate_volumes xs

printfn "%A" (calculate_volumes [2.0;3.0;4.0]) //Output is [33.51032164; 113.0973355; 268.0825731]

//Alternate version using List.map
let calculate_volumes2 list = List.map(fun x -> sphere_volume x) list
printfn "%A" (calculate_volumes2 [2.0;3.0;4.0]) //Output is [33.51032164; 113.0973355; 268.0825731]

let expre1 = (1,"cat",10.9) < (1,"dog",8.9)
printfn "%A" expre1 // TRUE since "cat" < "dog"

//let expre2 = (1,2,3) < (1,2,3,4) //Type error: lenght must be the same for tuple comparison
