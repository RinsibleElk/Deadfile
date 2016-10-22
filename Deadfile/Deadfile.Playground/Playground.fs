namespace Deadfile.Playground

[<RequireQualifiedAccess>]
module FakeData =
    let someMethod() =
        [
            0 .. 19
        ]
        |> List.fold (+) 0

