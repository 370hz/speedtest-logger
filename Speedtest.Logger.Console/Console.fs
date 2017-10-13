open System
open System.Text
open System.Net.Http
open Newtonsoft.Json

open Speedtest.Logger.Speedtester
open Speedtest.Logger.Console.Configuration

let getByteArray (client : HttpClient) (url : Url) : byte[] =
    match url with
    | Url s ->
        client.GetByteArrayAsync(s)
        |> Async.AwaitTask
        |> Async.RunSynchronously

let postSpeedtest (client : HttpClient) (url : string) (speed : Speedtest) =
    let json = JsonConvert.SerializeObject(speed);
    let content = new StringContent (json)
    content.Headers.ContentType <- Headers.MediaTypeHeaderValue "application/json"
    client.PostAsync (url, content) |> ignore
    speed

let time f =
    let timer = Diagnostics.Stopwatch()
    timer.Start()
    let r = f ()
    timer.Stop()
    let dt = ((float timer.ElapsedMilliseconds) / 1000.0) * 1.0<sec>
    (dt, r)

[<EntryPoint>]
let main argv =
    let conf = config
    let client = new HttpClient()
    let bytes = getByteArray client

    speedtest time bytes conf.Sites
    |> postSpeedtest client conf.ApiUrl
    |> fun st ->
        printfn "ID: %s, Timestamp: %s, Download speed: %f Mbps"
            (string st.Id)
            (string st.Timestamp)
            st.Download

    0
