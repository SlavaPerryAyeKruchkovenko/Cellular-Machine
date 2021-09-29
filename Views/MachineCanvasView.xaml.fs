namespace Views

open Avalonia
open Avalonia.Controls
open Avalonia.Markup.Xaml

type MachineCanvasView () as self = 
    inherit UserControl ()

    do AvaloniaXamlLoader.Load self
