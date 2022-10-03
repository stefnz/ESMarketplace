using ES.Framework;

namespace Marketplace.Domain;

public class Picture : Entity<PictureId> {
    internal PictureSize Size { get; set; }
    internal Uri Location { get; set; }
    internal int Order { get; set; }

    public void Resize(PictureSize newSize)
        => Apply(new ClassifiedAdEvents.PictureResized {
            PictureId = Id.Value,
            Height = newSize.Width,
            Width = newSize.Width
        });

    protected override void When(object @event) {
        switch (@event) {
            case ClassifiedAdEvents.PictureAddedToAClassifiedAd e:
                Id = new PictureId(e.PictureId);
                Location = new Uri(e.Url);
                Size = new PictureSize(e.Height, e.Width); // use of constructor means validation will be applied.
                Order = e.Order;
                break;
            case ClassifiedAdEvents.PictureResized e:
                Size = new PictureSize(e.Height, e.Width);
                break;
        }
    }

    public Picture(Action<object> applier) : base(applier) { }
}

public record PictureId {
    public Guid Value { get; }
    internal PictureId(Guid id) => Value = id;
}

public record PictureSize {
    public int Height { get; }
    public int Width { get; }

    public PictureSize(int height, int width) {
        if (height < 0) {
            throw new ArgumentOutOfRangeException(nameof(height), "Picture height must be 0 or more.");
        }

        if (width < 0) {
            throw new ArgumentOutOfRangeException(nameof(width), "Picture width must be 0 or more.");
        }

        Height = height;
        Width = width;
    }
}