namespace Services
open Avalonia
type Rule = 
    struct
        val mutable DieRule:int
        val mutable BornRule:int
    end

type Cell(loc,value) =
    class
        member val Neighbors:int = value with get,set
        member val Location: Point = loc with get,set
    end
