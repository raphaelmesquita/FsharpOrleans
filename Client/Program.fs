open System
open Microsoft.Extensions.Logging
open FSharp.Control.Tasks
open Orleans
open Orleans.Runtime.Configuration
open Orleans.Hosting
open FSharpGrains.Interfaces
open System.Threading.Tasks

let buildClient () =
      let config = ClientConfiguration.LocalhostSilo()
      ClientBuilder()
        .UseConfiguration(config)
        .ConfigureApplicationParts(fun parts -> parts.AddApplicationPart((typeof<IHello>).Assembly).WithCodeGeneration() |> ignore )
        .ConfigureLogging(fun logging -> logging.AddConsole() |> ignore)
        .Build()

let worker (client : IClusterClient) =
    task {
        let friend = client.GetGrain<IHello> 0L
        let! response = friend.SayHello ("Good morning, my friend!")
        printfn "%s" response
    }

[<EntryPoint>]
let main _ =
    let t = task {
        do! Task.Delay(5000)
        use client = buildClient()
        do! client.Connect()
        printfn "Client successfully connect to silo host"
        do! worker client
    }

    t.Wait()

    Console.ReadKey() |> ignore

    0
