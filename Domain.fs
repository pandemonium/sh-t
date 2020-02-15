namespace Shit

  module Domain =

    type Object =
      | Commit of CommitInfo
      | Tree   of TreeInfo
      | Blob   of BlobInfo
      | Tag    of TagInfo

    and CommitInfo =
      { tree:      Ref
        parents:   Ref list
        author:    Author
        committer: Author
        message:   string
      }

    and TreeInfo =
      { entries: TreeEntry list
      }

    and BlobInfo =
      { contents: string 
      }

    and TagInfo =
      { object:     Ref
        objectType: ObjectType
        tag:        TagName
        tagger:     Author
        message:    string
      }

    and TreeEntry = 
      | TreeEntryInfo of FileMode * FileName * Ref

    and Ref = 
      | Sha1Hash of string

    and TagName = Name of string

    and ObjectType =
      | Commit

    and Author =
      { name:  AuthorName
        email: ElectronicAddress
      }

    and AuthorName =
      Name of string

    and ElectronicAddress = 
      | Email of string

    and FileMode = 
      | FileMode of string

    and FileName = 
      | Name of string

    module TreeEntry =
      let name (TreeEntryInfo (_, Name name, _)) = name

    module Author =
      let make name email =
        { name  = AuthorName.Name name
          email = Email email
        }

    module Object =
      let blob contents =
        Object.Blob <|
        { contents = contents }

      let tree entries =
        Object.Tree <|
        { entries = entries }

      let commit tree parents author committer message =
        Object.Commit {
          tree      = tree
          parents   = parents
          author    = author
          committer = committer
          message   = message
        }
