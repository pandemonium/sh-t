namespace Shit

  open System
  open FParsec

  module ObjectParser =
    let nilChar = pchar '\000'
    let space   = pchar ' '

    let content length = anyString length

    let objectHeader name =
      skipString name >>. space >>. pint32 .>> nilChar

    let blob =
      objectHeader "blob" >>= content

    let bounded length (p: Parser<'a, 'b>) : Parser<'a, 'b> =
      fun input ->
        let chars = input.Read(length)
        use input' = new CharStream<'b>(chars, 0, length)
        p input'

    let fileMode =
      many1Satisfy isDigit 
      |>> Domain.FileMode

    let fileName =
      many1Satisfy ((<>) '\000') .>> nilChar
      |>> Domain.FileName.Name

    let formatByte b =
        sprintf "%02x" b

    let formatHexString =
        Array.map formatByte
        >> String.concat ""

    let binaryHexString count = 
        anyString count
        |>> Encodings.Identity.GetBytes
        |>> formatHexString

    let ref =
      binaryHexString 20
      |>> Domain.Sha1Hash

    let treeEntry =
      tuple3 fileMode fileName ref
      |>> Domain.TreeEntryInfo

    let treeContent length =
      many1 treeEntry

    let tree =
      objectHeader "tree"
      >>= treeContent
      |>> Domain.Object.tree

    let object =
      blob |>> Domain.Object.blob

    run blob "" |> ignore

  module Codec =
    type Data = char array
    type DecodeResult = 
      Result<Domain.Object, DecodeError>
    and DecodeError =
      | Omgwtfbbq of string

    module DecodeResult =
      let omgwtfbbq =
        Omgwtfbbq >>  Result.Error

    let encodeObject object : Data = 
      [| '\000' |]

    let tryDecodeObject (data: Data) : DecodeResult =
      data |> ignore

      "Write the code"
      |> DecodeResult.omgwtfbbq