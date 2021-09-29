namespace ViewModels

open Avalonia
open Avalonia.Controls.Shapes
open System.Collections.ObjectModel
open Avalonia.Media

type MachineCanvasViewModel() =
    inherit ViewModelBase();
    let list = new ObservableCollection<Rectangle>()
    member __.Cells with get() = list
    member this.AddChild(point:Point,size) =
        let cell = 
            let rect = new Rectangle()
            rect.Width <- size
            rect.Height <- size
            rect.Fill <- Brushes.Yellow
            rect.Tapped.Add(fun evArg -> this.Cells.Remove(rect)|> ignore)
            rect
        list.Add(cell);
