namespace ViewModule.Notes.ViewModles

open System

open ViewModule
open ViewModule.FSharp

open ViewModule.Notes.Model

type INoteContentViewModel =
    abstract member Guid        : Guid        with get
    abstract member NoteContent : NoteContent with get

type TextContentViewModel(noteContent) as self =
    inherit ViewModelBase()

    let (guid, text') = 
        noteContent |> function
        | NoteContent(guid, Text text) -> (guid, text)
        | NoteContent(_,    x)         -> failwithf "Cannot present content of type: %A." (x.GetType())

    let noteContentVM = self :> INoteContentViewModel
    let text = self.Factory.Backing(<@ self.Text @>, text')

    do 
        self.DependencyTracker.AddPropertyDependency(<@ self.Content @>, <@ self.Text @>)
        self.DependencyTracker.AddPropertyDependency(<@ noteContentVM.NoteContent @>, <@ self.Content @>)

    member x.Text    with get() = text.Value and set value = text.Value <- value
    member x.Content with get() = Content.Text.create x.Text

    interface INoteContentViewModel with
        member x.Guid        with get() = guid
        member x.NoteContent with get() = noteContent |> NoteContent.withContent x.Content

    new () = TextContentViewModel(Content.Text.createEmpty () |> Content.toNoteContent)

module NoteContentViewModel =
    let fromNoteContent = function
        | NoteContent (guid, Text t) as nc -> TextContentViewModel(nc) :> INoteContentViewModel
        | NoteContent (guid, x)            -> failwithf "Unsupported content type: %A." (x.GetType())

    let toNoteContent (noteContentVM : INoteContentViewModel) = noteContentVM.NoteContent
