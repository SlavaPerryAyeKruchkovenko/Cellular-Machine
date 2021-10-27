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
    let mutable canvas:Option<Canvas> = None
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
            let canvas2:Canvas = downcast object
            if canvas.IsNone then
                canvas <- Some canvas2
            let position = e.GetCurrentPoint(canvas2).Position      
            let point = new Point(CorrectPos(position.X,size.Width),CorrectPos(position.Y,size.Width))
            let cell = new Cell(point,0,size,true)
            match self.DataContext with
            | :? MachineCanvasViewModel as vm -> vm.AddChild(cell) 
                                                 vm.Holst <- canvas.Value
            | _ -> ()
        else
            canClick <- true

    member this.DeleteRect(object:obj,e:RoutedEventArgs) = 
        canClick <- false
        
        match object with 
        | :? ContentControl as cntrl -> 
            let point = new Point(Math.Round cntrl.Bounds.X,Math.Round cntrl.Bounds.Y)
            let cell = new Cell(point,0,new Size(),true)
            match self.DataContext with
            | :? MachineCanvasViewModel as vm -> vm.DeleteChildAboutLoc(cell)
            | _ -> ()
        | _ -> ()    