namespace Services
open Avalonia.Input
type MyParams =
    struct
        val Event:PointerPressedEventArgs
        val Target:obj
        new(event,target) = {Event= event;Target = target}
    end

