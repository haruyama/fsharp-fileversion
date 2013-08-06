open System.IO
open System.Runtime.Serialization.Json
open System.Text

let haneldWithDataContractJsonSerializer obj =
    let serializer = new DataContractJsonSerializer(obj.GetType());
    use ms = new MemoryStream()
    serializer.WriteObject(ms, obj)
    Encoding.UTF8.GetString(ms.ToArray())

type FileInfo = {FileName: string; FileVersion: string; ExceptionOccured: bool; ExceptionMessage: string}

let fileversion filename =
    try
        let info = System.Diagnostics.FileVersionInfo.GetVersionInfo(filename)
        {FileName = info.FileName; FileVersion = info.FileVersion; ExceptionOccured= false; ExceptionMessage = ""}
    with
        | ex -> {FileName= filename; FileVersion= ""; ExceptionOccured= true; ExceptionMessage= ex.ToString()}

[<EntryPoint>]
let main args =
    printfn "%s" <| haneldWithDataContractJsonSerializer([| for a in args -> fileversion(a) |])
    0
