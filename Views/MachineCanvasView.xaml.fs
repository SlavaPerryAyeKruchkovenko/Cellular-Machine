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
    let mutable canClick = true    

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