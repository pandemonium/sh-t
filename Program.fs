open System
open System.IO

open FParsec
open Shit

[<EntryPoint>]
let main argv = 
  match argv |> List.ofArray with 
  | command :: args ->
    printfn "Running %s %A" command args
    CommandLine.tryCallHandler command args
    0 // return an integer exit code
  | _ ->
    printfn "dotnet run <args>"
    1

