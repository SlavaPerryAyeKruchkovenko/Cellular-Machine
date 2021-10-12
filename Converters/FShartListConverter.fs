namespace Converters

open Avalonia.Data.Converters
open System.Globalization
open System.Linq
open System

type FShartListConverter() =
    interface IValueConverter with
        member this.Convert(value: obj, targetType: System.Type, parameter: obj, culture: CultureInfo): obj =          
            let list: int list = downcast value
            let mutable str = ""
            for y in 0..list.Count() do 
                if(y = 0) then
                    str <- str + list.[y].ToString()
                else
                    str <- str + "," + list.[y].ToString()
            upcast str            

        member this.ConvertBack(value: obj, targetType: System.Type, parameter: obj, culture: CultureInfo): obj =
            let str:string = downcast value
            let answer= try
                            let strArr = str.Split(",").Select(Int32.Parse).ToArray()
                            let answ = strArr|> Array.toList
                            answ
                        with
                        | _ -> [0]
            upcast answer   

