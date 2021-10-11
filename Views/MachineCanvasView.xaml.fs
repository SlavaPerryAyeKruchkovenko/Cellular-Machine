namespace Views

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Avalonia.Interactivity
open ViewModels
open Avalonia.Input
open Avalonia
open Avalonia.Controls.Shapes
open System.Linq

type MachineCanvasView () as self = 
    inherit UserControl ()

    do AvaloniaXamlLoader.Load self
    let canvas = self.FindControl<Canvas>("holst")
    member this.AddRectangle(object:obj,e:PointerPressedEventArgs)=
        let CorrectPos(num : float,size:float) = 
            let x = num % size
            if (x >= size/2.0) then
                num + size-x
            else
                num - x 
            
        let size = 10.0        
        let canvas:Canvas = downcast object
        let position = e.GetCurrentPoint(canvas).Position
        let rect = new Thickness(CorrectPos(position.X,size),CorrectPos(position.Y,size))
        match self.DataContext with
        | :? MachineCanvasViewModel as vm -> vm.AddChild(rect,size)
        | _ -> ()

    member this.DeleteRect(object:obj,e:RoutedEventArgs) = 
        match object with 
        | :? Rectangle as rect -> canvas.Children.Remove(rect) |> ignore
        | _ -> ()