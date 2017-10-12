open System
open System.Text
open System.Net.Http
open Speedtest.Logger
open Newtonsoft.Json

let downloadUrl (client : HttpClient) (url : string) : int =
    client.GetByteArrayAsync(url)
    |> Async.AwaitTask
    |> Async.RunSynchronously
    |> (fun r -> r.Length)

let downloadUrls (http : HttpClient) (urls : string list) = 
    urls
    |> List.map (downloadUrl http)
    |> List.fold (+) 0

let sites = [
    "https://computas.com/globalassets/ansattbilder/atle-gram-ag.jpg";
    "https://computas.com/globalassets/ansattbilder/lars-erik-evensen-lee.jpg";
    "https://computas.com/globalassets/ansattbilder/lars-frode-haugen-lfh.jpg";
    "https://computas.com/globalassets/ansattbilder/thomas-bech-pettersen-tp.jpg";
    "https://computas.com/globalassets/ansattbilder/kim-petersen-kvp.jpg";
    "https://computas.com/globalassets/ansattbilder/rune-hagbartsen-rh.jpg";
    "https://computas.com/globalassets/ansattbilder/trond-eilertsen-te.jpg";
    "https://computas.com/globalassets/ansattbilder/lak-.png";
    "https://computas.com/globalassets/ansattbilder/christine-langbrathen-cla.jpg";
    "https://computas.com/globalassets/ansattbilder/johanne-lovsland.png";
]

type Speedtest = {
    Id: Guid
    Download: double
    Timestamp: int
}

let postAsync (client : HttpClient) (speed : Speedtest)=
    let json = JsonConvert.SerializeObject(speed);
    let content = new StringContent (json)
    content.Headers.ContentType <- Headers.MediaTypeHeaderValue "application/json"
    client.PostAsync ("http://localhost:5000/speedtests", content)


// TODO, dette var gøy
[<Measure>] type ms

[<EntryPoint>]
let main argv =
    let hc = new HttpClient()
    
    let timer = Diagnostics.Stopwatch()
    timer.Start()

    let noOfByte =
        sites
        |> downloadUrls hc
    
    let t = timer.ElapsedMilliseconds 
    
    let speed = (double noOfByte / double t) * 0.008

    let s = {
        Id = Guid.NewGuid()
        Download = speed
        Timestamp = 1234
    }

    postAsync hc s |> ignore

    printfn "Download speed %f Mbps" speed

    0
