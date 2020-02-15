open System
open System.IO
open ICSharpCode.SharpZipLib.Zip.Compression.Streams

open FParsec
open Shit

[<EntryPoint>]
let main argv =
    let blobData = 
        "blob 16\000.ionide\nobj\nbin\n"

    let uncompress path =
        let treeObjectFile = FileInfo(path)
        use stream = treeObjectFile.OpenRead ()
        use inflater = new InflaterInputStream(stream)
        let buffer = new MemoryStream ()
        inflater.CopyTo(buffer)
        buffer.Position <- 0L

        buffer

    use stream = uncompress ".git/objects/8b/902e68847c2ec430f2a714269e38129f58734e"

    match runParserOnStream ObjectParser.tree () "" stream Encodings.Identity with
    | Success (r, _, _) ->
        printfn "Result: `%A`" r
    | error ->
        printfn "Error: %A" error
    
    0 // return an integer exit code
