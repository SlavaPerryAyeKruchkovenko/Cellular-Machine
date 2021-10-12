namespace ViewModels

open ReactiveUI

type MachineMenuViewModel() =
    inherit ViewModelBase()
    let mutable resoulution = [3]
    let mutable density = [2;3]
    let mutable value = 10;
    member __.StepText with get() = "Next Steep"
    member __.FinishText with get() = "Finish"
    member __.StartText with get() = "Start"
    member __.CloseText with get() = "Close"
    member __.Resoulution
        with get() = resoulution
        and set(value) = __.RaiseAndSetIfChanged(&resoulution, value) |> ignore
    member __.Density
        with get() = density
        and set(value) = __.RaiseAndSetIfChanged(&density, value) |> ignore
    member __.Value
        with get() = value
        and set(value2) = __.RaiseAndSetIfChanged(&value, value2) |> ignore


