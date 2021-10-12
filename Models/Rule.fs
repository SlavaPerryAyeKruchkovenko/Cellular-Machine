namespace Models
open Avalonia
open Avalonia.Controls.Shapes
type Rule = 
    struct
        val AliveRule:int list
        val BornRule:int list
        new(alive,born) = {AliveRule= alive;BornRule = born}
    end

type Cell(loc,value,size) =
        member val Neighbors:int = value with get,set
        member val Location: Point = loc with get
        member val Size:Size = size with get
        member val MarginLoc:Thickness = new Thickness(loc.X,loc.Y) with get