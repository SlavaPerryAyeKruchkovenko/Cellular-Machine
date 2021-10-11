namespace ViewModels

open Avalonia
open Avalonia.Controls.Shapes
open System.Collections.ObjectModel
open Avalonia.Media
open Services
open System.Linq
open System

type MachineCanvasViewModel() =
    inherit ViewModelBase();
    
    let mutable list = new ObservableCollection<Cell>()
    member __.Cells with get() = list
    member this.Finish() = this.Cells.Clear()
    member this.AddChild(thick:Thickness,size)  = 

        let cell = new Cell(new Point(thick.Right,thick.Top),0,new Size(size,size))
        if not (list.Contains cell) then
            list.Add(cell);

    member this.DeleteChild(cell:Cell)=

        let size = cell.Location
        let mutable curCell:Option<Cell> = None
        for cell in list do 
            if Math.Round cell.Location.X = size.X && Math.Round cell.Location.Y = size.Y then
                curCell <- Some cell;
        if curCell.IsSome then
            list.Remove(curCell.Value)|> ignore

    member this.ActivateMachine(cellSize:int,rules:Rule)=  
        let rec nextStepMachine(index,size) =

            let union(list1: Cell seq,list2:Cell seq) =               
                for cell in list1 do
                    for cell2 in list2 do
                        if cell2.Location = cell.Location then
                            cell.Neighbors <- cell2.Neighbors + cell.Neighbors
                let list = list1.Concat list2
                let lst = downcast list
                lst |> Seq.distinctBy(fun x -> x.Location) |> Seq.cache

            let getLine(point:Point, size, needCell) = 
                let checkNum(point:Point) =  (point.X >= 0.0 && point.Y >= 0.0)
                let result = seq{
                                let cell1 = new Cell(new Point(point.X,point.Y-size),1,new Size(size,size))
                                if(checkNum(cell1.Location)) then
                                    yield cell1
                                let cell2 = new Cell(new Point(point.X,point.Y),1,new Size(size,size))
                                if(checkNum(cell2.Location) && needCell) then
                                    yield cell2
                                let cell3 = new Cell(new Point(point.X,point.Y+size),1,new Size(size,size))
                                if(checkNum(cell3.Location)) then
                                    yield cell3
                                }
                result |> Seq.cache

            if not (index = size) then
                let cell = this.Cells.[index]   
                let point = cell.Location
                let cellList1 = getLine(new Point(point.X-(float)cellSize,point.Y),(float)cellSize,true)
                let cellList = (cellList1.Concat(getLine(point,(float)cellSize,false))).Concat(getLine(new Point(point.X+(float)cellSize,point.Y),(float)cellSize,true))
                
                union(cellList,nextStepMachine(index+1,size))
            else
                upcast []
        let cells = nextStepMachine(0,this.Cells.Count)
        for cell in cells.Where(fun x -> x.Neighbors <> 2 && x.Neighbors <> 3) do
            this.DeleteChild(cell)
        for cell in cells.Where(fun x-> x.Neighbors = 3) do
            let thick = new Thickness(cell.Location.X,cell.Location.Y)
            this.AddChild(thick,(float)cellSize)
        //list <- downcast (list |> Seq.distinctBy(fun x -> x.Bounds))        