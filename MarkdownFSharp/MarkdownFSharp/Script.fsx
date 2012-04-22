// This file is a script that can be executed with the F# Interactive.  
// It can be used to explore and test the library project.
// Note that script files will not be part of the project build.

#light

let y = 0

let data = [1.;2.;3.;4.]

let sqr x = x * x

type UserState = unit // doesn't have to be unit, of course
type Parser<'t> = Parser<'t, UserState>

#r @"..\lib\FParsecCS.dll"
#r @"..\lib\FParsec.dll"
//
//#load "Module1.fs"
//open Module1
//
open FParsec

let test p str = 
    match run p str with
    | Success(result,_,_) -> printfn "Success %A" result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

let str s = pstring s

let floatBetweenBrackets : Parser<_> = str "[" >>. pfloat .>> str "]"