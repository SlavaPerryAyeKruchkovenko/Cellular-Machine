namespace ViewModels

open Avalonia.Controls
open Avalonia
open Avalonia.Controls.Shapes
open Models
open System.Collections.ObjectModel
open System.Threading
open System.Threading.Tasks

type MainWindowViewModel() =
    inherit ViewModelBase()
    let menu = new MachineMenuViewModel()   
    let holst = new MachineCanvasViewModel()  
    let mutable token = new CancellationTokenSource()     
    member __.Menu with get() = menu
    member __.Holst with get() = holst
    member this.NextEtirationGame() = 
        let rule = new Rule(menu.Density,menu.Resoulution)
        let size = menu.Value
        holst.GenerateField((float)size)
        if(rule.BornRule.[0] > 0 && rule.AliveRule.Length > 0 && size > 0) then
            holst.ActivateMachine(size,rule)
    member this.StartGame() = 
        token <- new CancellationTokenSource()      
        let task() = 
            async{
                let rule = new Rule(menu.Density,menu.Resoulution)
                let size = menu.Value
                holst.GenerateField((float)size)
                if(rule.BornRule.[0] > 0 && rule.AliveRule.Length > 0 && size > 0) then
                    while not token.IsCancellationRequested do
                        do! Task.Run(fun () -> holst.ActivateMachine(size,rule)) |> Async.AwaitTask                      
            }
        task()
        |> Async.Start
        
    member this.CloseGame()= 
        token.Cancel()
    
        
            
