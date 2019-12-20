using Gaver.Web.Attributes;

namespace Gaver.Web.Options
{
    [GenerateTypeScript]
    public class FeatureFlags
    {
        public bool WishOptions { get; set; }
        public bool UserGroups { get; set; }
    }
}
