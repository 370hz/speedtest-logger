open System
open System.Text
open System.Net.Http
open Speedtest.Logger

let client = new HttpClient()

let rec download (urls:string list) = async {
    match urls with
    | head :: tail ->
        let! response =
            client.GetByteArrayAsync(head)
            |> Async.AwaitTask

        return response.Length + (download tail |> Async.RunSynchronously)
    | [] -> return 0
}

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

[<EntryPoint>]
let main argv =
    let timer = Diagnostics.Stopwatch()
    timer.Start()

    let noOfByte = sites |> download |> Async.RunSynchronously
    
    let t = timer.ElapsedMilliseconds 
    
    // System.Threading.Thread.Sleep(20)
    let speed = (double noOfByte / double t) * 0.008
    printfn "Download speed %f Mbps" speed

    0
