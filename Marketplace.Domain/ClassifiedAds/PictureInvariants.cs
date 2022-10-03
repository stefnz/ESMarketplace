namespace Marketplace.Domain; 

public static class PictureInvariants {
    public static bool HasCorrectSize(this Picture picture) => picture != null
                                                               && picture.Size.Height >= 600
                                                               && picture.Size.Width >= 800;
}