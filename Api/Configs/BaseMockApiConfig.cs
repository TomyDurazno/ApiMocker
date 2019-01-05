using System;

namespace APIMocker.Configs
{
    public interface IMockApiConfig<T>
    {
       Func<T, string> Key { get; set; }
    }

    public class MockApiConfig<T> : IMockApiConfig<T>
    {
        public Func<T, string> Key { get; set; }
        public string StoreName { get; set; }
        public string Seed { get; set; }

        public static MockApiConfig<T> Make(Func<T, string> GetKey, string Storename, string Seed = null)
        {
            return new MockApiConfig<T>()
            {
                Key = GetKey,
                StoreName = Storename,
                Seed = Seed
            };
        }
    }

    public class MockModelAPIConfig
    {
        public string StoreName { get; set; }
        public string Seed { get; set; }

        public static MockModelAPIConfig Make(string Storename, string Seed = null)
        {
            return new MockModelAPIConfig
            {
                StoreName = Storename,
                Seed = Seed
            };
        }
    }
}