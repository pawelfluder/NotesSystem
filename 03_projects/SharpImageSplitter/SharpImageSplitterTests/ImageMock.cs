namespace SharpImageSplitterTests;

// public class ImageMock : Image
// {
//     public ImageMock(
//         Configuration configuration,
//         PixelTypeInfo pixelType,
//         ImageMetadata metadata,
//         Size size)
//         : base(configuration, pixelType, metadata, size)
//     {
//     }
//
//     public override Image<TPixel2> CloneAs<TPixel2>(Configuration configuration)
//     {
//         throw new NotImplementedException();
//     }
//
//     protected override void Dispose(bool disposing)
//     {
//         throw new NotImplementedException();
//     }
//
//     protected override ImageFrameCollection NonGenericFrameCollection { get; }
//
//     // internal abstract void Accept(IImageVisitor visitor)
//     internal override void Accept(IImageVisitor visitor)
//     {
//     }
//     
//     // internal abstract Task AcceptAsync(IImageVisitorAsync visitor, CancellationToken cancellationToken);
//     internal override Task AcceptAsync(IImageVisitorAsync visitor, CancellationToken cancellationToken)
//     {
//         return new Task(() => Accept(null));
//     }
// }