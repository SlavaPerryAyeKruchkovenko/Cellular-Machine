﻿namespace ViewModels

open Avalonia
open Avalonia.Controls.Shapes
open System.Collections.ObjectModel
open Avalonia.Media
open Services
open System.Linq
open System

type MachineCanvasViewModel() =
    inherit ViewModelBase();
    
    let list = new ObservableCollection<Rectangle>()
    member __.Cells with get() = list
    member this.Finish() = this.Cells.Clear()
    member this.AddChild(thick:Thickness,size)  = 
        let cell = 
            let rect = new Rectangle()
            rect.Margin <- thick
            rect.Width <- size
            rect.Height <- size
            rect.Fill <- Brushes.Yellow
            rect.Tapped.Add(fun evArg -> this.Cells.Remove(rect)|> ignore)
            rect
        list.Add(cell);
    member this.ActivateMachine(cellSize:int,rules:Rule)=  
        let rec nextStepMachine(index,size,rules:Rule) =

            let union(list1: Cell seq,list2:Cell seq,rules:Rule) = 
                
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
                                let cell1 = new Cell(new Point(point.X,point.Y-size),1)
                                if(checkNum(cell1.Location)) then
                                    yield cell1
                                let cell2 = new Cell(new Point(point.X,point.Y),1)
                                if(checkNum(cell2.Location) && needCell) then
                                    yield cell2
                                let cell3 = new Cell(new Point(point.X,point.Y+size),1)
                                if(checkNum(cell3.Location)) then
                                    yield cell3
                                }
                result |> Seq.cache

            if not (index = size) then
                let cell = this.Cells.[index]   
                let point = new Point(Math.Round cell.Bounds.X,Math.Round cell.Bounds.Y)
                let cellList1 = getLine(new Point(point.X-(float)cellSize,point.Y),(float)cellSize,true)
                let cellList = (cellList1.Concat(getLine(point,(float)cellSize,false))).Concat(getLine(new Point(point.X+(float)cellSize,point.Y),(float)cellSize,true))
                
                union(cellList,nextStepMachine(index+1,size,rules),rules)
            else
                upcast []
        let cells = nextStepMachine(0,this.Cells.Count,rules)
        for cell in cells.Where(fun x-> x.Neighbors = rules.BornRule) do
            let thick = new Thickness(cell.Location.X,cell.Location.Y)
            this.AddChild(thick,(float)cellSize)