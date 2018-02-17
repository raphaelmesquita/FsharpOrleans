﻿namespace FSharpGrains

module Grains =

    open System.Threading.Tasks
    open Orleans
    open FSharpGrains.Interfaces

    type HelloGrain () =
        inherit Grain ()
        interface IHello with 
            member this.SayHello (greeting : string) : Task<string> = 
                greeting |> sprintf "You said: %s, I say: Hello!" |> Task.FromResult