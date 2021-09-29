namespace ViewModels

open Avalonia.Controls
open Avalonia
open Avalonia.Controls.Shapes
open Models
open System.Collections.ObjectModel

type MainWindowViewModel() =
    inherit ViewModelBase()
    let menu = new MachineMenuViewModel()   
    let holst = new MachineCanvasViewModel()  
    member __.Menu with get() = menu
    member __.Holst with get() = holst
    
        
            
