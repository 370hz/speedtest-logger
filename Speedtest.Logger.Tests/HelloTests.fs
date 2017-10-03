module Speedtest.Logger.HelloTests

open Expecto

[<Tests>]
let tests =
    testList "Hello tests" [
        testCase "should say hello something" <| fun _ ->
            Expect.equal "Hello World!" "Hello World!" "should say hello"
    ]