module Speedtest.Logger.SpeedtesterTests

open Expecto
open Speedtest.Logger.Speedtester

let oneMb (url : Url) = Array.init 125000 (fun i -> byte(0))
let oneSec (f : _ -> 'a) : float<sec> * 'a = (1.0<sec>, f ())

[<Tests>]
let tests =
    testList "Speedtester speedtest" [
        testCase "should calculate the download speed to be 1.0 Mbps when downloading 1 sites, each beeing 1 Mb (125000 byte), taking a total of 1 sec" <| fun _ ->
            let twoSites = [Url "http://test.no"]
            let st = speedtest oneSec oneMb twoSites

            Expect.equal st.Download 1.0<Mbps> "download speed should be 1 Mbps"

        testCase "should calculate the download speed to be 2.0 Mbps when downloading 2 sites, each beeing 1 Mb (125000 byte), taking a total of 1 sec" <| fun _ ->
            let twoSites = [Url "http://test.no"; Url "http://test.no"]
            let st = speedtest oneSec oneMb twoSites

            Expect.equal st.Download 2.0<Mbps> "download speed should be 2 Mbps"
    ]