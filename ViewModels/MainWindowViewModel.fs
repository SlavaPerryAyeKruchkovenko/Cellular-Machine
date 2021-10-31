namespace ViewModels

open Avalonia.Controls
open Avalonia
open Avalonia.Controls.Shapes
open Models
open System.Collections.ObjectModel
open System.Threading
open System.Threading.Tasks
open System.Linq
open System.Collections.Generic
open Avalonia.Threading
open Avalonia.Input
open System.Windows.Input

type MyCommand(holst:MachineCanvasViewModel) = 
    let mutable canClick = true
    member val event:PointerPressedEventArgs = null
    interface ICommand with        
        member this.CanExecute(parameter: obj): bool = 
            canClick
        [<CLIEvent>]
        member this.CanExecuteChanged: IEvent<System.EventHandler,System.EventArgs> = 
            raise (System.NotImplementedException())
        member this.Execute(parameter: obj): unit = 
            if canClick then
                let CorrectPos(num : float,size:float) = 
                    let x = num % size
                    if (x >= size/2.0) then
                        num + size-x
                    else
                        num - x 
                    
                let size = new Size(10.0,10.0)       
                let e:PointerPressedEventArgs = downcast parameter
                let position = e.GetCurrentPoint(canvas).Position      
                let point = new Point(CorrectPos(position.X,size.Width),CorrectPos(position.Y,size.Width))
                let cell = new Cell(point,0,size,true)
                holst.AddChild(cell) 
            else
                canClick <- true 
type MainWindowViewModel() =
    inherit ViewModelBase()
    let menu = new MachineMenuViewModel()   
    let holst = new MachineCanvasViewModel()  
    let command = new MyCommand(holst)
    let mutable token = new CancellationTokenSource()  
    member __.MyCommand with get() = command
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
                let mutable oldCells:List<Cell> = new List<Cell>()
                let mutable counter = 0
                let rule = new Rule(menu.Density,menu.Resoulution)
                let size = menu.Value
                holst.GenerateField((float)size)
                if rule.BornRule.[0] > 0 && rule.AliveRule.Length > 0 && size > 0 then
                    while not token.IsCancellationRequested do                    
                        lock holst.Cells (fun () -> holst.ActivateMachine(size,rule))                         
                        if (new HashSet<Cell>(oldCells)).SetEquals(holst.Cells) then
                            counter <- counter + 1
                        else
                            counter <- 0
                        oldCells <- holst.Cells.ToList()
                        if counter > 1 then
                            token.Cancel()
                        do! Task.Delay(100)|> Async.AwaitTask              
            }
        task()|> Async.Start
        
    member this.CloseGame()= 
        token.Cancel()       
        
            
