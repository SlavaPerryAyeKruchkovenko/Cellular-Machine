﻿namespace ViewModels

open Avalonia
open Avalonia.Controls.Shapes
open System.Collections.ObjectModel
open Avalonia.Media
open Models
open System.Linq
open System
open System.Threading.Tasks
open System.Collections.Generic
open Avalonia.Controls

type MachineCanvasViewModel() =
    inherit ViewModelBase();

    let mutable holst:Cell [][] = [|[||]|]
    let mutable list: ObservableCollection<Cell> = new ObservableCollection<Cell>()
    member val Holst:Canvas = new Canvas() with get,set
    member __.Cells with get() = list
    member this.Finish() = this.Cells.Clear()

    member this.AddChild(cell:Cell)  = 
        if  list.Contains cell then
            list.Remove(cell) |> ignore
        list.Add(cell)

    member this.GenerateField(size:float) =
        let clear = [|for i = 0.0 to Math.Ceiling this.Holst.Bounds.Height/size - 1.0 do
                        [|for j = 0.0 to Math.Ceiling this.Holst.Bounds.Width/size - 1.0 do
                            new Cell(new Point(i,j),0,new Size(size,size),false)|]|] 
        for cell in list do
            clear.[int (cell.Location.X/size)].[int (cell.Location.Y/size)] <- cell
        holst <- clear

    member this.DeleteChildAboutLoc(cell:Cell) =

        let mutable curCell:Option<Cell> = None
        for cell1 in list do 
            if cell.Equals(cell1) then
                curCell <- Some cell1;
        if curCell.IsSome then
            list.Remove(curCell.Value) |> ignore

    member this.ActivateMachine(cellSize:int,rules:Rule)=
        let checkNum(point:Point) =  (point.X >= 0.0 && point.Y >= 0.0) 
        let checkArray(point:Point,x,y)=     
            let a = holst.[0].Length
            let b = checkNum(point) && holst.Length > x && holst.[0].Length > y
            b
        let getCell(point:Point, size) =         
            if checkNum point then
                new Cell(new Point(point.X,point.Y),1,new Size(size,size),true)
            else
                new Cell(new Point(),0,new Size(),false)
        let checkNeighbors(point:Point,size) =
            let x = int (point.X/size)
            let y = int (point.Y/size)
            if checkArray(point,x,y) then
                if holst.[x].[y].IsAlive then 1 else 0
            else
                0
        let checkCell(point:Point,size) =
                checkNeighbors(new Point(point.X - size,point.Y - size),size) +
                checkNeighbors(new Point(point.X,point.Y - size),size) +
                checkNeighbors(new Point(point.X + size,point.Y - size),size) +
                checkNeighbors(new Point(point.X - size,point.Y),size) +
                checkNeighbors(new Point(point.X + size,point.Y),size) +
                checkNeighbors(new Point(point.X - size,point.Y + size),size) +
                checkNeighbors(new Point(point.X,point.Y + size),size) +
                checkNeighbors(new Point(point.X + size,point.Y + size),size)
            
        let size = float cellSize
        let neighbors = [for cell in list do 
                            let point = cell.Location
                            getCell(new Point(point.X - size,point.Y - size),size)
                            getCell(new Point(point.X,point.Y - size),size)
                            getCell(new Point(point.X + size,point.Y - size),size)
                            getCell(new Point(point.X - size,point.Y),size)
                            getCell(new Point(point.X + size,point.Y),size)
                            getCell(new Point(point.X - size,point.Y + size),size)
                            getCell(new Point(point.X,point.Y + size),size)
                            getCell(new Point(point.X + size,point.Y + size),size)] |> List.distinct
        let aliveCell = [for cell in neighbors do
                            let num = checkCell(cell.Location,size)
                            if rules.BornRule.Contains num then
                                cell]       
        aliveCell |> List.map(fun x-> let x1 = int x.Location.X/cellSize 
                                      let y = int x.Location.Y/cellSize
                                      if checkArray(x.Location,x1,y) then
                                        holst.[int x.Location.X/cellSize].[int x.Location.Y/cellSize] <- x
                                        list.Add(x)) |> ignore
        let deathCell = list.Where(fun x-> not (rules.AliveRule.Contains(checkCell(x.Location,size)))).ToList()
        for cell in deathCell do
            let point = cell.Location
            let x = int (point.X/size)
            let y = int (point.Y/size)
            holst.[x].[y] <- new Cell(new Point(float x,float y),0,new Size(size,size),false)  
            list.Remove(cell)|> ignore
        ()
        
        

        
                      
        
        
        //list <- downcast (list |> Seq.distinctBy(fun x -> x.Bounds))        