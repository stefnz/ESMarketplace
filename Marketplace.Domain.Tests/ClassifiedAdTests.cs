namespace Marketplace.Domain.Tests;

public class ClassifiedAdTests {
    [Fact]
    public void Create_new_classified_ad() {
        var ad = CreateNewAd();
        
        Assert.NotNull(ad);
        Assert.NotNull(ad.Id);
        Assert.NotNull(ad.OwnerId);
    }

    [Fact]
    public void Can_publish_a_valid_ad() {
        var classifiedAd = CreateNewAd();
        
        classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
        classifiedAd.UpdateText(ClassifiedAdText.FromString("Please buy my stuff"));
        classifiedAd.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));
        classifiedAd.RequestPublish();
        
        Assert.Equal(ClassifiedAd.ClassifiedAdState.PendingReview, classifiedAd.State);
    }

    [Fact]
    public void Cannot_publish_without_title()
    {
        var classifiedAd = CreateNewAd();
        
        classifiedAd.UpdateText(ClassifiedAdText.FromString("Please buy my stuff"));
        classifiedAd.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));
        
        Assert.Throws<InvalidEntityStateException>(() => classifiedAd.RequestPublish());
    }
    
    [Fact]
    public void Cannot_publish_without_text()
    {
        var classifiedAd = CreateNewAd();
        
        classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
        classifiedAd.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));
        
        Assert.Throws<InvalidEntityStateException>(() => classifiedAd.RequestPublish());
    }

    [Fact]
    public void Cannot_publish_without_price()
    {
        var classifiedAd = CreateNewAd();
        
        classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
        classifiedAd.UpdateText(ClassifiedAdText.FromString("Please buy my stuff"));
        
        Assert.Throws<InvalidEntityStateException>(() => classifiedAd.RequestPublish());
    }
    
    [Fact]
    public void Cannot_publish_with_zero_price()
    {
        var classifiedAd = CreateNewAd();
        
        classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
        classifiedAd.UpdateText(ClassifiedAdText.FromString("Please buy my stuff"));
        classifiedAd.UpdatePrice(Price.FromDecimal(0.0m, "EUR", new FakeCurrencyLookup()));
        
        Assert.Throws<InvalidEntityStateException>(() => classifiedAd.RequestPublish());
    }
    
    private ClassifiedAd CreateNewAd() {
        return new ClassifiedAd(new ClassifiedAdId(Guid.NewGuid()), new UserId(Guid.NewGuid()));
    }
}