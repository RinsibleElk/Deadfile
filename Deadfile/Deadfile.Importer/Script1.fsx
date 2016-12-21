open System.IO

let f = @"D:\Documents\Blah.json"
f
|> File.ReadAllLines
|> Seq.filter (fun line -> line.Contains("FullNameWithTitle") |> not)
|> fun s -> File.WriteAllLines(f, s)
