open System
open System.IO

open FParsec
open Shit

[<EntryPoint>]
let main argv =
    let blobData = 
        "blob 16\000.ionide\nobj\nbin\n"

    let aTree = "8b902e68847c2ec430f2a714269e38129f58734e"
    let tree = "3d4b6642df72c0bea4a34e07a12c1aded995034f"
    let commit = "176f9e4a716174406909d2dd547b66ea5d1d54d7"

    use stream = 
        Database.uncompress 
        <| Database.defaultObjectPath commit

    match runParserOnStream ObjectParser.object () "" stream Encodings.Identity with
    | Success (r, _, _) ->
        printfn "Result: `%A`" r
    | error ->
        printfn "Error: %A" error
    
    0 // return an integer exit code
