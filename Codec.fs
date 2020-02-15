namespace Shit

  open System
  open FParsec

  module ObjectParser =
    let nilChar     = pchar '\000'
    let space       = pchar ' '
    let lessThan    = pchar '<'
    let greaterThan = pchar '>'

    let content length = anyString length

    let objectHeader name =
      skipString name >>. space >>. pint32 .>> nilChar

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

    let treeObject =
      objectHeader "tree" >>= treeContent
      |>> Domain.Object.tree

    let labeledRef name =
      skipString name >>. space >>. many1Satisfy isHex
      |>> Domain.Sha1Hash

    let parent =
      labeledRef "parent"

    let parents =
      many parent

    let authorName =
      many1CharsTill anyChar lessThan

    let email =
      many1CharsTill anyChar greaterThan

    let authorDecl label = 
      skipString label 
      >>. pipe2 authorName email Domain.Author.make
      .>> skipMany1Till anyChar newline

    let author    = authorDecl "author"
    let committer = authorDecl "committer"

    let commitContent length = parse {
      let! tree      = labeledRef "tree" .>> newline
      let! parents   = parents .>> newline
      let! author    = author
      let! committer = committer .>> newline
      let! message   = many1Chars anyChar

      return Domain.Object.commit tree parents author committer message
    }

    let commitObject =
      objectHeader "commit" >>= commitContent

    let blobObject =
      objectHeader "blob" >>= content
      |>> Domain.Object.blob

    let object =
      [ blobObject
        treeObject
        commitObject
      ]
      |> choice

    run object "" |> ignore

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