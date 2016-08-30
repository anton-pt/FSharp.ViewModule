namespace ViewModule.Notes.ViewModles

open System
open System.Collections.ObjectModel

open ViewModule
open ViewModule.FSharp

open ViewModule.Notes.Model

type NoteViewModel(note : Note) as self =
    inherit ViewModelBase()

    let contentVMs = Note.contents note |> List.map NoteContentViewModel.fromNoteContent
    let contents = self.Factory.Backing(<@ self.Contents @>, ObservableCollection<_>(contentVMs))
    let insertText = self.Factory.CommandSyncParam(fun n -> contents.Value.Insert(n, TextContentViewModel()))

    do self.DependencyTracker.AddPropertyDependency(<@ self.Note @>, <@ self.Contents @>)
    
    member x.Guid with get() = Note.guid note
    member x.Contents with get() = contents.Value
    member x.Note with get() = note |> Note.withContents (contents.Value |> Seq.map NoteContentViewModel.toNoteContent)

    member x.InsertText with get() = insertText

    new () = NoteViewModel(Note.createEmpty ())