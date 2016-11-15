// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#r @"bin\Debug\itextsharp.dll"
open System
open System.IO
open iTextSharp.text
open iTextSharp.text.pdf

// Hello, world.
let chap0101() =
    let document = new Document()
    PdfWriter.GetInstance(document, new FileStream(@"D:\TextSharpPlaypen\1\Chap0101.pdf", FileMode.Create)) |> ignore
    document.Open()
    document.Add(new Paragraph("Hello world")) |> ignore
    document.Close()

// Background colour.
let chap0102() =
    let pageSize = new Rectangle(144.0f, 720.0f)
    pageSize.BackgroundColor <- new BaseColor(0xFF, 0xFF, 0xDE)
    let document = new Document(pageSize)
    PdfWriter.GetInstance(document, new FileStream("D:\TextSharpPlaypen\2\Chap0102.pdf", FileMode.Create)) |> ignore
    document.Open()
    [ 0 .. 4 ] |> Seq.iter (fun i -> document.Add(new Paragraph("Hello World")) |> ignore)
    document.Close()

// Phrases.
let chap0103() =
    let document = new Document(PageSize.A4.Rotate())
    PdfWriter.GetInstance(document, new FileStream("D:\TextSharpPlaypen\3\Chap0103.pdf", FileMode.Create)) |> ignore
    document.Open()
    [ 0 .. 19 ] |> Seq.iter (fun _ ->  document.Add(new Phrase("Hello World, Hello Sun, Hello Moon, Hello Stars, Hello Sea, Hello Land, Hello People. ")) |> ignore)
    document.Close()

// Paragraphs and align justified.
let chap0104() =
    let document = new Document(PageSize.A5, 36.0f, 72.0f, 108.0f, 180.0f)
    PdfWriter.GetInstance(document, new FileStream("D:\TextSharpPlaypen\4\Chap0104.pdf", FileMode.Create)) |> ignore
    document.Open()
    let paragraph = new Paragraph()
    paragraph.Alignment <- Element.ALIGN_JUSTIFIED
    [ 0 .. 19 ] |> Seq.iter (fun _ -> paragraph.Add("Hello World, Hello Sun, Hello Moon, Hello Stars, Hello Sea, Hello Land, Hello People. ") |> ignore)
    document.Add(paragraph) |> ignore
    document.Close()

// Meta data + opening the document.
let chap0106() =
    let document = new Document()
    PdfWriter.GetInstance(document, new FileStream("D:\TextSharpPlaypen\6\Chap0106.pdf", FileMode.Create)) |> ignore
    document.AddTitle("Hello World example") |> ignore
    document.AddSubject("This example explains step 6 in Chapter 1") |> ignore
    document.AddKeywords("Metadata, iText, step 6, tutorial") |> ignore
    document.AddCreator("My program using iText#") |> ignore
    document.AddAuthor("Oliver Samson") |> ignore
    document.AddHeader("Expires", "0") |> ignore
    document.Open()
    document.Add(new Paragraph("Hello World")) |> ignore
    document.Close()

