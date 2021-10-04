namespace ViewModels

open Avalonia.Controls
open Avalonia
open Avalonia.Controls.Shapes
open Services
open System.Collections.ObjectModel

type MainWindowViewModel() =
    inherit ViewModelBase()
    let menu = new MachineMenuViewModel()   
    let holst = new MachineCanvasViewModel()  
    member __.Menu with get() = menu
    member __.Holst with get() = holst
    member this.StartGame() = 
        let rule = new Rule(DieRule = menu.Density,BornRule = menu.Resoulution)
        let size = menu.Value
        if(rule.BornRule > 0 && rule.DieRule > 0 && size > 0) then
            holst.ActivateMachine(10,rule)
        else
            ()
        
            
