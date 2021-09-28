namespace ViewModels

type MainWindowViewModel() =
    inherit ViewModelBase()
    let menu = new MachineMenuViewModel()
    member __.Menu with get() = menu
