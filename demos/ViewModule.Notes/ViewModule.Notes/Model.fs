namespace ViewModule.Notes

open System
open System.Drawing

module Model =
    type Point = { X : float ; Y : float }
    type PointPair = { Start : Point ; End : Point }

    type Content =
      | Text    of string
      | Image   of Image
      | WebLink of Uri
      | Sketch  of PointPair list

    type NoteContent = NoteContent of Guid * Content

    type Note = Note of Guid * NoteContent list

    type Library = Library of Note list

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Content =
        let toNoteContent content = NoteContent (Guid.NewGuid(), content)

        module Text = 
            let createEmpty ()   = Text ""
            let create      str  = Text str
        
        module Image =
            let create img = Image img
           
        module WebLink =
            let create uri = WebLink uri

        module Sketch =
            let createEmpty () = Sketch List.empty

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module NoteContent =
        let createNew   content                         = NoteContent (Guid.NewGuid(), content)
        let withContent content (NoteContent (guid, _)) = NoteContent (guid,           content)

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Note =
        let createEmpty ()                         = Note (Guid.NewGuid(), List.empty)
        let withContents contents (Note (guid, _)) = Note (guid,           List.ofSeq contents)

        let guid     (Note (guid, _))     = guid
        let contents (Note (_, contents)) = contents