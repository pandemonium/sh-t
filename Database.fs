namespace Shit

  open System
  open System.IO
  open ICSharpCode.SharpZipLib.Zip.Compression.Streams
  open FParsec

  module Try =
    let exceptionalBlock (f: unit -> 'a) : Result<'a, exn> =
      try f () |> Result.Ok
      with e   -> Result.Error e

  (* Functions for locating files for the different types of object. *)
  module Database =
    type Problem =
      | Exception  of exn
      | ParseError of string * ParserError

    let objectsPath (hash: string) =
      sprintf "objects/%s/%s"
      <| hash.[0..1]
      <| hash.[2..]

    let defaultObjectsPath =
      objectsPath >> sprintf ".git/%s"

    let openStream pathSpec =
      FileInfo(pathSpec).OpenRead ()

    let inflateObject stream =
      use inflater = new InflaterInputStream(stream)
      let buffer = new MemoryStream ()
      inflater.CopyTo(buffer)
      buffer.Position <- 0L

      buffer

    let tryRunParser stream =
      let parseResult =
        runParserOnStream ObjectParser.object () "" stream Encodings.Identity

      match parseResult with
      | Success (object, _, _)   -> object |> Result.Ok
      | Failure (msg, errors, _) -> ParseError (msg, errors) |> Result.Error

    let tryGetObject hash =
      fun () -> openStream <| defaultObjectsPath hash
      |> Try.exceptionalBlock
      |> Result.mapError Exception
      |> Result.bind (inflateObject >> tryRunParser)