namespace Shit

  module Domain =

    type Object =
      | Commit of CommitInfo
      | Tree   of TreeInfo
      | Blob   of BlobInfo
      | Tag    of TagInfo

    and CommitInfo =
      { parents:   Ref list
        hash:      Ref
        committer: Author
        message:   string
      }

    and TreeInfo =
      { contents: TreeEntry list
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

    module Object =
      let blob contents =
        Object.Blob { contents = contents }

      let tree entries =
        Object.Tree { contents = entries }