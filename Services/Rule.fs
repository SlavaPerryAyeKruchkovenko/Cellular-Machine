namespace Services
open Avalonia
open Avalonia.Controls.Shapes
type Rule = 
    struct
        val mutable DieRule:int
        val mutable BornRule:int
    end

type Cell(loc,value,size) =
        member val Neighbors:int = value with get,set
        member val Location: Point = loc with get
        member val Size:Size = size with get
