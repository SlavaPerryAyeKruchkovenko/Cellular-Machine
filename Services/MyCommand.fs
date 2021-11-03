namespace Services

open Avalonia.Input
open Avalonia.Controls
open Avalonia
open System.Windows.Input
open System

type ChangeValueCommand(finish:Func<unit>,clear:Func<unit>)=
    let mutable oldValue = 0.0
    interface ICommand with      
        member this.CanExecute(parameter: obj): bool = 
            true
        [<CLIEvent>]
        member this.CanExecuteChanged: IEvent<EventHandler,EventArgs> = 
            raise (System.NotImplementedException())
        member this.Execute(parameter: obj): unit = 
            let e:PointerEventArgs = downcast parameter
            let slider:Slider = downcast e.Source
            if slider.Value <> oldValue then
                finish.Invoke()
                clear.Invoke()
                oldValue <- slider.Value
    

