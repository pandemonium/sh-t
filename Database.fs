namespace Shit

  open System
  open System.IO
  open ICSharpCode.SharpZipLib.Zip.Compression.Streams


  (* Functions for locating files for the different types of object. *)
  module Database =
    let objectsPath (hash: string) =
      sprintf "objects/%s/%s"
      <| hash.[0..1]
      <| hash.[2..]

    let defaultObjectPath =
      objectsPath >> sprintf ".git/%s"

    let uncompress pathSpec =
      let treeObjectFile = FileInfo(pathSpec)
      use stream = treeObjectFile.OpenRead ()
      use inflater = new InflaterInputStream(stream)
      let buffer = new MemoryStream ()
      inflater.CopyTo(buffer)
      buffer.Position <- 0L

      buffer
