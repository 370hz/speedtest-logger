module Speedtest.Logger.Speedtester

open System

[<Measure>] type sec
[<Measure>] type Mb
[<Measure>] type Mbps = Mb/sec

type Url = Url of string

type Speedtest = {
    Id: Guid
    Download: float<Mbps>
    Timestamp: DateTime
}

let downloadUrls (bytes : Url -> byte[]) (urls : Url list) = 
    let bits =
        urls
        |> List.map (bytes)
        |> List.map (fun r -> r.Length)
        |> List.fold (+) 0
    
    ((float bits * 8.0) / 10.0 ** 6.0) * 1.0<Mb>

let speedtest (time : (_ -> float<Mb>) -> float<sec> * float<Mb>) (bytes : Url -> byte[]) (urls : Url list) =
    let s, mb = time (fun _ -> urls |> downloadUrls bytes)
    {
        Id = Guid.NewGuid()
        Download = mb / s
        Timestamp = System.DateTime.Now.ToUniversalTime()
    }