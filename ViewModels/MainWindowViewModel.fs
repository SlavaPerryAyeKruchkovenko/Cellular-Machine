namespace ViewModels

open Models
open System.Threading
open System.Threading.Tasks
open System.Linq
open System.Collections.Generic
open Avalonia.Input
open System.Windows.Input
open Services
open Avalonia.Controls
open Avalonia
open System

type MyCommand(holst:MachineCanvasViewModel,menu:MachineMenuViewModel) = 
    interface ICommand with        
        member this.CanExecute(parameter: obj): bool = 
            true
        [<CLIEvent>]
        member this.CanExecuteChanged: IEvent<System.EventHandler,System.EventArgs> = 
            raise (System.NotImplementedException())
        member this.Execute(parameter: obj): unit = 
                let CorrectPos(num : float,size:float) = 
                    let x = num % size
                    if (x >= size/2.0) then
                        num + size-x
                    else
                        num - x 
                let value = menu.Value    
                let size = new Size(float value,float value)
                let e:PointerPressedEventArgs=  downcast parameter
                if e.Source :? Canvas then
                    let canvas:Canvas = downcast e.Source
                    let position = e.GetCurrentPoint(canvas).Position      
                    let point = new Point(CorrectPos(position.X,size.Width),CorrectPos(position.Y,size.Width))
                    let cell = new Cell(point,0,size,true)   
                    holst.AddChild cell
                    holst.GenerateField(size.Width,canvas)  
type MainWindowViewModel()=
    inherit ViewModelBase()
    let menu = new MachineMenuViewModel()   
    let holst = new MachineCanvasViewModel()  
    let command = new MyCommand(holst,menu)
    let mutable token = new CancellationTokenSource()  
    let mutable canClick = true
    member __.ChangeValueCommand with get() = new ChangeValueCommand(Func<unit>__.CloseGame,Func<unit>holst.Finish)
    member __.MyCommand with get() = command
    member __.Menu with get() = menu
    member __.Holst with get() = holst
    member this.NextEtirationGame() = 
        let rule = new Rule(menu.Density,menu.Resoulution)
        let size = menu.Value
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
    member this.AddRectangle(object:obj,e:PointerPressedEventArgs) = 
        if canClick then
            let CorrectPos(num : float,size:float) = 
                let x = num % size
                if (x >= size/2.0) then
                    num + size-x
                else
                    num - x 
                
            let size = new Size(float menu.Value,float menu.Value)
            let canvas:Canvas = downcast object
            let position = e.GetCurrentPoint(canvas).Position 
            let point = new Point(CorrectPos(position.X,size.Width),CorrectPos(position.Y,size.Width))
            let cell = new Cell(point,0,size,true)
            holst.AddChild(cell) 
        else
            canClick <- true 
            
