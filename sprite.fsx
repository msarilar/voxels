(**
# Sprite Conversion
Converts a vertical spritesheet into an horizontal spritesheet while applying a zoom factor.

Specify the `source` (path to vertical spritesheet), the `destination` (where the horizontal
spritesheet will be saved) as well as the number of `elements` in the spritesheet and the
`zoomFactor` (make it `1` to not change anything).
*)

open System.Drawing

let source = @"..\verticalSpritesheet.png"
let destination = @"..\horizontalSpritesheet.png"

let elements = 16
let zoomFactor = float 0.7

let getHorizontalImage source elements zoomFactor =

    let verticalImage = Image.FromFile(source)
    
    let height = verticalImage.Size.Height
    let width = verticalImage.Size.Width
    
    let individualHeight = height / elements
    
    let newWidth =
        (float width) * zoomFactor
        |> int
    let newHeight =
        (float individualHeight) * zoomFactor
        |> int

    let horizontalImage = new Bitmap(newWidth * 16, newWidth)
    let baseRectangle = new Rectangle(0, 0, width, individualHeight)
    use graphics = Graphics.FromImage(horizontalImage)
    
    Enumerable.Range(0, elements)
    |> Seq.map (fun _ -> new Bitmap(width, individualHeight))
    |> Seq.mapi (fun index image ->
        let position = index * individualHeight
        use subGraphics = Graphics.FromImage(image)
        subGraphics.DrawImage(verticalImage,
            baseRectangle,
            new Rectangle(0, position, width, individualHeight),
            GraphicsUnit.Pixel)
        (index, image))
    |> Seq.fold (fun horizontalImage (index, currentImage) ->
        let position = index * newWidth
        graphics.DrawImage(currentImage,
            new Rectangle(position, 0, newWidth, newHeight),
            baseRectangle,
            GraphicsUnit.Pixel)
        horizontalImage) horizontalImage

let horizontalImage = getHorizontalImage source elements zoomFactor

horizontalImage.Save(destination)
|> ignore