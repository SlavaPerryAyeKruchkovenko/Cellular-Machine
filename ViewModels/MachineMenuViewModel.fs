namespace ViewModels

open ReactiveUI

type MachineMenuViewModel() =
    inherit ViewModelBase()
    let mutable resoulution = 0
    let mutable density = 0
    member __.StartText with get() = "Start"
    member __.FinishText with get() = "Finish"
    member __.Resoulution
        with get() = resoulution
        and set(value) = __.RaiseAndSetIfChanged(&resoulution, value) |> ignore
    member __.Density
        with get() = density
        and set(value) = __.RaiseAndSetIfChanged(&density, value) |> ignore

