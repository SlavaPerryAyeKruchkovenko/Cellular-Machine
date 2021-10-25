namespace ViewModels

open Avalonia
open Avalonia.Controls.Shapes
open System.Collections.ObjectModel
open Avalonia.Media
open Models
open System.Linq
open System
open System.Threading.Tasks
open System.Collections.Generic

type MachineCanvasViewModel() =
    inherit ViewModelBase();
    
    let mutable list: ObservableCollection<Cell> = new ObservableCollection<Cell>()
    member __.Cells with get() = list;
    member this.Finish() = this.Cells.Clear()
    member this.AddChild(cell:Cell)  = 
        if  list.Contains cell then
            list.Remove(cell) |> ignore
        list.Add(cell)
    
    member this.DeleteChildAboutLoc(cell:Cell)=

        let size = cell.Location
        let mutable curCell:Option<Cell> = None
        for cell1 in list do 
            if Math.Round cell1.Location.X = size.X && Math.Round cell1.Location.Y = size.Y then
                curCell <- Some cell1;
        if curCell.IsSome then
            list.Remove(curCell.Value) |> ignore

    member this.ActivateMachine(cellSize:int,rules:Rule)=
        
        let nextStepMachine(cells:IEnumerable<Cell>,cellSize):Cell list =
            let getCell(point:Point, size) = 
                let checkNum(point:Point) =  (point.X >= 0.0 && point.Y >= 0.0) 
                if(checkNum(point)) then
                    new Cell(new Point(point.X,point.Y),1,new Size(size,size),false)
                else
                    new Cell(new Point(),0,new Size(),false)
            let getAllCells(size,cellSize) = [for i = 0 to size-1 do
                                                let cell = this.Cells.[i]   
                                                let point = cell.Location
                                                getCell(new Point(point.X - cellSize,point.Y - cellSize),cellSize)
                                                getCell(new Point(point.X,point.Y - cellSize),cellSize)
                                                getCell(new Point(point.X + cellSize,point.Y - cellSize),cellSize)
                                                getCell(new Point(point.X - cellSize,point.Y),cellSize)
                                                getCell(new Point(point.X + cellSize,point.Y),cellSize)
                                                getCell(new Point(point.X - cellSize,point.Y + cellSize),cellSize)
                                                getCell(new Point(point.X,point.Y + cellSize),cellSize)
                                                getCell(new Point(point.X + cellSize,point.Y + cellSize),cellSize)
                                                ]
                                                        
            let cells = getAllCells(cells.Count(),cellSize)
            let mutable cellsCopy = cells.ToList()
            for cell in cellsCopy do 
                let size = (cellsCopy.Where(fun x -> x.Equals(cell))).Count()
                cells.First(fun x-> x.Equals(cell)).Neighbors <- size
            cells |> List.distinct    
        let cells = nextStepMachine(list,(float)cellSize)
        this.Cells.Clear()
        for cell in cells.Where(fun x -> not (rules.AliveRule.Contains x.Neighbors) ) do
            this.Cells.Remove(cell)|> ignore
        let birthCells = new ObservableCollection<Cell>(cells |> List.filter(fun x-> rules.BornRule.Contains x.Neighbors))
        for cell in birthCells do
            this.AddChild(cell)
        

        
                      
        
        
        //list <- downcast (list |> Seq.distinctBy(fun x -> x.Bounds))        