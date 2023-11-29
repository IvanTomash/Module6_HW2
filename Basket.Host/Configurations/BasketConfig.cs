#pragma warning disable CS8618

#pragma warning disable SA1600

namespace Basket.Host.Configurations;

public class BasketConfig
{
    public string Host { get; set; }

    public string ImgUrl { get; set; }

    public string ConnectionString { get; set; }
}