namespace Shit

  open System


  module CommandLine =
    type Args    = string array
    type Handler = Args -> unit

    let showTree { Domain.TreeInfo.entries = entries } =
      entries
      |> List.map (Domain.TreeEntry.name >> printfn "%s")
      |> ignore

    let showAuthor { Domain.Author.name  = Domain.AuthorName.Name name
                     Domain.Author.email = Domain.Email email
                   } =
        sprintf "%s <%s>" name email

    let showCommit (commit: Domain.CommitInfo) =
      let (Domain.Ref.Sha1Hash treeRef) = commit.tree
      printfn "Author: %s"    <| showAuthor commit.author
      printfn "Committer: %s" <| showAuthor commit.committer

      Database.tryGetObject treeRef
      |> Result.map (function
        | Domain.Tree tree ->
          showTree tree
        | _ ->
          ()
      )
      |> ignore

      printfn "%s" commit.message

    let showBlob { Domain.BlobInfo.contents = contents } =
      printfn "%s" contents

    let showObject = function
      | Domain.Object.Blob blob     -> showBlob blob
      | Domain.Object.Commit commit -> showCommit commit
      | Domain.Object.Tree tree     -> showTree tree
      | Domain.Object.Tag tag       -> ()

    let showHandler = function
      | hash::_ ->
        Database.tryGetObject hash
        |> Result.map showObject
        |> ignore
      | _ ->
        printfn "show <hash>"

    let handlers =
      [ "show", showHandler ]
      |> Map.ofList

    module Option =
      let tryApply args =
        Option.map (fun f -> f args)

    let tryCallHandler name args =
      handlers
      |> Map.tryFind name
      |> Option.tryApply args
      |> ignore
