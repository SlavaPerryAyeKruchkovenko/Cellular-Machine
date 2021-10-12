namespace Views

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Avalonia.Interactivity
open ViewModels
open Avalonia.Input
open Avalonia
open Avalonia.Controls.Shapes
open Models
open System

type MachineCanvasView () as self = 
    inherit UserControl ()

    do AvaloniaXamlLoader.Load self
    let mutable Canvas:Option<Canvas> = None
    let mutable canClick = true 
    member this.AddRectangle(object:obj,e:PointerPressedEventArgs)=
        if canClick then
            let CorrectPos(num : float,size:float) = 
                let x = num % size
                if (x >= size/2.0) then
                    num + size-x
                else
                    num - x 
                
            let size = new Size(10.0,10.0)       
            let canvas:Canvas = downcast object
            if Canvas.IsNone then
                Canvas <- Some canvas
            let position = e.GetCurrentPoint(canvas).Position      
            let point = new Point(CorrectPos(position.X,size.Width),CorrectPos(position.Y,size.Width))
            let cell = new Cell(point,0,size)
            match self.DataContext with
            | :? MachineCanvasViewModel as vm -> vm.AddChild(cell)
            | _ -> ()
        else
            canClick <- true

    member this.DeleteRect(object:obj,e:RoutedEventArgs) = 
        canClick <- false
        
        match object with 
        | :? ContentControl as cntrl -> 
            let point = new Point(Math.Round cntrl.Bounds.X,Math.Round cntrl.Bounds.Y)
            let cell = new Cell(point,0,new Size())
            match self.DataContext with
            | :? MachineCanvasViewModel as vm -> vm.DeleteChildAboutLoc(cell)
            | _ -> ()
        | _ -> ()