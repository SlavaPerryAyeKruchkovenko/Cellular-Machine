namespace Models
open Avalonia
open Avalonia.Controls.Shapes
type Rule = 
    struct
        val AliveRule:int list
        val BornRule:int list
        new(alive,born) = {AliveRule= alive;BornRule = born}
    end

type Cell(loc,value,size,alive) =
        member val Neighbors:int = value with get,set
        member val Location: Point = loc with get
        member val Size:Size = size with get
        member val IsAlive:bool =alive with get,set
        member val MarginLoc:Thickness = new Thickness(loc.X,loc.Y) with get
        member this.Alive() = 
            this.IsAlive <- true;
        override this.Equals(cell) =
            match cell with 
            | :? Cell as c -> c.Location = this.Location
            | _ -> false
        override this.GetHashCode() =
            (int32)(this.Location.X + this.Location.Y) + this.Neighbors