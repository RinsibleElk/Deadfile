namespace Deadfile.DataImporter

module Entry =
    [<EntryPoint>]
    let main argv =
        let importer = new Importer(argv)
        importer.PerformImport()
