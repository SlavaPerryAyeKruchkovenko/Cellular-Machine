namespace Models

open Avalonia.Controls.Shapes
open Avalonia

type Cell(size)=
    let size = size
    let box = 
        let rect = new Rectangle()
        rect.Width <- size
        rect.Height <- size
        rect.Tapped.Add(fun evArg -> rect.SetValue(null,null))
        rect
    member __.Square = box
        
    