let readExistingInvoice() =
    let reader = new PdfReader("D:\TextSharpPlaypen\Invoices\Blank Invoice_i3D.pdf")
    let n = reader.NumberOfPages
    let psize = reader.GetPageSize(1)
    let width = psize.Width
    let height = psize.Height
    let document = new Document(psize)
    let writer = PdfWriter.GetInstance(document, new FileStream("D:\TextSharpPlaypen\Invoices\i3D_Test.pdf", FileMode.Create))
    document.Open()
    let cb = writer.DirectContent
    let page = writer.GetImportedPage(reader, 1)
    cb.AddTemplate(page, 0.0, 0.0)
    let bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)

    cb.BeginText()
    cb.SetFontAndSize(bf, 10.0f)
    cb.MoveText(405.0f, 612.5f)
    cb.ShowText("Hello world")
    cb.EndText()

    cb.BeginText()
    cb.SetFontAndSize(bf, 10.0f)
    cb.MoveText(405.0f, 597.0f)
    cb.ShowText("Hello world")
    cb.EndText()

    cb.BeginText()
    cb.SetFontAndSize(bf, 10.0f)
    cb.MoveText(405.0f, 567.0f)
    cb.ShowText("Hello world")
    cb.EndText()

    cb.BeginText()
    cb.SetFontAndSize(bf, 10.0f)
    cb.MoveText(405.0f, 538.0f)
    cb.ShowText("Hello world")
    cb.EndText()

    cb.BeginText()
    cb.SetFontAndSize(bf, 12.0f)
    cb.MoveText(320.0f, 457.0f)
    cb.ShowText("Hello world")
    cb.EndText()

    [ 0 .. 4 ]
    |> Seq.iter
        (fun i ->
            cb.BeginText()
            cb.SetFontAndSize(bf, 10.0f)
            cb.MoveText(36.0f, 358.0f - (15.0f * (float32 i)))
            cb.ShowText("Hello world")
            cb.EndText()

            cb.BeginText()
            cb.SetFontAndSize(bf, 10.0f)
            cb.MoveText(405.0f, 358.0f - (15.0f * (float32 i)))
            cb.ShowText("Hello world")
            cb.EndText())

    cb.BeginText()
    cb.SetFontAndSize(bf, 12.0f)
    cb.MoveText(405.0f, 153.0f)
    cb.ShowText("Hello world")
    cb.EndText()

    document.Close()

readExistingInvoice()



//    Rectangle psize = reader.GetPageSize(1);
//    float width = psize.Width;
//    float height = psize.Height;
//    Document document = new Document(psize, 50, 50, 50, 50);
//    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("Chap0112.pdf", FileMode.Create));
//    Watermark watermark = new Watermark(Image.GetInstance("watermark.jpg"), 200, 320);
//    document.Add(watermark);
//    document.Open();
//    PdfContentByte cb = writer.DirectContent;
//    int i = 0;
//    int p = 0;
//    Console.WriteLine("There are " + n + " pages in the document.");
//    while (i < n) 
//    {
//        document.NewPage();
//        p++;
//        i++;
//        PdfImportedPage page1 = writer.GetImportedPage(reader, i);
//        cb.AddTemplate(page1, .5f, 0, 0, .5f, 0, height / 2);
//        Console.WriteLine("processed page " + i);
//        if (i < n) 
//        {
//            i++;
//            PdfImportedPage page2 = writer.GetImportedPage(reader, i);
//            cb.AddTemplate(page2, .5f, 0, 0, .5f, width / 2, height / 2);
//            Console.WriteLine("processed page " + i);
//        }
//        if (i < n) 
//        {
//            i++;
//            PdfImportedPage page3 = writer.GetImportedPage(reader, i);
//            cb.AddTemplate(page3, .5f, 0, 0, .5f, 0, 0);
//            Console.WriteLine("processed page " + i);
//        }
//        if (i < n) 
//        {
//            i++;
//            PdfImportedPage page4 = writer.GetImportedPage(reader, i);
//            cb.AddTemplate(page4, .5f, 0, 0, .5f, width / 2, 0);
//            Console.WriteLine("processed page " + i);
//        }
//        cb.SetRGBColorStroke(255, 0, 0);
//        cb.MoveTo(0, height / 2);
//        cb.LineTo(width, height / 2);
//        cb.Stroke();
//        cb.MoveTo(width / 2, height);
//        cb.LineTo(width / 2, 0);
//        cb.Stroke();
//        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
//        cb.BeginText();
//        cb.SetFontAndSize(bf, 14);
//        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "page " + p + " of " + ((n / 4) + (n % 4 > 0? 1 : 0)), width / 2, 40, 0);
//        cb.EndText();
//    }
//    document.Close();

