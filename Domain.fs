namespace shit

  module Domain =

    type Object =
      | Commit of CommitInfo
      | Tree   of TreeInfo
      | Blob   of BlobInfo
      | Tag    of TagInfo

    and CommitInfo =
      { parents:   Hash list
        hash:      Hash
        committer: Author
        message:   string
      }

    and TagInfo =
      { object: Hash
        type: ObjectType
        tag: TagName
        tagger: Author
        message: string
      }

    and TagName = Name of string

    and ObjectType =
      | Commit

    and Author =
      { name:  Name
        email: ElectronicAddress
      }

    and ElectronicAddress = 
      | Email of string

    and TreeInfo =
      { contents: TreeEntry list
      }

    and TreeEntry =
      | Tree of TreeEntryInfo
      | Blob of TreeEntryInfo

    and TreeEntryInfo = FileMode * Hash * Name

    and FileMode = int

    and Hash = 
      | Sha1 of string

    and Name = 
      | Name of string

    and BlobInfo =
      { contents: string 
      }
