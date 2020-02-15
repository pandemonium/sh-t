namespace Shit

open System


type IdentityEncoding () =
    inherit Text.Encoding ()

    override __.GetByteCount(chars: char [], index: int, count: int) : int = 
        count

    override __.GetMaxByteCount(charCount: int) : int = 
        charCount

    override __.GetBytes(chars: char [], charIndex: int, charCount: int, bytes: byte [], byteIndex: int) : int =
        for i in 0 .. charCount-1 do
            bytes.[byteIndex + i] <- chars.[charIndex + i] |> byte

        charCount

    override __.GetCharCount(bytes: byte [], index: int, count: int) : int = 
        count
    
    override __.GetMaxCharCount(byteCount: int) : int = 
        byteCount

    override __.GetChars(bytes: byte [], byteIndex: int, byteCount: int, chars: char [], charIndex: int) : int =
        for i in 0 .. byteCount-1 do
            chars.[charIndex + i] <- bytes.[byteIndex + i] |> char

        byteCount

module Encodings =
    let Identity = IdentityEncoding ()
